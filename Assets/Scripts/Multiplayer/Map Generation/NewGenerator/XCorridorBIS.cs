using System;

public class XCorridorBIS {
    private XCell _topLeftVertex;
    private XCell _topRightVertex;
    private XCell _botLeftVertex;
    private XCell _botRightVertex;
    private XGrid _size;
    private Orientation _orientation;
    private XRoom _sourceRoom;
    private XRoom _destRoom;

    public enum Orientation {
        horizontal,
        vertical
    }

    public XCorridorBIS(XCell topLeftVertex, XGrid size, Orientation orientation) {
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
                    map[rowPos, colPos] = isVertical() ? (int)XGeneratorBehaviour.TileType.Corner_OUT_NW : (int)XGeneratorBehaviour.TileType.Corner_OUT_SE;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)XGeneratorBehaviour.TileType.Corner_OUT_NE : (int)XGeneratorBehaviour.TileType.Corner_OUT_SW;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)XGeneratorBehaviour.TileType.Corner_OUT_SE : (int)XGeneratorBehaviour.TileType.Corner_OUT_NW;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)XGeneratorBehaviour.TileType.Corner_OUT_SW : (int)XGeneratorBehaviour.TileType.Corner_OUT_NE;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex) && isOrizontal()) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex) && isVertical()) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex) && isOrizontal()) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex) && isVertical()) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_W;
                } else {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Floor;
                }
            }

            if (_sourceRoom == null) return;

            if (_sourceRoom.isSharingVertex(_botLeftVertex)) {
                map[_botLeftVertex._x, _botLeftVertex._y] = isVertical() ? (int)XGeneratorBehaviour.TileType.Wall_W : (int)XGeneratorBehaviour.TileType.Wall_S;
            }
            if (_sourceRoom.isSharingVertex(_topRightVertex)) {
                map[_topRightVertex._x, _topRightVertex._y] = isVertical() ? (int)XGeneratorBehaviour.TileType.Wall_E : (int)XGeneratorBehaviour.TileType.Wall_N;
            }
            if (_sourceRoom.isSharingVertex(_topLeftVertex)) {
                map[_topLeftVertex._x, _topLeftVertex._y] = isVertical() ? (int)XGeneratorBehaviour.TileType.Wall_W : (int)XGeneratorBehaviour.TileType.Wall_N;
            }
            if (_sourceRoom.isSharingVertex(_botRightVertex)) {
                map[_botRightVertex._x, _botRightVertex._y] = isVertical() ? (int)XGeneratorBehaviour.TileType.Wall_E : (int)XGeneratorBehaviour.TileType.Wall_S;
            }


            if (_destRoom != null) _destRoom.plotOn(map);
        }
    }

    public void setDestinationRoom(XRoom room) {
        _destRoom = room;
    }

    public void setSourceRoom(XRoom room) {
        _sourceRoom = room;
    }

    private bool isVertical() {
        return _orientation == Orientation.vertical;
    }
    private bool isOrizontal() {
        return _orientation == Orientation.horizontal;
    }

    public bool isSharingVertex(XCell vertex) {
        if (vertex.isEqual(_topLeftVertex)) return true;
        if (vertex.isEqual(_topRightVertex)) return true;
        if (vertex.isEqual(_botLeftVertex)) return true;
        if (vertex.isEqual(_botRightVertex)) return true;
        return false;
    }
}