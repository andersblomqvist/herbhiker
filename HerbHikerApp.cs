using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace HerbHikerApp
{
    public partial class HerbHiker : Form
    {
        private Generator gen;
        private HerbBot bot;
        private MemoryReader mem;

        private BackgroundWorker worker;
        private bool generator;

        public HerbHiker()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("en-US", true);
            mem = new MemoryReader();
            worker = new BackgroundWorker();
            generator = false;
            ToggleGeneratorCheckBox.Enabled = false;
        }

        private void CheckForGameButton(object sender, EventArgs e)
        {
            if (mem.OpenProc())
            {
                GameStatusLabel.Text = "Game Found";
                GameStatusLabel.ForeColor = System.Drawing.Color.Green;
                mem.Init();
                gen = new Generator(mem);
                ToggleGeneratorCheckBox.Enabled = true;
                NoclipCheckBox.Enabled = true;
            }
            else
            {
                Console.WriteLine("Failed to find process: Wow.exe");
                GameStatusLabel.Text = "Game Not Found";
                GameStatusLabel.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            bot.Start(LoopPathCheckbox.Checked);
        }

        private void TestPathButton_Click(object sender, EventArgs e)
        {
            gen.TestPath();
        }

        private void SavePathButton_Click(object sender, EventArgs e)
        {
            if (!gen.PathExists())
            {
                Console.WriteLine("Path does not exist, can't save anything!");
                return;
            }

            // open file dialog
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Textdocument|*.txt";
            dialog.Title = "Save generated path";
            dialog.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (dialog.FileName != "")
            {
                // get file stream from generator
                File.WriteAllText(dialog.FileName, gen.WritePath());
                Console.WriteLine("Successfully save path to file: {0}", dialog.FileName);
            }
        }

        private void LoadPathButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Textdocument|*.txt";
            dialog.Title = "Open path";
            dialog.ShowDialog();

            if(dialog.FileName != "")
            {
                Stream stream = dialog.OpenFile();
                List<Point> path = new List<Point>();
                using (StreamReader reader = new StreamReader(stream))
                {
                    // parse the string: <float>, <float>, <float>
                    while(!reader.EndOfStream)
                    {
                        string str = reader.ReadLine();
                        string[] values = str.Split(' ');
                        float x = float.Parse(values[0], CultureInfo.DefaultThreadCurrentCulture);
                        float y = float.Parse(values[1], CultureInfo.DefaultThreadCurrentCulture);
                        float z = float.Parse(values[2], CultureInfo.DefaultThreadCurrentCulture);
                        Point p = new Point(x, y, z);
                        Console.WriteLine("Read point: {0}", p);
                        path.Add(p);
                    }
                }
                
                Console.WriteLine("Successfully loaded path from file: {0}", dialog.FileName);

                string fname = dialog.FileName.Split('\\').Last();
                if (fname.Length > 29)
                    fname = fname.Substring(0, 29) + "...";
                LoadedPathLabel.Text = fname;

                if (generator)
                    gen.LoadPath(path);

                if (bot == null)
                    bot = new HerbBot(mem, path, worker, BotStatusLabel);

                BotControls.Visible = true;
            }
        }

        private void ObjectDumpButton_Click(object sender, EventArgs e)
        {
            mem.ReadObjects();
        }

        private void ToggleGeneratorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(generator)
            {
                // turn off generator
                Console.WriteLine("Disabling the waypoint generator ...");
                gen.Disable();
                PathControlGroup.Visible = false;
                generator = false;
            }
            else
            {
                // start generator
                Console.WriteLine("Starting the waypoint generator ...");
                gen.Enable();
                PathControlGroup.Visible = true;
                generator = true;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            bot.Stop();
        }

        private void NoclipCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(NoclipCheckBox.Checked)
                mem.ToggleNoclip(true);
            else
                mem.ToggleNoclip(false);
        }
    }
}
