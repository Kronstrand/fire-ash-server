using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server
{
    [Serializable]
    internal class RollModifier
    {
        public RollType Type;
        public int Modifer;

        public RollModifier(RollType rollType, int modifer)
        {
            Type = rollType;
            Modifer = modifer;
        }
    }
}
