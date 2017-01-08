using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRoom : IShape {
    private XGrid _grid;
    private XCell _topLeftVertex;
    private XCell _topRightVertex;
    private XCell _botLeftVertex;
    private XCell _botRightVertex;
    private XCorridor _outcomingCorridor;
    private XCorridor _incomingCorridor;

    public XRoom(XCell topLeftOrigin, XGrid size) {
        _topLeftVertex = topLeftOrigin;
        _topRightVertex = topLeftOrigin.plus(size.columnsOnly());
        _botLeftVertex = topLeftOrigin.plus(size.rowsOnly());
        _botRightVertex = topLeftOrigin.plus(size);
        _grid = size;
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
        for (int row = 0; row < _grid.rows(); row++) {
            for (int col = 0; col < _grid.columns(); col++) {
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

       
        joinWithCorridors(map);
        if (_outcomingCorridor != null) _outcomingCorridor.plotOn(map);
        
    }

    private bool existsCorridorSharingVertex(XCell vertex) {
        bool result = false;
        if (_incomingCorridor != null) {
            result = result || _incomingCorridor.isSharingVertex(vertex);
        }
        if (_outcomingCorridor != null) {
            result = result || _outcomingCorridor.isSharingVertex(vertex);
        }
        return result;
    }

    private void joinWithCorridors(int[,] map) {
        int xPosC;
        int yPosC;

        //TOP LEFT VERTEX
        //if (corr == null || !corr.isSharingVertex(_topLeftVertex)) {
        if (!existsCorridorSharingVertex(_topLeftVertex)) {
            xPosC = _topLeftVertex._x;
            yPosC = _topLeftVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NW;
            map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        //TOP RIGHT VERTEX
        if (!existsCorridorSharingVertex(_topRightVertex)) {
            xPosC = _topRightVertex._x;
            yPosC = _topRightVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NE;
            map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        //BOTTOM RIGHT VERTEX
        if (!existsCorridorSharingVertex(_botRightVertex)) {
            xPosC = _botRightVertex._x;
            yPosC = _botRightVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SE;
            map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }

        if (!existsCorridorSharingVertex(_botLeftVertex)) {
            xPosC = _botLeftVertex._x;
            yPosC = _botLeftVertex._y;
            map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SW;
            map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
            map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;
        }
    }

    public int height() {
        return _grid.rows();
    }

    internal int width() {
        return _grid.columns();
    }

    public bool isSharingVertex(XCell vertex) {
        if (vertex.isEqual(_topLeftVertex)) return true;
        if (vertex.isEqual(_topRightVertex)) return true;
        if (vertex.isEqual(_botLeftVertex)) return true;
        if (vertex.isEqual(_botRightVertex)) return true;
        return false;
    }

    public void setCorridorIncoming(XCorridor corr) {
        _incomingCorridor = corr;
    }

    public void setCorridorOutcoming(XCorridor corr) {
        _outcomingCorridor = corr;
    }

    public bool isWithin(XGrid container) {
        return _grid.isWithin(container, _topLeftVertex);
    }

    public bool collidesWith(IShape other) {
        XCell[] cells = _topLeftVertex.cells(_botRightVertex);
        foreach(XCell each in cells) {
            if (other.containsCell(each)) return true;
        }      
        return false;
    }

    public XCell topLeftVertex() {
        return _topLeftVertex;
    }

    public bool containsCell(XCell aCell) {
        return aCell.isWithin(_topLeftVertex, _botRightVertex);
    }

    public override string ToString() {
        return "XRoom: " + topLeftVertex() + " " + _grid;
    }

    public XCell topRightVertex() {
        return _topRightVertex;
    }

    public XCell bottomLeftVertex() {
        return _botLeftVertex;
    }
}
