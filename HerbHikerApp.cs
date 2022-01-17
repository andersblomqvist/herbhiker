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

        public HerbHiker()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("en-US", true);
            mem = new MemoryReader();
        }

        private void StartGenerator()
        {
            Console.WriteLine("Starting the waypoint generator ...");
            gen = new Generator(mem);

            PathControlGroup.Visible = true;
        }

        private void StartBot()
        {
            Console.WriteLine("Starting the bot ...");
        }

        private void CheckForGameButton(object sender, EventArgs e)
        {
            if (mem.OpenProc())
            {
                GameStatusLabel.Text = "Game Found";
                GameStatusLabel.ForeColor = System.Drawing.Color.Green;
                StartButton.Enabled = true;
                mem.Init();
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
            if (ToggleGeneratorCheckBox.Checked) StartGenerator();
            else StartBot();
        }

        private void TestPathButton_Click(object sender, EventArgs e)
        {
            gen.TestPath();
        }

        private void BotWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
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
                gen.LoadPath(path);
                Console.WriteLine("Successfully loaded path from file: {0}", dialog.FileName);
                bot = new HerbBot(mem, path);
            }
        }

        private void ButtonHoverToolTip(object sender, PopupEventArgs e) { }

        private void ObjectDumpButton_Click(object sender, EventArgs e)
        {
            mem.ReadObjects();
            Console.WriteLine("GameObject list:");
            mem.PrintObjects();
        }
    }
}
