using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class GridPlayer : MonoBehaviour
{  
    public Vector3 currentDestination = Vector2.zero;

    private List<Vector3> destinationPath = new List<Vector3>();
    private Vector2 destination = Vector2.zero;
    private Animator animator;

    void Start()
	{
        animator = GetComponent<Animator>();
        currentDestination = transform.position;
	}

	void Update () 
    {
        if (Server.instance == null) return;

        if (Vector3.Distance(currentDestination, transform.position) > 0.3f)
        {
            // Use moving animation
            animator.SetFloat("MoveSpeed", Mathf.Min(1, Vector3.Distance(currentDestination, transform.position) * Time.deltaTime * 2048));
            animator.speed = animator.GetFloat("MoveSpeed");
            transform.position = Vector3.Lerp(transform.position, currentDestination, Time.deltaTime * 4);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(currentDestination - transform.position), Time.deltaTime * 16);
        }
        else
        {
            // Cancel moving animation
            animator.SetFloat("MoveSpeed", 0);
            animator.speed = 1;
        }
    }
}
