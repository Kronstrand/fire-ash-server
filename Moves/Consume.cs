using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Moves
{
    [Serializable]
    internal class Consume : Move
    {
        public Consume(Soul soul, Consumable consumable) : base(MoveKey.c.ToString(), CreateName(soul, consumable), async () => { })
        {
            Prop = consumable;
            Action = CreateAction(soul, consumable);
        }
            

        private static string CreateName(Soul soul, Consumable consumable)
        {
            string name = $"Use {consumable.Name}";
            if (consumable.HasTarget && soul.Character.lookAtBeforeInventory is Prop)
                name += $" on {soul.Character.lookAtBeforeInventory.Name}";
            name += ".";

            return name;
        }
            

        private Func<Task> CreateAction(Soul soul, Consumable consumable)
        {
            return async () => {
                if (consumable.Requirement == null || consumable.Requirement(soul, consumable.Range, consumable.Weapon, soul.Character.lookAtBeforeInventory))
                {
                    await consumable.Consume(soul);
                    soul.Character.LookBackFromItem(consumable);
                    soul.Character.Inventory.Items.Remove(consumable);
                }
                else if (consumable.NotAvailable != null)
                {
                    consumable.NotAvailable(soul);
                    Type = MoveType.MinorAction;
                }
            };
        }
    }
}
