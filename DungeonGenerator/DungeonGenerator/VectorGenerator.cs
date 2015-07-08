using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class VectorGenerator
    {
        Vector northCorridor = null;
        Vector southCorridor = null;
        Vector perimeter = null;
        private Dungeon _dungeon;
        List<Vector> vectors;

        public VectorGenerator(Dungeon test)
        {
            this._dungeon = test;
        }



        public List<Vector> GenerateVectors()
        {
            vectors = new List<Vector>();
            for (int y = 0; y < _dungeon.Map.GetLength(1); y++)
            {
                for (int x = 0; x < _dungeon.Map.GetLength(0); x++)
                {
                    if ((_dungeon.Map[x, y] & Cells.Corridor) != 0)
                    {
                        StartOrContinueNorthCorridor(x, y);
                        StartOrContinueSouthCorridor(x, y);
                        EndPerimeter();
                    }
                    else if ((_dungeon.Map[x, y] & Cells.Perimeter) != 0)
                    {

                        StartPerimeter(x, y);
                        EndCorridor();
                    }
                    else
                    {
                        EndCorridor();
                        EndPerimeter();
                    }

                }
                EndCorridor();
                EndPerimeter();
            }

            return vectors;
        }

        private void EndPerimeter()
        {
            if (perimeter != null)
            {
                if (perimeter.StartX != perimeter.EndX)
                {
                    vectors.Add(perimeter);
                }

                perimeter = null;
            }
        }

        private void StartPerimeter(int x, int y)
        {
            if (perimeter == null)
            {
                perimeter = new Vector(x + 0.5M, y + 0.5M);
            }
            else
            {
                perimeter.EndX += 1;
            }
        }

        private void EndCorridor()
        {
            EndNorthCorridor();
            EndSouthCorridor();
        }

        private void EndNorthCorridor()
        {
            if (northCorridor != null)
            {
                vectors.Add(northCorridor);
                northCorridor = null;
            }
        }

        private void EndSouthCorridor()
        {

            if (southCorridor != null)
            {
                vectors.Add(southCorridor);
                southCorridor = null;
            }
        }

        private void StartOrContinueNorthCorridor(int x, int y)
        {
            if (y - 1 >= 0 && (_dungeon.Map[x, y - 1] & (Cells.Corridor | Cells.Door)) != 0)
            {
                EndNorthCorridor();
            }
            else
            {
                //north is no entrance and corridor
                if (northCorridor == null)
                {
                    northCorridor = new Vector(x, y);
                }
                northCorridor.EndX += 1;
            }
        }
        private void StartOrContinueSouthCorridor(int x, int y)
        {
            if (y + 1 <= _dungeon.Map.GetLength(1) && (_dungeon.Map[x, y + 1] & (Cells.Corridor | Cells.Door)) != 0)
            {
                EndSouthCorridor();
            }
            else
            {
                //north is no entrance and corridor
                if (southCorridor == null)
                {
                    southCorridor = new Vector(x, y + 1);
                }
                southCorridor.EndX += 1;
            }
        }
    }
}
