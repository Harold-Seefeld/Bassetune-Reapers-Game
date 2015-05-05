using DungeonGenerator.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    internal class Sill
    {

        public int SillR { get; set; }

        public int SillC { get; set; }

        public Directions Dir { get; set; }

        public int DoorR { get; set; }

        public int DoorC { get; set; }

        public int OutId { get; set; }
    }
}
