using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator.Rooms
{
    public interface IRoomGenerator
    {
        void FillWithRooms(Dungeon dungeon);
        void FixDoors(Dungeon dungeon);

    }

    public class RoomGenerator : IRoomGenerator
    {
        #region Fields
        private List<RoomData> room;
        private Dungeon _dungeon;
        private int connect = 0; //TODO: is that really needed?
        #endregion

        #region Properties
        public Random random { get; set; }

        public RoomLayouts RoomLayout { get; set; }
        /// <summary>
        /// minimum size of a room
        /// </summary>
        public int roomMin { get; set; }
        /// <summary>
        /// Maximum size of a room
        /// </summary>
        public int roomMax { get; set; }

        #endregion

        #region Constructor
        public RoomGenerator()
        {
            RoomLayout = RoomLayouts.Scattered;
            //RoomLayout = RoomLayouts.Packed;
            roomMin = 3;
            roomMax = 9;
        }
        #endregion

        public void FillWithRooms(Dungeon dungeon)
        {
            _dungeon = dungeon;
            room = new List<RoomData>();
            InitCells();
            EmplaceRooms();
            OpenRooms();
            LabelRooms();
        }

        public void FixDoors(Dungeon dungeon)
        {
            var cell = dungeon.Map;

            //TODO: implement fixing of doors
            foreach (var roomi in room)
            {
                foreach (var dir in roomi.Door.Keys)
                {
                    foreach (var door in roomi.Door[dir])
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

        private void InitCells()
        {
            var Map = _dungeon.Map;
            for (var r = 0; r <= _dungeon.rows; r++)
            {
                for (var c = 0; c <= _dungeon.cols; c++)
                {
                    Map[r, c] = Cells.Nothing;
                }
            }

            RoundMask();
        }

        private void RoundMask()
        {
            var Map = _dungeon.Map;
            var centerR = _dungeon.rows / 2;
            var centerC = _dungeon.cols / 2;
            for (var r = 0; r <= _dungeon.rows; r++)
            {
                for (var c = 0; c <= _dungeon.cols; c++)
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

            switch (RoomLayout)
            {
                case RoomLayouts.Packed:
                    PackRooms();
                    break;
                case RoomLayouts.Scattered:
                    ScatterRooms();
                    break;
                default:
                    break;
            }

        }

        private void PackRooms()
        {
            var cell = _dungeon.Map;
            for (int i = 0; i < _dungeon.rows / 2; i++)
            {
                var r = (i * 2 + 1);
                for (int j = 0; j < _dungeon.cols / 2; j++)
                {
                    var c = (j * 2) + 1;
                    if ((cell[i, j] & Cells.Room) != 0)
                    {
                        continue;
                    }
                    if ((i == 0 || j == 0) && random.Next(2) == 0)
                    {
                        continue;
                    }
                    var proto = new Proto() { i = i, j = j };
                    EmplaceRoom(proto);
                }
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
            var dungeon_area = _dungeon.cols * _dungeon.rows;
            var room_area = roomMax * roomMax;
            var rooms = (int)dungeon_area / room_area;

            return rooms;
        }

        private void EmplaceRoom(Proto proto2 = null)
        {
            if (room.Count == 999)
            {
                return;
            }
            var cell = _dungeon.Map;
            Proto proto = SetRoom(proto2);

            var NorthPerimeter = (proto.i * 2) + 1;
            var WestPerimeter = (proto.j * 2) + 1;
            var SouthPerimeter = ((proto.i + proto.height) * 2) - 1;
            var EastPerimeter = ((proto.j + proto.width) * 2) - 1;

            if (NorthPerimeter < 1 || SouthPerimeter > _dungeon.rows - 1)
            {
                return;
            }
            if (WestPerimeter < 1 || EastPerimeter > _dungeon.cols - 1)
            {
                return;
            }

            Hit hit = SoundRoom(NorthPerimeter, WestPerimeter, SouthPerimeter, EastPerimeter);
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
            //lastRoomId = roomId;

            for (var r = NorthPerimeter; r <= SouthPerimeter; r++)
            {
                for (var c = WestPerimeter; c <= EastPerimeter; c++)
                {
                    if ((cell[r, c] & Cells.Entrance) != 0)
                    {
                        cell[r, c] &= ~Cells.ESpace;
                    }
                    else if ((cell[r, c] & Cells.Perimeter) != 0)
                    {
                        cell[r, c] &= ~Cells.Perimeter;
                    }
                    cell[r, c] |= Cells.Room | (Cells)(roomId << 6);
                }
            }



            var roomData = new RoomData
            {
                Id = roomId,
                North = NorthPerimeter,
                South = SouthPerimeter,
                West = WestPerimeter,
                East = EastPerimeter,
            };

            room.Add(roomData);

            for (var r = NorthPerimeter - 1; r <= SouthPerimeter + 1; r++)
            {
                if ((long)(cell[r, WestPerimeter - 1] & (Cells.Room | Cells.Entrance)) == 0)
                {
                    cell[r, WestPerimeter - 1] |= Cells.Perimeter;
                }
                if ((long)(cell[r, EastPerimeter + 1] & (Cells.Room | Cells.Entrance)) == 0)
                {
                    cell[r, EastPerimeter + 1] |= Cells.Perimeter;
                }
            }
            for (var c = WestPerimeter - 1; c <= EastPerimeter + 1; c++)
            {
                if ((long)(cell[NorthPerimeter - 1, c] & (Cells.Room | Cells.Entrance)) == 0)
                {
                    cell[NorthPerimeter - 1, c] |= Cells.Perimeter;
                }
                if ((long)(cell[SouthPerimeter + 1, c] & (Cells.Room | Cells.Entrance)) == 0)
                {
                    cell[SouthPerimeter + 1, c] |= Cells.Perimeter;
                }
            }

        }

        private Proto SetRoom(Proto proto2 = null)
        {
            var basee = ((roomMin + 1) / 2);
            var radix = ((roomMax - roomMin) / 2) + 1;

            Proto proto;
            if (proto2 != null)
            {
                proto = proto2;
            }
            else
            {
                proto = new Proto();
            }
            if (proto.height == 0)
            {
                if (proto.i != 0)
                {
                    var a = _dungeon.rows / 2 - basee - proto.i;
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
                    var a = _dungeon.cols / 2 - basee - proto.j;
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
                proto.i = random.Next(_dungeon.rows / 2 - proto.height);
            }
            if (proto.j == 0)
            {
                proto.j = random.Next(_dungeon.cols / 2 - proto.width);
            }

            return proto;
        }

        private Hit SoundRoom(int r1, int c1, int r2, int c2)
        {
            var cell = _dungeon.Map;
            Hit hit = new Hit();

            for (var r = r1; r <= r2; r++)
            {
                for (var c = c1; c <= c2; c++)
                {
                    if ((cell[r, c] & Cells.Blocked) != 0)
                    {
                        hit.Blocked = true;
                        return hit;
                    }
                    if ((cell[r, c] & Cells.Room) != 0)
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
            for (var id = 0; id < room.Count; id++)
            {
                OpenRoom(room[id]);
            }
            connect = 0;
        }

        private void OpenRoom(RoomData room)
        {
            var doorSills = DoorSills(room);
            if (doorSills.Count == 0)
            {
                return;
            }
            int opens = AllocOpens(room);
            var cell = _dungeon.Map;
            for (var i = 0; i < opens; i++)
            {
                Cells doorCell;
                Sill sill;
                int doorR;
                int doorC;
                do
                {
                    if (doorSills.Count == 0)
                    {
                        return;
                    }
                    var tobeRemoved = random.Next(doorSills.Count - 1);
                    sill = doorSills[tobeRemoved];
                    doorSills.Remove(sill);
                    doorR = sill.DoorR;
                    doorC = sill.DoorC;
                    doorCell = cell[doorR, doorC];
                } while ((doorCell & Cells.DoorSpace) != 0 && doorSills.Count > 0);

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
                    var r = openR + Helper.di[openDir] * x;
                    var c = openC + Helper.dj[openDir] * x;

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
            var roomW = ((room.East - room.West) / 2) + 1;
            int flumph = (int)Math.Sqrt(roomW * roomH);
            return flumph + random.Next(flumph);
        }

        private List<Sill> DoorSills(RoomData room)
        {
            var list = new List<Sill>();
            var cell = _dungeon.Map;
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
            if (room.South <= _dungeon.rows - 3)
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
            if (room.East >= _dungeon.cols - 3)
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
            return Helper.Shuffle(list);
        }


        private Sill CheckSill(Cells[,] cell, RoomData room, int sillR, int sillC, Directions dir)
        {
            var result = new Sill();
            
            var doorR = sillR + Helper.di[dir];
            var doorC = sillC + Helper.dj[dir];
            var doorCell = cell[doorR, doorC];
            if ((doorCell & Cells.Perimeter) == 0)
            {
                return null;
            }
            if ((doorCell & Cells.BlockDoor) != 0)
            {
                return null;
            }
            var outR = doorR + Helper.di[dir];
            var outC = doorC + Helper.di[dir];
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



        private Cells DoorType()
        {
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
            var cell = _dungeon.Map;
            for (var id = 0; id < room.Count; id++)
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

    }
}
