using System;
using System.Collections;
using System.Collections.Generic;
using DungeonGeneration.Generator.Domain;
using DungeonGeneration;

public class Room : IShape {
    private Grid _grid;
    private Cell _topLeftVertex;
    private Cell _topRightVertex;
    private Cell _botLeftVertex;
    private Cell _botRightVertex;
    private Corridor _outcomingCorridor;
    private Corridor _incomingCorridor;

    public Room(Cell topLeftVertex, Grid size) {
        _topLeftVertex = topLeftVertex;
        _topRightVertex = size.absTopRightVertexUsing(_topLeftVertex);
        _botLeftVertex = size.absBotLeftVertexUsing(_topLeftVertex);
        _botRightVertex = size.absBotRightVertexUsing(_topLeftVertex);
        _grid = size;
    }

    public void plotOn(int[,] map) {      
        for (int row = 0; row < _grid.rows(); row++) {
            for (int col = 0; col < _grid.columns(); col++) {
                //test is in range
                Cell pos = _topLeftVertex.plusCell(row, col);
                int rowPos = pos.rowIndex();
                int colPos = pos.columnIndex();

                if (pos.isEqual(_topLeftVertex)) {
                    map[rowPos, colPos] = (int)TileType.Empty;
                } else if (pos.isEqual(_topRightVertex)) {
                    map[rowPos, colPos] = (int)TileType.Empty;
                } else if (pos.isEqual(_botRightVertex)) {
                    map[rowPos, colPos] = (int)TileType.Empty;
                } else if (pos.isEqual(_botLeftVertex)) {
                    map[rowPos, colPos] = (int)TileType.Empty;
                } else if (pos.isWithin(_topLeftVertex, _topRightVertex)) {
                    map[rowPos, colPos] = (int)TileType.Wall_N;
                } else if (pos.isWithin(_topRightVertex, _botRightVertex)) {
                    map[rowPos, colPos] = (int)TileType.Wall_E;
                } else if (pos.isWithin(_botLeftVertex, _botRightVertex)) {
                    map[rowPos, colPos] = (int)TileType.Wall_S;
                } else if (pos.isWithin(_topLeftVertex, _botLeftVertex)) {
                    map[rowPos, colPos] = (int)TileType.Wall_W;
                } else {
                    map[rowPos, colPos] = (int)TileType.Floor;
                }
            }
        }

       
        joinWithCorridors(map);
        if (_outcomingCorridor != null) _outcomingCorridor.plotOn(map);
        
    }

    private bool existsCorridorSharingVertex(Cell vertex) {
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
        if (!existsCorridorSharingVertex(_topLeftVertex)) {
            xPosC = _topLeftVertex.rowIndex();
            yPosC = _topLeftVertex.columnIndex();
            map[xPosC, yPosC] = (int)TileType.Corner_INN_NW;
            map[xPosC, yPosC + 1] = (int)TileType.Empty;
            map[xPosC + 1, yPosC] = (int)TileType.Empty;
        }

        //TOP RIGHT VERTEX
        if (!existsCorridorSharingVertex(_topRightVertex)) {
            xPosC = _topRightVertex.rowIndex();
            yPosC = _topRightVertex.columnIndex();
            map[xPosC, yPosC] = (int)TileType.Corner_INN_NE;
            map[xPosC, yPosC - 1] = (int)TileType.Empty;
            map[xPosC + 1, yPosC] = (int)TileType.Empty;
        }

        //BOTTOM RIGHT VERTEX
        if (!existsCorridorSharingVertex(_botRightVertex)) {
            xPosC = _botRightVertex.rowIndex();
            yPosC = _botRightVertex.columnIndex();
            map[xPosC, yPosC] = (int)TileType.Corner_INN_SE;
            map[xPosC, yPosC - 1] = (int)TileType.Empty;
            map[xPosC - 1, yPosC] = (int)TileType.Empty;
        }

        if (!existsCorridorSharingVertex(_botLeftVertex)) {
            xPosC = _botLeftVertex.rowIndex();
            yPosC = _botLeftVertex.columnIndex();
            map[xPosC, yPosC] = (int)TileType.Corner_INN_SW;
            map[xPosC, yPosC + 1] = (int)TileType.Empty;
            map[xPosC - 1, yPosC] = (int)TileType.Empty;
        }
    }

    public int height() {
        return _grid.rows();
    }

    internal int width() {
        return _grid.columns();
    }

    public bool isSharingVertex(Cell vertex) {
        if (vertex.isEqual(_topLeftVertex)) return true;
        if (vertex.isEqual(_topRightVertex)) return true;
        if (vertex.isEqual(_botLeftVertex)) return true;
        if (vertex.isEqual(_botRightVertex)) return true;
        return false;
    }

    public void setCorridorIncoming(Corridor corr) {
        _incomingCorridor = corr;
    }

    public void setCorridorOutcoming(Corridor corr) {
        _outcomingCorridor = corr;
    }

    public bool isWithin(Grid container) {
        return _grid.isWithin(container, _topLeftVertex);
    }

    public bool collidesWith(IShape other) {
        Cell[] cells = _topLeftVertex.cells(_botRightVertex);
        foreach(Cell each in cells) {
            if (other.containsCell(each)) return true;
        }      
        return false;
    }

    public Cell topLeftVertex() {
        return _topLeftVertex;
    }

    public bool containsCell(Cell aCell) {
        return aCell.isWithin(_topLeftVertex, _botRightVertex);
    }

    public override string ToString() {
        return "XRoom: " + topLeftVertex() + " " + _grid;
    }

    public Cell topRightVertex() {
        return _topRightVertex;
    }

    public Cell bottomLeftVertex() {
        return _botLeftVertex;
    }
}
