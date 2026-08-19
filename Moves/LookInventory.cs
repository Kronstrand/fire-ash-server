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
        public LookInventory(Soul soul) : this(soul, soul.Character, "Iventory")
        {
        }

        public LookInventory(Soul soul, Character targetCharacter) : this(soul, targetCharacter, $"Loot {targetCharacter.Name}")
        {
        }
        private LookInventory(Soul soul, Character targetCharacter, string description) : base(MoveKey.i.ToString(), description)
        {
            Type = MoveType.MinorAction;
            AllowedInTrade = false;

            if (targetCharacter == soul.Character) //is iventory
                Hidden = true;
            else //is Loot
                Prop = targetCharacter; 

            Action = async () =>
            {
                soul.Character.lookAtBeforeInventory = soul.Character.LookAt;

                bool hasEquippedItem = (targetCharacter.EquippedItems.Count  > 0);
                bool hasInventoryItem = (targetCharacter.Inventory.Items.Count > 0);

                if (!hasEquippedItem && !hasInventoryItem)
                    await soul.SendAsync($"{targetCharacter.Name} holds no items.");
                else
                {
                    string inventory = "";
                    if (hasEquippedItem)
                    {
                        inventory = $"{targetCharacter.Name} has the following items equipped:";
                        foreach (KeyValuePair<InventorySlot, Item> kvp in targetCharacter.EquippedItems.Where(i => !i.Value.IsLivingBodyPart()))
                        {
                            inventory += $"\n{Description(kvp.Key)}: {kvp.Value.Name}.";
                        }
                    }
                    if (hasInventoryItem)
                    {
                        if (inventory != "")
                            inventory += "\n\n";

                        inventory += $"{targetCharacter.Name} has the following items in their inventory:";
                        foreach (Item item in targetCharacter.Inventory.Items.Where(i => !i.IsLivingBodyPart()))
                        {
                            inventory += $"\n{item.Name}";
                        }
                    }
                    soul.Character.SetLookAt(targetCharacter.Inventory);
                    await soul.SendAsync(inventory);
                }                
            };
        }
    }
}
