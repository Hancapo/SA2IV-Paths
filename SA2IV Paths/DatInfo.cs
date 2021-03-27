using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SA2IV_Paths
{
    class DatInfo
    {
        public DatInfo(uint nodesCount, uint vehicleNodesCount, uint pedNodesCount, uint naviNodesCount, uint linksCount)
        {
            NodesCount = nodesCount;
            VehicleNodesCount = vehicleNodesCount;
            PedNodesCount = pedNodesCount;
            NaviNodesCount = naviNodesCount;
            LinksCount = linksCount;
        }

        public UInt32 NodesCount { get; set; }
        public UInt32 VehicleNodesCount { get; set; }
        public UInt32 PedNodesCount { get; set; }
        public UInt32 NaviNodesCount { get; set; }
        public UInt32 LinksCount { get; set; }
    }
}
