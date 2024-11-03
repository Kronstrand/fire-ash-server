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
using static fire_ash_server.Helpers;

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
            string priceString;
            Tuple<int, int> price = ConvertToGoldAndSilver(item.VendorValue);
            if (price.Item2 == 0)
                priceString = $"{price.Item1} gp";
            else if (price.Item1 == 0)
                priceString = $"{price.Item2} sp";
            else
                priceString = $"{price.Item1} gp, {price.Item2} sp";

            return $"Buy {item.Name} ({priceString}).";
        }

        private Action CreateAction(Soul soul, Item item)
        {
            return () => 
            {
                /*
                if (!item.IsPickupable())
                {
                    _ =  soul.SendAsync($"{item.Name} can't be picked up.");
                    EnablesCombat = false;
                    return;
                }*/

                Character? heldByCharacter = item.HeldByCharacter();
                if (heldByCharacter != null)
                {
                    Tuple<int, int> price = ConvertToGoldAndSilver(item.VendorValue); 

                    if (soul.Character.GetTotalCoin() >= item.VendorValue)
                    {
                        soul.Character.TransferCoinTo(heldByCharacter, price.Item1, price.Item2);
                        soul.Character.AddToInventory(item);
                        _ = soul.SendAsync($"You buy {item.Name} from {heldByCharacter.Name} for {price.Item1} gold and {price.Item2} silver.");
                    }
                    else
                    {
                        _ = soul.SendAsync($"You can't afford {item.Name}.");
                    }
                }
            };
        }
    }
}
