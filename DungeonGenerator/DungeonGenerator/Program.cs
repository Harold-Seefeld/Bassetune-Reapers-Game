using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class Program
    {
        public static void Main(string[] args) {
            var dungeon = new Dungeon();
            dungeon.Create();
            dungeon.PrintMap();
            Console.ReadLine();
        }
    }
}
