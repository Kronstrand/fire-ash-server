using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves.Attacks
{
    internal class MeleeAttack : Attack
    {
        public MeleeAttack(Soul soul, Character characterToAttack) : base("a", $"Attack {characterToAttack.Name} with {soul.Character.GetMainHand().Name}.", characterToAttack, false)
        {
            Character character = soul.Character;

            Action = () =>
            {
                if (!TryAttack(character, characterToAttack, character.AttackWithMainHand)) return;
            };
        }
    }
}
