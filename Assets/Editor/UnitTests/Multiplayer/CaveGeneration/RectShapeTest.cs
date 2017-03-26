using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using DungeonGeneration.Generator.Domain;

public class RectShapeTest { 

	[Test]
    public void countingFloorRegions() {
        RectShape shape = new RectShape(new Cell(0,0), new OIGrid(10, 10));
        Assert.AreEqual(0, shape.regionsNumber());

        shape.setCellValue(1, 1, XTile.FLOOR);
        Assert.AreEqual(1, shape.regionsNumber());

        shape.setCellValue(3, 1, XTile.FLOOR);
        shape.setCellValue(3, 2, XTile.FLOOR);
        shape.setCellValue(3, 3, XTile.FLOOR);
        Assert.AreEqual(2, shape.regionsNumber());
    }

    [Test]
    public void deleteRegionsButTheBiggest_withOneRegion() {
        RectShape shape = new RectShape(new Cell(0, 0), new OIGrid(10, 10));
        //REG 1
        shape.setCellValue(1, 1, XTile.FLOOR);
        Assert.AreEqual(1, shape.regionsNumber());

        shape.deleteRegionsButTheBiggest();
        Assert.AreEqual(1, shape.regionsNumber());

        Assert.IsTrue(shape.hasCellValue(1, 1, XTile.FLOOR));
    }

    [Test]
    public void deleteRegionsButTheBiggest_withTwoRegions() {
        RectShape shape = new RectShape(new Cell(0, 0), new OIGrid(10, 10));
        //REG 1
        shape.setCellValue(1, 1, XTile.FLOOR);
        //REG 2s
        shape.setCellValue(3, 1, XTile.FLOOR);
        shape.setCellValue(3, 2, XTile.FLOOR);
        shape.setCellValue(3, 3, XTile.FLOOR);
        Assert.AreEqual(2, shape.regionsNumber());

        shape.deleteRegionsButTheBiggest();
        Assert.AreEqual(1, shape.regionsNumber());

        Assert.IsTrue(shape.hasCellValue(1, 1, XTile.WALL));
        Assert.IsTrue(shape.hasCellValue(3, 1, XTile.FLOOR));
        Assert.IsTrue(shape.hasCellValue(3, 2, XTile.FLOOR));
        Assert.IsTrue(shape.hasCellValue(3, 3, XTile.FLOOR));
    }

    [Test]
    public void deleteRegionsButTheBiggest_withSixRegionsRoom() {
        RectShape shape = new RectShape(new Cell(0, 0), new OIGrid(40, 40));
        ShapeCellularAutomaton filler = new ShapeCellularAutomaton(48, 50, 5);
        filler.applyOn(shape);

        Assert.AreEqual(6, shape.regionsNumber());
        shape.deleteRegionsButTheBiggest();
        Assert.AreEqual(1, shape.regionsNumber());
    }

    [Test]
    public void deleteRegionsButTheBiggest_bug() {
        RectShape shape = new RectShape(new Cell(0, 0), new OIGrid(10, 10));
        ShapeCellularAutomaton auto = new ShapeCellularAutomaton(1683686970, 58, 5);

        auto.applyOn(shape);
        Assert.AreEqual(1, shape.regionsNumber());

        shape.deleteRegionsButTheBiggest();
        Assert.AreEqual(1, shape.regionsNumber());
    }

}
