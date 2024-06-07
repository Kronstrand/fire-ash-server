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
        public Attack(string key, string description, Character characterToAttack, bool isRanged) : base(key, description, characterToAttack)
        {
            Ranged = isRanged;
            EnablesCombat = true;
        }

        public override bool IsValid(Soul soul)
        {
            if (soul.Character.LookAt == null)
                return false;

            if (soul.Character.LookAt.GetType() == typeof(Character))
            {
                Character characterToAttack = (Character)soul.Character.LookAt;
                if (!Ranged && soul.Character.IsInGroupWith(characterToAttack) != true)
                    return false;

                if (!characterToAttack.Dead && !characterToAttack.IsHidden())
                    return true;            
            }
            return false;
        }

        public bool TryAttack(Character character, Character characterToAttack, Action<Character> attackAction)
        {
            if (!character.AttackTargetIsWithinReach(characterToAttack))
            {
                EnablesCombat = false;
                return false;
            }
            attackAction(characterToAttack);
            return true;
        }
    }
}
