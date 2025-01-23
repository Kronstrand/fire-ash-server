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
            attackingCharacter.AddOnBeforeMoveFromEvent(async (soul) => {
                if (attackingCharacter.InCombat)
                    return false;
                if (attackingCharacter.Dead)
                    return false;

                Move move = new MeleeAttack(attackingCharacter.Soul, soul.Character);
                await move.Execute(attackingCharacter.Soul, soul.Character);

                return true; 
            }, 
            true);
        }
    }
}
