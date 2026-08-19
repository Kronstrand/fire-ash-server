using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World;

namespace fire_ash_server.Moves
{
    internal class Consume : Move
    {
        public Consume(Soul soul, Consumable consumable) : base(MoveKey.cs.ToString(), CreateName(soul, consumable), async () => { })
        {
            Prop = consumable;
            Action = CreateAction(soul, consumable);
            AllowedInCombat = consumable.UsableInCombat;
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

                ConsumableList.ConsumableEffects.TryGetValue(consumable.ConsumeKey, out Func<Soul, Item, Task>? effect);

                if (effect != null)
                    await effect(soul, consumable);
                else
                    await soul.SendAsync($"{consumable.Name} doesn't do anything and is discarded."); //should not happen

                if (consumable.WasNotConsumed)
                {
                    consumable.WasNotConsumed = false;
                }
                else
                {
                    soul.Character.LookBackFromItem(consumable);
                    soul.Character.Inventory.Items.Remove(consumable);
                }
            };
        }
    }
}
