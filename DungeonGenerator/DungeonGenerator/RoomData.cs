using System.Collections;
using System;
using System.Collections.Generic;
using DungeonGenerator;

public class RoomData
{
    public int Id { get; set; }
    public int North { get; set; }
    public int South { get; set; }
    public int West { get; set; }
    public int East { get; set; }
    public int Height { get { return (South - North + 1) * 10; } }
    public int Width { get { return (East - West + 1) * 10; } }
    public int Area { get { return Height * Width}  }
    public Dictionary<Directions, List<Door>> Door = new Dictionary<Directions, List<Door>>();

    public void AddDoor(Door door, Directions direction)
    {
        if (!Door.ContainsKey(direction))
        {
            Door.Add(direction, new List<Door>());
        }
        Door[direction].Add(door);
    }
}











