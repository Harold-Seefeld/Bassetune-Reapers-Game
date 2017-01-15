using System;
using DungeonGeneration.Generator.Domain;

public class Corridor : IShape {
    private Cell _topLeftVertex;
    private Cell _topRightVertex;
    private Cell _botLeftVertex;
    private Cell _botRightVertex;
    private Grid _grid;
    private Orientation _orientation;
    private Room _sourceRoom;
    private Room _destRoom;

    public enum Orientation {
        horizontal,
        vertical
    }

    public Corridor(Cell topLeftVertex, Grid size, Orientation orientation) {
        _topLeftVertex = topLeftVertex;
        _topRightVertex = size.absTopRightVertexUsing(_topLeftVertex);
        _botLeftVertex = size.absBotLeftVertexUsing(_topLeftVertex);
        _botRightVertex = size.absBotRightVertexUsing(_topLeftVertex);
        _grid = size;
        _orientation = orientation;
    }

    public void plotOn(int[,] map) {
        if (_destRoom != null) _destRoom.plotOn(map);

        for (int row = 0; row < _grid.rows(); row++) {
            for (int col = 0; col < _grid.columns(); col++) {
                //test is in range
                Cell pos = _topLeftVertex.plusCell(row, col);
                int rowPos = pos.rowIndex();
                int colPos = pos.columnIndex();

                if (pos.isEqual(_topLeftVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)TileType.Corner_OUT_NW : (int)TileType.Corner_OUT_SE;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)TileType.Corner_OUT_NE : (int)TileType.Corner_OUT_SW;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)TileType.Corner_OUT_SE : (int)TileType.Corner_OUT_NW;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[rowPos, colPos] = isVertical() ? (int)TileType.Corner_OUT_SW : (int)TileType.Corner_OUT_NE;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex) && isOrizontal()) {
                    map[rowPos, colPos] = (int)TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex) && isVertical()) {
                    map[rowPos, colPos] = (int)TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex) && isOrizontal()) {
                    map[rowPos, colPos] = (int)TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex) && isVertical()) {
                    map[rowPos, colPos] = (int)TileType.Wall_W;
                } else {
                    map[rowPos, colPos] = (int)TileType.Floor;
                }
            }
        }

        if (_sourceRoom != null) {
            if (_sourceRoom.isSharingVertex(_botLeftVertex)) {
                map[_botLeftVertex.rowIndex(), _botLeftVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_W : (int)TileType.Wall_S;
            }
            if (_sourceRoom.isSharingVertex(_topRightVertex)) {
                map[_topRightVertex.rowIndex(), _topRightVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_E : (int)TileType.Wall_N;
            }
            if (_sourceRoom.isSharingVertex(_topLeftVertex)) {
                map[_topLeftVertex.rowIndex(), _topLeftVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_W : (int)TileType.Wall_N;
            }
            if (_sourceRoom.isSharingVertex(_botRightVertex)) {
                map[_botRightVertex.rowIndex(), _botRightVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_E : (int)TileType.Wall_S;
            }
        }

        if (_destRoom != null) {
            if (_destRoom.isSharingVertex(_botLeftVertex)) {
                map[_botLeftVertex.rowIndex(), _botLeftVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_W : (int)TileType.Wall_S;
            }
            if (_destRoom.isSharingVertex(_topRightVertex)) {
                map[_topRightVertex.rowIndex(), _topRightVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_E : (int)TileType.Wall_N;
            }
            if (_destRoom.isSharingVertex(_topLeftVertex)) {
                map[_topLeftVertex.rowIndex(), _topLeftVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_W : (int)TileType.Wall_N;
            }
            if (_destRoom.isSharingVertex(_botRightVertex)) {
                map[_botRightVertex.rowIndex(), _botRightVertex.columnIndex()] = isVertical() ? (int)TileType.Wall_E : (int)TileType.Wall_S;
            }
        }
    }

    public Cell topRightVertex() {
        return _topRightVertex;
    }

    public Cell topLeftVertex() {
        return _topLeftVertex;
    }

    public void setDestinationRoom(Room room) {
        _destRoom = room;
    }

    public int width() {
        return _grid.columns();
    }

    public Cell bottomLeftVertex() {
        return _botLeftVertex;
    }

    public void setSourceRoom(Room room) {
        _sourceRoom = room;
    }

    public int height() {
        return _grid.rows();      
    }

    public bool isVertical() {
        return _orientation == Orientation.vertical;
    }
    private bool isOrizontal() {
        return _orientation == Orientation.horizontal;
    }

    public bool isSharingVertex(Cell vertex) {
        if (vertex.isEqual(_topLeftVertex)) return true;
        if (vertex.isEqual(_topRightVertex)) return true;
        if (vertex.isEqual(_botLeftVertex)) return true;
        if (vertex.isEqual(_botRightVertex)) return true;
        return false;
    }

    public bool isWithin(Grid container) {
        return _grid.isWithin(container, _topLeftVertex);
    }

    public bool collidesWith(IShape each) {
        if (each.containsCell(_topLeftVertex)) return true;
        if (each.containsCell(_topRightVertex)) return true;
        if (each.containsCell(_botRightVertex)) return true;
        if (each.containsCell(_botLeftVertex)) return true;
        return false;
    }

    public bool containsCell(Cell aCell) {
        return aCell.isWithin(_topLeftVertex, _botRightVertex);
    }

    public override string ToString() {
        return "XCorridor: " + topLeftVertex() + " " + _grid;
    }
}