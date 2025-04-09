using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    [Serializable]
    internal class PickPocket : Move
    {
        public PickPocket(Soul soul, Character targetCharacter) : base(MoveKey.pp.ToString(), $"Pickpocket {targetCharacter.Name}", targetCharacter)
        {
            Type = MoveType.MinorAction;
            Action = async () =>
            {
                Roll pickpocketRoll = new Roll(soul.Character.GetModifer(Skill.SleightOfHand), RollType.SkillCheck, soul.Character);
                if (pickpocketRoll.GetSum() >= targetCharacter.GetPassiveDC(Skill.Perception))
                {
                    await soul.SendAsync($"You managed to snoop into the pockets of {targetCharacter.Name} with a roll of {pickpocketRoll}.");
                    soul.Character.SetLookAt(targetCharacter.Inventory);
                }
                else
                {
                    await soul.SendAsync($"{soul.Character.Name} failed to pickpocket {targetCharacter.Name} with a roll of {pickpocketRoll}.");
                    soul.Character.Unhide();
                    Type = MoveType.Action;
                    EnablesCombat = true;
                }
            };
        }

        public override bool IsValid(Soul soul)
        {
            return (soul.Character.IsHidden() && soul.Character.LookAt is Character);
        }
    }
}
