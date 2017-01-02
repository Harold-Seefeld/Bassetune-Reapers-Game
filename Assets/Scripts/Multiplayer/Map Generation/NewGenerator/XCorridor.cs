using System;

public class XCorridor {
    private XCoord _topLeftVertex;
    private XCoord _topRightVertex;
    private XCoord _botLeftVertex;
    private XCoord _botRightVertex;
    private XSize _size;
    private Orientation _orientation;

    public enum Orientation {
        horizontal,
        vertical
    }

    public XCorridor(XCoord topLeftVertex, XSize size, Orientation orientation) {
        //if (orientation == Orientation.horizontal) { 
        _topLeftVertex = topLeftVertex;
        _topRightVertex = topLeftVertex.plus(size.heightOnly());
        _botLeftVertex = topLeftVertex.plus(size.widthOnly());
        _botRightVertex = topLeftVertex.plus(size);
        /* } else {
            _topLeftVertex = topLeftVertex;
            _topRightVertex = topLeftVertex;
            _botLeftVertex = _topRightVertex.plus(size.heightOnly());
            _botRightVertex = topLeftVertex.plus(size);
        }
        */
        _size = size;
        _orientation = orientation;
    }

    public void plotOn(int[,] map) {
        for (int x = 0; x < _size.width(); x++) {
            for (int y = 0; y < _size.height(); y++) {
                //test is in range
                int xPos = _topLeftVertex._x + x;
                int yPos = _topLeftVertex._y + y;

                XCoord pos = new XCoord(xPos, yPos);
                if (pos.isEqual(_topLeftVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_NE;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_NW;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_SW;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Corner_OUT_SE;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex) 
                    && _orientation == Orientation.horizontal) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex)
                    && _orientation == Orientation.vertical) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex) 
                    && _orientation == Orientation.horizontal) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex)
                    && _orientation == Orientation.vertical) {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Wall_W;
                } else {
                    map[xPos, yPos] = (int)XGeneratorBehaviour.TileType.Floor;
                }
            }
        }
    }
}