using System;
namespace DungeonGenerator.Corridors
{
    public interface ICorridorGenerator
    {
        void Corridors(Dungeon dungeon);
        void RemoveDeadEnds(Dungeon dungeon);
    }
}
