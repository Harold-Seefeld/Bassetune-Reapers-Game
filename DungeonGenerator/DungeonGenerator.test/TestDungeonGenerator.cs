using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator.test
{
    [TestFixture]
    public class TestDungeonGenerator
    {
        [Test]
        public void TestSomething()
        {
            var dungeon = new Dungeon();
            dungeon.Create();
            dungeon.PrintMap();
            Console.WriteLine("Finished");
        }
    }
}
