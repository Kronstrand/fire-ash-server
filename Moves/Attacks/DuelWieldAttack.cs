using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Enums;

namespace fire_ash_server.Moves.Attacks
{
    internal class DuelWieldAttack : Attack
    {
        public DuelWieldAttack(Soul soul, Character characterToAttack) : base(MoveKey.aa.ToString(), $"Attack {characterToAttack.Name}. (Dual Wield)", characterToAttack, RangeType.CloseSingleTarget)
        {
            if (characterToAttack == null)
            {
                throw new ArgumentNullException($"{soul.Character.Name} has no target to attack with DualWield");
            }

            Character character = soul.Character;

            Action = () =>
            {
                if (!TryAttack(character, characterToAttack, character.AttackWithMainHand)) return;
                if (characterToAttack.Dead)
                    return;
                if (!TryAttack(character, characterToAttack, character.AttackWithOffhand)) return;
            };
        }



    }
}
