using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class IntInRangePicker {
    private int _max;
    private int _min;
    private SeededPickerStrategy _pickStrategy;

    public IntInRangePicker(int min, int max, SeededPickerStrategy pickStrategy) {
        _min = min;
        _max = max;
        _pickStrategy = pickStrategy;
    }

    public int draw() {
        return _pickStrategy.drawBetween(_min, _max);
    }
}
