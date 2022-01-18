using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herbhiker;
using Memory;

namespace HerbHikerApp
{
    public class MemoryReader
    {
        // The process we are reading/writing from/to
        private const string PROC_NAME = "Wow.exe";
        private int procId;

        private Mem mem;

        private ObjectDump objectDump;
        private GameObject[] objects;

        public MemoryReader()
        {
            mem = new Mem();
        }

        internal void Init()
        {
            objectDump = new ObjectDump(mem);
        }

        internal IntPtr GetHandle()
        {
            return mem.pHandle;
        }

        /// <summary>
        /// Reads nearby game objects.
        /// </summary>
        internal GameObject[] ReadObjects()
        {
            return objectDump.Dump();
        }

        /// <summary>
        /// Reads the current player XYZ position
        /// </summary>
        /// <returns>A point structure with the XYZ coords</returns>
        internal Point ReadPlayerPosition()
        {
            Point p = new Point(
                    mem.ReadFloat(Offsets.Player.POS_X),
                    mem.ReadFloat(Offsets.Player.POS_Y),
                    mem.ReadFloat(Offsets.Player.POS_Z));
            return p;
        }

        /// <summary>
        /// Searches through the objects array for the object position with specified guid. 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>A point structure with XYZ or null if GUID not found</returns>
        internal Point GetObjectPosition(ulong guid)
        {
            for(int i = 0; i < ObjectDump.SIZE; i++)
            {
                if (objects[i] == null)
                    break;

                if (objects[i].guid == guid)
                    return objects[i].position;
            }
            return null;
        }

        /// <summary>
        /// Attempts to open the process with "Wow.exe" as name
        /// </summary>
        /// <returns>true if found, otherwise false</returns>
        internal bool OpenProc()
        {
            procId = mem.GetProcIdFromName(PROC_NAME);
            return mem.OpenProcess(procId);
        }

        internal float ReadFloat(string address)
        {
            return mem.ReadFloat(address);
        }

        /// <summary>
        /// Sets the CTM X, Y, Z destination coordinates.
        /// </summary>
        /// <param name="p">Point with the coordinates</param>
        internal void SetDestionation(Point p)
        {
            mem.WriteMemory(Offsets.CTM.DESTINATION_X, "float", p.x.ToString());
            mem.WriteMemory(Offsets.CTM.DESTINATION_Y, "float", p.y.ToString());
            mem.WriteMemory(Offsets.CTM.DESTINATION_Z, "float", p.z.ToString());
        }

        /// <summary>
        /// Reads the guid of a nearby herb (yellow dot on minimap)
        /// </summary>
        /// <returns></returns>
        internal ulong ReadNearbyHerb()
        {
            int addr = mem.ReadInt(Offsets.NODE_GUID);
            if (addr == 0)
                return 0;
            return (ulong)mem.ReadLong(addr.ToString("X"));
        }

        internal void ClearNearbyHerb()
        {
            int addr = mem.ReadInt(Offsets.NODE_GUID);
            if (addr == 0)
                return;
            mem.WriteMemory(addr.ToString("X"), "long", "0");
        }

        internal void SetCTMAction(string actionType)
        {
            mem.WriteMemory(Offsets.CTM.ACTION, "int", actionType);
            if (actionType == "4")
                mem.WriteMemory(Offsets.CTM.DISTANCE, "float", "0.5");
        }

        /// <summary>
        /// Sets the CTM guid object id which is needed for looting. Due to memory.dll
        /// not supporting ulong (unsigned int64), only regular long, we have to write
        /// an array of size 8 with bytes instead.
        /// </summary>
        /// <param name="guid"></param>
        internal void SetCTMGUID(ulong guid)
        {
            // convert guid to an array of bytes. We know its 8 bytes.
            // this works but probably a better way of doing it.

            // generates: "F110000652000E7F"
            // goal is an array with: {0xF1, 0x10, 0x00, 0x06, 0x52, 0x00, 0x0E, 0x7F}
            string hex = guid.ToString("X");
            byte[] bytes = new byte[8];
            for(int i = 0; i < 8; i++)
            {
                // get the two bytes from string
                string twob = "" + hex.ElementAt(0 + (2 * i)) + hex.ElementAt(1 + (2 * i));

                // convert to int
                bytes[7 - i] = (byte)Convert.ToUInt32(twob, 16);
            }

            mem.WriteBytes(Offsets.CTM.GUID, bytes);
        }

        internal ulong ReadCTMGUID()
        {
            return (ulong) mem.ReadLong(Offsets.CTM.GUID);
        }

        internal void PrintObjects()
        {
            Console.WriteLine("nr\t GUID\t\t\t\t\tPos");
            for(int i = 0; i < objectDump.ObjectsCount(); i++)
            {
                Console.WriteLine("{0}\t {1}\t\t{2}", i, objects[i].guid, objects[i].position);
            }
        }
    }
}
