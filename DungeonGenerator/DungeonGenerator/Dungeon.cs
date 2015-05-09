using System.Collections;
using System;
using System.Collections.Generic;
using Assets;
using DungeonGenerator;
using System.Linq;
using DungeonGenerator.Rooms;
using DungeonGenerator.Corridors;

public class DungeonLayout
{
    public int[,] Box = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };
    public int[,] Cross = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };

}


public class CloseEnd
{
    public int[][] Walled;
    public int[] Close;
    public int[] Recurse;
}

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
    /// <summary>
    /// Must be an odd number
    /// </summary>
    public int rows = 39;
    /// <summary>
    /// Must be an odd number
    /// </summary>
    public int cols = 39;
    

    private int removeDeadEnds = 50; //in %

    public Cells[,] Map { get; set; }
    public int i;
    public int j;


    private Dictionary<Directions, CloseEnd> closeEnd = new Dictionary<Directions, CloseEnd>()
    {
    {Directions.North,new CloseEnd{
        Walled = new int[][]{ 
            new int[]{0,-1},
            new int[]{1,-1},
            new int[]{1,0},
            new int[]{1,1},
            new int[]{0,1}},
        Close = new int[]{0,0},
        Recurse = new int[]{-1,0}
    }},
    {Directions.South,new CloseEnd{
        Walled = new int[][]{new int[]{0,-1},new int[]{-1,-1},new int[]{-1,0},new int[]{-1,1},new int[]{0,1}},
        Close = new int[]{0,0},
        Recurse = new int[]{1,0}
    }},
    {Directions.West,new CloseEnd{
        Walled = new int[][]{new int[]{-1,0},new int[]{-1,1},new int[]{0,1},new int[]{1,1},new int[]{1,0}},
        Close = new int[]{0,0},
        Recurse = new int[]{-1,0}
    }},
    {Directions.East,new CloseEnd{
        Walled = new int[][]{new int[]{-1,0},new int[]{-1,-1},new int[]{0,-1},new int[]{1,-1},new int[]{1,0}},
        Close = new int[]{0,0},
        Recurse = new int[]{0,1}
    }},
    };


    #region Properties
    public IRoomGenerator Generator { get; set; }
    public ICorridorGenerator CorrGen { get; set; }
    public Random random { get; set; }
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
        i = rows / 2;
        j = cols / 2;

        Generator.FillWithRooms(this);
        CorrGen.Corridors(this);
        //TODO: EmplaceStairs();
        CleanDungeon();
    }





    private void CleanDungeon()
    {
        RemoveDeadEnds();
        FixDoors();
        //EmptyBlocks(); //TODO: dont delete room borders is this correctly implemented?
    }
    private void RemoveDeadEnds()
    {
        var p = removeDeadEnds;
        CollapseTunnels(p);
    }

    private void CollapseTunnels(int p)
    {
        if (p == 0)
        {
            return;
        }

        var all = p == 100;
        var cell = Map;
        for (var x = 0; x < i; x++)
        {
            var r = (x * 2) + 1;
            for (var y = 0; y < j; y++)
            {
                var c = (y * 2) + 1;
                if ((cell[r, c] & Cells.OpenSpace) == 0)
                {
                    continue;
                }
                if ((cell[r, c] & Cells.Stairs) != 0)
                {
                    continue;
                }
                if (!all && random.Next(100) >= p)
                {
                    continue;
                }
                Collapse(r, c);
            }
        }
    }

    private void Collapse(int r, int c)
    {
        var cell = Map;
        if ((cell[r, c] & Cells.OpenSpace) == 0)
        {
            return;
        }
        foreach (var dir in closeEnd.Keys)
        {
            if (CheckTunnel(r, c, closeEnd[dir]))
            {
                var p = closeEnd[dir].Close;
                cell[r + p[0], c + p[1]] = Cells.Nothing;
                p = closeEnd[dir].Recurse;
                if (p != null)
                {
                    Collapse(r + p[0], c + p[1]);
                }
            }
        }
    }

    private bool CheckTunnel(int r, int c, CloseEnd check)
    {
        var list = check.Walled;
        foreach (var p in list)
        {
            if ((Map[r + p[0], c + p[1]] & Cells.OpenSpace) != 0)
            {
                return false;
            }
        }

        return true;
    }

    private void FixDoors()
    {
        var cell = Map;

        //TODO: implement fixing of doors
        //foreach (var roomi in room)
        //{
        //    foreach (var dir in roomi.Door.Keys)
        //    {
        //        foreach (var door in roomi.Door[dir])
        //        {
        //            var doorR = door.Row;
        //            var doorC = door.Col;
        //            var doorCell = cell[doorR, doorC];
        //            if ((doorCell & Cells.OpenSpace) != 0)
        //            {
        //                continue;
        //            }
        //        }
        //    }
        //}
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




