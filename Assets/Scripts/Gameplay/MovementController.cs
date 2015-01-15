using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	public GameObject cursor;
	NavMeshAgent agent;

	float mouseDownTimer = 0f;
	bool useDirectMouseControl = false;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Vector3 destination = ScreenToNavPos(Input.mousePosition);
			agent.SetDestination (destination);
			mouseDownTimer = 0f;
			
			Destroy((GameObject)GameObject.Instantiate(cursor, destination, Quaternion.identity), 0.5f);
		} else if (Input.GetMouseButtonUp(0) && useDirectMouseControl){
			// If it's using direct mouse control and the mouse button no longer down. reset the path
			useDirectMouseControl = false;
			agent.ResetPath();
		} else if (Input.GetMouseButton(0) && mouseDownTimer > 0.25f){
			// Update new path every 0.25 second
			useDirectMouseControl = true;
			Vector3 destination = ScreenToNavPos(Input.mousePosition);

			// Uncomment this code if you want to directly manipulate the move and comment the code below this. not recomended
			// agent.Move((destination - transform.position).normalized * agent.speed * Time.deltaTime);

			agent.SetDestination(destination);
			mouseDownTimer = 0f;	// only calculate path every 0.25f seconds
		}

		Debug.DrawRay (transform.position, agent.velocity);
		mouseDownTimer += Time.deltaTime;
	}

	Vector3 ScreenToNavPos(Vector3 pos){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit)){
			return hit.point;
		}
		return transform.position;
	}
}
