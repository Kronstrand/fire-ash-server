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
    [Serializable]
    internal class BuyItem : Move
    {
        public BuyItem(Soul soul, Item prop, Character buyFrom) : base(MoveKey.bi.ToString(), CreateDescription(prop, buyFrom), prop, async () => { })
        {            
            AllowedInTrade = true;
            Action = CreateAction(soul, prop, buyFrom);
        }

        private static string CreateDescription(Item item, Character buyFrom)
        {
            double vendorValue = item.GetBuyPriceFromVendor(buyFrom);
            Tuple<int, int> price = ConvertToGoldAndSilver(vendorValue);
            return $"Buy {item.Name} ({PriceToString(price)}).";
        }

        private Func<Task> CreateAction(Soul soul, Item item, Character buyFrom)
        {
            return async () => 
            {
                double vendorValue = item.GetBuyPriceFromVendor(buyFrom);

                Tuple<int, int> price = ConvertToGoldAndSilver(vendorValue); 

                if (soul.Character.GetTotalCoinValue() >= vendorValue)
                {
                    Tuple<int, int> boughtPrice = soul.Character.TransferCoinTo(buyFrom, price.Item1, price.Item2);
                    soul.Character.AddToInventory(item);
                    _ = soul.SendAsync($"You buy {item.Name} from {buyFrom.Name} for {boughtPrice.Item1} gold and {boughtPrice.Item2} silver.");
                }
                else
                {
                    _ = soul.SendAsync($"You can't afford {item.Name}.");
                }

                await Task.CompletedTask;
            };
        }
    }
}
