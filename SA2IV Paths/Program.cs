using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SA2IV_Paths
{
    class Program
    {
        static UInt32 basado = 0;
        static byte unk1 = 0;
        


        static void Main(string[] args)
        {
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
                DatInfo di = new DatInfo(br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32(), br.ReadUInt32());

                for (int i = 0; i < di.VehicleNodesCount; i++)
                {
                    PathNodes pn = new PathNodes();

                    pn.MemAddress = br.ReadUInt32();
                    pn.Zero = br.ReadUInt32();

                    pn.PosX = br.ReadInt16() / 8;
                    pn.PosY = br.ReadInt16() / 8;
                    pn.PosZ = br.ReadInt16() / 8;

                    pn.HeuristicCost = Int16.MaxValue - 1;

                    pn.LinkID = br.ReadUInt16();
                    pn.AreaID = ushort.Parse(AreaID(datfile));
                    pn.NodeID = NodeID++;
                    pn.PathWidth = br.ReadByte();
                    pn.FloodFill = br.ReadByte();
                    pn.Flags = br.ReadUInt32();

                    PathNodesList.Add(pn);
                }

                //Skip Ped Nodes
                for (int i = 0; i < di.PedNodesCount; i++)
                {
                    br.ReadBytes(28);
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
                bw.Write(di.VehicleNodesCount);
                //Number of car-nodes
                bw.Write(di.VehicleNodesCount);
                //Number of intersections
                bw.Write(basado);
                //Number of links
                bw.Write(di.LinksCount);

                for (int i = 0; i < PathNodesList.Count; i++)
                {
                    bw.Write(PathNodesList[i].MemAddress);
                    bw.Write(basado);
                    bw.Write(PathNodesList[i].AreaID = ushort.Parse(AreaID(datfile)));
                    bw.Write(PathNodesList[i].NodeID);
                    bw.Write(basado);
                    bw.Write(PathNodesList[i].HeuristicCost);
                    bw.Write(PathNodesList[i].LinkID);
                    bw.Write(PathNodesList[i].PosX * 8);
                    bw.Write(PathNodesList[i].PosY * 8);
                    bw.Write(PathNodesList[i].PosZ * 128);
                    bw.Write(PathNodesList[i].PathWidth);
                    bw.Write(PathNodesList[i].FloodFill);
                    bw.Write(PathNodesList[i].Flags);

                }

                for (int i = 0; i < LinksList.Count; i++)
                {
                    bw.Write(LinksList[i].AreaID);
                    bw.Write(LinksList[i].NodeID);
                    bw.Write(unk1);
                    bw.Write(LinkLengthList[i]);
                    bw.Write(20);
                }
                bw.Close();

                
            }


            Console.ReadKey();

        }


        public static string AreaID(string nodfile)
        {
            return Path.GetFileNameWithoutExtension(nodfile).Replace("NODES", "");
        }
    }
}
