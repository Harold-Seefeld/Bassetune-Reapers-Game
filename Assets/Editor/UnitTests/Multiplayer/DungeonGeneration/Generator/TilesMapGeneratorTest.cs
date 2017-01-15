using NUnit.Framework;
using System;
using DungeonGeneration.Logging;
namespace DungeonGeneration.Generator {
    public class TilesMapGeneratorTest {
        [Test]
        public void testScenario1() {
            TilesMapGenerator generator = new TilesMapGenerator();
            generator.setMapSize(15, 15);
            generator.setRoomsNumberRange(2, 2);
            generator.setRoomSizeRange(5, 7);
            generator.setCorridorSizeRange(2, 4);
            generator.setSeed(1234567);
            //generator.setLogger(new ConsoleLogger());

            int[,] expected = {  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 6, 0, 2, 2, 0, 7, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 3, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 3, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 3, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 9, 0, 4, 11, 1, 3, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 3, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 13, 2, 0, 7, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 1, 3, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 9, 0, 4, 4, 0, 8, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
            Assert.AreEqual(expected, generator.result());
        }

        [Test]
        public void testScenario2() {
            TilesMapGenerator generator = new TilesMapGenerator();
            generator.setMapSize(15, 15);
            generator.setRoomsNumberRange(2, 2);
            generator.setRoomSizeRange(5, 7);
            generator.setCorridorSizeRange(2, 4);
            generator.setSeed(-1910733923);

            int[,] expected = {  {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 6, 0, 2, 0, 7, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 9, 0, 11, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 6, 0, 12, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 5, 1, 1, 1, 3, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 9, 0, 4, 0, 8, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                 {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}};
            Assert.AreEqual(expected, generator.result());
        }

    }
}
