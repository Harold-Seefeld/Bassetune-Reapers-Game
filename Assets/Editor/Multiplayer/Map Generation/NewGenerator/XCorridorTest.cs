using NUnit.Framework;

public class XCooridorTest {
    [Test]
    public void plotting_oneCorridor3x4Horizontally() {
        XCorridor room = new XCorridor(new XCell(0, 0), new XGrid(3, 4), XCorridor.Orientation.horizontal);

        int[,] result = new int[3, 4];
        room.plotOn(result);

        int[,] expected = new int[3, 4] { { 13, 2, 2, 12},
                                            { 1, 1, 1, 1},
                                            { 10, 4, 4, 11}};
            
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void plotting_oneCorridor4x3Vertically() {
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(4, 3), XCorridor.Orientation.vertical);

        int[,] result = new int[4, 3];
        corr.plotOn(result);

        int[,] expected = new int[4, 3] { { 11, 1, 10},
                                            { 5, 1, 3},
                                            { 5, 1, 3},
                                            { 12, 1, 13}
        };
        Assert.IsTrue(XTestUtils.areEquals(expected, result));
    }

    [Test]
    public void isWithin_oneCorridorWithinBounds() {
        XCorridor corr = new XCorridor(new XCell(0, 0), new XGrid(4, 3), XCorridor.Orientation.vertical);
        Assert.IsTrue(corr.isWithin(new XGrid(5, 5)));
    }

    [Test]
    public void isWithin_oneCorridorOutsideBounds() {
        XCorridor corr = new XCorridor(new XCell(-1, 0), new XGrid(4, 3), XCorridor.Orientation.vertical);
        Assert.IsFalse(corr.isWithin(new XGrid(5, 5)));
    }

}
