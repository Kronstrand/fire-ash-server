using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.Abstract_Entities;

namespace fire_ash_server.Moves
{
    internal class DropItem : Move
    {
        public DropItem(Soul soul, Item prop) : base(MoveKey.d.ToString(), CreateDescription(prop), prop, () => { })
        {
            EnablesCombat = false;
            Action = CreateAction(soul, prop);
        }

        private static string CreateDescription(Item item)
        {
            return "Drop " + item.Name + ".";
        }

        private Action CreateAction(Soul soul, Item item)
        {
            return () =>
            {
                RemoveItemFromCharacter(soul.Character, item);
                soul.Character.CurrentRoom.AddItem(item);
                item.MoveToGroup(soul.Character);

                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} dropped {item.Name}.");

                soul.Character.LookBackFromItem(item);
            };
        }

        private void RemoveItemFromCharacter(Character character, Item item)
        {
            if (character.Inventory.Items.Remove(item))
                return;

            foreach (var pair in character.EquippedItems)
            {
                if (pair.Value == item)
                {
                    character.EquippedItems.TryRemove(pair.Key, out _);
                    return;
                }
            }
        }
    }
}
