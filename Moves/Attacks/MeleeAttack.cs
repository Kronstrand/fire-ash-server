using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Moves.Attacks
{
    [Serializable]
    internal class MeleeAttack : Attack
    {
        Weapon? weapoon = null;

        public MeleeAttack(Soul soul, Character characterToAttack) : base(MoveKey.a.ToString(), $"Attack {characterToAttack.Name} with {soul.Character.GetMainHand().Name}.", characterToAttack, RangeType.CloseSingleTarget)
        {
            Character character = soul.Character;

            Action = () =>
            {
                Console.WriteLine("BufferText from Action is _" + character.Soul.BufferText + "_ at " + DateTime.Now);
                TryAttack(character, characterToAttack, weapoon, character.AttackWithMainHand);
                return Task.CompletedTask;
                
            };
        }
    }
}
