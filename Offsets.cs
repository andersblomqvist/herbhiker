using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herbhiker
{
    static class Offsets
    {
        public struct Player
        {
            public const string POS_X = "base+0x6DF4E4";
            public const string POS_Y = "base+0x6DF4E8";
            public const string POS_Z = "base+0x6DF4EC";
        }

        public struct CTM
        {
            public const string DESTINATION_X = "base+0x8A1264";
            public const string DESTINATION_Y = "base+0x8A1268";
            public const string DESTINATION_Z = "base+0x8A126C";

            public const string DISTANCE = "0xCA11E4";

            public const string ACTION = "0x00CA11F4";
            public const string GUID = "0x00CA11F8";

            public const string ACTION_TYPE_IDLE = "13";
            public const string ACTION_TYPE_LOOT = "7";
            public const string ACTION_TYPE_MOVE = "4";
        }

        public struct ObjManager
        {
            public const string CLIENT_CONNECTION = "0xC79CE0";
            public const string OBJ_MANAGER = "0x2ED0"; // clientConn + 2ed0
            public const string LIST_START = "0xAC"; // objManager + AC
        }

        public const string NODE_GUID = "base+0x007F8120";
    }
}
