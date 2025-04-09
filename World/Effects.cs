using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World
{
    [Serializable]
    internal static class Effects
    {
        public static Effect Get(EffectKey key)
        {
            Effect effect;
            if (key == EffectKey.BrightLight)
            {
                effect = new Effect(Description(EffectKey.BrightLight));
                effect.LightRadiusModifer = Light.Bright;
                return effect;
            }
            else if (key == EffectKey.DimLight)
            {
                effect = new Effect(Description(EffectKey.DimLight));
                effect.LightRadiusModifer = Light.Dim;
                return effect;
            }
            else if (key == EffectKey.LightPointer)
            {
                effect = new Effect(Description(EffectKey.LightPointer));
                effect.LightPointerModifer = Light.Bright;
                effect.LightRadiusModifer = Light.Dim;
                return effect;
            }
            else if (key == EffectKey.Darkvision)
            {
                effect = new Effect(Description(EffectKey.Darkvision));
                return effect;
            }
            throw new Exception("No effect was found for key " + key);
            
        }
    }
}
