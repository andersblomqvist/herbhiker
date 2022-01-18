using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbhiker;
using Memory;

namespace HerbHikerApp
{
    /// <summary>
    /// Reads the Object manager and its linked-list of objects.
    /// </summary>
    public class ObjectDump
    {
        public enum WowObjectType
        {
            None = 0,
            Item = 1,
            Container = 2,
            Unit = 3,
            Player = 4,
            GameObject = 5,
            DynamicObject = 6,
            Corpse = 7,
            AiGroup = 8,
            AreaTrigger = 9
        }

        // How many objects we read, usually it ends at around 300
        public static int SIZE = 512;

        private readonly int clientConnection;
        private readonly int objectManager;
        private readonly int listStart;

        private const int objManager = 0x2ED0;
        private const int firstObj = 0xAC;
        private const int nextObj = 0x3C;

        private readonly Mem mem;

        private int count;

        public ObjectDump(Mem memory) 
        {
            this.mem = memory;
            count = 0;

            clientConnection = mem.ReadInt(Offsets.ObjManager.CLIENT_CONNECTION);
            string clientHex = toHex(clientConnection);
            Console.WriteLine("Got clientConn={0}", clientHex);

            objectManager = clientConnection + objManager;
            Console.WriteLine("Got objManager={0}", toHex(objectManager));
            objectManager = mem.ReadInt(toHex(objectManager));
            
            listStart = objectManager + firstObj;
            Console.WriteLine("Got listStart ={0}", toHex(listStart));
        }


        /// <summary>
        /// Creates an array of GameObjects from the linked-list. Only GameObjects,
        /// which means all objects will not be added. We only need Herbs.
        /// </summary>
        /// <returns>Array of GameObjects near player</returns>
        internal GameObject[] Dump()
        {
            GameObject[] objects = new GameObject[SIZE];
            count = 0; // number of GameObjects found.

            int currObj = mem.ReadInt(toHex(listStart));

            // Console.WriteLine("first={0}", toHex(currObj));
            // Console.WriteLine("Dumping object from Object Manager");
            // Console.WriteLine("addr\t\tnr\t\tType\t\tGUID");

            for (int i = 0; i < SIZE; i++)
            {
                long desc = mem.ReadLong(toHex(currObj + 0x8));
                int type = mem.ReadInt(toHex(currObj + 0x14));

                if (type < 0 || type > 40)
                {
                    // Console.WriteLine("Aborting - object seems off: {0}", toHex(currObj));
                    break;
                }

                // additional printouts
                string info = "";

                // Herbs etc ... and a lot of other stuff
                if(type == (int)WowObjectType.GameObject)
                {
                    ulong guid = (ulong)mem.ReadLong(toHex(currObj + 0x30));
                    Point pos = new Point(
                        mem.ReadFloat(toHex(currObj + 0xE8)),
                        mem.ReadFloat(toHex(currObj + 0xEC)),
                        mem.ReadFloat(toHex(currObj + 0xF0)));

                    GameObject obj = new GameObject(guid, pos);
                    objects[count] = obj;
                    count++;

                    info = guid.ToString() + " " + pos.ToString();
                }

                // Console.WriteLine("{0}\t{1}\t\t{2}\t\t\t{3} {4}", toHex(currObj), i, type, desc, info);

                // Go to next object in list
                currObj += nextObj;
                currObj = mem.ReadInt(toHex(currObj));
            }

            return objects;
        }

        internal int ObjectsCount()
        {
            return count;
        }

        private string toHex(int value)
        {
            return value.ToString("X");
        }
    }
}
