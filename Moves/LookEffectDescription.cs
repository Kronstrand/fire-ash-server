using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    [Serializable]
    internal class LookEffectDescription : Move
    {
        public LookEffectDescription(Soul soul, string effectName) : base(MoveKey.le.ToString(), $"Check {effectName}.")
        {
            AllowedInTrade = true;
            Type = MoveType.MinorAction;
            Action = async () => { 
                await soul.SendAsync(GetEffectDescription(effectName)); 
            };
        }

        private static string GetEffectDescription(string effectName)
        {
                if(Description(EffectKey.DimLight) == effectName) return 
                        "Dim light radiates from the object, " +
                        "gently revealing the shapes of nearby objects " +
                        "and creatures concealed in the shadows.";
                if (Description(EffectKey.BrightLight) == effectName) return
                        "Bright light radiates from the object, " +
                        "illuminating nearby objects and creatures, " +
                        "pushing shadows back in its immediate vicinity.";
                if (Description(EffectKey.LightPointer) == effectName) return
                        "A focused beam of bright light projects from the object, " +
                        "illuminating a specific spot with precision, " +
                        "cutting through darkness to reveal details in the targeted area.";
                if (Description(EffectKey.Darkvision) == effectName) return
                        "Darkvision grants the ability to see in complete darkness as if it were bright light, " +
                        "revealing vivid details and illuminating areas otherwise concealed in shadow.";
            return "";

        }

    }
}
