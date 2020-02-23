﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
 * This is base Actor for all creature  actor.
 * If you overriding Unity function from this class, don't forget to call Base[FunctionToOverride] before writing your scripts.
 * Remember base.[FunctionToOverride] won't work due to unity reflection architectures.
 */

[AddComponentMenu("Actor/CreatureBase")]
public class CreatureBase : MonoBehaviour
{
    // Agent for pathfinding and movement
    private NavMeshAgent agent;

    // Max length of anchor chain
    public float anchorRange = 20f;

    // Attack range of creatures
    public float attackRange = 1f;

    // Time needed to do attack movement
    public float attackTime = 0.1f;

    // Cool down time between attack movement
    public float cooldownTime = 1.5f;

    // Anchor of randomization waypoint, it act as chain that makes random patrol only randomized in certain area
    public Transform creatureAnchor;

    // Current target
    private Transform currentTarget;

    // Track current waypoint index
    private int currentWaypoint;

    // Debug AI
    public GameObject debugLabel;

    protected Text debugLabelText;

    // Idle time between waypoint
    public float idleTime = 1f;

    // Attacking flag
    private bool isAttacking;

    // Picking Target Couroutine flag
    private bool isPickTargetRunning;

    // Obstacle for avoidance
    private NavMeshObstacle obstacle;

    // List of patrol waypoint for character, leave it empty to use random waypoint
    public Transform[] patrolWaypoints;

    // Sight distance of creatures
    public float sightDistance = 10f;

    // This hold all characters that currently inside creature sight
    public List<Transform> target = new List<Transform>();

    // Timer for used internally
    private float timer;

    // Unity Start function
    private void Start()
    {
        BaseStart();
    }

    // Base function of Start
    protected void BaseStart()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        agent.stoppingDistance = attackRange;

#if UNITY_EDITOR
        debugLabel = Instantiate(debugLabel);
        debugLabelText = debugLabel.GetComponent<Text>();
        debugLabel.transform.SetParent(GameObject.Find("InGameCanvas").transform);
#else
		debugLabel.SetActive(false);
		debugLabelText.text = "";
#endif
    }

    // Unity Update function
    private void Update()
    {
        BaseUpdate();
    }

    // Base function of update
    protected void BaseUpdate()
    {
        if (currentTarget)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) > attackRange)
            {
                // Disable obstacle and activate agent since this actor will move around
                agent.enabled = true;
                obstacle.enabled = false;

                // Chase
                Chase();

#if UNITY_EDITOR
                debugLabelText.text = "Chasing";
#endif
            }
            else
            {
                // Agent will now stay in place, we'll treat it as obstacle
                agent.enabled = false;
                obstacle.enabled = true;

                var status = Attack();

#if UNITY_EDITOR
                debugLabelText.text = status ? "Attack " : "Cooldown";
#endif
            }
        }
        else
        {
            // Disable obstacle and activate agent since this actor will move around
            agent.enabled = true;
            obstacle.enabled = false;

            Patrol();

#if UNITY_EDITOR
            debugLabelText.text = "Patrol";
#endif
        }

#if UNITY_EDITOR
        // Update Debug Label position
        var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, gameObject.transform.position);
        debugLabelText.rectTransform.anchoredPosition =
            screenPoint - debugLabelText.canvas.GetComponent<RectTransform>().sizeDelta / 2f;
#endif
    }

    protected virtual bool Attack()
    {
        if (isAttacking && timer < attackTime || !isAttacking && timer < cooldownTime)
        {
            timer += Time.deltaTime;
            return isAttacking;
        }

        // TODO: Attack Code
        timer = 0f;
        isAttacking = !isAttacking;

        return isAttacking;
    }

    protected virtual void Chase()
    {
        if (timer < 0.25f)
        {
            timer += Time.deltaTime;
            return;
        }

        agent.SetDestination(currentTarget.position);
    }

    protected virtual void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (timer > idleTime)
            {
                if (patrolWaypoints.Length > 1)
                {
                    currentWaypoint = currentWaypoint + 1 < patrolWaypoints.Length ? currentWaypoint + 1 : 0;
                    agent.SetDestination(patrolWaypoints[currentWaypoint].position);
                    timer = 0f;
                }
                else
                {
                    if (!creatureAnchor)
                    {
                        Debug.LogWarning("Warning, Creature Anchor is not set");
                        return;
                    }

                    RaycastHit rHit;
                    var randomPosition = Random.insideUnitSphere * anchorRange;
                    randomPosition.Set(randomPosition.x, 0, randomPosition.z);
                    if (Physics.Raycast(creatureAnchor.position, randomPosition, out rHit))
                        randomPosition = rHit.point;
                    else
                        randomPosition += creatureAnchor.position;

                    NavMeshHit hit;
                    NavMesh.SamplePosition(randomPosition, out hit, anchorRange, 1);
                    agent.SetDestination(hit.position);
                    timer = 0f;
                }
            }

            timer += Time.deltaTime;
        }

        // Look for target
        if (!isPickTargetRunning && target.Count > 0) StartCoroutine("PickTarget");
        Debug.DrawLine(transform.position, agent.destination, Color.red);
    }

    private IEnumerator PickTarget()
    {
        isPickTargetRunning = true;
        Transform t = null;
        var closestDistance = float.MaxValue;
        IEnumerator<Transform> e = target.GetEnumerator();

        while (e.MoveNext())
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, e.Current.position, out hit))
                if (hit.transform.tag == "Knight" && hit.distance < closestDistance)
                {
                    t = e.Current.transform;
                    closestDistance = hit.distance;
                }

            yield return null;
        }

        currentTarget = t;
        isPickTargetRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseOnTriggerEnter(other);
    }

    protected void BaseOnTriggerEnter(Collider other)
    {
        if (other.tag == "Knight")
        {
            target.Add(other.transform);

            // Stop Couroutine
            StopCoroutine("PickTarget");
            isPickTargetRunning = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseOnTriggerExit(other);
    }

    protected void BaseOnTriggerExit(Collider other)
    {
        if (other.tag == "Knight")
        {
            target.Remove(other.transform);

            // Stop Couroutine
            StopCoroutine("PickTarget");
            isPickTargetRunning = false;

            if (currentTarget && currentTarget.gameObject == other.gameObject) currentTarget = null;
        }
    }
}