using DungeonGeneration.Generator.Domain;
using DungeonGeneration.Generator.Plotters;

public class Room : IShape {
    private Grid _grid;
    private Cell _topLeftVertex;
    private Cell _topRightVertex;
    private Cell _botLeftVertex;
    private Cell _botRightVertex;
    private Corridor _outcomingCorridor;
    private Corridor _incomingCorridor;
    private IPlotter _tilingStrategy;

    public Room(Cell topLeftVertex, Grid size) { 
        _topLeftVertex = topLeftVertex;
        _topRightVertex = size.absTopRightVertexUsing(_topLeftVertex);
        _botLeftVertex = size.absBotLeftVertexUsing(_topLeftVertex);
        _botRightVertex = size.absBotRightVertexUsing(_topLeftVertex);
        _grid = size;
    }

    public Cell bottomRightVertex() {
        return _botRightVertex;
    }

    public void plotOn(int[,] map, IPlotter plotter) {
        plotter.applyOnRoom(this, map);
        if (_outcomingCorridor != null) _outcomingCorridor.plotOn(map, plotter);
    }

    public bool hasCorridorSharingVertex(Cell vertex) {
        bool result = false;
        if (_incomingCorridor != null) {
            result = result || _incomingCorridor.isSharingVertex(vertex);
        }
        if (_outcomingCorridor != null) {
            result = result || _outcomingCorridor.isSharingVertex(vertex);
        }
        return result;
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
        return "Room: " + topLeftVertex() + " " + _grid;
    }

    public Cell topRightVertex() {
        return _topRightVertex;
    }

    public Cell bottomLeftVertex() {
        return _botLeftVertex;
    }
}
