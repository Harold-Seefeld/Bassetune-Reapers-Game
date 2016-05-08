using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : Pathfinding
{
    public Camera playerCam;
    public Camera minimapCam;

    public GUIStyle bgStyle;
    Vector3 direction;

    public Vector3 currentDestination = Vector2.zero;
    public float speed = 6f;

    private List<Vector3> destinationPath = new List<Vector3>();
    private SocketIOComponent socket;
    private CharacterData characterData;

    void Start()
    {
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        currentDestination = transform.position;

        if (characterData.CharacterOwner == Server.instance.currentPlayerID)
        {
            StartCoroutine(SendDestination());
        }
    }

    void Update()
    {
        if (Path.Count > 0)
        {
            MoveMethod();
        }

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

        if (Input.GetButtonDown("Fire1") && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F)
        {
            //Call to the player map
            Ray ray = minimapCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //destination = new Vector2(hit.point.x, hit.point.z);
                StartCoroutine(UpdateDestinationPath(transform.position, new Vector3(hit.point.x, 5, hit.point.z)));
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
                //destination = new Vector2(hit.point.x, hit.point.z);
                StartCoroutine(UpdateDestinationPath(transform.position, hit.point));
            }
        }
    }

    private IEnumerator UpdateDestinationPath(Vector3 initialPosition, Vector3 endPosition)
    {
        yield return StartCoroutine(FindPath(initialPosition, endPosition));
        destinationPath = new List<Vector3>();
        if (Path.Count > 0)
        {
            destinationPath.Add(new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y, Path[Path.Count - 1].z));
        }

        for (var i = 0; i < Path.Count; i++)
        {
            //TODO: Find way to only change when direction changes
            destinationPath.Add(new Vector3(Path[i].x, Path[i].y, Path[i].z));
        }

        if (Path.Count > 0)
        {
            destinationPath.Add(new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y, Path[Path.Count - 1].z));
        }

        // Set path for pathfinding again
        StartCoroutine(FindPath(transform.position, new Vector3(currentDestination.x, 5, currentDestination.z)));
    }

    private void MoveMethod()
    {
        //create a JSONobject that will catch the movement data for the players.  
        JSONObject movementData = new JSONObject(JSONObject.Type.OBJECT);

        if (Path.Count > 0)
        {
            Vector3 direction = (Path[0] - transform.position).normalized;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * speed);
            if (transform.position.x < Path[0].x + 0.2F && transform.position.x > Path[0].x - 0.2F && transform.position.z > Path[0].z - 0.2F && transform.position.z < Path[0].z + 0.2F)
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

    private Vector2 sentDestination = new Vector2();
    IEnumerator SendDestination()
    {
        yield return new WaitForSeconds(0.083f);

        if (destinationPath.Count > 0)
        {
            Vector2 destination = new Vector2(destinationPath[0].x, destinationPath[0].z);
            if (destination != sentDestination)
            {
                // Emitting X, Z to server for verification
                CharacterManager.instance.AddLocation(characterData.CharacterID.ToString(), destination);
                sentDestination = destination;
            }
        }

        StartCoroutine(SendDestination());
    }
}
