using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public GameObject cursor;
	NavMeshAgent agent;

	Knight knight; // (dmongs)

	float mouseDownTimer = 0f;
	bool useDirectMouseControl = false;


	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		knight = GetComponent<Knight> ();
	}

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

		// knight
		knight.SetAniParamMoveSpeed (agent.velocity.magnitude);


		Debug.DrawRay (transform.position, agent.velocity);
		mouseDownTimer += Time.deltaTime;
	}

	Vector3 ScreenToNavPos(Vector3 pos){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 100, 1 << 8)){	// 1 << 8 is Terrain layer mask
			return hit.point;
		}
		return transform.position;
	}
}
