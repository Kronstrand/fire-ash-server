using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server
{
    internal class Effect
    {
        public string Name = "";
        public List<RollModifier> rollModifiers = new List<RollModifier>();
        public Light LightRadiusModifer = Light.None;
        public Light LightPointerModifer = Light.None;

        public Effect() { }

        public Effect(string name)
        {
            Name = name;
        }
    }
}
