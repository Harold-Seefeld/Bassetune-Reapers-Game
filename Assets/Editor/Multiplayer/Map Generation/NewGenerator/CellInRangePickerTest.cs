using NUnit.Framework;


public class CellInRangePickerTest {
    [Test]
    public void pickingCellBetweenCellsOnSameRow() { 
        CellInRangePicker picker = new CellInRangePicker(new SeededPickerStrategy(0));
        XCell result = picker.draw(new XCell(0, 0), new XCell(0, 5));
        Assert.AreEqual(new XCell(0, 3), result);
    }

    [Test]
    public void pickingCellBetweenCellsOnSameColumn() {
        CellInRangePicker picker = new CellInRangePicker(new SeededPickerStrategy(0));
        XCell result = picker.draw(new XCell(0, 0), new XCell(5, 0));
        Assert.AreEqual(new XCell(3, 0), result);
    }
}


