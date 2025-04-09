using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.World;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items
{
    [Serializable]
    internal class Item : Prop
    {
        public Prop? HeldBy;
        public bool IsContainer;
        public double VendorValue = 0;
        public bool Sellable = true;
        public ThreadSafeList<InventorySlot> CarriableByInventorySlots = new ThreadSafeList<InventorySlot>();
        public List<Effect> EquipEffects = new List<Effect>();
        public Character? SetBy;

        public Item(string name, string description, double value) : base(name, description)
        {
            MakePickable();
            VendorValue = value;
        }
        public Item(string name, string Context, string description) : base(name, description)
        {
            ContextDescription = Context;
        }

        public new Item Hide(int DC)
        {
            base.Hide(DC);
            return this;
        }

        public Character? HeldByCharacter()
        {
            Item itemPointer = this;
            while (true)
            {
                if (itemPointer.HeldBy == null)
                    return null;

                if (itemPointer.HeldBy is Character)
                    return (Character)itemPointer.HeldBy;

                if (itemPointer.HeldBy is Inventory)
                {
                    Inventory inventory = (Inventory)itemPointer.HeldBy;
                    if (inventory.HeldBy is Character)
                        return (Character)inventory.HeldBy;

                    return null;
                }

                if (!(itemPointer.HeldBy is Item))
                    return null;

                itemPointer = (Item)itemPointer.HeldBy;
            }
        }
        public void ClearHeldBy()
        {
            if (HeldBy != null)
            {
                HeldBy.Items.Remove(this);
                if (HeldBy.GetType() == typeof(Character))
                {
                    Character character = (Character)HeldBy;
                    foreach (KeyValuePair<InventorySlot, Item> kvp in character.EquippedItems)
                    {
                        if (kvp.Value == this)
                            character.EquippedItems.TryRemove(kvp.Key, out Item? removedItem);
                    }
                }
            }
        }

        public void AddEquipEffect(EffectKey effectKey)
        {
            EquipEffects.Add(World.Effects.Get(effectKey));
        }

        public Room? LocatedInRoom()
        {
            Item itemPointer = this;
            while (true)
            {
                if (itemPointer.HeldBy == null)
                    return null;

                if (itemPointer.HeldBy is Room)
                    return (Room)itemPointer.HeldBy;

                if (itemPointer.HeldBy is Exit)
                    return ((Exit)itemPointer.HeldBy).LocatedInRoom;

                if (itemPointer.HeldBy is Item)
                    itemPointer = (Item)itemPointer.HeldBy;

                if (itemPointer.HeldBy is Character)
                    return ((Character)itemPointer.HeldBy).CurrentRoom;
            }
        }

        public double GetBuyPriceFromVendor(Character buyFrom)
        {
            return Math.Round(VendorValue * (1 + buyFrom.tradeModifier), 2);
        }

        public double GetSellPriceFromVendor(Character buyFrom)
        {
            return Math.Round(VendorValue * (1 - buyFrom.tradeModifier), 2); //maybe just 1, not 2
        }

    }
}
