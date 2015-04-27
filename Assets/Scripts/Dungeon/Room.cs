using UnityEngine;
using System.Collections;

/// <summary>
/// class should handle a Room and its Generation
/// </summary>
public class Room : MonoBehaviour
{

    public int MaxSize;
    public int MinSize;
    /// <summary>
    /// depth and width of the room
    /// </summary>
    public IntVector2 Size;

    public IntVector2 StartPoints;

    public MazeWall wallPrefab;
    public MazeCell cellPrefab;
    public DungeonMap dungeonPrefab;

    public MazeCell[,] cells;

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
        cells = new MazeCell[Size.x, Size.z];
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.z; j++)
            {
                //TODO: that goes simpler somehow
                var cell =
                    CreateCell(new IntVector2(i, j));
                if (j == 0)
                {
                    var direction = MazeDirection.North;
                    CreateWall(cell, null, direction.GetOpposite());
                    Debug.Log(string.Format("j: {0} EndJ: {1}", j, StartPoints.z));
                }
                if (j == Size.z - 1)
                {
                    var direction = MazeDirection.South;
                    CreateWall(cell, null, direction.GetOpposite());
                    //Debug.Log(string.Format("j: {0} EndJ: {1}", j, StartPoints.z));
                }
                if (i == 0)
                {
                    var direction = MazeDirection.East;
                    CreateWall(cell, null, direction.GetOpposite());
                    //Debug.Log(string.Format("i: {0} EndI: {1}", i, StartPoints.x));
                }
                if (i == Size.x - 1)
                {
                    var direction = MazeDirection.West;
                    CreateWall(cell, null, direction.GetOpposite());
                    //Debug.Log(string.Format("i: {0} EndI: {1}", i, StartPoints.x));
                }
            }
        }
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition =
            new Vector3(coordinates.x + StartPoints.x - dungeonPrefab.size.x * 0.5f + 0.5f, 0f, coordinates.z + StartPoints.z - dungeonPrefab.size.z * 0.5f + 0.5f);
        return newCell;
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }
}
