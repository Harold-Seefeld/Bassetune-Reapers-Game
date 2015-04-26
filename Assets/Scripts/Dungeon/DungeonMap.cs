using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Class handles the DungeonMap and its Contents
/// </summary>
public class DungeonMap : MonoBehaviour {

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
            room.name = "RoomId: " + (i + 1);
            rooms.Add(room);
            yield return delay;
        }

    }
}
