using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class Vector
    {
        public Vector(decimal x, decimal y)
        {
            // TODO: Complete member initialization
            StartX = x;
            StartY = y;
            EndX = x;
            EndY = y;

        }
        public decimal StartX { get; set; }
        public decimal StartY { get; set; }
        public decimal EndX { get; set; }
        public decimal EndY { get; set; }

        public override string ToString()
        {
            return string.Format("Start[{0},{1}] End[{2},{3}]",StartX,StartY,EndX,EndY);
        }
    }
}
