using NUnit.Framework;

public class XRoomTest {
    [Test]
    public void plotting_oneRoom5x5() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 5));

        int[,] result = new int[5, 5];
        room.plotOn(result);

        int[,] expected = new int[5, 5] { { 6, 0, 2, 0, 7},
                                            { 0, 1, 1, 1, 0},
                                            { 5, 1, 1, 1, 3},
                                            { 0, 1, 1, 1, 0},
                                            { 9, 0, 4, 0, 8}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoom4x8() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(4, 8));
        int[,] result = new int[4, 8];
        room.plotOn(result);

        int[,] expected = new int[4, 8] { { 6,0,2,2,2,2,0,7 },
                                            { 0,1,1,1,1,1,1,0},
                                            { 0,1,1,1,1,1,1,0},
                                            { 9,0,4,4,4,4,0,8}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }


    public void plotting_twoRoomsWithoutOverlapping() {
        XRoom room5x5 = new XRoom(new XCell(0, 0), new XGrid(5, 5));
        XRoom room3x3 = new XRoom(new XCell(6, 0), new XGrid(4, 4));

        int[,] result = new int[10, 5];
        room5x5.plotOn(result);
        room3x3.plotOn(result);
        
        int[,] expected = new int[10, 5]{ { 6, 2, 2, 0, 7},
                                            { 0, 1, 1, 1, 0},
                                            { 5, 1, 1, 1, 3},
                                            { 0, 1, 1, 1, 0},
                                            { 9, 0, 4, 0, 8},
                                            { 0, 0, 0, 0, 0},
                                            { 6, 0, 0, 7, 0},
                                            { 0, 1, 1, 0, 0},
                                            { 0, 1, 1, 0, 0},
                                            { 9, 0, 0, 8, 0}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorHorizontalSharingBottomRightRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 4));
        XCorridor corr = new XCorridor(new XCell(2, 3), new XGrid(3, 3), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[5, 6];
        room.plotOn(result);
            
        int[,] expected = new int[5, 6]{ { 6, 0, 0, 7, 0, 0  },
                                            { 0, 1, 1, 0, 0, 0  },
                                            { 5, 1, 1,13, 2, 12 },
                                            { 0, 1, 1, 1, 1, 1  },
                                            { 9, 0, 4, 4, 4, 11 }};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorVerticalSharingBottomRightRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(4, 5));
        XCorridor corr = new XCorridor(new XCell(3, 2), new XGrid(3, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[6, 5];
        room.plotOn(result);

        int[,] expected = new int[6, 5]{ { 6, 0, 2, 0, 7 },
                                            { 0, 1, 1, 1, 0 },
                                            { 0, 1, 1, 1, 3 },
                                            { 9, 0,11, 1, 3 },
                                            { 0, 0, 5, 1, 3 },
                                            { 0, 0, 12, 1,13 }};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorVerticalSharingBottomLeftRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(4, 0), new XGrid(3, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[7, 5];
        room.plotOn(result);

        int[,] expected = new int[7, 5]{ {  6, 0,  2, 0, 7 },
                                            {  0, 1,  1, 1, 0 },
                                            {  5, 1,  1, 1, 3 },
                                            {  5, 1,  1, 1, 0 },
                                            {  5, 1, 10, 0, 8 },
                                            {  5, 1,  3, 0, 0 },
                                            { 12, 1, 13, 0, 0 }
        };
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorHorizontalSharingBottomLeftRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 2), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(2, 0), new XGrid(3, 3), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[5, 7];
        room.plotOn(result);

        int[,] expected = new int[5, 7]{ {0,  0,  6, 0, 2, 0, 7},
                                            {0,  0,  0, 1, 1, 1, 0},
                                            {13, 2, 12, 1, 1, 1, 3},
                                            {1,  1,  1, 1, 1, 1, 0},
                                            {10, 4,  4, 4, 4, 0, 8}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorHorizontalSharingTopLeftRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 4), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(3, 3), XCorridor.Orientation.horizontal);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[5, 9];
        room.plotOn(result);
       
        int[,] expected = new int[5, 9]{ {13, 2, 12, 0, 6, 0, 2, 0, 7},
                                            {1, 1, 1, 0, 0, 1, 1, 1, 0},
                                            {10, 4, 11, 0, 5, 1, 1, 1, 3},
                                            {0, 0, 0, 0, 0, 1, 1, 1, 0},
                                            {0, 0, 0, 0, 9, 0, 4, 0, 8}
        };
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorVerticalSharingTopLeftRoomVertex() {
        XRoom room = new XRoom(new XCell(2, 0), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(3, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[7, 5];
        room.plotOn(result);

        int[,] expected = new int[7, 5]{ {11, 1, 10, 0, 0},
                                            {5, 1, 3, 0, 0},
                                            {5, 1, 13, 0, 7},
                                            {5, 1, 1, 1, 0},
                                            {5, 1, 1, 1, 3},
                                            {0, 1, 1, 1, 0},
                                            {9, 0, 4, 0, 8}
        };
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorVerticalSharingTopRightRoomVertex() {
        XRoom room = new XRoom(new XCell(3, 0), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 2), new XGrid(3, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[8, 5];
        room.plotOn(result);

        int[,] expected = new int[8, 5]{  {0, 0, 11, 1, 10},
                                            {0, 0, 5,  1, 3},
                                            {0, 0, 12, 1, 13},
                                            {6, 0,  2,  0, 7},
                                            {0, 1, 1, 1, 0},
                                            {5, 1, 1, 1, 3},
                                            {0, 1, 1, 1, 0},
                                            {9, 0, 4, 0, 8}
        };
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneCorridorHorizontalSharingTopRightRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 4), new XGrid(3, 3), XCorridor.Orientation.vertical);
        room.setCorridorOutcoming(corr);
        corr.setSourceRoom(room);

        int[,] result = new int[5, 8];
        room.plotOn(result);

        int[,] expected = new int[5, 8]{  {6, 0, 2, 2, 5, 1, 10, 0},
                                            {0, 1, 1, 1, 5, 1, 3, 0},
                                            {5, 1, 1, 1, 12, 1, 13, 0},
                                            {0, 1, 1, 1, 0, 0, 0, 0},
                                            {9, 0, 4, 0, 8, 0, 0, 0}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneRoomAndOneIncomingHorizontalCorridorSharingTopRightRoomVertex() {
        XRoom room = new XRoom(new XCell(0, 2), new XGrid(5, 5));
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(3, 3), XCorridor.Orientation.horizontal);
        room.setCorridorIncoming(corr);
        corr.setDestinationRoom(room);

        int[,] result = new int[5, 7];
        //room.plotOn(result);
        corr.plotOn(result);

        int[,] expected = new int[5, 7]{  {13, 2, 2, 2, 2, 0, 7},
                                            {1, 1, 1, 1, 1, 1, 0},
                                            {10, 4, 11, 1, 1, 1, 3},
                                            {0, 0, 0, 1, 1, 1, 0},
                                            {0, 0, 9, 0, 4, 0, 8}};
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void isWithin_aRoomWithinBounds() {
        XRoom room = new XRoom(new XCell(46, 62), new XGrid(14, 14));
        XGrid container = new XGrid(100, 100);
            
        Assert.IsTrue(room.isWithin(container));
    }

    [Test]
    public void collision_twoRoomsCollideOnCellsWhichAreNotVertexes() {
        XRoom room = new XRoom(new XCell(0, 4), new XGrid(10, 10));
        XRoom room2 = new XRoom(new XCell(3, 0), new XGrid(4, 20));

        Assert.IsTrue(room.collidesWith(room2));
    }

    [Test]
    public void collision_twoRoomsCollideOnASide() {
        XRoom room = new XRoom(new XCell(0, 0), new XGrid(5, 5));
        XRoom room2 = new XRoom(new XCell(0, 4), new XGrid(4, 4));

        Assert.IsTrue(room.collidesWith(room2));
    }
}

