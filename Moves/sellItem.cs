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
    internal class SellItem : Move
    {
        public SellItem(Soul soul, Item prop, Character sellFrom) : base(MoveKey.bi.ToString(), CreateDescription(prop, sellFrom), prop, async () => { })
        {
            AllowedInTrade = true;
            Action = CreateAction(soul, prop, sellFrom);
        }

        private static string CreateDescription(Item item, Character sellFrom)
        {
            double vendorValue = item.GetSellPriceFromVendor(sellFrom);
            Tuple<int, int> price = ConvertToGoldAndSilver(vendorValue);
            return $"Sell {item.Name} ({PriceToString(price)}).";
        }

        private Func<Task> CreateAction(Soul soul, Item item, Character sellTo)
        {
            return async () =>
            {
                double vendorValue = item.GetSellPriceFromVendor(sellTo);

                Tuple<int, int> price = ConvertToGoldAndSilver(vendorValue);

                if (sellTo.GetTotalCoinValue() >= vendorValue)
                {
                    Tuple<int, int> soldPrice = sellTo.TransferCoinTo(soul.Character, price.Item1, price.Item2);
                    sellTo.AddToInventory(item);
                    soul.Character.LookBackFromItem(item);
                    _ = soul.SendAsync($"You sell {item.Name} to {sellTo.Name} for {soldPrice.Item1} gold and {soldPrice.Item2} silver.");
                }
                else
                {
                    _ = soul.SendAsync($"{sellTo.Name} can't afford {item.Name}.");
                }

                await Task.CompletedTask;

            };
        }
    }
}
