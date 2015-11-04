using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : Pathfinding
{
    public Camera playerCam;
    public Camera minimapCam;

	//adding the socketIO gameobject
	private GameObject socketObject;
	private SocketIOComponent socket;
    
	public GUIStyle bgStyle;

	//this start function will initialize the socket emitter.
	void Start()
	{
		socketObject = GameObject.Find ("SocketIO");
		socket = socketObject.GetComponent<SocketIOComponent>();
		socket.On("listening", OpenSocket);
	
	}

	void Update () 
    {
        FindPath();
        if (Path.Count > 0)
        {
            MoveMethod();
        }
	}

	private void OpenSocket(SocketIOEvent ev)
	{
		Debug.Log("listening for emitter");
	}

    private void FindPath()
    {

        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F)
        {
            //Call to the player map
            Ray ray = minimapCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {             
               FindPath(transform.position, hit.point);
            }          
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            //Call minimap
            Ray ray = playerCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                FindPath(transform.position, hit.point);
            }      
        }
    }

    private void MoveMethod()
    {
		//create a JSONobject that will catch the movement data for the players.  
		JSONObject movementData = new JSONObject(JSONObject.Type.OBJECT);

		if (Path.Count > 0)
        {
            Vector3 direction = (Path[0] - transform.position).normalized;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * 14F);
            if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
            {
                Path.RemoveAt(0);
            }

            RaycastHit[] hit = Physics.RaycastAll(transform.position + (Vector3.up * 20F), Vector3.down, 100);
            float maxY = -Mathf.Infinity;
            foreach (RaycastHit h in hit)
            {
                if (h.transform.tag == "Untagged")
                {
                    if (maxY < h.point.y)
                    {
                        maxY = h.point.y;
                    }
                }
            }
            transform.position = new Vector3(transform.position.x, maxY + 1F, transform.position.z);
        	
			//adding some fields and emitting them from the socket
			movementData.AddField("x", direction.x);
			movementData.AddField("y", direction.y);
			movementData.AddField("z", direction.z);

			socket.Emit("making movement data", movementData);

		}
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", bgStyle);
    }
}
