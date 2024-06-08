using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class Move
    {
        public string Key;
        public string Description;
        public Action Action;
        public bool Repeatable = true;
        public Prop? Prop;
        public Prop? PropPosition;
        public bool Hidden;
        public MoveType Type = MoveType.Action;
        public bool EnablesCombat;
        public bool IsRanged = false;

        public Move(string key, string description, Action action)
        {
            Key = key;
            Description = description;
            Action = action;

        }
        public Move(string key, string description, Prop prop, Action action)
        {
            Key = key;
            Description = description;
            Prop = prop;
            PropPosition = prop.GetPropPosition();
            Action = action;
        }

        public Move(string key, string description)
        {
            Key = key;
            Description = description;
            Action = () => { throw new NotImplementedException("Action not implemented"); };
        }

        public Move(string key, string description, Prop prop)
        {
            Key = key;
            Description = description;
            Prop = prop;
            Action = () => { throw new NotImplementedException("Action not implemented"); };
        }


        public string GetObjectName()
        {
            string moveName = GetType().Name;
            if (GetType() == typeof(SkillCheck))
            {
                SkillCheck skillCheck = (SkillCheck)this;
                moveName += skillCheck.SkillNumber.Skill;
            }
            return moveName;
        }

        public void ExecutePostAction(Character character)
        {
            if (Prop == null)
                return;
            if (this is Stealth)
                return;
            if (!character.IsHidden() || character.InCombat || EnablesCombat)
                return;
            
            List<Character> characters = new List<Character>();

            if (Type == MoveType.Action)
                characters = Prop.GetCharactersLookingAt().Where(c => c != character && character.GetRelationshipStatus(c) != RelationshipStatus.good).ToList();
                
            if (Prop is Character && ((Character)Prop != character) && ((Character)Prop).GetRelationshipStatus(character) != RelationshipStatus.good)
                characters.Add((Character)Prop);

            if (characters.Any())
            {
                int targetDC = 0;
                foreach (Character lookingChar in characters)
                    targetDC += lookingChar.GetModifer(Skill.Perception);
                targetDC = Math.Max(targetDC, 1);

                Roll stealthRoll = new Roll(character.GetModifer(Skill.Stealth), RollType.SkillCheck, character);

                if (!stealthRoll.BeatsDC(targetDC))
                {
                    Character target = characters[new Random().Next(characters.Count)];

                    character.Unhide();
                    Relationship rel = character.GetRelationShipTo(target);
                    string message = "";

                    RelationshipStatus relStatus = rel.GetStatus();
                    if (relStatus == RelationshipStatus.bad)
                    {
                        message = $"{character.Name} fails to stay hidden with a roll of {stealthRoll} and is aggressively engaged by {target.Name}.";
                        EnablesCombat = true;
                        Type = MoveType.Action;
                    }
                    else //if is neutral
                    {
                        character.ModifyRelationshipTo(target, -1);
                        message = $"{character.Name} fails to stay hidden with a roll of {stealthRoll} and is seen by {target.Name}. (Impacts relationship slightly negativly)";
                    }
                    character.CurrentRoom.BroadcastToSoulsInRoom(message);
                }
            }
            
        }

        public virtual string GetCompleteMoveKey()
        {
            if (Prop != null)
            {
                return Key + " " + Prop.Name;
            }
            else
                return Key;
        }

        public virtual bool IsValid(Soul soul)
        {
            return true;
        }
    }
}
