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
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public short HeuristicCost { get; set; }
        public ushort LinkID { get; set; }
        public ushort AreaID { get; set; }
        public ushort NodeID { get; set; }
        public byte PathWidth { get; set; }
        public byte FloodFill { get; set; }
        public UInt32 Flags { get; set; }

    }
}
