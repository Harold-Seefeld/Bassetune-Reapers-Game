using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : Pathfinding
{
    public Camera minimapCam;

	private SocketIOComponent socket;
    private CharacterData characterData;
   
	public GUIStyle bgStyle;

	private Vector3 direction;
    //private Vector3 lastPositionMoved;
    //private Vector3 lastPositionSent;
    private Vector2 destination;

    void Start()
	{
        characterData = GetComponent<CharacterData>();

        socket = FindObjectOfType<SocketIOComponent>();

        minimapCam = GameObject.Find("MinimapCam").GetComponent<Camera>();

        if (characterData.CharacterOwner == Server.instance.currentPlayerID)
        {
            StartCoroutine(SendDestination());
        }
	}

	void Update () 
    {
		FindPath();
        if (Path.Count > 0)
        {
            MoveMethod();
        }
	}

    private void FindPath()
    {
        if (Server.instance.currentPlayerID != characterData.CharacterOwner)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F)
        {
            //Call to the player map
            Ray ray = minimapCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                destination = new Vector2(hit.point.x, hit.point.z);
                //FindPath(transform.position, new Vector3(hit.point.x, 5, hit.point.z));
            }          
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            //Call minimap
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                destination = new Vector2(hit.point.x, hit.point.z);
                //FindPath(transform.position, hit.point);
            }      
        }
    }


    private void MoveMethod()
    {
		if (Path.Count > 0)
        {
            direction = (Path[0] - transform.position).normalized;

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

            //lastPositionMoved = transform.position;
		}
    }

    IEnumerator SendDestination()
    {
        yield return new WaitForSeconds(0.1f);

        if (destination != Vector2.zero)//(lastPositionSent != lastPositionMoved)
        {
            // Emitting X, Z to server for verification
            //new Vector2(lastPositionMoved.x, lastPositionMoved.z);
            CharacterManager.instance.AddLocation(characterData.CharacterID.ToString(), destination);

            destination = Vector2.zero;
        }
        StartCoroutine(SendDestination());
    }

    //IEnumerator SendMovement()
    //{
    //    if (characterData.CharacterOwner == Server.instance.currentPlayerID)
    //    {
    //        yield return new WaitForSeconds(0.1f);

    //        if (Path.Count > 1)//(lastPositionSent != lastPositionMoved)
    //        {
    //            // Emitting X, Z to server for verification
    //            Vector2 movementData = new Vector2(Path[0].x, Path[0].z); //new Vector2(lastPositionMoved.x, lastPositionMoved.z);
    //            CharacterManager.instance.AddLocation(characterData.CharacterID.ToString(), movementData);
    //            lastPositionSent = lastPositionMoved;
    //        }

    //        StartCoroutine(SendMovement());
    //    }
    //    yield return 0;
    //}

    //    void OnGUI()
    //    {
    //        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", bgStyle);
    //    }
}
