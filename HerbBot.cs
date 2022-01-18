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
        private const double DISTANCE_THRESHOLD = 3d;

        // If player closer than this we initiate loot action
        private const double LOOT_DISTANCE = 15d;

        private readonly MemoryReader mem;
        private List<Point> path;
        private BackgroundWorker worker;
        private Label statusLabel;
        private Loot looting;

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

            this.looting = new Loot();
        }

        internal void Start()
        {
            Console.WriteLine("Starting the bot ...");

            // Create a background worker and start moving along loaded path.
            if(!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Main part for the bot routine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            // go through the path
            for (int i = 0; i < path.Count; i++)
            {
                Point p = path[i];

                MoveToPoint(p);

                // Calculate percentage complete (how many points we have gone through)
                int percentComplete = (int)((float)i / (float)path.Count * 100);
                worker.ReportProgress(percentComplete);

                Console.WriteLine("Moving to point ...");
                while (DistanceToPoint(p) > DISTANCE_THRESHOLD)
                {
                    // while we haven't reached the point we should search for nearby herbs
                    ulong herb = mem.ReadNearbyHerb();
                    if(herb != 0)
                    {
                        Console.WriteLine("Searching for the nearby herb");
                        GameObject[] objects = mem.ReadObjects();

                        // search the object d wump list
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
                                Console.WriteLine("Successfully looted herb");

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

                // Check if we have a cancel pending
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    Console.WriteLine("Stopping the bot ...");
                    break;
                }
            }

            Console.WriteLine("Path finished");
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

            // Set ctm action to 4
            mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_MOVE);
        }

        private void PickupHerb(ulong guid, Point p)
        {
            // set ctm stuff to pickup herb
            mem.SetCTMGUID(guid);
            mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_LOOT);
            Console.WriteLine("Looting herb soon ...");

            // time it will take for player to move last distance to herb and gather it.
            System.Threading.Thread.Sleep(8000);
            Console.WriteLine("Begin looting ...");
            looting.LootAll();
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Subroutine for leaving path and going to gather herb
        /// </summary>
        private void GatherHerb(ulong guid, Point herbPosition)
        {
            MoveToPoint(herbPosition);

            // wait until we're close enough for pickup action
            while (Point.Distance(mem.ReadPlayerPosition(), herbPosition) > LOOT_DISTANCE) {}

            // start pickup procedure
            PickupHerb(guid, herbPosition);
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
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusLabel.Text = "running ... " + e.ProgressPercentage + "%";
            statusLabel.ForeColor = Color.DarkOrange;
        }

        private void Resume() {}
        private void Pause() {}

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
    }
}
