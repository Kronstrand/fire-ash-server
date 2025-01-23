using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armor;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Moves
{
    internal class LookAt : Move
    {
        public LookAt(Soul soul, Prop prop) : base(MoveKey.l.ToString(), CreateDescription(soul.Character, prop), prop)
        {
            InitValues();

            Action = async () =>
                {
                    soul.Character.SetLookAt(prop);
                    await LookAtAction(soul, prop);
                };
        }

        public LookAt(Soul soul) : base(MoveKey.l.ToString(), $"Look at current prop.")
        {
            InitValues();

            Hidden = true;
            Action = async () =>
                {
                    if (!soul.Character.PropTargetIsValid(this, soul.Character.LookAt))
                        return;

                    await LookAtAction(soul, soul.Character.LookAt);
                };
        }

        private static string CreateDescription(Character character, Prop targetProp)
        {
            string description = $"{targetProp.GetLightEffectedName("Look at ", "Look into the ", character)}.";

            if (targetProp is Character)
            {
                Character targetCharacter = (Character)targetProp;
                if (targetCharacter.Dead)
                    description += " (Dead)";
            }
            return description;
        }

        private void InitValues()
        {
            Type = MoveType.MinorAction;
            Range = RangeType.None;
            AllowedInTrade = true;
        }

        public static async Task LookAtAction(Soul soul, Prop? prop)
        {
            await HandleLookAtAction(soul, prop);

            if (prop != null && prop.OnAfterLookAt != null)
                    prop.OnAfterLookAt(soul);
        }

        private static async Task HandleLookAtAction(Soul soul, Prop? prop)
        {
            if (prop == null)
            {
                await soul.SendAsync($"{soul.Character.Name} are staring absentmindedly into the air...");
                return;
            }
            else if (prop is Room)
            {
                Room room = (Room)prop;
                await soul.SendAsync(room.GetDescription(soul.Character, true));
                await soul.SendAsync(room.GetAdditionalRoomDescription(soul.Character));
                return;
            }
            else if (prop is Character)
            {
                Character character = (Character)prop;
                await soul.SendAsync(prop.GetDescription(soul.Character) + "\n\n" + $"The relationship to {character.Name} is {Description(soul.Character.GetRelationshipStatus(character))}.");
                return;
            }
            else if (prop is Armor)
            {
                Armor armor = (Armor)prop;
                await soul.SendAsync(prop.GetDescription(soul.Character)  + 
                    "\n\n" + "Type: Armor" + 
                    "\n" + $"Armor Class: {armor.AC}");
                return;
            }
            else if (prop is Shield)
            {
                Shield shield = (Shield)prop;
                await soul.SendAsync(prop.GetDescription(soul.Character) +
                    "\n\n" + "Type: Shield" +
                    "\n" + "Armor Bonus: +2");
                return;
            }
            else if (prop is Weapon)
            {
                Weapon weapon = (Weapon)prop;

                string equipable = "";
                foreach (InventorySlot slot in weapon.CarriableByInventorySlots)
                {
                    if (equipable == "")
                        equipable += "Equipable as ";
                    else
                        equipable += ", ";
                    equipable += Helpers.Description(slot);
                }

                string output = 
                    prop.GetDescription(soul.Character) +
                    "\n\n" + "Type: Weapon" +
                    "\n" + $"Damage: {weapon.GetDmgAsString()}" +
                    "\n" + $"Damage Type: {weapon.DamageType}" +
                    "\n" + equipable;


                if (weapon.EquipEffects.Any())
                {
                    string effects = "";
                    foreach (Effect effect in weapon.EquipEffects)
                    {
                        if (effects == "")
                            effects = "Effects: ";
                        else
                            effects += ", ";
                        effects += effect.Name;
                    }
                    output += "\n" + effects;
                }

                await soul.SendAsync(output);
                return;
            }
            else if (prop is Armor)
            {
                Armor armor = (Armor)prop;
                await soul.SendAsync(prop.GetDescription(soul.Character) + "\n\n" + $"Armor Class: {armor.AC}");
                return;
            }
            else if (prop is Item && !prop.IsPickupable())
            {
                Item character = (Item)prop;
                string output = prop.GetDescription(soul.Character);
                string items = prop.ListItemsAsString(soul.Character);
                if (items != "")
                    output += "\n\n" + items;
                await soul.SendAsync(output);
                return;
            }
            await soul.SendAsync(prop.GetDescription(soul.Character));
        }
    }
}
