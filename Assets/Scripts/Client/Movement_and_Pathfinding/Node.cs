using UnityEngine;
using System.Collections;

public class Node 
{
	public bool _walkable;
	public Vector3 _worldPosition;

	public Node(bool walkable, Vector3 worldPosition)
	{
		_walkable = walkable;
		_worldPosition = worldPosition;
	}
}
