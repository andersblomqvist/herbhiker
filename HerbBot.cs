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

        private readonly MemoryReader mem;
        private List<Point> path;
        private BackgroundWorker worker;
        private Label statusLabel;

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

                // Set ctm destination x, y, z
                mem.SetDestionation(p);

                // Set ctm action to 4
                mem.SetCTMAction(Offsets.CTM.ACTION_TYPE_MOVE);

                // Calculate percentage complete (how many points we have gone through)
                int percentComplete = (int)((float)i / (float)path.Count * 100);
                worker.ReportProgress(percentComplete);

                Console.WriteLine("Moving to point ...");
                while (DistanceToPoint(p) > DISTANCE_THRESHOLD)
                {
                    // while we've not reached the point we should search for nearby herbs
                    // if a herb is found we leave path and go pick it up
                    // after pickup we return to path
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
            double distance = Math.Sqrt(
                (p.x - player.x) * (p.x - player.x) +
                (p.y - player.y) * (p.y - player.y) +
                (p.z - player.z) * (p.z - player.z));
            return distance;
        }
    }
}
