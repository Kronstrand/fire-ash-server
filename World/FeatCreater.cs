using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;

namespace fire_ash_server.World
{
    internal static class FeatCreater
    {
        public static Feat? Get(string key, Soul soul, Prop? target)
        {
            Feat feat;
            if (key == Description(FeatKey.PickPocket))
            {
                if (target is Character)
                {
                    feat = new Feat(Description(FeatKey.PickPocket));
                    feat.Moves.Add(new PickPocket(soul, (Character)target));
                    return feat;
                }
                return null;
            }
            else if (key == Description(FeatKey.Stealth))
            {
                feat = new Feat(Description(FeatKey.Stealth));
                feat.Moves.Add(new Stealth(soul));
                feat.Moves.Add(new UnStealth(soul));
                return feat;
            }
            else if (key == Description(FeatKey.MeleeAttack))
            {
                if (target is Character)
                {
                    feat = new Feat(Description(FeatKey.MeleeAttack));
                    feat.Moves.Add(new MeleeAttack(soul, (Character)target));
                    return feat;
                }
                return null;
            }
            else if (key == Description(FeatKey.DualWield))
            {
                if (target is Character)
                {
                    feat = new Feat(Description(FeatKey.DualWield));
                    feat.Moves.Add(new DuelWieldAttack(soul, (Character)target));
                    return feat;
                }
                return null;
            }
            throw new Exception("no feat was found for key " +  key);        
        }
    }
}
