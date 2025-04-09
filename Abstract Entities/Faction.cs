using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server
{
    [Serializable]
    internal class Faction
    {
        public string Name;
        
        public Faction(string name)
        {
            Name = name;
        }

        public static Faction Get(FactionKey key)
        {
            return Program.WorldSoul.GetFaction(key);
        }
    }
}
