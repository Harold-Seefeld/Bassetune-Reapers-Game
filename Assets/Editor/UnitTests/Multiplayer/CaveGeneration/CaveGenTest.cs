using NUnit.Framework;

using DungeonGeneration.Generator.Domain;
using DungeonGeneration.Logging;
using DungeonGeneration.Generator.Pickers;

public class CaveGenTest {

    [Test]
    public void _check() {
        ShapeCellularAutomaton auto = new ShapeCellularAutomaton(30, 60, 5);
        

        ElliShape room1 = new ElliShape(new Cell(0, 0), new OIGrid(32, 32));
        IXShape room2 = new RectShape(room1.topRightVertex().plusCell(0, 10), new OIGrid(32, 32));

        auto.applyOn(room1);
        auto.applyOn(room2);

        OIGrid result = new OIGrid(70, 70);
        room1.accept(new OIGridFiller(result));
        room2.accept(new OIGridFiller(result));

        result.printOnConsole();


    }

    [Test]
    public void _check2() {
        int aah = 30;
        CustomSeededPickerStrategy str = new CustomSeededPickerStrategy(aah);
        System.Console.WriteLine(aah);
        System.Console.WriteLine(str.drawBetween(0, 100));
        System.Console.WriteLine(str.drawBetween(0, 100));
        System.Console.WriteLine(str.drawBetween(0, 100));
        System.Console.WriteLine(str.drawBetween(0, 100));
        System.Console.WriteLine(str.drawBetween(0, 100));
    }

}
