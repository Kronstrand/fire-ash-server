using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves.Attacks
{
    internal class Attack : Move
    {
        public Attack(string key, string description, Character characterToAttack, RangeType rangeType) : base(key, description, characterToAttack)
        {
            Range = rangeType;
            EnablesCombat = true;
        }

        public override bool IsValid(Soul soul)
        {
            /*if (soul.Character.LookAt == null)
                return false;

            if (soul.Character.LookAt is Character)
            {
                Character characterToAttack = (Character)soul.Character.LookAt;
                if (characterToAttack.GetLightState(soul.Character) == Light.Darkness)
                    return false;
                if (Range == RangeType.CloseSingleTarget && soul.Character.IsInGroupWith(characterToAttack) != true)
                    return false;
                if (Range == RangeType.RangeSingleTarget && soul.Character.GetRangedWeapon() == null)
                    return false;

                if (!characterToAttack.Dead && !characterToAttack.IsHidden())
                    return true;            
            }
            return false;*/
            if (Range == RangeType.RangeSingleTarget)
            {
                return AttackIsValid(soul, Range, soul.Character.GetRangedWeapon(), soul.Character.LookAt);
            }

            return AttackIsValid(soul, Range, null, soul.Character.LookAt);
        }

        public static void InvalidAttack(Soul soul)
        {
            _ = soul.SendAsync("There is no valid target.");
        }

        public static bool AttackIsValid(Soul soul, RangeType range, Weapon? rangedWeapon, Prop? target)
        {
            if (target == null)
                return false;

            if (target is Character)
            {
                Character characterToAttack = (Character)target;
                if (characterToAttack.GetLightState(soul.Character) == Light.Darkness)
                    return false;
                if (range == RangeType.CloseSingleTarget && soul.Character.IsInGroupWith(characterToAttack) != true)
                    return false;
                if (range == RangeType.RangeSingleTarget && rangedWeapon == null)
                    return false;

                if (!characterToAttack.Dead && !characterToAttack.IsHidden())
                    return true;
            }
            return false;
        }

        public bool TryAttack(Character character, Character characterToAttack, Weapon? weapon, Action<Character, Weapon?> attackAction)
        {
            if (!character.AttackTargetIsWithinReach(characterToAttack, Range))
            {
                EnablesCombat = false;
                return false;
            }
            if (characterToAttack.GetLightState(character) == Light.Dim)
            {
                Roll roll = new Roll(new Die(1, 100), 0, RollType.DimLightAttack, character);
                if (roll.GetSum() <= 15)
                {
                    character.CurrentRoom.BroadcastToSoulsInRoom(
                        GetDimLightFail(
                            character.Name, 
                            characterToAttack.Name));
                    return true;
                }
            }
            attackAction(characterToAttack, weapon);
            return true;
        }

        private static string GetDimLightFail(string attacker, string opponent)
        {
            List<string> missDescriptions = new List<string>
{
                $"{opponent} seems to vanish momentarily as {attacker} tries to find a good attack opportunity in the dim light.",
                $"The shadows shift, and {attacker} loses sight of {opponent} for just a moment, enough to throw off {FormatPossessive(attacker)} aim.",
                $"The dim light blurs the details, making it hard to focus on {opponent}, causing {attacker} to miss their attack.",
                $"As {attacker} moves to strike, the dim light plays tricks on {FormatPossessive(attacker)} eyes, making {opponent} appear to flicker and fade.",
                $"In the dim light, {attacker} misjudges the distance and angle, and the attack fails to connect with {opponent}.",
                $"The poor lighting causes {opponent} to blend into the shadows, and {FormatPossessive(attacker)} strike finds nothing but air.",
                $"{attacker} hesitates for a moment, trying to see {opponent} clearly in the dim light, and the opportunity slips away.",
                $"The dim light obscures {opponent}'s movements, making it hard for {attacker} to land a precise hit.",
                $"The shadows seem to warp and twist around {opponent}, causing {FormatPossessive(attacker)} attack to miss its mark.",
                $"In the murky light, {opponent}'s silhouette shifts unpredictably, and {FormatPossessive(attacker)} attack goes wide.",
                $"The faint light distorts {FormatPossessive(attacker)} perception, making it difficult to accurately gauge {opponent}'s position.",
                $"{attacker} struggles to keep {opponent} in sight through the shifting shadows, and {FormatPossessive(attacker)} attack falters.",
                $"The dim light clouds {FormatPossessive(attacker)} vision, and {FormatPossessive(attacker)} strike passes through empty space where {opponent} should be.",
                $"The darkness plays tricks on {FormatPossessive(attacker)} mind, causing {attacker} to misjudge the timing of the attack on {opponent}.",
                $"{attacker} loses sight of {opponent} for a brief moment in the dim light, and {FormatPossessive(attacker)} attack misses the mark."
            };
            Random random = new Random();
            int index = random.Next(missDescriptions.Count);
            return missDescriptions[index];
        }
    }
}
