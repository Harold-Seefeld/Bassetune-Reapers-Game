using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatureController : MonoBehaviour {
	public Transform target;
	public Transform [] patrolWaypoints;
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

		// Initialize path
		if (patrolWaypoints.Length > 1){
			agent.SetDestination(patrolWaypoints[0].position);
		} else {
			// Random
		}
		
		#if UNITY_EDITOR
		debugLabel = Instantiate (debugLabel) as GameObject;
		debugLabelText = debugLabel.GetComponent<Text> ();
		debugLabel.transform.SetParent (GameObject.Find ("InGameCanvas").transform);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			if (Vector3.Distance(target.transform.position, transform.position) > attackRange){
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

		agent.SetDestination(target.position);
	}

	void Patrol(){
		if (patrolWaypoints.Length > 1){
			if (agent.remainingDistance <= float.Epsilon){
				if (timer > idleTime){
					currentWaypoint = (currentWaypoint + 1) < patrolWaypoints.Length ? currentWaypoint + 1 : 0;
					agent.SetDestination(patrolWaypoints[currentWaypoint].position);
					timer = 0f;
				}
				timer += Time.deltaTime;
			}
			Debug.DrawLine(transform.position, agent.destination, Color.red);
		} else {
			Vector3 randomPos = Random.insideUnitSphere + (transform.forward * 2f);
			agent.SetDestination(transform.position + new Vector3(randomPos.x, 0, randomPos.z));
			Debug.DrawRay(transform.position, new Vector3(randomPos.x, 0, randomPos.z));
		}
	}
}
