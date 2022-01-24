using System;
using System.Runtime.InteropServices;

namespace HerbHikerApp
{
    public class Loot
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Virtual Keys
        /// </summary>
        public enum VirtualKeyStates : int
        {
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            //
            VK_0 = 0x30,
            VK_1 = 0x31,
            VK_2 = 0x32,
            VK_3 = 0x33,
            VK_4 = 0x34,
            VK_5 = 0x35,
            VK_6 = 0x36,
            VK_7 = 0x37,
            VK_8 = 0x38,
            VK_9 = 0x39,

            VK_X = 0x58,
            VK_SPACEBAR = 0x20
        }

        /// <summary>
        /// Sends a Keydown message(0x100) to the specified window with a Virtual Key
        /// </summary>
        /// <param name="winTitle">Window Title</param>
        /// <param name="Key">Virtual Key to Send</param>
        public static void KeyDown(string winTitle, VirtualKeyStates Key)
        {
            IntPtr hWnd = FindWindow(null, winTitle);
            SendMessage(hWnd, 0x100, (int)Key, 0);
        }

        /// <summary>
        /// Sends a Keydup message(0x101) to the specified window with a Virtual Key
        /// </summary>
        /// <param name="winTitle">Window Title</param>
        /// <param name="Key">Virtual Key to Send</param>
        public static void KeyUp(string winTitle, VirtualKeyStates Key)
        {
            IntPtr hWnd = FindWindow(null, winTitle);
            SendMessage(hWnd, 0x101, (int)Key, 0);
        }
    }
}
