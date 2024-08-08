using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World.BioMechWorld;

namespace fire_ash_server.Moves
{

    internal class BuyItem : Move
    {
        public BuyItem(Soul soul, Item prop) : base(MoveKey.bi.ToString(), CreateDescription(prop), prop, () => { })
        {            
            AllowedInTrade = true;
            Action = CreateAction(soul, prop);
        }

        private static string CreateDescription(Item item)
        {

            return $"Buy {item.Name} ({Prices.GetPrice(item)} gp).";
        }

        private Action CreateAction(Soul soul, Item item)
        {
            return () => 
            {
                if (!item.IsPickupable())
                {
                    _ =  soul.SendAsync($"{item.Name} can't be picked up.");
                    EnablesCombat = false;
                    return;
                }

                Character? heldByCharacter = item.HeldByCharacter();
                if (heldByCharacter != null)
                {
                    int price = Prices.GetPrice(item);
                    if (soul.Character.GP >= price)
                    {
                        heldByCharacter.GP += price;
                        soul.Character.GP -= price;
                        soul.Character.AddToInventory(item);
                        _ = soul.SendAsync($"You buy {item.Name} from {heldByCharacter.Name} for {price} gold.");
                    }
                    else
                    {
                        _ = soul.SendAsync($"You need {price - soul.Character.GP} more gold pieces to buy {item.Name}.");
                    }
                }
            };
        }
    }
}
