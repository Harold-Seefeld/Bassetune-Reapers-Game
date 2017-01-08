using NUnit.Framework;

public class XCellTest {
    [Test]
    public void checkCoordWithinOthersCoords() {
        Assert.IsTrue(new XCell(0, 0).isWithin(new XCell(0, 0), new XCell(0, 10)));
        Assert.IsTrue(new XCell(0, 5).isWithin(new XCell(0, 0), new XCell(0, 10)));
        Assert.IsFalse(new XCell(1, 5).isWithin(new XCell(0, 0), new XCell(0, 10)));

        Assert.IsTrue(new XCell(4, 5).isWithin(new XCell(0, 0), new XCell(10, 10)));
        Assert.IsFalse(new XCell(4, 11).isWithin(new XCell(0, 0), new XCell(10, 10)));

        Assert.IsTrue(new XCell(2, 4).isWithin(new XCell(0, 4), new XCell(4, 4)));
    }

    [Test]
    public void checkCellWithinGrid() {
        Assert.IsTrue(new XCell(0,0).isWithin(new XGrid(1,1)));
        Assert.IsFalse(new XCell(-1, 0).isWithin(new XGrid(1, 1)));
    }

    [Test]
    public void addingSizeToCoord() {
        XCell orig = new XCell(0, 0);
        XCell coord_9_0 = orig.plus(new XGrid(10, 1));
        XCell coord_0_9 = orig.plus(new XGrid(1, 10));
        XCell coord_9_9 = orig.plus(new XGrid(10, 10));

        Assert.AreEqual(new XCell(0, 9), coord_0_9);
        Assert.AreEqual(new XCell(9, 0), coord_9_0);
        Assert.AreEqual(new XCell(9, 9), coord_9_9);
    }

    [Test]
    public void distanceBetweenCellsOnSameRow() {
        XCell cell1 = new XCell(0, 0);
        XCell cell2 = new XCell(0, 5);
        Assert.AreEqual(6, cell1.distance(cell2));
        Assert.AreEqual(6, cell2.distance(cell1));
    }

    [Test]
    public void distanceBetweenCellsOnSameColumn() {
        XCell cell1 = new XCell(0, 0);
        XCell cell2 = new XCell(5, 0);
        Assert.AreEqual(6, cell1.distance(cell2));
        Assert.AreEqual(6, cell2.distance(cell1));
    }

    [Test]
    public void distanceBetweenCellsOnDifferentRowAndColumn_case1() {
        XCell cell1 = new XCell(0, 0);
        XCell cell2 = new XCell(1, 1);
        Assert.AreEqual(4, cell1.distance(cell2));
        Assert.AreEqual(4, cell2.distance(cell1));
    }

    [Test]
    public void cellArrayBetweenTwoCells() {
        XCell cell1 = new XCell(0, 0);
        XCell cell2 = new XCell(1, 1);

        XCell[] result = cell1.cells(cell2);
        Assert.AreEqual(4, result.Length);
        Assert.AreEqual(new XCell(0, 0), result[0]);
        Assert.AreEqual(new XCell(0, 1), result[1]);
        Assert.AreEqual(new XCell(1, 0), result[2]);
        Assert.AreEqual(new XCell(1, 1), result[3]);

        result = cell2.cells(cell1);
        Assert.AreEqual(4, result.Length);
        Assert.AreEqual(new XCell(0, 0), result[0]);
        Assert.AreEqual(new XCell(0, 1), result[1]);
        Assert.AreEqual(new XCell(1, 0), result[2]);
        Assert.AreEqual(new XCell(1, 1), result[3]);

    }

    [Test]
    public void distanceBetweenCellsOnDifferentRowAndColumn_case2() {
        XCell cell1 = new XCell(0, 0);
        XCell cell2 = new XCell(2, 2);         
        Assert.AreEqual(9, cell1.distance(cell2));
        Assert.AreEqual(9, cell2.distance(cell1));
    }

}


