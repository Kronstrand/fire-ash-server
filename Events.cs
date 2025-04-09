using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

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
                if (soul.Character.IsHidden())
                    return false; //idealy this would would not trigger the event so it could be re-run..


                Move move = new MeleeAttack(attackingCharacter.Soul, soul.Character);
                await move.Execute(attackingCharacter.Soul, soul.Character);

                return true; 
            }, 
            true);
        }

        public static void AddCharacterMoveToAndIsrangedAttacked(Character attackingCharacter, Prop moveTo)
        {
            moveTo.AddOnAfterMoveToEvent(async (soul) =>
            {
                if (attackingCharacter.InCombat)
                    return false;
                if (attackingCharacter.Dead)
                    return false;
                if (soul.Character.IsHidden())
                    return false;
                if (!attackingCharacter.EquippedItems.TryGetValue(Enums.InventorySlot.Ranged, out Item? rangedWeaoon))
                    return false;

                Move move = new RangedAttack(attackingCharacter.Soul, soul.Character);
                await move.Execute(attackingCharacter.Soul, soul.Character);

                return true;
            },
            true);
        }
    }
}
