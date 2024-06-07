using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Moves;

namespace fire_ash_server
{
    internal class Feat
    {
        public string Name;
        public List<Move> Moves = new List<Move>();

        public Feat(string name)
        {
            Name = name;
        }   
    }
}
