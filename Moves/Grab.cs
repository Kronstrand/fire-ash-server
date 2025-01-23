using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.Moves
{

    internal class Grab : Move
    {
        public Grab(Soul soul, Item prop) : base(MoveKey.g.ToString(), CreateDescription(prop), prop, async () => { })
        {            
            AllowedInTrade = false;
            EnablesCombat = true;
            Action = CreateAction(soul, prop);
        }

        private static string CreateDescription(Item item)
        {
            return "Grab " + item.Name + ".";
        }

        private Func<Task> CreateAction(Soul soul, Item item)
        {
            return async () => 
            {
                if (!item.IsPickupable())
                {
                    _ =  soul.SendAsync($"{item.Name} can't be picked up.");
                    EnablesCombat = false;
                    return;
                }

                Character? heldByCharacter = item.HeldByCharacter();
                if (heldByCharacter != null && !heldByCharacter.Dead)
                {
                    Roll strRoll = new Roll(soul.Character.GetModifer(Ability.Strength), RollType.SkillCheck, soul.Character);
                    if (strRoll.GetSum() >= heldByCharacter.GetPassiveDC(Ability.Strength))
                    {
                        soul.Character.AddToInventory(item);
                        soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} tries to forcefully take {item.Name} from {heldByCharacter.Name}, and succeeds with a roll of {strRoll}.");
                    }
                    else
                    {
                        soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} tries to forcefully take {item.Name} from {heldByCharacter.Name}, but fails with a roll of {strRoll}...");
                    }
                }
                else
                {
                    string grabDescriptions = soul.Character.AddToInventory(item);

                    if (grabDescriptions != "")
                        soul.Character.BroadcastToSoulsInRoom(grabDescriptions);
                    EnablesCombat = false;
                }

                await Task.CompletedTask;
            };
        }
        
        public override bool IsValid(Soul soul)
        {
            if (!(soul.Character.LookAt is Item))
                return false;

            Item item = (Item)soul.Character.LookAt;
            
            if (item.HeldByCharacter() == soul.Character)
                return false;

            return true;
        }
    }
}
