using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SeededPickerStrategy {
    private Random _random;
    private int _seed;

    public SeededPickerStrategy(int seed) {
        _seed = seed;
        _random = new Random(seed);
    }

    public int drawBetween(int min, int max) {
        return _random.Next(min, max);
    }
}
