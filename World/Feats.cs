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
    internal static class Feats
    {
        public static Feat? GetWithoutMoves(string key, Soul soul) //soul is not needed
        {
            return Get(key, soul, null, true);
        }
        public static Feat? Get(string key, Soul soul, Prop? target)
        {
            return Get(key, soul, target, false);
        }
        private static Feat? Get(string key, Soul soul, Prop? target, bool skipMove)
        {
            Feat feat;

            if (key == Description(FeatKey.PickPocket))
            {
                if (CreateFeatAndSkipMove(key, out feat, skipMove)) return feat;

                if (target is Character character)
                {
                    feat.Moves.Add(new PickPocket(soul, character));
                    return feat;
                }
            }
            else if (key == Description(FeatKey.Stealth))
            {
                if (CreateFeatAndSkipMove(key, out feat, skipMove)) return feat;

                feat.Moves.Add(new Stealth(soul));
                feat.Moves.Add(new UnStealth(soul));
                return feat;
            }
            else if (key == Description(FeatKey.MeleeAttack))
            {
                if (CreateFeatAndSkipMove(key, out feat, skipMove)) return feat;

                if (target is Character character)
                {
                    feat.Moves.Add(new MeleeAttack(soul, character));
                    return feat;
                }
            }
            else if (key == Description(FeatKey.DualWield))
            {
                if (CreateFeatAndSkipMove(key, out feat, skipMove)) return feat;

                if (target is Character character)
                {
                    feat.Moves.Add(new DuelWieldAttack(soul, character));
                    return feat;
                }
            }
            else if (key == Description(FeatKey.RangedAttack))
            {
                if (CreateFeatAndSkipMove(key, out feat, skipMove)) return feat;

                if (target is Character character)
                {
                    feat.Moves.Add(new RangedAttack(soul, character));
                    return feat;
                }
            }
            else if (key == Description(FeatKey.DarkVision))
            {
                CreateFeatAndSkipMove(key, out feat, skipMove);
                feat.AddEffect(EffectKey.Darkvision);
                return feat;
            }
            else
            {
                throw new Exception("No feat was found for key " + key);
            }

            return null;
        }

        private static bool CreateFeatAndSkipMove(string key, out Feat feat, bool skipMove)
        {
            feat = new Feat(key);
            return skipMove;
        }
    }
}
