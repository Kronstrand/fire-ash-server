using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    [Serializable]
    internal class Sword : Weapon
    {
        public Sword(string name, string description) : base(name, description, new Die(1, 6), DamageType.Slashing, 5)
        {
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                InventorySlot.MainHand,
                InventorySlot.OffHand
            };

            SetGeneralAttackDescriptionsForType();
            SetGeneralOffHandAttackDescriptionsForType();
            SetHumanoidAttackDescriptionsForType();
            SetHumanoidOffHandAttackDescriptionsForType();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Sword)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} swings {w.Name} in a wide arc, aiming a slashing strike at {r}");
                descriptions.Add((a, r, w) => $"{a} strikes decisively with {w.Name}, aiming to cut into {r}");
                descriptions.Add((a, r, w) => $"{a} swings {w.Name} forcefully toward {r}, the blade whistling through the air");
                descriptions.Add((a, r, w) => $"{a} drives {w.Name} forward with a powerful thrust toward {r}");
                descriptions.Add((a, r, w) => $"{a} brings {w.Name} down with brutal force, seeking to strike {r}");
                descriptions.Add((a, r, w) => $"{a} sweeps {w.Name} in a clean motion, aiming directly at {r}");
                descriptions.Add((a, r, w) => $"{a} chops at {r} with {w.Name}, aiming to cleave through");
                descriptions.Add((a, r, w) => $"{a} swings {w.Name} from the side, hoping to cut into {r}");
                descriptions.Add((a, r, w) => $"{a} slashes with precision, guiding {w.Name} in a swift strike toward {r}");
                descriptions.Add((a, r, w) => $"{a} brings {w.Name} forward with lethal intent, striking at {r}");
                // Additional descriptions without movement
                descriptions.Add((a, r, w) => $"{a} unleashes a rapid slash with {w.Name} aimed at {r}");
                descriptions.Add((a, r, w) => $"{a} aims a calculated strike with {w.Name} toward {r}");
                descriptions.Add((a, r, w) => $"{a} delivers a fierce blow with {w.Name} directly at {r}");
                descriptions.Add((a, r, w) => $"{a} spins {w.Name} skillfully, slashing at {r}");
                descriptions.Add((a, r, w) => $"{a} feints before delivering a precise cut with {w.Name} at {r}");
                descriptions.Add((a, r, w) => $"{a} directs a powerful thrust with {w.Name} toward {r}");
                descriptions.Add((a, r, w) => $"{a} executes a swift slash with {w.Name}, targeting {r}");
                descriptions.Add((a, r, w) => $"{a} makes a quick slash with {w.Name}, aiming for {r}");
                descriptions.Add((a, r, w) => $"{a} channels strength into {w.Name}, striking at {r}");
                descriptions.Add((a, r, w) => $"{a} delivers a lethal cut with {w.Name} aimed at {r}");

                GeneralAttackDescriptionsForType.Add(typeof(Sword), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!GeneralOffHandAttackDescriptionsForType.ContainsKey(typeof(Sword)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} deftly swings {w.Name} with their off-hand, aiming a quick slash at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts {w.Name} to their off-hand and delivers a swift strike toward {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} maneuvers {w.Name} in their off-hand, aiming a precise cut at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} flicks {w.Name} in their off-hand, making a quick, cutting motion at {r}");
                // Additional descriptions without movement
                offhandDescriptions.Add((a, r, w) => $"{a} rapidly switches {w.Name} to their off-hand and slices toward {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers an unexpected off-hand strike with {w.Name} at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} parries with the main hand and counters with an off-hand slash from {w.Name} toward {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} spins {w.Name} in their off-hand before striking at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} quickly slashes at {r} with {w.Name} in their off-hand");
                offhandDescriptions.Add((a, r, w) => $"{a} directs a swift off-hand cut with {w.Name} at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} makes a sharp off-hand attack with {w.Name} toward {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} skillfully maneuvers {w.Name} in their off-hand, striking at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} executes a precise off-hand slash with {w.Name} aimed at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} uses {w.Name} in their off-hand to deliver a quick cut at {r}");

                GeneralOffHandAttackDescriptionsForType.Add(typeof(Sword), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Sword)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} swings {w.Name} in a powerful arc, aiming at {FormatPossessive(r)} torso");
                descriptions.Add((a, r, w) => $"{a} drives {w.Name} into a vicious slash aimed at {FormatPossessive(r)} arm");
                descriptions.Add((a, r, w) => $"{a} pulls back and swings {w.Name} with all their strength, targeting {FormatPossessive(r)} leg");
                descriptions.Add((a, r, w) => $"{a} arcs {w.Name} gracefully through the air, aiming to cut {FormatPossessive(r)} shoulder");
                descriptions.Add((a, r, w) => $"{a} delivers a sweeping blow with {w.Name}, targeting {FormatPossessive(r)} side");
                descriptions.Add((a, r, w) => $"{a} strikes swiftly with {w.Name}, the blade flashing toward {FormatPossessive(r)} neck");
                descriptions.Add((a, r, w) => $"{a} expertly twists {w.Name}, slashing through the air at {FormatPossessive(r)} chest");
                descriptions.Add((a, r, w) => $"{a} guides {w.Name} toward {FormatPossessive(r)} head in a controlled strike");
                descriptions.Add((a, r, w) => $"{a} feints and then delivers a powerful cut with {w.Name}, aiming for {FormatPossessive(r)} abdomen");
                descriptions.Add((a, r, w) => $"{a} unleashes a flurry of slashes with {w.Name} at {FormatPossessive(r)} limbs");
                descriptions.Add((a, r, w) => $"{a} channels strength into {w.Name}, delivering a devastating strike at {FormatPossessive(r)} ribcage");
                descriptions.Add((a, r, w) => $"{a} makes a precise cut with {w.Name}, aiming directly at {FormatPossessive(r)} forearm");
                descriptions.Add((a, r, w) => $"{a} swiftly brings {w.Name} around to slash at {FormatPossessive(r)} thigh");
                descriptions.Add((a, r, w) => $"{a} focuses intensely and slashes at {FormatPossessive(r)} waist with {w.Name}");
                descriptions.Add((a, r, w) => $"{a} executes a masterful strike with {w.Name}, targeting {FormatPossessive(r)} collarbone");
                descriptions.Add((a, r, w) => $"{a} delivers a fierce thrust with {w.Name} aimed at {FormatPossessive(r)} stomach");
                descriptions.Add((a, r, w) => $"{a} performs a swift combination of cuts with {w.Name} at {FormatPossessive(r)} limbs");
                descriptions.Add((a, r, w) => $"{a} whirls {w.Name} gracefully, striking at {FormatPossessive(r)} back");
                descriptions.Add((a, r, w) => $"{a} directs a lethal slash with {w.Name} toward {FormatPossessive(r)} hip");
                descriptions.Add((a, r, w) => $"{a} applies expert technique with {w.Name}, attacking {FormatPossessive(r)} knee");

                HumanoidAttackDescriptionsForType.Add(typeof(Sword), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptionsForType()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Sword)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} deftly shifts {w.Name} in their off-hand, striking swiftly at {FormatPossessive(r)} side");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers a calculated off-hand slash with {w.Name}, aiming at {FormatPossessive(r)} arm");
                offhandDescriptions.Add((a, r, w) => $"{a} swings {w.Name} with precise control, targeting {FormatPossessive(r)} leg with their off-hand");
                offhandDescriptions.Add((a, r, w) => $"{a} twists their body, slashing with {w.Name} in the off-hand toward {FormatPossessive(r)} torso");
                offhandDescriptions.Add((a, r, w) => $"{a} blocks with the main hand and swiftly counters with {w.Name} in the off-hand at {FormatPossessive(r)} neck");
                offhandDescriptions.Add((a, r, w) => $"{a} feints high with the main weapon, then strikes low with {w.Name} in the off-hand at {FormatPossessive(r)} knee");
                offhandDescriptions.Add((a, r, w) => $"{a} performs a quick off-hand jab with {w.Name} toward {FormatPossessive(r)} shoulder");
                offhandDescriptions.Add((a, r, w) => $"{a} slashes horizontally with {w.Name} in their off-hand, aiming at {FormatPossessive(r)} waist");
                // Additional descriptions without movement
                offhandDescriptions.Add((a, r, w) => $"{a} quickly flicks {w.Name} in their off-hand toward {FormatPossessive(r)} wrist");
                offhandDescriptions.Add((a, r, w) => $"{a} makes a swift off-hand strike with {w.Name} at {FormatPossessive(r)} thigh");
                offhandDescriptions.Add((a, r, w) => $"{a} uses {w.Name} in the off-hand for a precise attack on {FormatPossessive(r)} elbow");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers a sharp off-hand cut with {w.Name} aimed at {FormatPossessive(r)} forearm");
                offhandDescriptions.Add((a, r, w) => $"{a} expertly controls {w.Name} in their off-hand, striking at {FormatPossessive(r)} ankle");
                offhandDescriptions.Add((a, r, w) => $"{a} attacks {FormatPossessive(r)} ribs with a quick off-hand slash using {w.Name}");
                offhandDescriptions.Add((a, r, w) => $"{a} guides {w.Name} smoothly in their off-hand toward {FormatPossessive(r)} abdomen");
                offhandDescriptions.Add((a, r, w) => $"{a} executes a precise off-hand maneuver with {w.Name} at {FormatPossessive(r)} chest");
                offhandDescriptions.Add((a, r, w) => $"{a} swiftly directs {w.Name} in the off-hand to strike {FormatPossessive(r)} collarbone");
                offhandDescriptions.Add((a, r, w) => $"{a} attacks from an unexpected angle with {w.Name} in their off-hand at {FormatPossessive(r)} shoulder");
                offhandDescriptions.Add((a, r, w) => $"{a} makes a calculated off-hand thrust with {w.Name} toward {FormatPossessive(r)} stomach");

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Sword), offhandDescriptions);
            }
        }
    }
}
