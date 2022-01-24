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
        internal ulong ReadNearbyHerb(List<ulong> blacklist)
        {
            int addr = mem.ReadInt(Offsets.NODE_GUID);
            ulong guid = (ulong)mem.ReadLong(addr.ToString("X"));
            if (addr == 0)
                return 0;
            else if (blacklist.Contains(guid))
            {
                // search next nearby herb.
                for (int i = 0; i < 10; i++)
                {
                    addr += Offsets.NEXT_NODE_GUID;
                    guid = (ulong)mem.ReadLong(addr.ToString("X"));

                    // If guid not vaild, stop and return 0.
                    if (guid.ToString("X").ElementAt(0) != 'F')
                        break;
                    // if it was valid, check if not blacklisted.
                    else if(!blacklist.Contains(guid))
                        return (ulong)mem.ReadLong(addr.ToString("X"));
                }
            }
            else
                return guid;

            return 0;
        }

        /// <summary>
        /// Sets the nearby herb to 0. If no other herb is close it will remain 0.
        /// Otherwise it will be a new GUID
        /// </summary>
        internal void ClearNearbyHerb()
        {
            int addr = mem.ReadInt(Offsets.NODE_GUID);
            if (addr == 0)
                return;
            mem.WriteMemory(addr.ToString("X"), "long", "0");
        }

        /// <summary>
        /// Set the action for CTM. The distance is how near the player needs to be
        /// in order for the game to recognize the action is finished.
        /// </summary>
        /// <param name="actionType"></param>
        internal void SetCTMAction(string actionType)
        {
            mem.WriteMemory(Offsets.CTM.ACTION, "int", actionType);
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

            // guid toString generates: "F110000652000E7F"
            string hex = guid.ToString("X");
            
            // goal is an byte array with: {0xF1, 0x10, 0x00, 0x06, 0x52, 0x00, 0x0E, 0x7F}
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

        internal int ReadCTMAction()
        {
            return mem.ReadInt(Offsets.CTM.ACTION);
        }

        internal void SetCTMAction()
        {
            mem.WriteMemory(Offsets.CTM.ACTION, "int", "4");
        }

        /// <summary>
        /// Toggle noclip for player. It will set the player width to 0.
        /// </summary>
        /// <param name="state"></param>
        internal void ToggleNoclip(bool state)
        {
            if (objectDump.GetPlayerPointer() == 0)
                objectDump.Dump();

            if (state)
                mem.WriteMemory((objectDump.GetPlayerPointer() + Offsets.ObjManager.WIDTH_OFFSET).ToString("X"), "float", "0");
            else
                mem.WriteMemory((objectDump.GetPlayerPointer() + Offsets.ObjManager.WIDTH_OFFSET).ToString("X"), "float", "0.4");
        }

        /// <summary>
        /// Returns the player health. If it failed return -1
        /// </summary>
        /// <returns></returns>
        internal int ReadPlayerHealth()
        {
            if (objectDump.GetPlayerPointer() == 0)
                objectDump.Dump();

            int health = mem.ReadInt((objectDump.GetPlayerPointer() + Offsets.ObjManager.HEALTH_OFFSET).ToString("X"));
            if (health < 0 && health > 50000)
                return -1;
            else
                return health;
        }

        internal bool ReadLootWindow()
        {
            int value = mem.ReadInt(Offsets.LOOT_WINDOW_OPEN);
            if (value == 0)
                return false;
            else
                return true;
        }
    }
}
