using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server
{
    internal class Faction
    {
        public string Name;

        public Faction() { }
        
        public Faction(string name)
        {
            Name = name;
        }

        public bool KeyIs(FactionKey factionKey)
        {
            return Name == Description(factionKey);
        }

        public static Faction Get(FactionKey key)
        {
            return Program.WorldSoul.GetFaction(key);
        }
    }
}
