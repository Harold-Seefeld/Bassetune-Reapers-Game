using NUnit.Framework;

namespace DungeonGeneration.Generator.Domain {
    public class GridTest {
        [Test]
        public void vertex_checkAbsoluteVertexes() {
            Grid container = new Grid(100, 100);
            Cell absTopLeftVertex = new Cell(46, 62);

            Assert.AreEqual(new Cell(46, 161), container.absTopRightVertexUsing(absTopLeftVertex));
            Assert.AreEqual(new Cell(145, 161), container.absBotRightVertexUsing(absTopLeftVertex));
            Assert.AreEqual(new Cell(145, 62), container.absBotLeftVertexUsing(absTopLeftVertex));
        }

        [Test]
        public void isWithin_gridWithinBounds_case1() {
            Grid containee = new Grid(4, 4);
            Grid container = new Grid(4, 4);
            Assert.IsTrue(containee.isWithin(container, new Cell(0, 0)));
        }

        [Test]
        public void isWithin_gridWithinBounds_case2() {
            Grid containee = new Grid(14, 14);
            Grid container = new Grid(100, 100);
            Assert.IsTrue(containee.isWithin(container, new Cell(46, 62)));
        }

        [Test]
        public void isWithin_gridOutsideBounds() {
            Grid containee = new Grid(4, 4);
            Grid container = new Grid(4, 4);
            Assert.IsFalse(containee.isWithin(container, new Cell(-1, 0)));
        }

        [Test]
        public void checkIfCellExistInGrid() {
            Grid container = new Grid(4, 4);
            Assert.IsTrue(container.hasCell(0, 0));
            Assert.IsFalse(container.hasCell(-1, 0));
        }
    }
}