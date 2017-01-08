using NUnit.Framework;


public class BRDungeonBoardTest {
    [Test]
    public void fitting_oneCorridorWithinBounds() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(10, 10));
        bool result = board.fitsIn(new XCorridor(new XCell(0, 0), new XGrid(4, 4), XCorridor.Orientation.horizontal));
        Assert.IsTrue(result);
    }

    [Test]
    public void fitting_oneCorridorOutsideBounds() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(10, 10));
        bool result = board.fitsIn(new XCorridor(new XCell(-1, 0), new XGrid(4, 4), XCorridor.Orientation.horizontal));
        Assert.IsFalse(result);
    }

    [Test]
    public void fitting_oneRoomWithinBounds() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(100, 100));
        bool result = board.fitsIn(new XRoom(new XCell(70, 66), new XGrid(15, 6)));
        Assert.IsTrue(result);
    }

    [Test]
    public void fitting_case1() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(100, 100));
        XRoom room1 = new XRoom(new XCell(0, 0), new XGrid(13, 5));
        XCorridor corr1 = new XCorridor(new XCell(4, 4), new XGrid(3, 5), XCorridor.Orientation.horizontal);
        XRoom room2 = new XRoom(new XCell(2, 8), new XGrid(9, 5));
        XCorridor corr2 = new XCorridor(new XCell(1, 8), new XGrid(2, 3), XCorridor.Orientation.vertical);
        board.add(room1);
        board.add(corr1);
        board.add(room2);
        board.add(corr2);

        board.removeLast();

        XCorridor corr3 = new XCorridor(new XCell(4, 12), new XGrid(3, 4), XCorridor.Orientation.horizontal);
        Assert.IsTrue(board.fitsIn(corr3));
    }

    [Test]
    public void fitting_case2() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(100, 100));
        XCorridor corr = new XCorridor(new XCell(75, 61), new XGrid(3, 6), XCorridor.Orientation.horizontal);
        board.add(corr);

        XRoom room = new XRoom(new XCell(70, 66), new XGrid(15, 6));
        Assert.IsTrue(board.fitsIn(room));
    }


    [Test]
    public void checkSizeAfterRemovingLast() {
        BRDungeonBoard board = new BRDungeonBoard(new XGrid(100, 100));
        XRoom room1 = new XRoom(new XCell(0, 0), new XGrid(13, 5));
        XCorridor corr1 = new XCorridor(new XCell(4, 4), new XGrid(3, 5), XCorridor.Orientation.horizontal);
        XRoom room2 = new XRoom(new XCell(2, 8), new XGrid(9, 5));
        XCorridor corr2 = new XCorridor(new XCell(1, 8), new XGrid(2, 3), XCorridor.Orientation.vertical);
        board.add(room1);
        board.add(corr1);
        board.add(room2);
        board.add(corr2);

        Assert.AreEqual(4, board.numberOfRoomsAndCorridors());
        board.removeLast();

        Assert.AreEqual(3, board.numberOfRoomsAndCorridors());

    }
}

