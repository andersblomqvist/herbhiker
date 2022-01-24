using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Herbhiker;

namespace HerbHikerApp
{
    public class HerbBot
    {
        // How near player need to be in order to count as point is reached.
        private const double DISTANCE_THRESHOLD = 3.0d;

        private readonly MemoryReader mem;
        private readonly BackgroundWorker worker;
        private readonly Label statusLabel;

        private List<Point> path;
        private bool loop;

        // Herbs we've visited but failed to pickup, due to mobs or other stuff.
        // We don't want to go there again and get stuck
        private List<ulong> blacklist;

        private readonly Random rand;

        public HerbBot(MemoryReader mem, List<Point> path, BackgroundWorker worker, Label statusLabel)
        {
            this.mem = mem;
            this.path = path;
            this.worker = worker;
            this.statusLabel = statusLabel;

            Console.WriteLine("Loaded bot with path.");
            this.statusLabel.Text = "ready";
            this.statusLabel.ForeColor = Color.Green;

            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new DoWorkEventHandler(DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PathFinished);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);

            blacklist = new List<ulong>();
            rand = new Random();
        }

        internal void Start(bool loop)
        {
            Console.WriteLine("Starting the bot ...");

            this.loop = loop;

            // Create a background worker and start moving along loaded path.
            if(!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        /// <summary>
        /// Main part for the bot routine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (loop)
                while (true)
                {
                    // Check if we have a cancel pending
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        Console.WriteLine("Stopping the bot ...");
                        break;
                    }

                    FollowPath(e);

                    // clear blacklist after iteration
                    blacklist = new List<ulong>();
                }
            else
                FollowPath(e);

            Console.WriteLine("Path finished");
        }

        /// <summary>
        /// Make player move along path and search for herbs.
        /// </summary>
        /// <param name="e"></param>
        private void FollowPath(DoWorkEventArgs e)
        {
            mem.ToggleNoclip(true);
            mem.ClearNearbyHerb();

            // Begin at closest point in path
            int closestPointToPlayer = ClosestPointInPath();

            // go through the path (start at the point which is closest)
            for (int i = closestPointToPlayer; i < path.Count; i++)
            {
                Point p = path[i];
                MoveToPoint(p);

                // Calculate percentage complete (how many points we have gone through)
                int percentComplete = (int)((float)i / (float)path.Count * 100);
                worker.ReportProgress(percentComplete);

                Console.WriteLine("Moving to point i={0}", i);
                while (DistanceToPoint(p) > DISTANCE_THRESHOLD)
                {
                    RefreshAction(p);

                    // Check if we have a cancel pending
                    if (worker.CancellationPending)
                    {
                        e.Cancel = true;
                        i = path.Count;
                        Console.WriteLine("Stopping the bot ...");
                        break;
                    }

                    // while we haven't reached the point we should search for nearby herbs
                    ulong herb = mem.ReadNearbyHerb(blacklist);
                    if (herb != 0)
                    {
                        Console.WriteLine("Searching for the nearby herb");
                        GameObject[] objects = mem.ReadObjects();

                        // search the object dump list
                        for (int j = 0; j < ObjectDump.SIZE; j++)
                        {
                            // when object is null there's no more objects in array
                            if (objects[j] == null)
                                break;
                            // if a herb is found we leave path and go pick it up
                            else if (objects[j].guid == herb)
                            {
                                Console.WriteLine("Found nearby herb!");
                                GatherHerb(herb, objects[j].position);

                                // sets the herb guid to 0. Will be set to new id if another herb is close
                                mem.ClearNearbyHerb();
                                break;
                            }
                        }

                        // after pickup we return to path
                        MoveToPoint(p);
                        Console.WriteLine("Moving back to point in path");
                    }
                }
                Console.WriteLine("Reached point, going to next!");
            }
        }

        /// <summary>
        /// Set the CTM destionation and action for moving to a point.
        /// The action type is set to 4 (MOVE)
        /// </summary>
        /// <param name="p"></param>
        private void MoveToPoint(Point p)
        {
            // Set ctm destination x, y, z
            mem.SetDestionation(p);

            System.Threading.Thread.Sleep(250);

            // Set ctm action to 4
            mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_MOVE);
        }

        /// <summary>
        /// Do the actual pickup and looting of the herb
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="herbPosition"></param>
        private void PickupHerb(ulong guid, Point herbPosition)
        {
            int healthBefore = mem.ReadPlayerHealth();
            int healthAfter;

            float r1 = (float)(rand.NextDouble() - 0.5d) * 15;
            float r2 = (float)(rand.NextDouble() - 0.5d) * 15;

            // now move to herb, needs to be a bit off in order for loot action to work
            Point aboveHerb = new Point(herbPosition.x + r1, herbPosition.y + r2, herbPosition.z + 10f);
            MoveToPoint(aboveHerb);
            Console.WriteLine("Going above ground!");
            while (Point.Distance(mem.ReadPlayerPosition(), aboveHerb) > DISTANCE_THRESHOLD)
                RefreshAction(aboveHerb);

            mem.ToggleNoclip(false);

            Console.WriteLine("Above herb, start grounding ...");

            // make player grounded.
            XGround(250);
            XGround(250);
            XGround(350);

            Console.WriteLine("Looting herb soon ...");

            // set ctm stuff to pickup herb
            mem.SetCTMGUID(guid);
            mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_LOOT);

            // time it will take for player to move last distance to herb and gather it.
            int time = 0;
            bool success = false;
            bool dangerous = false;
            while(time < 45)
            {
                bool open = mem.ReadLootWindow();
                if(open)
                {
                    Console.WriteLine("Loot window open!");
                    Loot.KeyDown("World of Warcraft", Loot.VirtualKeyStates.VK_1);
                    System.Threading.Thread.Sleep(50);
                    Loot.KeyUp("World of Warcraft", Loot.VirtualKeyStates.VK_1);
                    System.Threading.Thread.Sleep(50);
                    Console.WriteLine("Successfully looted herb!");
                    success = true;
                    break;
                }
                System.Threading.Thread.Sleep(100);
                time += 1;

                // quit if we are losing health
                healthAfter = mem.ReadPlayerHealth();
                if(healthAfter + 100 < healthBefore)
                {
                    Console.WriteLine("Health check: before={0}, after={1}", healthBefore, healthAfter);
                    dangerous = true;
                    break;
                }
            }

            // blacklist this herb if no success and we've lost health!
            if (!success && dangerous)
            {
                blacklist.Add(guid);
                Console.WriteLine("Blacklisting: {0} - we took damage!", guid.ToString("X"));
                Console.WriteLine("Leaving this herb");
            }
        }


        /// <summary>
        /// Press X key for ms millisec. X key is move down which makes us grounded.
        /// </summary>
        /// <param name="ms"></param>
        private void XGround(int ms)
        {
            Loot.KeyDown("World of Warcraft", Loot.VirtualKeyStates.VK_X);
            System.Threading.Thread.Sleep(ms);
            Loot.KeyUp("World of Warcraft", Loot.VirtualKeyStates.VK_X);
        }

        /// <summary>
        /// Subroutine for leaving path and going to gather herb
        /// </summary>
        private void GatherHerb(ulong guid, Point herbPosition)
        {
            // Move directly under herb
            Point underHerb = new Point(herbPosition.x, herbPosition.y, herbPosition.z - 25f);
            MoveToPoint(underHerb);
            while (Point.Distance(mem.ReadPlayerPosition(), underHerb) > DISTANCE_THRESHOLD)
                RefreshAction(underHerb);

            Console.WriteLine("Under herb now.");

            // start pickup procedure when we are under it
            PickupHerb(guid, herbPosition);

            // go back under map again
            mem.ToggleNoclip(true);
            MoveToPoint(underHerb);
            while (Point.Distance(mem.ReadPlayerPosition(), underHerb) > DISTANCE_THRESHOLD)
                RefreshAction(underHerb);
        }

        private void PathFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("Successfully stopped the bot");
                statusLabel.Text = "finished";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                // we succeeded.
                statusLabel.Text = "finished";
                statusLabel.ForeColor = Color.Green;
            }

            mem.ToggleNoclip(false);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusLabel.Text = "running ... " + e.ProgressPercentage + "%";
            statusLabel.ForeColor = Color.DarkOrange;
        }

        internal void Stop()
        {
            statusLabel.Text = "stopping ...";
            worker.CancelAsync();
        }

        internal void LoadPath(List<Point> path)
        {
            Console.WriteLine("Loading in new path to bot");
            this.path = path;
        }

        /// <summary>
        /// Checks the distance between current player position and CTM destination
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Distance from player to next point in path</returns>
        private double DistanceToPoint(Point p)
        {
            Point player = mem.ReadPlayerPosition();
            return Point.Distance(p, player);
        }

        /// <summary>
        /// Sometimes when crossing meshes with noclip the CTM action will set to
        /// idle again while we're flying and not reached point. So refresh it!
        /// </summary>
        /// <param name="p"></param>
        private void RefreshAction(Point p)
        {
            int action = mem.ReadCTMAction();
            if (action == 0 || action == 13)
                MoveToPoint(p);
        }

        /// <summary>
        /// Searches the path and finds the point in path which is currently closest
        /// to the player. It will return that point's index.
        /// </summary>
        /// <returns>Index of closest point to player</returns>
        private int ClosestPointInPath()
        {
            double min = double.MaxValue;
            int minIndex = -1;
            Point player = mem.ReadPlayerPosition();

            for(int i = 0; i < path.Count; i++)
            {
                double d = Point.Distance(path[i], player);
                if(d < min)
                {
                    min = d;
                    minIndex = i;
                }
            }

            // If the closest were at the end, we just start from the beginning
            if (path.Count - 10 < minIndex)
                return 0;
            else
                return minIndex;
        }
    }
}
