using System;
using System.Collections.Generic;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    [Serializable]
    internal class VenomousSnakeBite : Weapon
    {
        public VenomousSnakeBite() : base("Venomous Snake Teeth", "Venomous Snake Teeth", new Die(1, 4), DamageType.Piercing, 1)
        {
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptions();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(VenomousSnakeBite)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} strikes quickly, attempting to sink their venomous fangs into {r}");
                descriptions.Add((a, r, w) => $"{a} lashes out, aiming to inject venom into {r}");
                descriptions.Add((a, r, w) => $"{a} curls and launches a vicious bite toward {r}");
                descriptions.Add((a, r, w) => $"{a} coils tightly before darting forward to attempt a bite on {r}");
                descriptions.Add((a, r, w) => $"{a} strikes with a swift bite, targeting {FormatPossessive(r)} exposed flesh");
                descriptions.Add((a, r, w) => $"{a} darts forward, aiming to sink fangs into {r}");
                descriptions.Add((a, r, w) => $"{a} twists and lunges, fangs bared, attempting to reach {r}");
                descriptions.Add((a, r, w) => $"{a} snaps its jaws toward {r}, venom dripping from its fangs in an attempt to strike");
                descriptions.Add((a, r, w) => $"{a} coils tightly, then lashes out, attempting to sink fangs into {r}");
                descriptions.Add((a, r, w) => $"{a} quickly darts at {r}, aiming to inject venom into their veins");
                descriptions.Add((a, r, w) => $"{a} strikes toward {r}, venom glistening on its sharp fangs as it attempts to bite");
                descriptions.Add((a, r, w) => $"{a} slithers forward, launching a venomous bite in the direction of {r}");
                descriptions.Add((a, r, w) => $"{a} lunges at {r} with terrifying speed, fangs bared in an attempt to bite");
                descriptions.Add((a, r, w) => $"{a} circles {r} before quickly snapping toward their exposed skin");
                descriptions.Add((a, r, w) => $"{a} strikes low, aiming to sink its fangs into {FormatPossessive(r)} leg");

                GeneralAttackDescriptionsForType.Add(typeof(VenomousSnakeBite), descriptions);
            }
        }

        public static void SetHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(VenomousSnakeBite)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => $"{a} attempts to bite deeply into {FormatPossessive(r)} neck, injecting venom");
                descriptions.Add((a, r, w) => $"{a} snaps toward {FormatPossessive(r)} wrist, trying to inject venom");
                descriptions.Add((a, r, w) => $"{a} strikes at {FormatPossessive(r)} calf with a venomous bite attempt");
                descriptions.Add((a, r, w) => $"{a} attempts to sink its fangs into {FormatPossessive(r)} arm, venom seeping into the wound");
                descriptions.Add((a, r, w) => $"{a} lunges forward, trying to bite into {FormatPossessive(r)} chest");
                descriptions.Add((a, r, w) => $"{a} strikes at {FormatPossessive(r)} throat, aiming to inject venom");
                descriptions.Add((a, r, w) => $"{a} attempts to bite {FormatPossessive(r)} leg, trying to paralyze them with venom");
                descriptions.Add((a, r, w) => $"{a} latches onto {FormatPossessive(r)} side, attempting to inject venom as it bites");
                descriptions.Add((a, r, w) => $"{a} quickly strikes at {FormatPossessive(r)} forearm, venom flowing from its fangs in an attempt to bite");
                descriptions.Add((a, r, w) => $"{a} darts forward, attempting to sink fangs into {FormatPossessive(r)} ribs");
                descriptions.Add((a, r, w) => $"{a} strikes upward, aiming its bite at {FormatPossessive(r)} face");
                descriptions.Add((a, r, w) => $"{a} wraps around {FormatPossessive(r)} arm, attempting to sink its fangs into their flesh");
                descriptions.Add((a, r, w) => $"{a} lunges at {FormatPossessive(r)} shoulder, trying to bite deeply");
                descriptions.Add((a, r, w) => $"{a} snaps toward {FormatPossessive(r)} ankle, attempting to inject venom into the wound");
                descriptions.Add((a, r, w) => $"{a} attempts to sink its fangs into {FormatPossessive(r)} torso, venom seeping into the bite");

                HumanoidAttackDescriptionsForType.Add(typeof(VenomousSnakeBite), descriptions);
            }
        }

    }
}
