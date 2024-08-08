using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Moves.Attacks
{
    internal class RangedAttack : Attack
    {
        public RangedAttack(Soul soul, Character characterToAttack) : base(MoveKey.ra.ToString(), GetName(soul.Character, characterToAttack), characterToAttack, RangeType.RangeSingleTarget)
        {
            Character character = soul.Character;

            Action = () =>
            {
                if (!TryAttack(character, characterToAttack, character.AttackWithRanged)) return;
            };
        }

        public static string GetName(Character character, Character characterToAttack)
        {
            Weapon? weapon = character.GetRangedWeapon();

            if (weapon != null)
                return $"Attack {characterToAttack.Name} with {weapon.Name}.";

            return $"Attack {characterToAttack.Name} with ranged weapon."; //should not happen
        }
    }
}
