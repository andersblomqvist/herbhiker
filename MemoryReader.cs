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

        /// <summary>
        /// Reads nearby game objects.
        /// </summary>
        internal void ReadObjects()
        {
            objects = objectDump.Dump();
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

        internal void SetCTMAction(string actionType)
        {
            mem.WriteMemory(Offsets.CTM.ACTION, "int", actionType);
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
