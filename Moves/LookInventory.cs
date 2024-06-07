using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class LookInventory : Move
    {
        public LookInventory(Soul soul) : base("i", "Inventory")
        {
            Hidden = true;
            Type = MoveType.MinorAction;

            Action = async () =>
            {
                bool hasEquippedItem = (soul.Character.EquippedItems.Count  > 0);
                bool hasInventoryItem = (soul.Character.Inventory.Items.Count > 0);

                if (!hasEquippedItem && !hasInventoryItem)
                    await soul.SendAsync($"{soul.Character.Name} holds no items.");
                else
                {
                    string inventory = "";
                    if (hasEquippedItem)
                    {
                        inventory = $"{soul.Character.Name} has the following items equipped:";
                        foreach (KeyValuePair<InventorySlot, Item> kvp in soul.Character.EquippedItems)
                        {
                            inventory += $"\n{Description(kvp.Key)}: {kvp.Value.Name}.";
                        }
                    }
                    if (hasInventoryItem)
                    {
                        if (inventory != "")
                            inventory += "\n\n";

                        inventory += $"{soul.Character.Name} has the following items in their inventory:";
                        foreach (Item item in soul.Character.Inventory.Items)
                        {
                            inventory += $"\n{item.Name}";
                        }
                    }
                    soul.Character.SetLookAt(soul.Character.Inventory);
                    await soul.SendAsync(inventory);
                }                
            };
        }
    }
}
