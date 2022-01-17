using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerbHikerApp
{
    public class GameObject
    {
        public ulong guid;
        public Point position;

        public GameObject() {}

        public GameObject(ulong guid)
        {
            this.guid = guid;
        }

        public GameObject(ulong guid, Point p)
        {
            this.guid = guid;
            position = p;
        }
    }
}
