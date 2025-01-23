using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class Move
    {
        public string Key;
        public string Description;
        //public Action Action;
        public Func<Task> Action;
        public bool Repeatable = true;
        public bool AllowedInTrade = false;
        public bool AllowedInCombat = true;
        public Prop? Prop;
        public Prop? PropPosition;
        public bool Hidden;
        public MoveType Type = MoveType.Action;
        public bool EnablesCombat;
        public RangeType Range = RangeType.CloseSingleTarget;

        public Move(string key, string description, Func<Task> action)
        {
            Key = key;
            Description = description;
            Action = action;

        }
        public Move(string key, string description, Prop prop, Func<Task> action)
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

        public async Task Execute(Soul soul)
        {
            await Execute(soul, soul.Character);
        }
        public async Task Execute(/*ref Move move,*/Soul soul, Character activeCharacter)
        {
            if (!soul.Character.PropTargetIsValid(this))
                return;

            soul.Character.RegisterUsedMoveOnProp(this);


            if (this is Attack)
                if (soul.Character.GetLightState(null) == Light.Darkness)
                {
                    if (Prop != null)
                        SetThreadBasedBufferText($"From the darkness, ");
                    else
                        SetThreadBasedBufferText($"Within the darkness, ");
                    Console.WriteLine("Buffer is set at " + DateTime.Now);
                }


            if (EnablesCombat)
            {
                if (soul.Character.IsHidden())
                    soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} reveals themselves from the shadows...");

                await Action();
                RemoveBufferTextForThread();

                if (soul.Character.IsHidden())
                    soul.Character.Unhide();
            }
            else
                await Action();

            ExecutePostAction(soul.Character);

            if (EnablesCombat)
            {
                if (activeCharacter.EnableCombatWith == null)
                {
                    Character? targetCharacer = null;
                    if (Prop is Item)
                    {
                        Item item = (Item)Prop;
                        Character? heldByCharacter = item.HeldByCharacter();
                        if (heldByCharacter != null || heldByCharacter != soul.Character)
                        {
                            targetCharacer = heldByCharacter;
                        }
                    }
                    else if (Prop is Character)
                        targetCharacer = (Character)Prop;

                    if (targetCharacer != null && EnablesCombat)
                        if (targetCharacer != activeCharacter)
                            activeCharacter.EnableCombatWith = new ToxicRelationship(targetCharacer, false);
                        else
                            activeCharacter.EnableCombatWith = new ToxicRelationship(soul.Character, true);
                }

            }
        }
    }
}
