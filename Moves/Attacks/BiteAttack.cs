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
    internal class BiteAttack : Attack
    {
        Weapon? weapon = null;
        public BiteAttack(Soul soul, Character characterToAttack) : base(MoveKey.ba.ToString(), $"Attack {characterToAttack.Name} with {soul.Character.GetTeethWeapon().Name}.", characterToAttack, RangeType.CloseSingleTarget)
        {
            Character character = soul.Character;

            Action = () =>
            {
                TryAttack(character, characterToAttack, weapon, character.AttackWithTeeth);
                return Task.CompletedTask;
            };
        }
    }
}
