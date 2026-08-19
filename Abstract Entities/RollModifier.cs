using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server
{
    internal class RollModifier
    {
        public RollType Type;
        public int Modifer;

        public RollModifier(RollType rollType, int modifer)
        {
            Type = rollType;
            Modifer = modifer;
        }

        public override string ToString() 
        {
            string toString = $"{Description(Type)} ";
            if (Modifer < 0)
                toString += "-";
            else
                toString += "+";
            toString += Modifer;

            return toString;
        }
    }
}
