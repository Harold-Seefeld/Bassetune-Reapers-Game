using NUnit.Framework;

namespace DungeonGeneration.Generator.Domain {

    public class BoardTest {
        [Test]
        public void fitting_oneCorridorWithinBounds() {
            Board board = new Board(new Grid(10, 10));
            bool result = board.fitsIn(new Corridor(new Cell(0, 0), new Grid(4, 4), Corridor.Orientation.horizontal));
            Assert.IsTrue(result);
        }

        [Test]
        public void fitting_oneCorridorOutsideBounds() {
            Board board = new Board(new Grid(10, 10));
            bool result = board.fitsIn(new Corridor(new Cell(-1, 0), new Grid(4, 4), Corridor.Orientation.horizontal));
            Assert.IsFalse(result);
        }

        [Test]
        public void fitting_oneRoomWithinBounds() {
            Board board = new Board(new Grid(100, 100));
            bool result = board.fitsIn(new Room(new Cell(70, 66), new Grid(15, 6)));
            Assert.IsTrue(result);
        }

        [Test]
        public void fitting_case1() {
            Board board = new Board(new Grid(100, 100));
            Room room1 = new Room(new Cell(0, 0), new Grid(13, 5));
            Corridor corr1 = new Corridor(new Cell(4, 4), new Grid(3, 5), Corridor.Orientation.horizontal);
            Room room2 = new Room(new Cell(2, 8), new Grid(9, 5));
            Corridor corr2 = new Corridor(new Cell(1, 8), new Grid(2, 3), Corridor.Orientation.vertical);
            board.add(room1);
            board.add(corr1);
            board.add(room2);
            board.add(corr2);

            board.removeLast();

            Corridor corr3 = new Corridor(new Cell(4, 12), new Grid(3, 4), Corridor.Orientation.horizontal);
            Assert.IsTrue(board.fitsIn(corr3));
        }

        [Test]
        public void fitting_case2() {
            Board board = new Board(new Grid(100, 100));
            Corridor corr = new Corridor(new Cell(75, 61), new Grid(3, 6), Corridor.Orientation.horizontal);
            board.add(corr);

            Room room = new Room(new Cell(70, 66), new Grid(15, 6));
            Assert.IsTrue(board.fitsIn(room));
        }


        [Test]
        public void checkSizeAfterRemovingLast() {
            Board board = new Board(new Grid(100, 100));
            Room room1 = new Room(new Cell(0, 0), new Grid(13, 5));
            Corridor corr1 = new Corridor(new Cell(4, 4), new Grid(3, 5), Corridor.Orientation.horizontal);
            Room room2 = new Room(new Cell(2, 8), new Grid(9, 5));
            Corridor corr2 = new Corridor(new Cell(1, 8), new Grid(2, 3), Corridor.Orientation.vertical);
            board.add(room1);
            board.add(corr1);
            board.add(room2);
            board.add(corr2);

            Assert.AreEqual(4, board.numberOfRoomsAndCorridors());
            board.removeLast();

            Assert.AreEqual(3, board.numberOfRoomsAndCorridors());

        }
    }

}