using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : MonoBehaviour
{
    public Camera minimapCam;
   
    public Vector3 currentDestination = Vector2.zero;

    private List<Vector3> destinationPath = new List<Vector3>();
    private Vector2 destination = Vector2.zero;
    private Vector2 sentDestination = Vector2.zero;
    private SocketIOComponent socket;
    private CharacterData characterData;
    private Animator animator;

    void Start()
	{
        // Set local variables
        characterData = GetComponent<CharacterData>();
        socket = FindObjectOfType<SocketIOComponent>();
        animator = GetComponent<Animator>();
        minimapCam = GameObject.Find("MinimapCam").GetComponent<Camera>();
        currentDestination = transform.position;
	}

	void Update () 
    {
        FindPath();

        if (characterData.CharacterOwner == Server.instance.currentPlayerID && destination != sentDestination)
        {
            // Emitting X, Z to server for verification
            CharacterManager.instance.AddLocation(characterData.CharacterID.ToString(), destination);
            sentDestination = destination;
        }

        if (currentDestination != transform.position)
        {
            // Use moving animation
            animator.SetFloat("MoveSpeed", Mathf.Min(1, Vector3.Distance(currentDestination, transform.position) * Time.deltaTime * 64));
            animator.speed = animator.GetFloat("MoveSpeed");
            transform.position = Vector3.Lerp(transform.position, currentDestination, Time.deltaTime * 4);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentDestination - transform.position), Time.deltaTime * 4);
        }
        else
        {
            // Cancel moving animation
            animator.SetFloat("MoveSpeed", 0);
            animator.speed = 1;
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
            }      
        }
    }
}
