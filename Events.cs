using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;

namespace fire_ash_server
{
    internal static class Events
    {
        public static void AddCharacterMoveFromCharacterAndIsAttacked(Character attackingCharacter)
        {
            attackingCharacter.AddOnBeforeMoveFromEvent((soul) => {
                if (attackingCharacter.InCombat)
                    return false;
                if (attackingCharacter.Dead)
                    return false;

                Move move = new MeleeAttack(attackingCharacter.Soul, soul.Character);
                attackingCharacter.Soul.Execute(ref move, soul.Character);

                //attackingCharacter.AttackWithMainHand(soul.Character);

                //soul.Character.EnableCombatWith = attackingCharacter;

                return true; 
            }, 
            true);
        }
    }
}
