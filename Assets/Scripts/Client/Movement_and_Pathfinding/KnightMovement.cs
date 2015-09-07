using UnityEngine;
using System.Collections;
using SocketIO;

// The movement for knight/player is almost complete. 
// I need to finish the A* pathfinding this week if possible.
public class KnightMovement : MonoBehaviour 
{
	private bool check = false;
	private Vector3 endPoint;
	private float yAxis;
	private GameObject socketRef;
	private SocketIOComponent socket;
	public float speed;

	// Use this for initialization
	void Start () 
	{
		socketRef = GameObject.Find("SocketIO");

		socket = socketRef.GetComponent<SocketIOComponent>();

		//this will keep the 
		yAxis = gameObject.transform.position.y;
	}
	
	//I am still in the process of trying to add A* pathfinding into this script.
	void Update () 
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray;

			//Get information from the Raycast.
			RaycastHit hit;

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit))
			{
				check = true;

				endPoint = hit.point;

				endPoint.y = yAxis;

				Debug.Log(endPoint);
			}
		}

		if(check && !Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endPoint, 1/(speed*(Vector3.Distance(gameObject.transform.position, endPoint))));

			//send the emitting data
			//socket.Emit("Emitting player's movement", EmitPlayerMovement);
			//socket.Emit("it is working");

			Debug.Log ("Moving, sir!");
		}
     	else if(check && Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
     	{
			check = false;
		}
	}

	public void EmitPlayerMovement(RaycastHit playerHit)
	{

	}


//IN PROGRESS!
//this will detect the pathfinding/calculation of the path of the player. 
//	void PathFinding(RaycastHit pathCheck) 
//	{
//
//	}
	
}
