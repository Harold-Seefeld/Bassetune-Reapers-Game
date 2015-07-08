using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator.test
{
    [TestFixture]
    public class RoomVectorsTest
    {

        VectorGenerator _gen;
        Cells[,] _map;
        [SetUp]
        public void Setup() {

            _map = new Cells[5, 5];
            Dungeon test = new Dungeon() { Map = _map };

            _map[1, 0] = Cells.Corridor;
            _map[2, 0] = Cells.Corridor;

            _gen = new VectorGenerator(test);
            
        }

        [Test]
        public void TestCorridorGenerationXAxis()
        {
            //assign

            //act
            List<Vector> result = _gen.GenerateVectors();
            //assert
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Assert.GreaterOrEqual(result.Count, 2);
            Assert.AreEqual(result[0].StartX, 1);
            Assert.AreEqual(result[0].EndX, 3);
            Assert.AreEqual(result[0].StartY, 0);
            Assert.AreEqual(result[0].EndY, 0);
            Assert.AreEqual(result[1].StartX, 1);
            Assert.AreEqual(result[1].EndX, 3);
            Assert.AreEqual(result[1].StartY, 1);
            Assert.AreEqual(result[1].EndY, 1);

        }

        [Test]
        public void TestCorridorGenerationXAxisWithSideCorridor()
        {
            //assign
            _map[3, 0] = Cells.Corridor;
            _map[2, 1] = Cells.Corridor;
            //act
            List<Vector> result = _gen.GenerateVectors();
            //assert
            Assert.GreaterOrEqual(result.Count, 4);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }


    }
}
