using DungeonGenerator.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRoomGenerator Generator = new RoomGenerator();
            do
            {
                Console.Clear();
                var dungeon = new Dungeon();
                Generator.FillWithRooms(dungeon);
                dungeon.Create();
                dungeon.PrintMap();
            } while ((Console.ReadKey().Key == ConsoleKey.Spacebar));

        }
    }
}
