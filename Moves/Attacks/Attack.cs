using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves.Attacks
{
    internal class Attack : Move
    {
        public Attack(string key, string description, Character characterToAttack, RangeType rangeType) : base(key, description, characterToAttack)
        {
            Range = rangeType;
            EnablesCombat = true;
        }

        public override bool IsValid(Soul soul)
        {
            if (soul.Character.LookAt == null)
                return false;

            if (soul.Character.LookAt is Character)
            {
                Character characterToAttack = (Character)soul.Character.LookAt;
                if (Range == RangeType.CloseSingleTarget && soul.Character.IsInGroupWith(characterToAttack) != true)
                    return false;
                if (Range == RangeType.RangeSingleTarget && soul.Character.GetRangedWeapon() == null)
                    return false;

                if (!characterToAttack.Dead && !characterToAttack.IsHidden())
                    return true;            
            }
            return false;
        }

        public bool TryAttack(Character character, Character characterToAttack, Action<Character> attackAction)
        {
            if (!character.AttackTargetIsWithinReach(characterToAttack, Range))
            {
                EnablesCombat = false;
                return false;
            }
            attackAction(characterToAttack);
            return true;
        }
    }
}
