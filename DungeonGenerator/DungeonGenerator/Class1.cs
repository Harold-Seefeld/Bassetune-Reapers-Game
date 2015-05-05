using System.Collections;
using System;
using System.Collections.Generic;
using Assets;
using DungeonGenerator;
using System.Linq;

public enum Directions
{
    North,
    South,
    West,
    East
}

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
public class MapStyle
{
    public string Fill = "000000";
    public string Open = "FFFFFF";
    public string OpenGrid = "CCCCCC";
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

    private int rows = 39;
    private int cols = 39;
    private int roomMin = 3;
    private int roomMax = 9;
    private CorridorLayout corridorLayout = CorridorLayout.Bent;
    private int removeDeadEnds = 50; //in %

    private int maxRow;
    private int maxCol;
    public Cells[,] Map { get; set; }
    private int seed;
    private int centerR;
    private int centerC;
    private int i;
    private int j;
    private int lastRoomId;
    private Dictionary<int, RoomData> room;
    private int roomBase;
    private int roomRadix;
    private int connect = 0;
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

    private RoomLayouts RoomLayout = RoomLayouts.Scattered;
    private Random random = new Random();

    public Dungeon()
    {
        Map = new Cells[rows, cols];
        jDirs = dj.Keys.ToList();
        jDirs.Sort();
    }

    public void PrintMap()
    {

        for (int i = 0; i < Map.GetLength(0); i++)
        {
            string result = string.Empty;
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Console.SetCursorPosition(j + 1, i + 1);
                if ((Map[i, j] & Cells.Perimeter) == Cells.Perimeter)
                {
                    Console.Write('P');
                }
                else if ((Map[i, j] & Cells.Entrance) == Cells.Entrance)
                {
                    Console.Write('E');
                }
                else if ((Map[i, j] & Cells.Corridor) == Cells.Corridor)
                {
                    Console.Write('C');
                }

                //result += string.Format("{0:00} ", (long)Map[i, j]);
                //Console.Write(string.Format("{0} ",(long) map[i, j]));
            }

            Console.Write(result + Environment.NewLine);
        }
    }

    public void Create()
    {
        Map = new Cells[rows + 1, cols + 1];
        i = rows / 2;
        j = cols / 2;
        maxRow = rows - 1;
        maxCol = cols - 1;

        roomBase = ((roomMin + 1) / 2);
        roomRadix = ((roomMax - roomMin) / 2) + 1;

        room = new Dictionary<int, RoomData>();
        InitCells();
        EmplaceRooms();
        OpenRooms();
        LabelRooms();
        Corridors();
        //TODO: EmplaceStairs();
        CleanDungeon();
    }


    private void InitCells()
    {

        for (var r = 0; r <= rows; r++)
        {

            for (var c = 0; c <= cols; c++)
            {
                Map[r, c] = Cells.Nothing;
            }
        }

        System.Random rnd = new System.Random();
        seed = rnd.Next();

        RoundMask();
    }

    private void RoundMask()
    {
        centerR = rows / 2;
        centerC = cols / 2;
        for (var r = 0; r <= rows; r++)
        {
            for (var c = 0; c <= cols; c++)
            {
                var d = Math.Sqrt(Math.Pow(r - centerR, 2) + (Math.Pow(c - centerC, 2)));
                if (d > centerC)
                {
                    Map[r, c] = Cells.Blocked;
                }
            }
        }
    }

    private void EmplaceRooms()
    {
        if (RoomLayout == RoomLayouts.Packed)
        {
            //			PackRooms();
        }
        else
        {
            ScatterRooms();
        }
    }

    private void ScatterRooms()
    {
        var rooms = AllocRooms();
        for (var i = 0; i < rooms; i++)
        {
            EmplaceRoom();
        }
        Console.WriteLine("Number of Rooms Created: {0}", room.Count);
    }

    private int AllocRooms()
    {
        var dungeon_area = cols * rows;
        var room_area = roomMax * roomMax;
        var rooms = (int)dungeon_area / room_area;

        return rooms;
    }

    private void EmplaceRoom()
    {
        if (room.Count == 999)
        {
            return;
        }
        var cell = Map;
        Proto proto = SetRoom();

        var r1 = (proto.i * 2) + 1;
        var c1 = (proto.j * 2) + 1;
        var r2 = ((proto.i + proto.height) * 2) - 1;
        var c2 = ((proto.j + proto.width) * 2) - 1;

        if (r1 < 1 || r2 > maxRow)
        {
            return;
        }
        if (c1 < 1 || c2 > maxCol)
        {
            return;
        }

        Hit hit = SoundRoom(r1, c1, r2, c2);
        if (hit.Blocked)
        {
            return;
        }

        int roomId;
        if (hit.Count != 0)
        {
            return;
        }

        roomId = room.Count + 1;
        lastRoomId = roomId;

        for (var r = r1; r <= r2; r++)
        {
            for (var c = c1; c <= c2; c++)
            {
                if ((cell[r, c] & Cells.Entrance) == Cells.Entrance)
                {
                    cell[r, c] &= ~Cells.ESpace;
                }
                else if ((cell[r, c] & Cells.Perimeter) == Cells.Perimeter)
                {
                    cell[r, c] &= ~Cells.Perimeter;
                }
                cell[r, c] |= Cells.Room | (Cells)(roomId << 6);
            }
        }



        var roomData = new RoomData
        {
            Id = roomId,
            North = r1,
            South = r2,
            West = c1,
            East = c2,
        };

        room.Add(roomId, roomData);

        for (var r = r1 - 1; r <= r2 + 1; r++)
        {
            if (!((long)(cell[r, c1 - 1] & (Cells.Room | Cells.Entrance)) != 0))
            {
                cell[r, c1 - 1] |= Cells.Perimeter;
            }
            if (!((long)(cell[r, c2 + 1] & (Cells.Room | Cells.Entrance)) != 0))
            {
                cell[r, c2 + 1] |= Cells.Perimeter;
            }
        }
        for (var c = c1 - 1; c <= c2 + 1; c++)
        {
            if (!((long)(cell[r1 - 1, c] & (Cells.Room | Cells.Entrance)) != 0))
            {
                cell[r1 - 1, c] |= Cells.Perimeter;
            }
            if (!((long)(cell[r2 + 1, c] & (Cells.Room | Cells.Entrance)) != 0))
            {
                cell[r2 + 1, c] |= Cells.Perimeter;
            }
        }

    }

    private Proto SetRoom()
    {
        var basee = roomBase;
        var radix = roomRadix;
        Proto proto = new Proto();
        if (proto.height == 0)
        {
            if (proto.i != 0)
            {
                var a = i - basee - proto.i;
                if (a < 0)
                {
                    a = 0;
                }
                var r = (a < radix) ? a : radix;

                proto.height = random.Next(r) + basee;
            }
            else
            {
                proto.height = random.Next(radix) + basee;
            }
        }
        if (proto.width == 0)
        {
            if (proto.j != 0)
            {
                var a = j - basee - proto.j;
                if (a < 0)
                {
                    a = 0;
                }
                var r = (a < radix) ? a : radix;

                proto.width = random.Next(r) + basee;
            }
            else
            {
                proto.width = random.Next(radix) + basee;
            }
        }

        if (proto.i == 0)
        {
            proto.i = random.Next(i - proto.height);
        }
        if (proto.j == 0)
        {
            proto.j = random.Next(j - proto.width);
        }

        return proto;
    }

    private Hit SoundRoom(int r1, int c1, int r2, int c2)
    {
        var cell = Map;
        Hit hit = new Hit();

        for (var r = r1; r <= r2; r++)
        {
            for (var c = c1; c <= c2; c++)
            {
                if ((cell[r, c] & Cells.Blocked) == Cells.Blocked)
                {
                    hit.Blocked = true;
                    return hit;
                }
                if ((cell[r, c] & Cells.Room) == Cells.Room)
                {
                    var id = (int)(cell[r, c] & Cells.RoomId) >> 6;
                    hit.Count += 1;
                }
            }
        }

        return hit;
    }

    private void OpenRooms()
    {
        for (var id = 1; id <= room.Count; id++)
        {
            OpenRoom(room[id]);
        }
        connect = 0;
    }

    private void OpenRoom(RoomData room)
    {
        var list = DoorSills(room);
        if (list == null)
        {
            return;
        }
        int opens = AllocOpens(room);
        var cell = Map;
        for (var i = 0; i < opens; i++)
        {
            Cells doorCell;
            Sill sill;
            int doorR;
            int doorC;
            do
            {
                var tobeRemoved = random.Next(list.Count - 1);
                sill = list[tobeRemoved];
                list.Remove(sill);
                doorR = sill.DoorR;
                doorC = sill.DoorC;
                doorCell = cell[doorR, doorC];
            } while ((doorCell & Cells.DoorSpace) == Cells.DoorSpace);

            var outId = sill.OutId;
            if (outId != 0)  //TODO: correctly implemented
            {
                connect++;
            }

            var openR = sill.SillR;
            var openC = sill.SillC;
            var openDir = sill.Dir;

            //open door
            for (var x = 0; x < 3; x++)
            {
                var r = openR + di[openDir] * x;
                var c = openC + dj[openDir] * x;

                cell[r, c] &= ~Cells.Perimeter;
                cell[r, c] |= Cells.Entrance;
            }
            var doorType = DoorType();
            Door door = new Door()
            {
                Row = doorR,
                Col = doorC,
            };
            cell[doorR, doorC] |= doorType;
            door.Key = doorType;
            switch (doorType)
            {
                case Cells.Arch:
                    door.Type = "Archway";
                    break;
                case Cells.Door:
                    cell[doorR, doorC] |= (Cells)('o' << 24);
                    door.Type = "Unlocked Door";
                    break;
                case Cells.Locked:
                    cell[doorR, doorC] |= (Cells)('x' << 24);
                    door.Type = "Locked Door";
                    break;
                case Cells.Trapped:
                    cell[doorR, doorC] |= (Cells)('t' << 24);
                    door.Type = "Trapped Door";
                    break;
                case Cells.Secret:
                    cell[doorR, doorC] |= (Cells)('s' << 24);
                    door.Type = "Secret Door";
                    break;
                case Cells.Portc:
                    cell[doorR, doorC] |= (Cells)('#' << 24);
                    door.Type = "Portcullis";
                    break;
                default:
                    break;
            }
            door.OutId = outId;
            room.AddDoor(door, openDir);
        }
    }



    public int AllocOpens(RoomData room)
    {
        var roomH = ((room.South - room.North) / 2) + 1;
        //var random = new System.Random();
        var roomW = ((room.East - room.West) / 2) + 1;
        int flumph = (int)Math.Sqrt(roomW * roomH);
        return flumph + random.Next(flumph);
    }

    private List<Sill> DoorSills(RoomData room)
    {
        var list = new List<Sill>();
        var cell = Map;
        if (room.North >= 3)
        {
            for (var c = room.West; c <= room.East; c += 2)
            {
                var sill = CheckSill(cell, room, room.North, c, Directions.North);
                if (sill != null)
                {
                    list.Add(sill);
                }
            }
        }
        if (room.South <= rows - 3)
        {
            for (var c = room.West; c <= room.East; c += 2)
            {
                var sill = CheckSill(cell, room, room.South, c, Directions.South);
                if (sill != null)
                {
                    list.Add(sill);
                }
            }
        }
        if (room.West >= 3)
        {
            for (var c = room.North; c <= room.South; c += 2)
            {
                var sill = CheckSill(cell, room, c, room.West, Directions.West);
                if (sill != null)
                {
                    list.Add(sill);
                }
            }
        }
        if (room.East >= cols - 3)
        {
            for (var c = room.North; c <= room.South; c += 2)
            {
                var sill = CheckSill(cell, room, c, room.East, Directions.East);
                if (sill != null)
                {
                    list.Add(sill);
                }
            }
        }
        return Shuffle(list);
    }


    private Sill CheckSill(Cells[,] cell, RoomData room, int sillR, int sillC, Directions dir)
    {
        var doorR = sillR + di[dir];
        var doorC = sillC + dj[dir];
        var doorCell = cell[doorR, doorC];
        if ((doorCell & Cells.Perimeter) == 0)
        {
            return null;
        }
        if ((doorCell & Cells.BlockDoor) != 0)
        {
            return null;
        }
        var outR = doorR + di[dir];
        var outC = doorC + di[dir];
        var outCell = cell[outR, outC];
        if ((outCell & Cells.Blocked) != 0)
        {
            return null;
        }
        int outId = 0;
        if ((outCell & Cells.Room) != 0)
        {
            outId = (int)((long)(outCell & Cells.RoomId) >> 6);
            if (outId == room.Id)
            {
                return null;
            }
        }

        return new Sill()
        {
            SillR = sillR,
            SillC = sillC,
            Dir = dir,
            DoorR = doorR,
            DoorC = doorC,
            OutId = outId,
        };
    }

    private List<T> Shuffle<T>(List<T> list)
    {

        //return list[index];
        var result = new List<T>(list);
        result.Shuffle();
        return result;
    }

    private Cells DoorType()
    {
        var random = new Random();
        var i = random.Next(110);

        if (i < 15)
        {
            return Cells.Arch;
        }
        else if (i < 60)
        {
            return Cells.Door;
        }
        else if (i < 75)
        {
            return Cells.Locked;
        }
        else if (i < 90)
        {
            return Cells.Trapped;
        }
        else if (i < 100)
        {
            return Cells.Secret;
        }
        return Cells.Portc;
    }

    private void LabelRooms()
    {
        var cell = Map;
        for (var id = 1; id <= room.Count; id++)
        {
            var room1 = room[id];
            var label = room1.Id.ToString();
            var length = label.Length;
            var labelR = (room1.North + room1.South) / 2;
            var labelC = ((room1.West + room1.East - length) / 2) + 1;

            for (var c = 0; c < length; c++)
            {
                var testchar = label[c];
                cell[labelR, labelC + c] |= (Cells)((testchar) << 24);
            }
        }
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
                if ((cell[r, c] & Cells.Corridor) == Cells.Corridor)
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
        var dirs = Shuffle(jDirs);
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
            if ((Map[r + p[0], c + p[1]] & Cells.OpenSpace)!= null)
            {
                return false;
            }
        }

        return true;
    }

    private void FixDoors()
    {
        var cell = Map;

        foreach (var roomi in room)
        {
            foreach (var dir in roomi.Value.Door.Keys)
            {
                foreach (var door in roomi.Value.Door[dir])
                {
                    var doorR = door.Row;
                    var doorC = door.Col;
                    var doorCell = cell[doorR, doorC];
                    if ((doorCell & Cells.OpenSpace) != 0)
                    {
                        continue;
                    }
                }
            }
        }
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




