using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal class Effect
    {
        public string Name;
        public List<RollModifier> rollModifiers = new List<RollModifier>();

        public Effect(string name)
        {
            Name = name;
        }
    }
}
