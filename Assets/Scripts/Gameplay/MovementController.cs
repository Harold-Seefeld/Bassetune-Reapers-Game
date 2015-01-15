using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	public GameObject cursor;
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Vector3 destination = ScreenToNavPos(Input.mousePosition);
			agent.SetDestination (destination);
			Destroy((GameObject)GameObject.Instantiate(cursor, destination, Quaternion.identity), 0.5f);
		}
	}

	Vector3 ScreenToNavPos(Vector3 pos){
		Ray r = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit)){
			return hit.point;
		}
		return pos;
	}
}
