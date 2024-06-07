using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class Equip : Move
    {
        public InventorySlot TargetInventorySlot;
        public Equip(Soul soul, Item item, InventorySlot inventorySlot) : base("e", CreateDescription(item, inventorySlot), CreateAction(soul, item, inventorySlot))
        {
            Prop = item;
            TargetInventorySlot = inventorySlot;
        }

        private static string CreateDescription(Item item, InventorySlot inventorySlot)
        {
            return "Equip " + item.Name + " at " + Description(inventorySlot) + ".";
        }

        private static Action CreateAction(Soul soul, Item item, InventorySlot inventorySlot)
        {
            if (item.HeldBy == null) throw new ArgumentNullException(nameof(item.HeldBy), "Item has to be held when grabbed.");

            string GrabFrom = item.HeldBy.Name;

            return async () =>
            {
                if (!item.IsPickupable())
                {
                    await soul.SendAsync($"{item.Name} can't be picked up.");
                }
                else if (GrabFrom == item.HeldBy.Name)
                {
                    soul.Character.TryUnequipFromSlot(inventorySlot);
                    soul.Character.AddEquippedItem(inventorySlot, item);

                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} eqiupped {item.Name}.");
                }
                else
                {
                    soul.Character.LookBackFromItem(item);
                    await soul.SendAsync($"{item.Name} is not there anymore.");
                }
            };
        }

        public override string GetCompleteMoveKey()
        {
            return base.GetCompleteMoveKey() + " at " + Description(TargetInventorySlot);
        }
    }
}
