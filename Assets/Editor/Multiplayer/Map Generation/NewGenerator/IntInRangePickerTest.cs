using System;
using NUnit.Framework;

public class IntInRangePickerTest {
    [Test]
    public void drawingIntSequenceUsingSeededStrategy() {
        IntInRangePicker picker = new IntInRangePicker(1, 5, new SeededPickerStrategy(123456));
        Assert.AreEqual(2, picker.draw());
        Assert.AreEqual(1, picker.draw());
        Assert.AreEqual(1, picker.draw());
        Assert.AreEqual(2, picker.draw());
        Assert.AreEqual(4, picker.draw());
    }
}   
