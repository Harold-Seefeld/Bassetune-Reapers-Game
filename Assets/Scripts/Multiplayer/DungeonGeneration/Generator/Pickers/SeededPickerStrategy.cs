using System;

namespace DungeonGeneration.Generator.Pickers {

    public class SeededPickerStrategy : IPickerStrategy {
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
}