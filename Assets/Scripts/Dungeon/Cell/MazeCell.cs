using UnityEngine;
using System.Collections;

public class MazeCell : MonoBehaviour {

    public IntVector2 coordinates;
    private int initializedEdgeCount;
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }
}
