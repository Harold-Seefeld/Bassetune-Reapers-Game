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
        Vector EastWestperimeter = null;
        Vector eastCorridor = null;
        Vector westCorridor = null;
        Vector NorthSouthperimeter = null;
        private Dungeon _dungeon;
        List<Vector> vectors;

        public VectorGenerator(Dungeon test)
        {
            this._dungeon = test;
        }

        public List<Vector> GenerateVectors()
        {
            vectors = new List<Vector>();
            var maxLength = Math.Max(_dungeon.Map.GetLength(0), _dungeon.Map.GetLength(1));
            for (int y = 0; y < maxLength; y++)
            {
                for (int x = 0; x < maxLength; x++)
                {
                    if (_dungeon.Map.GetLength(0) > y && _dungeon.Map.GetLength(1) > x)
                    {
                        if ((_dungeon.Map[x, y] & Cells.Corridor) != 0)
                        {
                            StartOrContinueNorthCorridor(x, y);
                            StartOrContinueSouthCorridor(x, y);
                            EndEastWestPerimeter();
                        }
                        else if ((_dungeon.Map[x, y] & Cells.Perimeter) != 0)
                        {

                            StartEastWestPerimeter(x, y);
                            EndEastWestCorridor();
                        }
                        else
                        {
                            EndEastWestCorridor();
                            EndEastWestPerimeter();
                        }
                    }
                    if (_dungeon.Map.GetLength(0) > x && _dungeon.Map.GetLength(1) > y)
                    {
                        if ((_dungeon.Map[y, x] & Cells.Corridor) != 0)
                        {
                            StartOrContinueEastCorridor(y, x);
                            StartOrContinueWestCorridor(y, x);
                            EndNorthSouthPerimeter();
                        }
                        else if ((_dungeon.Map[y, x] & Cells.Perimeter) != 0)
                        {

                            StartNorthSouthPerimeter(y, x);
                            EndNorthSouthCorridor();
                        }
                        else
                        {
                            EndNorthSouthCorridor();
                            EndNorthSouthPerimeter();
                        }

                    }
                }
                EndEastWestCorridor();
                EndEastWestPerimeter();
            }

            return vectors;
        }

        private void EndEastWestPerimeter()
        {
            if (EastWestperimeter != null)
            {
                if (EastWestperimeter.StartX != EastWestperimeter.EndX)
                {
                    vectors.Add(EastWestperimeter);
                }

                EastWestperimeter = null;
            }
        }

        private void EndNorthSouthPerimeter()
        {
            if (NorthSouthperimeter != null)
            {
                if (NorthSouthperimeter.StartX != NorthSouthperimeter.EndX)
                {
                    vectors.Add(NorthSouthperimeter);
                }

                NorthSouthperimeter = null;
            }
        }

        private void StartEastWestPerimeter(int x, int y)
        {
            if (EastWestperimeter == null)
            {
                EastWestperimeter = new Vector(x + 0.5M, y + 0.5M);
            }
            else
            {
                EastWestperimeter.EndX += 1;
            }
        }

        private void StartNorthSouthPerimeter(int x, int y)
        {
            if (NorthSouthperimeter == null)
            {
                NorthSouthperimeter = new Vector(x + 0.5M, y + 0.5M);
            }
            else
            {
                NorthSouthperimeter.EndY += 1;
            }
        }

        private void EndEastWestCorridor()
        {
            EndNorthCorridor();
            EndSouthCorridor();
        }

        private void EndNorthSouthCorridor()
        {
            EndEastCorridor();
            EndWestCorridor();
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

        private void EndEastCorridor()
        {
            if (eastCorridor != null)
            {
                vectors.Add(eastCorridor);
                eastCorridor = null;
            }
        }

        private void EndWestCorridor()
        {

            if (westCorridor != null)
            {
                vectors.Add(westCorridor);
                westCorridor = null;
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
                if (southCorridor == null)
                {
                    southCorridor = new Vector(x, y + 1);
                }
                southCorridor.EndX += 1;
            }
        }

        private void StartOrContinueEastCorridor(int x, int y)
        {
            if (x - 1 >= 0 && (_dungeon.Map[x - 1, y] & (Cells.Corridor | Cells.Door)) != 0)
            {
                EndEastCorridor();
            }
            else
            {
                if (eastCorridor == null)
                {
                    eastCorridor = new Vector(x, y);
                }
                eastCorridor.EndY += 1;
            }
        }
        private void StartOrContinueWestCorridor(int x, int y)
        {
            if (x + 1 <= _dungeon.Map.GetLength(0) && (_dungeon.Map[x + 1, y] & (Cells.Corridor | Cells.Door)) != 0)
            {
                EndWestCorridor();
            }
            else
            {
                if (westCorridor == null)
                {
                    westCorridor = new Vector(x + 1, y);
                }
                westCorridor.EndY += 1;
            }
        }

    }
}
