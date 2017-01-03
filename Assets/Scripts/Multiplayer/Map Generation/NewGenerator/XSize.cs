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
    
}