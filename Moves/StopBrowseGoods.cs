using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class StopBrowseGoods : Move
    {
        public StopBrowseGoods(Soul soul) : base(MoveKey.bg.ToString(), $"Stop Trading..")
        {
            Type = MoveType.MinorAction;
            AllowedInTrade = true;

            Action = async () =>
            {
                if (soul.Character.TradingWith == null)
                    return;
                await soul.SendAsync($"You finished trading with {soul.Character.TradingWith.Name}.");
                //soul.Character.SetLookAt(soul.Character.TradingWith);
                soul.Character.LookBack();
                soul.Character.TradingWith = null;
            };
        }
    }
}
