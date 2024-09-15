using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class InsectClaw : Weapon
    {
        public InsectClaw() : base("Insect Claw", "Insect Claw", new Die(1, 4), DamageType.Piercing, 0)
        {
            SetGeneralAttackDescriptionsForType();
            SetGeneralOffHandAttackDescriptionsForType();
            SetHumanoidAttackDescriptions();
            SetHumanoidOffHandAttackDescriptions();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(InsectClaw)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} thrusts their razor-sharp claw towards {r}.");
                descriptions.Add((a, r, w) => $"{a} swiftly lashes out at {r} with their claw, seeking to tear through.");
                descriptions.Add((a, r, w) => $"{a} lunges forward, driving their claw towards {r}.");
                descriptions.Add((a, r, w) => $"{a} curls their leg and strikes, aiming to embed their claw into {r}.");
                descriptions.Add((a, r, w) => $"{a} leaps at {r}, trying to stab their claw into the target.");
                descriptions.Add((a, r, w) => $"{a} swiftly slashes towards {r} with their sharp claw.");
                descriptions.Add((a, r, w) => $"{a} darts forward, claw bared, aiming for {r}.");
                descriptions.Add((a, r, w) => $"{a} twists and drives their claw towards {r}.");
                descriptions.Add((a, r, w) => $"{a} lunges at {r}, attempting to pierce with the claw.");
                descriptions.Add((a, r, w) => $"{a} slashes their claw at {r}, trying to cause deep cuts.");
                descriptions.Add((a, r, w) => $"{a} quickly strikes at {r} with their sharp claw.");
                descriptions.Add((a, r, w) => $"{a} lunges and strikes at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} strikes at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} darts in, aiming to stab {r} with their sharp claw.");
                descriptions.Add((a, r, w) => $"{a} swiftly stabs at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} lunges forward and drives their claw towards {r}.");
                descriptions.Add((a, r, w) => $"{a} strikes at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} quickly slashes at {r} with their sharp claw.");
                descriptions.Add((a, r, w) => $"{a} twists and lunges, stabbing at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} strikes at {r} with a quick jab of their claw.");
                descriptions.Add((a, r, w) => $"{a} drives their claw towards {r} with a quick thrust.");
                descriptions.Add((a, r, w) => $"{a} slashes at {r} with their sharp claw.");
                descriptions.Add((a, r, w) => $"{a} leaps and strikes at {r} with their claw.");
                descriptions.Add((a, r, w) => $"{a} darts in with a quick stab at {r} with their claw.");

                GeneralAttackDescriptionsForType.Add(typeof(InsectClaw), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!GeneralOffHandAttackDescriptionsForType.ContainsKey(typeof(InsectClaw)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} quickly snaps an off-hand claw strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} unexpectedly lunges with an off-hand claw, aiming for {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an off-hand claw to stab at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} performs a swift off-hand claw strike at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} rotates unexpectedly and strikes with an off-hand claw at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} flicks an off-hand claw quickly at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an unanticipated off-hand claw strike from a side stance at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} twists slightly and strikes with an off-hand claw from a lower posture towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts their weight subtly and snaps a quick off-hand claw from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers an off-hand claw strike from an adjusted stance, targeting {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} generates an off-hand claw strike from an off-angle, aiming directly at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} leans slightly and delivers an off-hand claw strike from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} sets their feet in a shifted stance and explodes with an off-hand claw strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} subtly clenches and snaps an off-hand claw from a side angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} quickly steps in from the opposite side and delivers an off-hand claw strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} finds an opening and sends an off-hand claw strike from a less expected angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand claw strike from a lowered stance targeting {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} concentrates and lashes out with an off-hand claw from a side posture towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} primes and connects an off-hand claw strike from an unconventional angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} braces and launches an off-hand claw strike from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts weight and drives an off-hand claw strike from a lowered position towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand claw strike from an off-angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} angles their body slightly and strikes with an off-hand claw from the side towards {r}.");

                GeneralOffHandAttackDescriptionsForType.Add(typeof(InsectClaw), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(InsectClaw)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => { return $"{a} thrusts their razor-sharp claw towards {FormatPossessive(r)} heart"; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly lashes out at {r} with their claw, seeking to tear through the chest"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward, driving their claw towards {FormatPossessive(r)} throat"; });
                descriptions.Add((a, r, w) => { return $"{a} curls their leg and strikes, aiming to embed their claw into {FormatPossessive(r)} abdomen"; });
                descriptions.Add((a, r, w) => { return $"{a} leaps at {r}, trying to stab their claw into the back"; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly slashes towards {FormatPossessive(r)} chest with their sharp claw"; });
                descriptions.Add((a, r, w) => { return $"{a} darts forward, claw bared, aiming for {FormatPossessive(r)} neck"; });
                descriptions.Add((a, r, w) => { return $"{a} twists and drives their claw towards {FormatPossessive(r)} ribs"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges at {r}, attempting to pierce their chest with the claw"; });
                descriptions.Add((a, r, w) => { return $"{a} slashes their claw at {FormatPossessive(r)} side, trying to cause deep cuts"; });
                descriptions.Add((a, r, w) => { return $"{a} quickly strikes at {FormatPossessive(r)} head with their sharp claw"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges and strikes at {FormatPossessive(r)} chest with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} strikes at {FormatPossessive(r)} heart with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} darts in, aiming to stab {r} with their sharp claw"; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly stabs at {FormatPossessive(r)} neck with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward and drives their claw towards {FormatPossessive(r)} chest"; });
                descriptions.Add((a, r, w) => { return $"{a} strikes at {r} with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} quickly slashes at {FormatPossessive(r)} chest with their sharp claw"; });
                descriptions.Add((a, r, w) => { return $"{a} twists and lunges, stabbing at {r} with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} strikes at {FormatPossessive(r)} heart with a quick jab of their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} drives their claw towards {FormatPossessive(r)} chest with a quick thrust"; });
                descriptions.Add((a, r, w) => { return $"{a} slashes at {FormatPossessive(r)} head with their sharp claw"; });
                descriptions.Add((a, r, w) => { return $"{a} leaps and strikes at {FormatPossessive(r)} chest with their claw"; });
                descriptions.Add((a, r, w) => { return $"{a} darts in with a quick stab at {FormatPossessive(r)} ribs with their claw"; });

                HumanoidAttackDescriptionsForType.Add(typeof(InsectClaw), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptions()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(InsectClaw)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly snaps an off-hand claw strike towards {FormatPossessive(r)} neck"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} unexpectedly lunges with an off-hand claw, aiming for {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an off-hand claw to stab at {FormatPossessive(r)} throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} performs a swift off-hand claw strike at {FormatPossessive(r)} heart"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} rotates unexpectedly and strikes with an off-hand claw at {FormatPossessive(r)} ribs"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} flicks an off-hand claw quickly at {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an unanticipated off-hand claw strike from a side stance at {FormatPossessive(r)} back"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} twists slightly and strikes with an off-hand claw from a lower posture towards {FormatPossessive(r)} abdomen"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their weight subtly and snaps a quick off-hand claw from the side towards {FormatPossessive(r)} throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} delivers an off-hand claw strike from an adjusted stance, targeting {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} generates an off-hand claw strike from an off-angle, aiming directly at {FormatPossessive(r)} head"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} leans slightly and delivers an off-hand claw strike from the side towards {FormatPossessive(r)} heart"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} sets their feet in a shifted stance and explodes with an off-hand claw strike towards {FormatPossessive(r)} throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly clenches and snaps an off-hand claw from a side angle towards {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly steps in from the opposite side and delivers an off-hand claw strike towards {FormatPossessive(r)} ribs"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} finds an opening and sends an off-hand claw strike from a less expected angle towards {FormatPossessive(r)} neck"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand claw strike from a lowered stance targeting {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} concentrates and lashes out with an off-hand claw from a side posture towards {FormatPossessive(r)} throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} primes and connects an off-hand claw strike from an unconventional angle towards {FormatPossessive(r)} heart"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} braces and launches an off-hand claw strike from the side towards {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts weight and drives an off-hand claw strike from a lowered position towards {FormatPossessive(r)} ribs"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand claw strike from an off-angle towards {FormatPossessive(r)} chest"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} angles their body slightly and strikes with an off-hand claw from the side towards {FormatPossessive(r)} neck"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(InsectClaw), offhandDescriptions);
            }
        }
    }
}
