using System;

public class XGrid {
    private int _columns;
    private int _rows;

    public XGrid(int rows, int columns) {
        _columns = columns;
        _rows = rows;
    }

    public int columns() {
        return _columns;
    }

    public int rows() {
        return _rows;
    }

    public int columnsZeroBased() {
        return _columns - 1;
    }

    public int rowsZeroBased() {
        return _rows - 1;
    }

    public XGrid columnsOnly() {
        return new XGrid(1, _columns);
    }

    public XGrid rowsOnly() {
        return new XGrid(_rows, 1);
    }

    public bool isWithin(XGrid container, XCell topLeftVertex) {
        if (!topLeftVertex.isWithin(container)) return false;
        if (!absTopRightVertexUsing(topLeftVertex).isWithin(container)) return false;
        if (!absBotRightVertexUsing(topLeftVertex).isWithin(container)) return false;
        if (!absBotLeftVertexUsing(topLeftVertex).isWithin(container)) return false;
        return true;
    }

    private XCell absBotLeftVertexUsing(XCell topLeftVertex) {
        return vertexBottomLeft().plus(topLeftVertex);
    }

    private XCell absBotRightVertexUsing(XCell topLeftVertex) {
        return vertexBottomRight().plus(topLeftVertex);
    }

    private XCell absTopRightVertexUsing(XCell topLeftVertex) {
        return topLeftVertex.plus(topLeftVertex);
    }

    public bool hasCell(int rowIndex, int colIndex) {
        if (rowIndex < 0 && rowIndex >= _rows) return false;
        if (colIndex < 0 && colIndex >= _rows) return false;
        return true;
    }

    public XCell vertexTopLeft() {
        return new XCell(0, 0);
    }
    public XCell vertexTopRight() {
        return new XCell(0, _columns - 1);
    }
    public XCell vertexBottomLeft() {
        return new XCell(_rows - 1, 0);
    }
    public XCell vertexBottomRight() {
        return new XCell(_rows - 1, _columns - 1);
    }





}