using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;

namespace fire_ash_server
{
    internal class Feat
    {
        public string Name;
        public List<Move> Moves = new List<Move>();
        public List<Effect> Effects = new List<Effect>();

        public Feat() { }
        public Feat(string name)
        {
            Name = name;
        }

        public void AddEffect(EffectKey effectKey)
        {
            Effects.Add(World.Effects.Get(effectKey));
        }
    }
    
}
