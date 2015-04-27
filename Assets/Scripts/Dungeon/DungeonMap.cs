using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Class handles the DungeonMap and its Contents
/// </summary>
public class DungeonMap : MonoBehaviour
{

    public Room roomPrefab;
    public float generationStepDelay;
    public IntVector2 size;
    private MazeCell[,] cells;
    private List<Room> rooms;

    /// <summary>
    /// Generates a DungeonMap
    /// </summary>
    /// <returns></returns>
    public IEnumerator Generate()
    {
        WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
        cells = new MazeCell[size.x, size.z];
        rooms = new List<Room>();

        var dungeonArea = size.x * size.z;
        var roomArea = roomPrefab.MaxSize * roomPrefab.MaxSize;
        var maxRooms = dungeonArea / roomArea;

        for (int i = 0; i < maxRooms; i++)
        {
            var room = Instantiate(roomPrefab) as Room;
            room.Generate();
            room.transform.parent = transform;
            room.name = "RoomId: " + (rooms.Count + 1);
            if (SoundRoom(room))
            {
                rooms.Add(room);
            }
            else
            {

                Destroy(room.gameObject);
            }

            yield return delay;
        }

    }

    private bool SoundRoom(Room room)
    {
        for (int i = 0; i < room.Size.x; i++)
        {
            for (int j = 0; j < room.Size.z; j++)
            {
                if (cells[i + room.StartPoints.x, j + room.StartPoints.z] != null)
                {
                    Debug.Log("Room hit");
                    return false;
                }
                cells[i + room.StartPoints.x, j + room.StartPoints.z] = room.cells[i, j];
            }
        }
        return true;
    }
}
