using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour 
{
	public LayerMask unwalkableMask;
	public Vector2 gridWorld;
	public float nodeRadius;
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	Node[,] grid;


	void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorld.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorld.y/nodeDiameter);
		CreateGrid();
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3(gridWorld.x, 1, gridWorld.y));

		if(grid != null)
		{
			foreach(Node n in grid)
			{
				Gizmos.color = (n._walkable) ? Color.blue : Color.red;
				Gizmos.DrawCube(n._worldPosition, Vector3.one * (nodeDiameter - .1f));

			}
		}

	}

	void CreateGrid()
	{
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 mapBottomLeft = transform.position - Vector3.right * gridWorld.x / 2 - Vector3.forward * gridWorld.y / 2;


		for(int x = 0; x < gridSizeX; x++)
		{
			for(int y = 0; y < gridSizeY; y++)
			{
				Vector3 mapPoint = mapBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); 
				bool walkable = !(Physics.CheckSphere(mapPoint, nodeRadius, unwalkableMask));
				grid [x, y] = new Node(walkable, mapPoint);
			}

		}
	}

}
