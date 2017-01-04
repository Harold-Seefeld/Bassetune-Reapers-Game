using System;
using System.Collections;
using System.Collections.Generic;

public class XRoom {
    private XGrid _size;
    private XCell _topLeftVertex;
    private XCell _topRightVertex;
    private XCell _botLeftVertex;
    private XCell _botRightVertex;
    private XCorridorBIS _corridor;

    public XRoom(XCell topLeftOrigin, XGrid size) {
        _topLeftVertex = topLeftOrigin;
        _topRightVertex = topLeftOrigin.plus(size.columnsOnly());
        _botLeftVertex = topLeftOrigin.plus(size.rowsOnly());
        _botRightVertex = topLeftOrigin.plus(size);
        _size = size;
    }

    /*
    public void printVertex() {
        Console.Write(_topLeftVertex);
        Console.Write(" TOP ");
        Console.WriteLine(_topRightVertex);
        Console.Write(_botLeftVertex);
        Console.Write(" BOT ");
        Console.WriteLine(_botRightVertex);
    }
    */

    public void plotOn(int[,] map) {      
        for (int row = 0; row < _size.rows(); row++) {
            for (int col = 0; col < _size.columns(); col++) {
                //test is in range
                int rowPos = _topLeftVertex._x + row;
                int colPos = _topLeftVertex._y + col;

                XCell pos = new XCell(rowPos, colPos);
                if (pos.isEqual(_topLeftVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex)) {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Wall_W;
                } else {
                    map[rowPos, colPos] = (int)XGeneratorBehaviour.TileType.Floor;
                }
            }
        }

        int xPosC;
        int yPosC;

        //TOP LEFT VERTEX
        if (_corridor == null || !_corridor.isSharingVertex(_topLeftVertex)) {
            xPosC = _topLeftVertex._x;
            yPosC = _topLeftVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NW;
            map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        //TOP RIGHT VERTEX
        if (_corridor == null || !_corridor.isSharingVertex(_topRightVertex)) {
            xPosC = _topRightVertex._x;
            yPosC = _topRightVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NE;
            map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        //BOTTOM RIGHT VERTEX
        if (_corridor == null || !_corridor.isSharingVertex(_botRightVertex)) {
            xPosC = _botRightVertex._x;
            yPosC = _botRightVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SE;
            map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        if (_corridor == null || !_corridor.isSharingVertex(_botLeftVertex)) { 
            xPosC = _botLeftVertex._x;
            yPosC = _botLeftVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SW;
            map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }


        if (_corridor != null) _corridor.plotOn(map);
    }

    public void setCorridorIncoming(XCorridorBIS corr) {
        
    }

    public bool isSharingVertex(XCell vertex) {
        if (vertex.isEqual(_topLeftVertex)) return true;
        if (vertex.isEqual(_topRightVertex)) return true;
        if (vertex.isEqual(_botLeftVertex)) return true;
        if (vertex.isEqual(_botRightVertex)) return true;
        return false;
    }

    public void setCorridorOutcoming(XCorridorBIS corr) {
        _corridor = corr;
    }
}
