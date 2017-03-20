using System;
using System.Collections.Generic;
using DungeonGeneration.Generator.Domain;

public abstract class APolyShape : IXShape {

    private OIGrid _grid;
    private Cell _topLeftVertex;

    public APolyShape(Cell topLeftVertex, OIGrid cells) {
        _topLeftVertex = topLeftVertex;
        _grid = cells;
    }

    public Cell topLeftVertex() {
        return _topLeftVertex;
    }

    public void accept(IShapeVisitor visitor) {
        visitor.visit(this);
    }

    public OIGrid grid() {
        return _grid;
    }

    public Cell topRightVertex() {
        return _topLeftVertex.plusSize(0, _grid.columns());
    }

    public void forEachCell(Action<int, int, IXShape> doFunct) {
        for (int x = 0; x < _grid.rows(); x++) {
            for (int y = 0; y < _grid.columns(); y++) {
                if (isCellValid(x, y)) doFunct(x, y, this);
            }
        }
    }

    public void forEachCellAbs(Action<int, int, int> doFunct) {
        for (int x = 0; x < _grid.rows(); x++) {
            for (int y = 0; y < _grid.columns(); y++) {
                if (isCellValid(x, y)) {
                    int vX = topLeftVertex().row();
                    int vY = topLeftVertex().col();
                    int absX = vX + x;
                    int absY = vY + y;
                    doFunct(absX, absY, _grid.valueForCell(x, y));
                }
            }
        }
    }

    public Cell bottomLeftVertex() {
        return _topLeftVertex.plusSize(_grid.rows(), 0);
    }

    public Cell bottomRightVertex() {
        return _topLeftVertex.plusSize(_grid.rows(), _grid.columns());
    }

    public void setCellValue(int cellX, int cellY, int v) {
        _grid.setCellValue(cellX, cellY, v);
    }

    public int getCellValue(int cellX, int cellY) {
        return _grid.valueForCell(cellX, cellY);
    }

    public abstract bool isCellValid(int x, int y); 

    public bool hasCellValue(int x, int y, int value) {
        return _grid.hasCellValue(x, y, value);
    }

    public CellPair shortestCellPair(IXShape aShape) {
        List<Cell> myEdge = edge();
        List<Cell> yourEdge = aShape.edge();

        double shortestPath = 0;
        Cell cell1Sel = null;
        Cell cell2Sel = null;
        bool firstTime = true;

        foreach (Cell myCell in myEdge) {
            Cell myCellAbs = myCell.plus(topLeftVertex());
            foreach (Cell yourCell in yourEdge) {
                Cell yourCellAbs = yourCell.plus(aShape.topLeftVertex());
                double magn = myCellAbs.magnetude(yourCellAbs);
                if (firstTime) {
                    shortestPath = magn;
                    firstTime = false;
                    cell1Sel = myCellAbs;
                    cell2Sel = yourCellAbs;
                } else if (magn < shortestPath) {
                    shortestPath = magn;
                    cell1Sel = myCellAbs;
                    cell2Sel = yourCellAbs;
                }
            }
        }

        if (firstTime) return null;
        return new CellPair(cell1Sel, cell2Sel);
    }
    public List<Cell> edge() {
        List<Cell> result = new List<Cell>();
        forEachCell((x, y, shape) => {
            if (shape.hasCellValue(x, y, XTile.FLOOR)) {
                for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++) {
                    for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++) {
                        if (shape.isCellValid(neighbourX, neighbourY)) {
                            if (neighbourX != x || neighbourY != y) {
                                if (shape.hasCellValue(neighbourX, neighbourY, XTile.WALL)) {
                                    Cell cell = new Cell(x, y);
                                    if (!result.Contains(cell)) result.Add(cell);
                                }
                            }
                        } else {
                            Cell cell = new Cell(x, y);
                            if (!result.Contains(cell)) result.Add(cell);
                        }
                    }
                }
            }
        });
        return result;
    }

    public int regionsNumber() {
        return regionsWithValue(XTile.FLOOR).Count;
    }


    private List<List<Cell>> regionsWithValue(int value) {
        List<List<Cell>> regions = new List<List<Cell>>();
        int rows = grid().rows();
        int columns = grid().columns();

        int[,] mapFlags = new int[rows, columns];

        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < columns; y++) {
                if (mapFlags[x, y] == 0 && hasCellValue(x, y, value)) {
                    List<Cell> newRegion = regionFrom(x, y);
                    regions.Add(newRegion);

                    foreach (Cell tile in newRegion) {
                        mapFlags[tile.row(), tile.col()] = 1;
                    }
                }
            }
        }
        return regions;
    }

    public void deleteRegionsButTheBiggest() {
        List<List<Cell>> regions = regionsWithValue(XTile.FLOOR);
        if (regions.Count == 0) return;
        if (regions.Count == 1) return;
        List<Cell> biggest = null;
        List<Cell> toDelete = new List<Cell>();
        foreach (List<Cell> each in regions) {
            if (biggest == null) {
                biggest = each;
            } else if (each.Count > biggest.Count) {
                toDelete.AddRange(biggest);
                biggest = each;
            } else {
                toDelete.AddRange(each);
            }
        }

        foreach (Cell each in toDelete) {
            setCellValue(each.row(), each.col(), XTile.WALL);
        }
    }

    public bool hasRegions() {
        return regionsNumber() > 0;        
    }

    private List<Cell> regionFrom(int startX, int startY) {
        List<Cell> tiles = new List<Cell>();
        int rows = grid().rows();
        int columns = grid().columns();

        int[,] mapFlags = new int[rows, columns];
        int tileType = getCellValue(startX, startY);

        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(new Cell(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Cell tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.row() - 1; x <= tile.row() + 1; x++) {
                for (int y = tile.col() - 1; y <= tile.col() + 1; y++) {
                    if (isCellValid(x, y) && (y == tile.col() || x == tile.row())) {
                        if (mapFlags[x, y] == 0 && hasCellValue(x, y, tileType)) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Cell(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    public bool isWithin(OIGrid container) {
        return grid().isWithin(container, topLeftVertex());
    }

    public bool collidesWith(IXShape other) {
        Cell[] cells = _topLeftVertex.cells(bottomRightVertex());
        foreach (Cell each in cells) {
            if (other.containsCell(each)) return true;
        }
        return false;
    }

    public bool containsCell(Cell aCell) {
        return aCell.isWithin(topLeftVertex(), bottomRightVertex());
    }

}