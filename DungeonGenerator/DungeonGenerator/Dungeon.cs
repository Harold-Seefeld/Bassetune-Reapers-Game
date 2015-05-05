using System.Collections;
using System;
using System.Collections.Generic;
using Assets;
using DungeonGenerator;
using System.Linq;
using DungeonGenerator.Rooms;

public class DungeonLayout
{
    public int[,] Box = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };
    public int[,] Cross = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } };

}
public enum CorridorLayout
{
    Labyrinth = 0,
    Bent = 50,
    Straight = 100,

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
    private Random random = new Random();
    private CorridorLayout corridorLayout = CorridorLayout.Bent;
    private int removeDeadEnds = 50; //in %

    public Cells[,] Map { get; set; }
    public int i;
    public int j;
    
    
    private Dictionary<Directions, int> di = new Dictionary<Directions, int>()
    {
    {Directions.North,-1},
    {Directions.South,1},
    {Directions.West,0},
    {Directions.East,0},
    };
    private Dictionary<Directions, int> dj = new Dictionary<Directions, int>()
    {
    {Directions.North,0},
    {Directions.South,0},
    {Directions.West,-1},
    {Directions.East,1},
    };
    private List<Directions> jDirs;

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

    
//TODO: Constructor should only be used for declaring dependencies! move the other stuff
    public Dungeon()
    {
        //Map = new Cells[rows, cols];
        jDirs = dj.Keys.ToList();
        jDirs.Sort();
        Map = new Cells[rows + 1, cols + 1];
        i = rows / 2;
        j = cols / 2;

    }

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


        Corridors();
        //TODO: EmplaceStairs();
        CleanDungeon();
    }


    

    private void Corridors()
    {
        var cell = Map;
        for (var x = 1; x < i; x++)
        {
            var r = (x * 2) + 1;
            for (var y = 1; y < j; y++)
            {
                var c = (y * 2) + 1;
                if ((cell[r, c] & Cells.Corridor) != 0)
                {
                    continue;
                }
                Tunnel(x, y);
            }
        }
    }

    private void Tunnel(int x, int y, Directions? lastDir = null)
    {
        List<Directions> dirs = TunnelDirs(lastDir);
        foreach (var dir in dirs)
        {
            if (OpenTunnel(x, y, dir))
            {
                var nextX = x + di[dir];
                var nextY = y + dj[dir];
                Tunnel(nextX, nextY, dir);
            }
        }
    }

    private List<Directions> TunnelDirs(Directions? lastDir)
    {
        var p = corridorLayout;
        var dirs = Helper.Shuffle(jDirs);
        if (lastDir != null)
        {
            if (random.Next(100) < (int)p)
            {
                dirs.Insert(0, (Directions)lastDir);
            }
        }
        return dirs;
    }

    private bool OpenTunnel(int x, int y, Directions dir)
    {
        var thisR = x * 2 + 1;
        var thisC = y * 2 + 1;
        var nextR = ((x + di[dir]) * 2) + 1;
        var nextC = ((y + dj[dir]) * 2) + 1;
        var midR = (thisR + nextR) / 2;
        var midC = (thisC + nextC) / 2;

        if (SoundTunnel(midR, midC, nextR, nextC))
        {
            DelveTunnel(thisR, thisC, nextR, nextC);
            return true;
        }
        return false;
    }

    private bool SoundTunnel(int midR, int midC, int nextR, int nextC)
    {
        if (nextR < 0 || nextR > rows)
        {
            return false;
        }
        if (nextC < 0 || nextC > cols)
        {
            return false;
        }

        var cell = Map;
        List<int> rs = new List<int>() { midR, nextR };
        rs.Sort();
        List<int> cs = new List<int>() { midC, nextC };
        cs.Sort();
        for (var r = rs[0]; r <= rs[1]; r++)
        {
            for (var c = cs[0]; c <= cs[1]; c++)
            {
                if ((cell[r, c] & Cells.BlockCorr) != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void DelveTunnel(int thisR, int thisC, int nextR, int nextC)
    {
        var cell = Map;
        List<int> rs = new List<int>() { thisR, nextR };
        rs.Sort();
        List<int> cs = new List<int>() { thisC, nextC };
        cs.Sort();
        for (var r = rs[0]; r <= rs[1]; r++)
        {
            for (var c = cs[0]; c <= cs[1]; c++)
            {
                cell[r, c] &= ~Cells.Entrance;
                cell[r, c] |= Cells.Corridor;

            }
        }
    }

    private void CleanDungeon()
    {
        RemoveDeadEnds();
        FixDoors();
        EmptyBlocks();
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




