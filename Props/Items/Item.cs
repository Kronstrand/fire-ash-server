using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Props.Items;
using fire_ash_server.World;
using static fire_ash_server.Helpers;
using fire_ash_server.Props.Items.Armoring;

namespace fire_ash_server.Props.Items
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")] 
    [JsonDerivedType(typeof(Weapon), "Weapon")]
    [JsonDerivedType(typeof(Armor), "Armor")]
    [JsonDerivedType(typeof(Head), "Head")]
    [JsonDerivedType(typeof(Shield), "Shield")]
    [JsonDerivedType(typeof(Coins), "Coins")]
    [JsonDerivedType(typeof(Consumable), "Consumable")]
    [JsonDerivedType(typeof(Inventory), "Iventory")]

    //Weapon
    [JsonDerivedType(typeof(AetherRotCannon2000), "AetherRotCannon2000")]
    [JsonDerivedType(typeof(AssaultRifle), "AssaultRifle")]
    [JsonDerivedType(typeof(BeastClaw), "BeastClaw")]
    [JsonDerivedType(typeof(Club), "Club")]
    [JsonDerivedType(typeof(Dagger), "Dagger")]
    [JsonDerivedType(typeof(Fist), "Fist")]
    [JsonDerivedType(typeof(InsectClaw), "InsectClaw")]
    [JsonDerivedType(typeof(ShortBow), "ShortBow")]
    [JsonDerivedType(typeof(Sling), "Sling")]
    [JsonDerivedType(typeof(Sword), "Sword")]
    [JsonDerivedType(typeof(TailSnakeBite), "TailSnakeBite")]
    [JsonDerivedType(typeof(Tendril), "Tendril")]
    [JsonDerivedType(typeof(VenomousSnakeBite), "VenomousSnakeBite")]

    internal class Item : Prop
    {

        [JsonInclude]    public Prop? HeldBy;
        [JsonInclude]   public Prop? LastHeldBy;

        /*[JsonIgnore]    public string? HeldById;
        [JsonPropertyName("HeldById")]
        [JsonInclude]   public string? HeldByIdSerializable
                        {
                            get
                            {
                                if (HeldBy == null)                   
                                    return null;
                                else
                                    return HeldBy.Id; 
                            }
                            set => HeldById = value;
                        }   
        */
        [JsonInclude]   public bool IsContainer;
        [JsonInclude]   public bool IsLootable = true;
        [JsonInclude]   public string IsBodyPartOf = "";
        [JsonInclude]   public bool UsableInCombat = true;
        [JsonInclude]   public double VendorValue = 0;
        [JsonInclude]   public bool Sellable = true;
        [JsonIgnore]    public ThreadSafeList<InventorySlot> CarriableByInventorySlots = new ThreadSafeList<InventorySlot>();
        [JsonPropertyName("CarriableByInventorySlots")]
        [JsonInclude]   public List<InventorySlot> CarriableByInventorySlotsSerializable
                        {
                            get => CarriableByInventorySlots.ToList();
                            set => CarriableByInventorySlots = new ThreadSafeList<InventorySlot>(value);
                        }
        [JsonInclude]   public List<Effect> EquipEffects = new List<Effect>();
        [JsonInclude]   public string? SetByCharacterId; //not implemented in saves
        [JsonInclude]   public bool IsPlural = false;

        public Item() { }

        public Item(string name, string description, double value) : base(name, description, name + "-" + Guid.NewGuid().ToString())
        {
            MakePickable();
            VendorValue = value;
        }
        //to be deleted
        public Item(string name, string Context, string description) : base(name, description, name + "-" + Guid.NewGuid().ToString())
        {
            WorldProp = true;
            ContextDescription = Context;
        }

        public Item(string name, string Context, string description, string id) : base(name, description, id)
        {
            WorldProp = true;
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
                else if (HeldBy.GetType() == typeof(Room))
                {
                    RemoveFromCurrentGrouping();
                }
            }
        }

        public void SetIsBodyPart()
        {
            Character? heldByCharacter = HeldByCharacter();
            
            if (heldByCharacter == null) 
                return;

            IsBodyPartOf = heldByCharacter.Id;
        }

        public bool IsLivingBodyPart()
        {
            Character? heldByCharacter = HeldByCharacter();

            if (heldByCharacter == null)
                return false;

            if (heldByCharacter.Dead)
                return false;

            return IsBodyPartOf == heldByCharacter.Id;
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
            return Math.Round(VendorValue * (1 + buyFrom.TradeModifier), 2);
        }

        public double GetSellPriceFromVendor(Character buyFrom)
        {
            return Math.Round(VendorValue * (1 - buyFrom.TradeModifier), 2); //maybe just 1, not 2
        }

        public void AddPersistetItemsRecursive(Item addToItem)
        {
            foreach (Item child in Items)
            {
                if (!child.WorldProp)
                {
                    addToItem.AddItem(child);
                }
                else
                {
                    Item? realChild = GetItemById(child.Id);
                    if (realChild != null)
                        child.AddPersistetItemsRecursive(realChild);
                }
            }
        }

        public void ReplaceItem(Item newItem)
        {
            Prop? heldByProp = HeldBy;
            if (heldByProp != null)
            {
                heldByProp.AddItem(newItem);
                if (heldByProp is Room)
                {
                    heldByProp.MoveToGroup(newItem);
                }
            }
        }

    }
}
