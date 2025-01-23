using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class Stealth : Move
    {
        public Stealth(Soul soul) : base (MoveKey.s.ToString(), SkillCheck.CreateDescription("Hide.", Skill.Stealth))
        {
            Range = RangeType.None;
            Character character = soul.Character;
            Action = async () =>
            {
                Light lightStateOfCharacter = soul.Character.GetLightState(null, true);
                List<Character> relevantNonFriends = character.CurrentRoom.Characters.ToList(); 
                if (lightStateOfCharacter == Light.Darkness)
                {
                    List<Character> lookingAtCharacters = soul.Character.GetCharactersLookingAt();
                    relevantNonFriends = lookingAtCharacters.Where(c => c.HasEffect(EffectKey.Darkvision)).ToList();
                }
                int countedNonFriends = relevantNonFriends.Where(c => character.GetRelationshipStatus(c) != RelationshipStatus.good && c != character).Count();
                if (countedNonFriends == 0)
                {
                    SetSuccessEffects(soul.Character);
                    character.CurrentRoom.BroadcastToSoulsInRoom($"{character.Name} disappears into the shadows...");
                    return;
                }
                
                Roll stealthRoll = new Roll(character.GetModifer(Skill.Stealth), RollType.SkillCheck, character);

                if(lightStateOfCharacter == Light.Bright)
                    countedNonFriends *= 3;

                if (stealthRoll.GetSum() >= (countedNonFriends + 5))
                {
                    SetSuccessEffects(soul.Character);
                    character.CurrentRoom.BroadcastToSoulsInRoom($"{character.Name} tries to disappear into the shadows and succeeds with a roll of {stealthRoll}.");                        
                }
                else
                {
                    character.CurrentRoom.BroadcastToSoulsInRoom($"{character.Name} tries to disappear into the shadows and fails with a roll of {stealthRoll}.");
                }
            };
        }

        private void SetSuccessEffects(Character character)
        {
            character.Hide(10 + character.GetModifer(Skill.Stealth));

            Effect effect = new Effect(Description(EffectKey.Stealth));
            effect.rollModifiers.Add(new RollModifier(RollType.AttackRoll, 1));
            character.Effects.Add(effect);
        }

        public override bool IsValid(Soul soul)
        {
            Light lightSate = soul.Character.GetLightState(null);
            return !soul.Character.IsHidden() && lightSate < Light.Bright;
        }
    }
}
