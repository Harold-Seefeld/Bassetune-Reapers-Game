using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreatureController : MonoBehaviour {
	public List<Transform> target;
	public Transform [] patrolWaypoints;
	public Transform randomAnchor;
	public float sightDistance = 10f;
	public float attackRange = 1f;
	public float idleTime = 1f;
	public float attackTime = 0.1f;
	public float cooldownTime = 1.5f;

	NavMeshAgent agent;
	bool isAttacking = false;
	int currentWaypoint = 0;
	float timer = 0f;
	
	public GameObject debugLabel;
	Text debugLabelText;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();

		#if UNITY_EDITOR
		debugLabel = Instantiate (debugLabel) as GameObject;
		debugLabelText = debugLabel.GetComponent<Text> ();
		debugLabel.transform.SetParent (GameObject.Find ("InGameCanvas").transform);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
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
	
	bool Attack(){
		if ((isAttacking && timer < attackTime) || (!isAttacking && timer < cooldownTime)){
			timer += Time.deltaTime;
			return isAttacking;
		}

		// TODO: Attack Code
		timer = 0f;
		isAttacking = !isAttacking;
		return isAttacking;
	}

	void Chase(){
		if (timer < 0.25f){
			timer += Time.deltaTime;
			return;
		}

		agent.SetDestination(target[0].position);
	}

	void Patrol(){
		if (agent.remainingDistance <= float.Epsilon){
			if (timer > idleTime){
				if (patrolWaypoints.Length > 1){
					currentWaypoint = (currentWaypoint + 1) < patrolWaypoints.Length ? currentWaypoint + 1 : 0;
					agent.SetDestination(patrolWaypoints[currentWaypoint].position);
					timer = 0f;
				} else {
					if (!randomAnchor){
						Debug.LogWarning("Warning, Random Anchor is not set");
						return;
					}
					RaycastHit rHit;
					Vector3 randomPosition = Random.insideUnitSphere * 20;
					randomPosition.Set(randomPosition.x, 0, randomPosition.z);
					if (Physics.Raycast (randomAnchor.position, randomPosition, out rHit)){
						randomPosition = rHit.point;
					} else {
						randomPosition += randomAnchor.position;
					}

					NavMeshHit hit;
					NavMesh.SamplePosition(randomPosition, out hit, 20, 1);
					agent.SetDestination(hit.position);
					timer = 0f;
				}
			}
			timer += Time.deltaTime;
		}
		Debug.DrawLine(transform.position, agent.destination, Color.red);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Knight"){
			target.Add(other.transform);
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Knight"){
			target.Remove(other.transform);
		}
	}
}
