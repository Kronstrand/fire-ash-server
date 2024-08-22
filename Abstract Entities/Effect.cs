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
        public Light LightRadiusModifer = Light.None;
        public Light LightPointerModifer = Light.None;

        public Effect(string name)
        {
            Name = name;
        }
    }
}
