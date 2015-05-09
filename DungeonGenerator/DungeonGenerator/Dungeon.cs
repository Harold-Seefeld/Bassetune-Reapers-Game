using System.Collections;
using System;
using System.Collections.Generic;
using Assets;
using DungeonGenerator;
using System.Linq;
using DungeonGenerator.Rooms;
using DungeonGenerator.Corridors;

[Flags]
public enum Cells : long
{
    Nothing = 0x0,
    Blocked = 0x1,
    Room = 0x2,
    Corridor = 0x4,
    Perimeter = 0x10,
    Entrance = 0x20,
    RoomId = 0xFFC0,
    Arch = 0x10000,
    Door = 0x20000,
    Locked = 0x40000,
    Trapped = 0x80000,
    Secret = 0x00100000,
    Portc = 0x00200000,
    StairDown = 0x00400000,
    StairUp = 0x00800000,
    Label = 0xFF000000,
    OpenSpace = Room | Corridor,
    DoorSpace = Arch | Door | Locked | Trapped | Secret | Portc,
    ESpace = Entrance | DoorSpace | 0xFF000000,
    Stairs = StairDown | StairUp,

    BockRoom = Blocked | Room,
    BlockCorr = Blocked | Perimeter | Corridor,
    BlockDoor = Blocked | DoorSpace,
}

public class Dungeon
{

    #region Properties
    public Cells[,] Map { get; private set; }
    public IRoomGenerator RoomGen { get; set; }
    public ICorridorGenerator CorrGen { get; set; }
    public Random random { get; set; }
    /// <summary>
    /// Must be an odd number
    /// </summary>
    public int rows { get; set; }
    /// <summary>
    /// Must be an odd number
    /// </summary>
    public int cols { get; set; }
    #endregion

    #region Constructor
    public Dungeon()
    {
        rows = 39;
        cols = 39;
    }
    #endregion

    public void PrintMap()
    {

        for (int i = 0; i < Map.GetLength(0); i++)
        {
            string result = string.Empty;
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Console.SetCursorPosition(j + 1, i + 1);
                if ((Map[i, j] & Cells.Perimeter) != 0)
                {
                    Console.Write('P');
                }
                else if ((Map[i, j] & Cells.DoorSpace) != 0)
                {
                    Console.Write('E');
                }
                else if ((Map[i, j] & Cells.Corridor) != 0)
                {
                    if ((Map[i, j] & Cells.Room) == 0)
                    {
                        Console.Write('C');
                    }
                }
            }

            Console.Write(result + Environment.NewLine);
        }
    }

    public void Create()
    {

        Map = new Cells[rows + 1, cols + 1];
        RoomGen.FillWithRooms(this);
        CorrGen.Corridors(this);
        //TODO: EmplaceStairs();
        CleanDungeon();
    }





    private void CleanDungeon()
    {
        CorrGen.RemoveDeadEnds(this);
        RoomGen.FixDoors(this);
        //EmptyBlocks(); //TODO: dont delete room borders is this correctly implemented?
    }


    private void EmptyBlocks()
    {
        var cell = Map;
        for (var r = 0; r <= rows; r++)
        {
            for (var c = 0; c <= cols; c++)
            {
                if ((cell[r, c] & Cells.Blocked) != 0)
                {
                    cell[r, c] = Cells.Nothing;
                }

            }
        }
    }

}




