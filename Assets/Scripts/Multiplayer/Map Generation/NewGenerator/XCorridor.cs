using System;

public class XCorridor {
    private XCell _topLeftVertex;
    private XCell _topRightVertex;
    private XCell _botLeftVertex;
    private XCell _botRightVertex;
    private XGrid _size;
    private Orientation _orientation;

    public enum Orientation {
        horizontal,
        vertical
    }

    public XCorridor(XCell topLeftVertex, XGrid size, Orientation orientation) {
        _topLeftVertex = topLeftVertex;
        _topRightVertex = topLeftVertex.plus(size.columnsOnly());
        _botLeftVertex = topLeftVertex.plus(size.rowsOnly());
        _botRightVertex = topLeftVertex.plus(size);
        _size = size;
        _orientation = orientation;
    }

    public void printVertex() {
        Console.Write(_topLeftVertex);
        Console.Write(" TOP ");
        Console.WriteLine(_topRightVertex);
        Console.Write(_botLeftVertex);
        Console.Write(" BOT ");
        Console.WriteLine(_botRightVertex);
    }

    public void plotOn(int[,] map) {
        for (int row = 0; row < _size.rows(); row++) {
            for (int col = 0; col < _size.columns(); col++) {
                //test is in range
                int rowPos = _topLeftVertex._x + row;
                int colPos = _topLeftVertex._y + col;

                XCell pos = new XCell(rowPos, colPos);
                if (pos.isEqual(_topLeftVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_NW;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_NE;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_SE;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_SW;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex) 
                    && _orientation == Orientation.horizontal) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex)
                    && _orientation == Orientation.vertical) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex) 
                    && _orientation == Orientation.horizontal) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex)
                    && _orientation == Orientation.vertical) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_W;
                } else {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Floor;
                }
            }
        }
    }
}