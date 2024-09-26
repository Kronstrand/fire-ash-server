using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves.Attacks
{
    internal class BiteAttack : Attack
    {
        public BiteAttack(Soul soul, Character characterToAttack) : base(MoveKey.ba.ToString(), $"Attack {characterToAttack.Name} with {soul.Character.GetTeethWeapon().Name}.", characterToAttack, RangeType.CloseSingleTarget)
        {
            Character character = soul.Character;

            Action = () =>
            {
                if (!TryAttack(character, characterToAttack, character.AttackWithTeeth)) return;
                //add poison
                
            };
        }
    }
}
