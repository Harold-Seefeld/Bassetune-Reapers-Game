using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using DungeonGenerator.Corridors;
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

            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            var dungeon = container.Resolve<Dungeon>();
            do
            {
                Console.Clear();
                
                dungeon.Create();
                dungeon.PrintMap();
            } while ((Console.ReadKey().Key == ConsoleKey.Spacebar));

        }
    }
}
