using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonGenerator
{
    public class Door
    {

        public int Row { get; set; }
        public int Col{ get; set; }
        public Cells Key { get; set; }
        public string Type { get; set; }
        public int OutId { get; set; }
    }
}
