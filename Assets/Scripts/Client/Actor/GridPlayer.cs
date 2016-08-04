using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : MonoBehaviour
{
    public Camera minimapCam;
   
    public Vector3 currentDestination = Vector2.zero;
    public float speed = 6f;

    private List<Vector3> destinationPath = new List<Vector3>();
    private Vector2 destination = Vector2.zero;
    private SocketIOComponent socket;
    private CharacterData characterData;

    void Start()
	{
        characterData = GetComponent<CharacterData>();

        socket = FindObjectOfType<SocketIOComponent>();

        minimapCam = GameObject.Find("MinimapCam").GetComponent<Camera>();

        currentDestination = transform.position;

        if (characterData.CharacterOwner == Server.instance.currentPlayerID)
        {
            StartCoroutine(SendDestination());
        }
	}

	void Update () 
    {
        FindPath();

        if (destinationPath.Count > 0)
        {
            if (transform.position.x < destinationPath[0].x + 0.2F && transform.position.x > destinationPath[0].x - 0.2F && transform.position.z > destinationPath[0].z - 0.2F && transform.position.z < destinationPath[0].z + 0.2F)
            {
                destinationPath.RemoveAt(0);
            }
        }
    }

    private void FindPath()
    {
        if (Server.instance.currentPlayerID != characterData.CharacterOwner)
        {
            return;
        }

        if (Input.GetButtonDown("Move") && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F)
        {
            //Call to the player map
            Ray ray = minimapCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                destination = new Vector2(hit.point.x, hit.point.z);
                //StartCoroutine(UpdateDestinationPath(transform.position, new Vector3(hit.point.x, 5, hit.point.z)));
            }          
        }
        else if (Input.GetButtonDown("Move"))
        {
            //Call minimap
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                destination = new Vector2(hit.point.x, hit.point.z);
                //StartCoroutine(UpdateDestinationPath(transform.position, hit.point));
            }      
        }
    }

    private Vector2 sentDestination = new Vector2();
    IEnumerator SendDestination()
    {
        yield return new WaitForSeconds(0.083f);

        //if (destinationPath.Count > 0)
        //{
            //Vector2 destination = new Vector2(destinationPath[0].x, destinationPath[0].z);
            if (destination != sentDestination)
            {
                // Emitting X, Z to server for verification
                CharacterManager.instance.AddLocation(characterData.CharacterID.ToString(), destination);
                sentDestination = destination;
            }
        //}
 
        StartCoroutine(SendDestination());
    }
}
