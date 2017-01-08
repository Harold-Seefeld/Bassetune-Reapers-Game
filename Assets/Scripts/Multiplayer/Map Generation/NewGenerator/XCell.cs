using System;

public class XCell {
    public int _x;
    public int _y;

    public XCell(int x, int y) {
        _x = x;
        _y = y;
    }

    public override bool Equals(object obj) {
        return isEqual((XCell)obj);
    }

    public override string ToString() {
        return "XCell: ["+ _x +", "+ _y+"]";
    }

    public bool hasNegativeIndexes() {
        if (_x < 0) return true;
        if (_y < 0) return true;
        return false;
    }

    public XCell toNearestPositive() {
        int row = _x < 0 ? 0: _x;
        int col = _y < 0 ? 0 : _y;
        return new XCell(row, col);
    }

    public int distance(XCell other) {
        int rowDistance = Math.Abs(_x - other._x) + 1;

        int colDistance = Math.Abs(_y - other._y) + 1;
        return rowDistance * colDistance;
    }

    public bool hasSameRow(XCell other) {
        return _x == other._x;
    }

    public bool hasSameColumn(XCell other) {
        return _y == other._y;
    }

    public XCell plus(XGrid size) {
        return new XCell(_x + size.rowsZeroBased(), _y + size.columnsZeroBased());
    }

    public bool isWithin(XCell start, XCell end) {
        if (_x < start._x) return false;
        if (_y < start._y) return false;
        if (_x > end._x) return false;
        if (_y > end._y) return false;
        return true;
    }

    public bool isWithin(XGrid aGrid) {
        return aGrid.hasCell(_x, _y);
    }

    public bool isEqual(XCell other) {
        if (other._x != _x) return false;
        if (other._y != _y) return false;
        return true;
    }

    //CAPIRE BENE SE HA SENSO SOMMARE DUE CELLE
    //forse bisogna trovare la distanza e sommare quella
    public XCell plus(XCell other) {
        return new XCell(_x + other._x, _y + other._y);
    }

    public XCell plus(int rowSize, int colSize) {
        if (rowSize == 0) rowSize = 1;
        if (colSize == 0) colSize = 1;
        return new XCell(_x + (rowSize - 1), _y + (colSize - 1));
    }

    public XCell minus(int rowSize, int colSize) {
        if (rowSize == 0) rowSize = 1;
        if (colSize == 0) colSize = 1;
        return new XCell(_x - (rowSize - 1), _y - (colSize - 1));
    }

    public XCell[] cells(XCell other) {
        XCell min = this;
        XCell max = other;
        if (isGreatherThan(other)) {
            min = other;
            max = this;
        } 
        
        int cellsNumber = distance(other);
        XCell[] result = new XCell[cellsNumber];

        int cellIndex = 0;
        for (int row = min._x; row <= max._x; row++) {
            for (int col = min._y; col <= max._y; col++) {
                result[cellIndex] = new XCell(row, col);
                cellIndex++;
            }
        }
        return result;
    }

    public bool isGreatherThan(XCell other) {
        int distanceA = new XCell(0, 0).distance(this);
        int distanceB = new XCell(0, 0).distance(other);
        return distanceA > distanceB;
    }
}