using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA2IV_Paths
{
    class PathNodes
    {
        public uint MemAddress { get; set; }
        public uint Zero { get; set; }
        public Int16 PosX { get; set; }
        public Int16 PosY { get; set; }
        public Int16 PosZ { get; set; }
        public ushort HeuristicCost { get; set; }
        public ushort LinkID { get; set; }
        public ushort AreaID { get; set; }
        public ushort NodeID { get; set; }
        public byte PathWidth { get; set; }
        public byte FloodFill { get; set; }
        public UInt32 Flags { get; set; }

    }
}
