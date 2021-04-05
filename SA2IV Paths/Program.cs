using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SA2IV_Paths
{
    class Program
    {
        static Int16 basado = 0;
        static byte unk1 = 0;
        static UInt32 basado2 = 0;
        


        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            Console.WriteLine("DAT folder");
            string datfilespath = Console.ReadLine();
            string[] datfiles = Directory.GetFiles(datfilespath, "*.dat");


            foreach (var datfile in datfiles)
            {
                UInt16 NodeID = 0;
                List<PathNodes> PathNodesList = new List<PathNodes>();
                List<Links> LinksList = new List<Links>();
                List<byte> LinkLengthList = new List<byte>();
                var f = File.OpenRead(datfile);
                BinaryReader br = new BinaryReader(f);
                uint a = br.ReadUInt32();
                uint b = br.ReadUInt32();
                uint c = br.ReadUInt32();
                uint d = br.ReadUInt32();
                uint e = br.ReadUInt32();

                DatInfo di = new DatInfo(a, b, c, d, e);

                for (int i = 0; i < di.VehicleNodesCount; i++)
                {
                    PathNodes pn = new PathNodes();
                    //hardcoding memaddress
                    br.ReadUInt32();
                    pn.MemAddress = 349765952;

                    //zero hardcoding
                    br.ReadUInt32();
                    pn.Zero = 0;

                    pn.PosX = br.ReadInt16();
                    pn.PosY = br.ReadInt16();
                    pn.PosZ = br.ReadInt16();

                    //lmao DON'T SKIP 
                    br.ReadInt16();
                    pn.HeuristicCost = 32766;

                    pn.LinkID = br.ReadUInt16();

                    //DON'T SKIP AREAID
                    br.ReadUInt16();
                    pn.AreaID = AreaID(datfile);
                    //DON'T SKIP NODEID
                    br.ReadUInt16();
                    pn.NodeID = NodeID++;

                    pn.PathWidth = br.ReadByte();
                    pn.FloodFill = br.ReadByte();
                    //don't skip flags
                    pn.Flags = br.ReadUInt32();

                    PathNodesList.Add(pn);
                }

                //Ped Nodes
                if (di.PedNodesCount != 0)
                {
                    for (int i = 0; i < di.PedNodesCount; i++)
                    {
                        PathNodes pn = new PathNodes();
                        //hardcoding memaddress
                        br.ReadUInt32();
                        pn.MemAddress = 349765952;

                        //zero hardcoding
                        br.ReadUInt32();
                        pn.Zero = 0;

                        pn.PosX = br.ReadInt16();
                        pn.PosY = br.ReadInt16();
                        pn.PosZ = br.ReadInt16();

                        //lmao DON'T SKIP 
                        br.ReadInt16();
                        pn.HeuristicCost = 32766;

                        pn.LinkID = br.ReadUInt16();

                        //DON'T SKIP AREAID
                        br.ReadUInt16();
                        pn.AreaID = AreaID(datfile);
                        //DON'T SKIP NODEID
                        br.ReadUInt16();
                        pn.NodeID = NodeID++;

                        pn.PathWidth = br.ReadByte();
                        pn.FloodFill = br.ReadByte();
                        //don't skip flags
                        pn.Flags = br.ReadUInt32();

                        PathNodesList.Add(pn);
                    }

                }
                //Skip Navi Nodes
                for (int i = 0; i < di.NaviNodesCount; i++)
                {
                    br.ReadBytes(14);
                    
                }

                //Read Links

                for (int i = 0; i < di.LinksCount; i++)
                {
                    Links li = new Links
                    {
                        AreaID = br.ReadUInt16(),
                        NodeID = br.ReadUInt16()
                    };
                    LinksList.Add(li);
                }

                //Skip filler
                br.ReadBytes(768);
                //Skip Navi links
                for (int i = 0; i < di.LinksCount; i++)
                {
                    br.ReadBytes(2);
                }

                //Link lengths????????
                for (int i = 0; i < di.LinksCount; i++)
                {
                    byte LinkLength = br.ReadByte();
                    LinkLengthList.Add(LinkLength);
                    
                }






                br.Close();
                //Export IV .NOD paths
                string nodname = Path.GetFileNameWithoutExtension(datfile).ToLowerInvariant();
                Directory.CreateDirectory(Path.GetDirectoryName(datfile) + "/nod");
                BinaryWriter bw = new BinaryWriter(File.Open(Path.GetDirectoryName(datfile) + "/nod/" + nodname + ".nod", FileMode.Create));

                //Writing IV header

                //Number of nodes
                bw.Write(di.NodesCount);
                //Number of car-nodes
                bw.Write(di.VehicleNodesCount);
                //Number of intersections
                bw.Write(basado2);
                //Number of links
                bw.Write(di.LinksCount);




                for (int i = 0; i < PathNodesList.Count; i++)
                {
                    decimal testing = (decimal)(PathNodesList[i].PosZ / 8f);
                    bw.Write((UInt32)PathNodesList[i].MemAddress);
                    bw.Write((UInt32)0);
                    bw.Write((UInt16)PathNodesList[i].AreaID);
                    bw.Write((UInt16)PathNodesList[i].NodeID);
                    bw.Write((UInt32)0);
                    bw.Write((UInt16)PathNodesList[i].HeuristicCost);
                    bw.Write((UInt16)PathNodesList[i].LinkID);
                    bw.Write((Int16)PathNodesList[i].PosX);
                    bw.Write((Int16)PathNodesList[i].PosY);
                    if ((testing * 128) > 256)
                    {
                        bw.Write((Int16)256);
                    }
                    else
                    {
                        bw.Write((Int16)(testing * 128));

                    }
                    bw.Write((byte)PathNodesList[i].PathWidth);
                    bw.Write((byte)PathNodesList[i].FloodFill);
                    bw.Write(PathNodesList[i].Flags);

                }

                for (int i = 0; i < LinksList.Count; i++)
                {
                    bw.Write(LinksList[i].AreaID);
                    bw.Write(LinksList[i].NodeID);
                    bw.Write((byte)36);
                    bw.Write((byte)LinkLengthList[i]);
                    bw.Write((UInt16)0);
                }
                bw.Close();


            }


            Console.ReadKey();

        }


        public static ushort AreaID(string nodfile)
        {
            return ushort.Parse(Path.GetFileNameWithoutExtension(nodfile).Replace("NODES", ""));
        }
    }
}
