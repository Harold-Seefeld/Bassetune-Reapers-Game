public class CellInRangePicker {
    private SeededPickerStrategy seedStrategy;

    public CellInRangePicker(SeededPickerStrategy seedStrategy) {
        this.seedStrategy = seedStrategy;
    }

    public XCell draw(XCell min, XCell max) {
        if (min.hasNegativeIndexes() && max.hasNegativeIndexes()) return min;
        if (min.hasNegativeIndexes()) min = min.toNearestPositive();
        if (max.hasNegativeIndexes()) max = max.toNearestPositive();

        int distance = min.distance(max);
        int selectedCellPosition = seedStrategy.drawBetween(0, distance-1);

        XCell[] cells = min.cells(max);
        return cells[selectedCellPosition];
    }
}