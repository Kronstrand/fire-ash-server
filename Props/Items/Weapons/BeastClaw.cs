using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class BeastClaw : Weapon
    {
        public BeastClaw() : base("Beast Claw", "Beast Claw", new Die(1, 4), DamageType.Slashing, 0)
        {
            SetGeneralAttackDescriptionsForType();
            SetGeneralOffHandAttackDescriptionsForType();
            SetHumanoidAttackDescriptions();
            SetHumanoidOffHandAttackDescriptions();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(BeastClaw)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} swipes their claw at {r}, aiming to tear through flesh");
                descriptions.Add((a, r, w) => $"{a} lunges forward, slashing at {r} with their sharp claws");
                descriptions.Add((a, r, w) => $"{a} snarls and strikes at {r} with a fierce claw swipe");
                descriptions.Add((a, r, w) => $"{a} leaps towards {r}, claws extended in an attempt to rake them");
                descriptions.Add((a, r, w) => $"{a} darts forward, claws outstretched, aiming for {FormatPossessive(r)} vulnerable spots");
                descriptions.Add((a, r, w) => $"{a} slashes at {FormatPossessive(r)} body with a quick, precise claw attack");
                descriptions.Add((a, r, w) => $"{a} twists and brings their claws down on {r} with a vicious strike");
                descriptions.Add((a, r, w) => $"{a} pounces on {r}, claws flashing in the dim light");
                descriptions.Add((a, r, w) => $"{a} strikes at {r} with a swift claw attack, aiming to wound deeply");
                descriptions.Add((a, r, w) => $"{a} advances on {r}, claw raised for a strike");

                GeneralAttackDescriptionsForType.Add(typeof(BeastClaw), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!GeneralOffHandAttackDescriptionsForType.ContainsKey(typeof(BeastClaw)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} swiftly lashes out with their off-hand claw at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} snaps an off-hand claw towards {r}, aiming for a quick strike");
                offhandDescriptions.Add((a, r, w) => $"{a} uses their off-hand to swipe at {r} with a sharp claw");
                offhandDescriptions.Add((a, r, w) => $"{a} spins and strikes {r} with an off-hand claw attack");
                offhandDescriptions.Add((a, r, w) => $"{a} lunges with an off-hand claw, attempting to slash at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} brings their off-hand claw down on {r} in a quick strike");
                offhandDescriptions.Add((a, r, w) => $"{a} surprises {r} with a sudden off-hand claw swipe");
                offhandDescriptions.Add((a, r, w) => $"{a} strikes out with an off-hand claw, aiming at {FormatPossessive(r)} flank");
                offhandDescriptions.Add((a, r, w) => $"{a} deftly slashes at {r} with an off-hand claw");
                offhandDescriptions.Add((a, r, w) => $"{a} makes a quick off-hand strike with their claws towards {r}");

                GeneralOffHandAttackDescriptionsForType.Add(typeof(BeastClaw), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(BeastClaw)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} slashes their claws towards {FormatPossessive(r)} face, aiming to blind");
                descriptions.Add((a, r, w) => $"{a} rakes their claws across {FormatPossessive(r)} chest, seeking to rend flesh");
                descriptions.Add((a, r, w) => $"{a} lunges at {FormatPossessive(r)} throat with their claws");
                descriptions.Add((a, r, w) => $"{a} swipes at {FormatPossessive(r)} legs, trying to trip them");
                descriptions.Add((a, r, w) => $"{a} leaps at {FormatPossessive(r)} back, claws aiming for a quick strike");
                descriptions.Add((a, r, w) => $"{a} strikes at {FormatPossessive(r)} arms, attempting to disarm");
                descriptions.Add((a, r, w) => $"{a} slashes at {FormatPossessive(r)} midsection with a fierce claw swipe");
                descriptions.Add((a, r, w) => $"{a} tries to tear through {FormatPossessive(r)} defenses with a powerful claw attack");
                descriptions.Add((a, r, w) => $"{a} aims a vicious claw strike at {FormatPossessive(r)} neck");
                descriptions.Add((a, r, w) => $"{a} lashes out at {FormatPossessive(r)} eyes with a swift claw attack");

                HumanoidAttackDescriptionsForType.Add(typeof(BeastClaw), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptions()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(BeastClaw)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} quickly snaps an off-hand claw towards {FormatPossessive(r)} arm");
                offhandDescriptions.Add((a, r, w) => $"{a} strikes with an off-hand claw at {FormatPossessive(r)} face");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an off-hand claw to swipe at {FormatPossessive(r)} side");
                offhandDescriptions.Add((a, r, w) => $"{a} slashes at {FormatPossessive(r)} leg with an off-hand claw");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers an off-hand claw strike to {FormatPossessive(r)} torso");
                offhandDescriptions.Add((a, r, w) => $"{a} surprises {r} with a quick off-hand claw slash");
                offhandDescriptions.Add((a, r, w) => $"{a} lunges with an off-hand claw, aiming at {FormatPossessive(r)} chest");
                offhandDescriptions.Add((a, r, w) => $"{a} swipes at {FormatPossessive(r)} form with an off-hand claw, aiming for a vulnerable spot");
                offhandDescriptions.Add((a, r, w) => $"{a} swiftly claws at {FormatPossessive(r)} form with an off-hand strike");
                offhandDescriptions.Add((a, r, w) => $"{a} slashes at {FormatPossessive(r)} body with an off-hand claw, aiming to cause a deep cut");

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(BeastClaw), offhandDescriptions);
            }
        }
    }
}
