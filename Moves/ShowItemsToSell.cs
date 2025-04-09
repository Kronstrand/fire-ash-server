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
    internal class ShowItemsToSell : Move
    {
        public ShowItemsToSell(Soul soul, Character targetCharacter) : base(MoveKey.st.ToString(), $"Sell to {targetCharacter.Name}", targetCharacter)
        {
            Type = MoveType.MinorAction;
            AllowedInCombat = false;
            Action = async () =>
            {
                await soul.SendAsync($"{soul.Character.Name} has the following items for sale.");
                soul.Character.SetLookAt(soul.Character.Inventory);
                soul.Character.TradingWith = targetCharacter;
            };
        }
    }
}
