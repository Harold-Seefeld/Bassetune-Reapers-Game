public interface IShape {
    bool isWithin(XGrid mapGrid);
    bool collidesWith(IShape each);
    bool containsCell(XCell _topLeftVertex);
    void plotOn(int[,] map);
}