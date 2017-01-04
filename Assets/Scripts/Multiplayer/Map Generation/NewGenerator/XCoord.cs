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
        return "XCoord: ["+ _x +", "+ _y+"]";
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
        return new XCell(_x + rowSize-1, _y + colSize-1);
    }
}