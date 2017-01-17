namespace DungeonGeneration.Generator.Plotters {
    public interface IPlotter {
        void applyOnRoom(Room room, int[,] map);
        void applyOnCorridor(Corridor corridor, int[,] map);
    }
}