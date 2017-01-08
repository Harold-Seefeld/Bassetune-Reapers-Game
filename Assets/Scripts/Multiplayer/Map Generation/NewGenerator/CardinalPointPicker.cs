using System;

internal class CardinalPointPicker {
    private IntInRangePicker _intRangePicker;
    public CardinalPointPicker(SeededPickerStrategy seedStrategy) {
        _intRangePicker = new IntInRangePicker(0, 3, seedStrategy);
    }

    public CardinalPoint draw() {
        return (CardinalPoint)_intRangePicker.draw();
    }

    public CardinalPoint nextClockwise(CardinalPoint aPoint) {
        int next = ((int)aPoint + 1) % 4;
        return (CardinalPoint)next;
    }
}