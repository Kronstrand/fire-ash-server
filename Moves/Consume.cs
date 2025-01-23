using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;

namespace fire_ash_server.Moves
{
    internal class Consume : Move
    {
        public Consume(Soul soul, Consumable consumable) : base(MoveKey.c.ToString(), $"Consume {consumable.Name}.", CreateAction(soul, consumable))
        {

        }

        private static Func<Task> CreateAction(Soul soul, Consumable consumable)
        {
            return async () => { 
                consumable.Consume(soul);
                soul.Character.LookBackFromItem(consumable);
                soul.Character.Inventory.Items.Remove(consumable);
            };
        }
    }
}
