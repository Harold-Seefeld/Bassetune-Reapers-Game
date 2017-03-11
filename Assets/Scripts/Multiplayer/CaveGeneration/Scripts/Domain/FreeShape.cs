using System;
using System.Collections.Generic;
using DungeonGeneration.Generator.Domain;

public class FreeShape : IXShape {
    private List<Cell> _cells;

    public FreeShape() {
        _cells = new List<Cell>();
    }

    public void add(List<Cell> vCells) {
        foreach(Cell each in vCells) {
            add(each);
        }
    }

    public void add(Cell each) {
        if (_cells.Contains(each)) return;
        _cells.Add(each);
    }

    public void accept(IShapeVisitor visitor) {
        visitor.visit(this);
    }


    public Cell bottomLeftVertex() {
        throw new NotImplementedException();
    }

    public List<Cell> edge() {
        throw new NotImplementedException();
    }

    public void forEachCell(Action<int, int, IXShape> doFunct) {
        foreach(Cell each in _cells) {
            doFunct(each.row(), each.col(), this);
        }
    }

    public void forEachCellAbs(Action<int, int, int> doFunct) {
        foreach (Cell each in _cells) {
            doFunct(each.row(), each.col(), XTile.FLOOR);
        }
    }

    public bool hasCellValue(int neighbourX, int neighbourY, int v) {
        throw new NotImplementedException();
    }

    public bool isCellValid(int neighbourX, int neighbourY) {
        throw new NotImplementedException();
    }

    public OIGrid grid() {
        throw new NotImplementedException();
    }

    public void setCellValue(int cellX, int cellY, int v) {
        throw new NotImplementedException();
    }

    public CellPair shortestCellPair(IXShape other) {
        throw new NotImplementedException();
    }

    public Cell topLeftVertex() {
        throw new NotImplementedException();
    }

    public Cell topRightVertex() {
        throw new NotImplementedException();
    }

    public bool isWithin(OIGrid grid) {
        throw new NotImplementedException();
        //TODO: COME FACCIO>
    }

    public bool collidesWith(IXShape each) {
        throw new NotImplementedException();
    }

    public bool containsCell(Cell each) {
        throw new NotImplementedException();
    }
}