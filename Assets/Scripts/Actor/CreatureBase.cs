﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * This is base Actor for all creature  actor.
 * If you overriding Unity function from this class, don't forget to call Base[FunctionToOverride] before writing your scripts.
 * Remember base.[FunctionToOverride] won't work due to unity reflection architectures.
 */

[AddComponentMenu("Actor/CreatureBase")]
public class CreatureBase : MonoBehaviour {
	// List of patrol waypoint for character, leave it empty to use random waypoint
	public Transform [] patrolWaypoints;
	// Anchor of randomization waypoint, it act as chain that makes random patrol only randomized in certain area
	public Transform creatureAnchor;
	// Max length of anchor chain
	public float anchorRange = 20f;
	// Sight distance of creatures
	public float sightDistance = 10f;
	// Attack range of creatures
	public float attackRange = 1f;
	// Idle time between waypoint
	public float idleTime = 1f;
	// Time needed to do attack movement
	public float attackTime = 0.1f;
	// Cool down time between attack movement
	public float cooldownTime = 1.5f;

	// This hold all characters that currently inside creature sight
	List<Transform> target = new List<Transform> ();
	// Agent for pathfinding and movement
	NavMeshAgent agent;
	// Attacking flag
	bool isAttacking = false;
	// Track current waypoint index
	int currentWaypoint = 0;
	// Timer for used internally
	float timer = 0f;
	// Debug AI
	public GameObject debugLabel;
	Text debugLabelText;

	// Unity Start function
	void Start () {
		BaseStart ();
	}

	// Base function of Start
	protected void BaseStart(){
		agent = GetComponent<NavMeshAgent> ();
		
		#if UNITY_EDITOR
		debugLabel = Instantiate (debugLabel) as GameObject;
		debugLabelText = debugLabel.GetComponent<Text> ();
		debugLabel.transform.SetParent (GameObject.Find ("InGameCanvas").transform);
		#endif
	}

	// Unity Update function
	void Update () {
		BaseUpdate ();
	}

	// Base function of update
	protected void BaseUpdate(){
		if (target.Count > 0) {
			if (Vector3.Distance(target[0].position, transform.position) > attackRange){
				Chase();
				
				#if UNITY_EDITOR
				debugLabelText.text = "Chasing";
				#endif
			} else {
				bool status = Attack();
				
				#if UNITY_EDITOR
				debugLabelText.text = status ? "Attack " : "Cooldown";
				#endif
			}
		} else {
			Patrol();
			
			#if UNITY_EDITOR
			debugLabelText.text = "Patrol";
			#endif
		}
		
		#if UNITY_EDITOR
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, gameObject.transform.position);
		debugLabelText.rectTransform.anchoredPosition = screenPoint - debugLabelText.canvas.GetComponent<RectTransform> ().sizeDelta / 2f;
		#endif
	}
	
	protected virtual bool Attack(){
		if ((isAttacking && timer < attackTime) || (!isAttacking && timer < cooldownTime)){
			timer += Time.deltaTime;
			return isAttacking;
		}

		// TODO: Attack Code
		timer = 0f;
		isAttacking = !isAttacking;

		return isAttacking;
	}

	protected virtual void Chase(){
		if (timer < 0.25f){
			timer += Time.deltaTime;
			return;
		}

		agent.SetDestination(target[0].position);
	}

	protected virtual void Patrol(){
		if (agent.remainingDistance <= float.Epsilon){
			if (timer > idleTime){
				if (patrolWaypoints.Length > 1){
					currentWaypoint = (currentWaypoint + 1) < patrolWaypoints.Length ? currentWaypoint + 1 : 0;
					agent.SetDestination(patrolWaypoints[currentWaypoint].position);
					timer = 0f;
				} else {
					if (!creatureAnchor){
						Debug.LogWarning("Warning, Creature Anchor is not set");
						return;
					}
					RaycastHit rHit;
					Vector3 randomPosition = Random.insideUnitSphere * anchorRange;
					randomPosition.Set(randomPosition.x, 0, randomPosition.z);
					if (Physics.Raycast (creatureAnchor.position, randomPosition, out rHit)){
						randomPosition = rHit.point;
					} else {
						randomPosition += creatureAnchor.position;
					}

					NavMeshHit hit;
					NavMesh.SamplePosition(randomPosition, out hit, anchorRange, 1);
					agent.SetDestination(hit.position);
					timer = 0f;
				}
			}
			timer += Time.deltaTime;
		}
		Debug.DrawLine(transform.position, agent.destination, Color.red);
	}
	
	void OnTriggerEnter(Collider other) {
		BaseOnTriggerEnter (other);
	}

	protected void BaseOnTriggerEnter(Collider other){
		if (other.tag == "Knight"){
			target.Add(other.transform);
		}
	}
	
	void OnTriggerExit(Collider other) {
		BaseOnTriggerExit (other);
	}

	protected void BaseOnTriggerExit(Collider other){
		if (other.tag == "Knight"){
			target.Remove(other.transform);
		}
	}
}
