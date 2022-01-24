using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Herbhiker;
using Memory;

namespace HerbHikerApp
{
    /// <summary>
    /// Generates a flying route my manually moving the character ingame
    /// and manually adding waypoints by hitting global hotkey
    /// </summary>
    public class Generator
    {
        private readonly GlobalHotKey hook = new GlobalHotKey();

        private readonly MemoryReader Mem;
        private List<Point> points;

        private bool enabled;

        public Generator(MemoryReader mem)
        {
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(HotKeyPressed);
            this.Mem = mem;
            enabled = true;
            points = new List<Point>();
        }

        internal void Disable()
        {
            enabled = false;
            hook.UnregisterHotKey();
        }

        internal void Enable()
        {
            enabled = true;
            hook.RegisterHotKey(ModifierKeys.Shift, Keys.E);
        }

        private void SavePoint()
        {
            // Read (x,y,z) and save it as point nr n.
            Point p = Mem.ReadPlayerPosition();
            points.Add(p);
            Console.WriteLine("point={0}\t\t{1}", points.Count, p);
        }

        private void HotKeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (!enabled)
                return;
            SavePoint();
        }

        internal void TestPath()
        {
            foreach(Point p in points)
            {
                // Set ctm destination x, y, z
                Mem.SetDestionation(p);

                // Set ctm action to 4 (int32)
                Mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_MOVE);

                // wait until we've reached the point.
                Console.WriteLine("Moving to point ...");
                while(DistanceToPoint(p) > 3) {}
                Console.WriteLine("Reached point, going to next!");
            }

            Console.WriteLine("Finished testing path!");
        }

        /// <summary>
        /// Checks the distance between current player position and CTM destination
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Distance from player to next point in path</returns>
        private double DistanceToPoint(Point p)
        {
            Point player = Mem.ReadPlayerPosition();
            double distance = Math.Sqrt(
                (p.x - player.x) * (p.x - player.x) +
                (p.y - player.y) * (p.y - player.y) +
                (p.z - player.z) * (p.z - player.z));
            return distance;
        }

        /// <summary>
        /// Checks if a path is loaded or not
        /// </summary>
        /// <returns>True if there is a path to run, otherwise false</returns>
        internal bool PathExists()
        {
            if (points.Count > 0) return true;
            else return false;
        }

        /// <summary>
        /// Loops through each point in the path and writes its coords to the string
        /// One point per line is written
        /// </summary>
        /// <returns></returns>
        internal string WritePath()
        {
            // loops through the points list and writes each coordinate to string
            var str = new StringBuilder();
            Console.WriteLine("Writing content to file:");
            foreach(Point p in points)
            {
                str.Append(p.ToString() + "\n");
                Console.WriteLine(p.ToString());
            }
            return str.ToString();
        }

        /// <summary>
        /// Init the path list from file.
        /// </summary>
        /// <param name="path">Path which should come from file.</param>
        internal void LoadPath(List<Point> path)
        {
            this.points = path;
        }
    }
}
