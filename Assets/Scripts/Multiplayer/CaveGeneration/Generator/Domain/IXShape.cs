using System;
using System.Collections.Generic;
using DungeonGeneration.Generator.Domain;

public interface IXShape {
    Cell topLeftVertex();
    OIGrid grid();
    Cell topRightVertex();
    Cell bottomLeftVertex();
    void setCellValue(int cellX, int cellY, int v);

    void forEachCell(Action<int, int, IXShape> p);
    void forEachCellAbs(Action<int, int, int> doFunct);
    bool isCellValid(int neighbourX, int neighbourY);
    bool hasCellValue(int neighbourX, int neighbourY, int v);
    bool isWithin(OIGrid grid);
    void accept(IShapeVisitor visitor);
    List<Cell> edge();
    CellPair shortestCellPair(IXShape other);
    bool collidesWith(IXShape each);
    bool containsCell(Cell each);
    void forEachEdgeCellAbs(Action<int, int, int> doFunct);
}