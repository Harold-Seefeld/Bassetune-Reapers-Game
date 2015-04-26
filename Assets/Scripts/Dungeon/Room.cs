using UnityEngine;
using System.Collections;

/// <summary>
/// class should handle a Room and its Generation
/// </summary>
public class Room : MonoBehaviour
{

    public int MaxSize;
    public int MinSize;
    public IntVector2 Size;
    public IntVector2 StartPoints;

    public MazeCell cellPrefab;
    public DungeonMap dungeonPrefab;

    private MazeCell[,] cells;

    /// <summary>
    /// Generates a Room with the predefined Values from the Editor.
    /// </summary>
    public void Generate()
    {
        CreateRoomBoundaries();
        CreateCells();
    }

    private void CreateRoomBoundaries()
    {
        var roomBase = ((MinSize + 1) / 2);
        var roomRadix = ((MaxSize - MinSize) / 2) + 1;
        
        var i = dungeonPrefab.size.x / 2;
        Size.x = Random.Range(0, roomRadix) + roomBase;
        StartPoints.x = Random.Range(0, i - Size.x);
        
        var j = dungeonPrefab.size.z / 2;
        Size.z = Random.Range(0, roomRadix) + roomBase;
        StartPoints.z = Random.Range(0, j - Size.z);
    }

    private void CreateCells()
    {
        cells = new MazeCell[Size.x,Size.z];
        for (int i = StartPoints.x; i <StartPoints.x+ Size.x; i++)
        {
            for (int j = StartPoints.z; j <StartPoints.z+ Size.z; j++)
            {
                CreateCell(new IntVector2(i, j));
            }
        }
    }

    private void CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x-StartPoints.x, coordinates.z-StartPoints.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coordinates.x - Size.x * 0.5f + 0.5f, 0f, coordinates.z - Size.z * 0.5f + 0.5f);
    }
}
