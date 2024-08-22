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
        public MeleeAttack(Soul soul, Character characterToAttack) : base(MoveKey.a.ToString(), $"Attack {characterToAttack.Name} with {soul.Character.GetMainHand().Name}.", characterToAttack, RangeType.CloseSingleTarget)
        {
            Character character = soul.Character;

            Action = () =>
            {
                Console.WriteLine("BufferText from Action is _" + character.Soul.BufferText + "_ at " + DateTime.Now);
                if (!TryAttack(character, characterToAttack, character.AttackWithMainHand)) return;
                
            };
        }
    }
}
