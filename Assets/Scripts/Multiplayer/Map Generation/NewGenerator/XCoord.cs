using System;

public class XCoord {
    public int _x;
    public int _y;

    public XCoord(int x, int y) {
        _x = x;
        _y = y;
    }

    public override bool Equals(object obj) {
        return isEqual((XCoord)obj);
    }

    public override string ToString() {
        return "XCoord: ["+ _x +", "+ _y+"]";
    }

    public XCoord plus(XSize size) {
        return new XCoord(_x + size.widthZeroBased(), _y + size.heightZeroBased());
    }

    public bool isWithin(XCoord start, XCoord end) {
        if (_x < start._x) return false;
        if (_y < start._y) return false;
        if (_x > end._x) return false;
        if (_y > end._y) return false;
        return true;
    }

    public bool isEqual(XCoord other) {
        if (other._x != _x) return false;
        if (other._y != _y) return false;
        return true;
    }
}