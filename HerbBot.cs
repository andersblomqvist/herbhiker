using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerbHikerApp
{
    public class HerbBot
    {
        private readonly MemoryReader mem;

        private List<Point> path;

        public HerbBot(MemoryReader mem, List<Point> path)
        {
            this.mem = mem;
            this.path = path;
        }

        internal void Start()
        {

        }

        internal void DoWork()
        {

        }

        internal void Cancel()
        {

        }
    }
}
