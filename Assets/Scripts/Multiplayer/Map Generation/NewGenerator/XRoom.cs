using System;
using System.Collections;
using System.Collections.Generic;

public class XRoom {
    private XSize _size;
    private XCoord _topLeftVertex;
    private XCoord _topRightVertex;
    private XCoord _botLeftVertex;
    private XCoord _botRightVertex;

    public XRoom(XCoord topLeftOrigin, XSize size) {
        _topLeftVertex = topLeftOrigin;
        _topRightVertex = topLeftOrigin.plus(size.heightOnly());
        _botLeftVertex = topLeftOrigin.plus(size.widthOnly());
        _botRightVertex = topLeftOrigin.plus(size);
        _size = size;
    }

    public void plotOn(int[,] map) {      
        for (int x = 0; x < _size.width(); x++) {
            for (int y = 0; y < _size.height(); y++) {
                //test is in range
                int xPos = _topLeftVertex._x + x;
                int yPos = _topLeftVertex._y + y;

                XCoord pos = new XCoord(xPos, yPos);
                if (pos.isEqual(_topLeftVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Empty;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_W;
                } else {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Floor;
                }
            }
        }

        int xPosC = _topLeftVertex._x;
        int yPosC = _topLeftVertex._y;
        map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NW;
        map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
        map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;

        xPosC = _topRightVertex._x;
        yPosC = _topRightVertex._y;
        map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_NE;
        map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
        map[xPosC + 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;

        xPosC = _botRightVertex._x;
        yPosC = _botRightVertex._y;
        map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SE;
        map[xPosC, yPosC - 1] = (int)XGeneratorBehaviour.TileType.Empty;
        map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;

        xPosC = _botLeftVertex._x;
        yPosC = _botLeftVertex._y;
        map[xPosC, yPosC] = (int)XGeneratorBehaviour.TileType.Corner_INN_SW;
        map[xPosC, yPosC + 1] = (int)XGeneratorBehaviour.TileType.Empty;
        map[xPosC - 1, yPosC] = (int)XGeneratorBehaviour.TileType.Empty;

    }
}
