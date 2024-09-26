using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Club : Weapon
    {
        public Club(string name, string description, double value) : base(name, description, new Die(1, 4), DamageType.Bludgeoning, value)
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
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Club)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => { return $"{a} swings {w.Name} forcefully, aiming to smash {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} brings {w.Name} down in a crushing blow towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} slams {w.Name} with a heavy thud at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} smashes {w.Name} into {r}, aiming to bludgeon"; });
                descriptions.Add((a, r, w) => { return $"{a} drives {w.Name} with a powerful swing towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} pounds {w.Name} down with brute force at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} batters {w.Name} against {r}, with all their might"; });
                descriptions.Add((a, r, w) => { return $"{a} launches a punishing blow with {w.Name} at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} hammers {w.Name} down towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} crashes {w.Name} against {r} with a loud crack"; });
                GeneralAttackDescriptionsForType.Add(typeof(Club), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Club)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} swings their off-hand {w.Name}, aiming a heavy blow towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} bashes {w.Name} with a brutal off-hand swing towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their off-hand and slams {w.Name} against {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} crushes {r} with a forceful off-hand strike from {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} batters {r} with a quick, powerful off-hand strike from {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} slams {r} with a brutal off-hand hit using {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses their off-hand to deliver a crushing blow with {w.Name} at {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} brings {w.Name} down with force in an off-hand swing at {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} hammers {w.Name} against {r} with their off-hand"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} brings their off-hand {w.Name} crashing down on {r}"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Club), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Club)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => { return $"{a} swings their {w.Name} with full force, aiming to crush {FormatPossessive(r)} skull"; });
                descriptions.Add((a, r, w) => { return $"{a} drives their {w.Name} down like a hammer on {FormatPossessive(r)} shoulder"; });
                descriptions.Add((a, r, w) => { return $"{a} slams their {w.Name} in a heavy, bone-crunching swing at {FormatPossessive(r)} ribs"; });
                descriptions.Add((a, r, w) => { return $"{a} brings {w.Name} crashing down on {FormatPossessive(r)} chest with a devastating impact"; });
                descriptions.Add((a, r, w) => { return $"{a} batters {FormatPossessive(r)} arm with a ferocious swing of their {w.Name}"; });
                descriptions.Add((a, r, w) => { return $"{a} hurls their {w.Name} in a brutal arc towards {FormatPossessive(r)} head"; });
                descriptions.Add((a, r, w) => { return $"{a} bludgeons {FormatPossessive(r)} torso with a vicious swing of their {w.Name}"; });
                descriptions.Add((a, r, w) => { return $"{a} pounds {w.Name} down on {FormatPossessive(r)} leg with relentless force"; });
                descriptions.Add((a, r, w) => { return $"{a} launches a crushing blow with their {w.Name} towards {FormatPossessive(r)} spine"; });
                descriptions.Add((a, r, w) => { return $"{a} drives their {w.Name} into {FormatPossessive(r)} face, aiming to pulverize"; });
                HumanoidAttackDescriptionsForType.Add(typeof(Club), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptionsForType()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Club)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} swings their off-hand {w.Name}, aiming a heavy blow towards {FormatPossessive(r)} jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} bashes {FormatPossessive(r)} temple with a brutal off-hand swing from {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their off-hand and slams {w.Name} against {FormatPossessive(r)} knee"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} crushes {FormatPossessive(r)} collarbone with a forceful off-hand strike from {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} batters {FormatPossessive(r)} ribcage with a quick, powerful off-hand strike from {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} slams {FormatPossessive(r)} abdomen with a brutal off-hand hit using {w.Name}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses their off-hand to deliver a crushing blow with {w.Name} to {FormatPossessive(r)} side"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} brings {w.Name} down with force in an off-hand swing at {FormatPossessive(r)} shoulder"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} hammers {w.Name} against {FormatPossessive(r)} back with their off-hand"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} brings their off-hand {w.Name} crashing down on {FormatPossessive(r)} skull"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Club), offhandDescriptions);
            }
        }
    }
}
