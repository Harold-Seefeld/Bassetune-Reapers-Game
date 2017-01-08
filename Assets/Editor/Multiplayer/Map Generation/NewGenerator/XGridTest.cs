using NUnit.Framework;

public class XGridTest {
    [Test]
    public void vertex_checkAbsoluteVertexes() {
        XGrid container = new XGrid(100, 100);
        XCell absTopLeftVertex = new XCell(46, 62);

        Assert.AreEqual(new XCell(46, 161), container.absTopRightVertexUsing(absTopLeftVertex));
        Assert.AreEqual(new XCell(145, 161), container.absBotRightVertexUsing(absTopLeftVertex));
        Assert.AreEqual(new XCell(145, 62), container.absBotLeftVertexUsing(absTopLeftVertex));
    }

    [Test]
    public void isWithin_gridWithinBounds_case1() {
        XGrid containee = new XGrid(4, 4);
        XGrid container = new XGrid(4, 4);
        Assert.IsTrue(containee.isWithin(container, new XCell(0,0)));
    }

    [Test]
    public void isWithin_gridWithinBounds_case2() {
        XGrid containee = new XGrid(14, 14);
        XGrid container = new XGrid(100, 100);
        Assert.IsTrue(containee.isWithin(container, new XCell(46, 62)));
    }

    [Test]
    public void isWithin_gridOutsideBounds() {
        XGrid containee = new XGrid(4, 4);
        XGrid container = new XGrid(4, 4);
        Assert.IsFalse(containee.isWithin(container, new XCell(-1, 0)));
    }

    [Test]
    public void checkIfCellExistInGrid() {
        XGrid container = new XGrid(4, 4);
        Assert.IsTrue(container.hasCell(0, 0));
        Assert.IsFalse(container.hasCell(-1, 0));
    }
}

