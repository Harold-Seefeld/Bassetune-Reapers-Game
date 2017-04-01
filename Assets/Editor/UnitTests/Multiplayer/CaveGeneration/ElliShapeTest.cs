using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using DungeonGeneration.Generator.Domain;

public class ElliShapeTest {

    [Test]
    public void cellValidity_withinCircle() {
        ElliShape shape = new ElliShape(new Cell(0, 0), new OIGrid(10, 10));
        Assert.IsTrue(shape.isCellValid(5, 5));
        Assert.IsTrue(shape.isCellValid(5, 9));
        Assert.IsTrue(shape.isCellValid(0, 5));

        Assert.IsFalse(shape.isCellValid(5, 10));
        Assert.IsFalse(shape.isCellValid(9, 9));
        Assert.IsFalse(shape.isCellValid(10, 10));
    }

    [Test]
    public void cellValidity_withinEllipse() {
        ElliShape shape = new ElliShape(new Cell(0, 0), new OIGrid(3, 5));
        Assert.IsTrue(shape.isCellValid(1, 1));
        Assert.IsTrue(shape.isCellValid(1, 4));
    }

    [Test]
    public void deleteRegionsButTheBiggest_Ellibug() {
        ElliShape shape = new ElliShape(new Cell(0, 0), new OIGrid(10, 10));
        shape.setCellValue(5, 9, XTile.FLOOR);
        Assert.AreEqual(1, shape.regionsNumber());

        shape.deleteRegionsButTheBiggest();
        Assert.AreEqual(1, shape.regionsNumber());
    }

}
