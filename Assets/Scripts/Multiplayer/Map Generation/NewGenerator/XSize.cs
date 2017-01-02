using System;

public class XSize {
    private int _width;
    private int _height;

    public XSize(int width, int height) {
        _width = width;
        _height = height;
    }

    public int width() {
        return _width;
    }

    public int height() {
        return _height;
    }

    public int widthZeroBased() {
        return _width - 1;
    }

    public int heightZeroBased() {
        return _height - 1;
    }

    public XSize widthOnly() {
        return new XSize(_width, 1);
    }

    public XSize heightOnly() {
        return new XSize(1, _height);
    }
    
}