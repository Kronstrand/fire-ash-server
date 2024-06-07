using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server
{
    internal class Die
    {
        public int NumberOfDies { get; set; }
        public int Sides { get; set; }
        public Die(int numberOfDies, int numberOfSides)
        {
            NumberOfDies = numberOfDies;
            Sides = numberOfSides;
        }
    }
}
