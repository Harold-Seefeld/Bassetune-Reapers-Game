using DungeonGenerator.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DungeonGenerator.Corridors
{
    public class CorridorGenerator : ICorridorGenerator
    {

        #region Fields
        private Dungeon _dungeon;
        private Dictionary<Directions, int> di = new Dictionary<Directions, int>()
            {
                {Directions.North,-1},
                {Directions.South,1},
                {Directions.West,0},
                {Directions.East,0},
            };
        private Dictionary<Directions, int> dj = new Dictionary<Directions, int>()
            {
                {Directions.North,0},
                {Directions.South,0},
                {Directions.West,-1},
                {Directions.East,1},
            };
        private List<Directions> jDirs;

        private CorridorLayout corridorLayout = CorridorLayout.Bent;
        
        #endregion

        #region Properties
        public Random random { get; set; }
        #endregion

        public CorridorGenerator()
        {
            jDirs = dj.Keys.ToList();
            jDirs.Sort();
        }

        public void Corridors(Dungeon dungeon)
        {
            _dungeon = dungeon;
            var cell = _dungeon.Map;
            for (var x = 1; x < _dungeon.rows/2 ; x++)
            {
                var r = (x * 2) + 1;
                for (var y = 1; y < _dungeon.cols / 2; y++)
                {
                    var c = (y * 2) + 1;
                    if ((cell[r, c] & Cells.Corridor) != 0)
                    {
                        continue;
                    }
                    Tunnel(x, y);
                }
            }
        }

        private void Tunnel(int x, int y, Directions? lastDir = null)
        {
            List<Directions> dirs = TunnelDirs(lastDir);
            foreach (var dir in dirs)
            {
                if (OpenTunnel(x, y, dir))
                {
                    var nextX = x + di[dir];
                    var nextY = y + dj[dir];
                    Tunnel(nextX, nextY, dir);
                }
            }
        }

        private List<Directions> TunnelDirs(Directions? lastDir)
        {
            var p = corridorLayout;
            var dirs = Helper.Shuffle(jDirs);
            if (lastDir != null)
            {
                if ( random.Next(100) < (int)p)
                {
                    dirs.Insert(0, (Directions)lastDir);
                }
            }
            return dirs;
        }

        private bool OpenTunnel(int x, int y, Directions dir)
        {
            var thisR = x * 2 + 1;
            var thisC = y * 2 + 1;
            var nextR = ((x + di[dir]) * 2) + 1;
            var nextC = ((y + dj[dir]) * 2) + 1;
            var midR = (thisR + nextR) / 2;
            var midC = (thisC + nextC) / 2;

            if (SoundTunnel(midR, midC, nextR, nextC))
            {
                DelveTunnel(thisR, thisC, nextR, nextC);
                return true;
            }
            return false;
        }

        private bool SoundTunnel(int midR, int midC, int nextR, int nextC)
        {
            if (nextR < 0 || nextR > _dungeon.rows)
            {
                return false;
            }
            if (nextC < 0 || nextC > _dungeon.cols)
            {
                return false;
            }

            var cell = _dungeon.Map;
            List<int> rs = new List<int>() { midR, nextR };
            rs.Sort();
            List<int> cs = new List<int>() { midC, nextC };
            cs.Sort();
            for (var r = rs[0]; r <= rs[1]; r++)
            {
                for (var c = cs[0]; c <= cs[1]; c++)
                {
                    if ((cell[r, c] & Cells.BlockCorr) != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void DelveTunnel(int thisR, int thisC, int nextR, int nextC)
        {
            var cell = _dungeon.Map;
            List<int> rs = new List<int>() { thisR, nextR };
            rs.Sort();
            List<int> cs = new List<int>() { thisC, nextC };
            cs.Sort();
            for (var r = rs[0]; r <= rs[1]; r++)
            {
                for (var c = cs[0]; c <= cs[1]; c++)
                {
                    cell[r, c] &= ~Cells.Entrance;
                    cell[r, c] |= Cells.Corridor;

                }
            }
        }
    }
}
