using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Tendril : Weapon
    {
        public Tendril() : base("Tendril", "Tendril", new Die(1, 4), DamageType.Slashing, 0)
        {
            SetGeneralAttackDescriptionsForType();
            SetGeneralOffHandAttackDescriptionsForType();
            SetHumanoidAttackDescriptions();
            SetHumanoidOffHandAttackDescriptions();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Tendril)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} lashes out at {r} with a swift strike of their tendril.");
                descriptions.Add((a, r, w) => $"{a} whips their tendril towards {r}, aiming to slice through flesh.");
                descriptions.Add((a, r, w) => $"{a} snaps their tendril at {r}, looking to inflict deep cuts.");
                descriptions.Add((a, r, w) => $"{a} slashes their tendril at {r}, the motion quick and precise.");
                descriptions.Add((a, r, w) => $"{a} swings their tendril in a wide arc towards {r}.");
                descriptions.Add((a, r, w) => $"{a} flicks their tendril at {r}, the tip cutting through the air.");
                descriptions.Add((a, r, w) => $"{a} coils their tendril before snapping it towards {r}.");
                descriptions.Add((a, r, w) => $"{a} twists their body and strikes at {r} with their tendril.");
                descriptions.Add((a, r, w) => $"{a} aims a sharp tendril strike at {r}'s limbs.");
                descriptions.Add((a, r, w) => $"{a} lunges forward, tendril whipping towards {r}.");

                GeneralAttackDescriptionsForType.Add(typeof(Tendril), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!GeneralOffHandAttackDescriptionsForType.ContainsKey(typeof(Tendril)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();

                offhandDescriptions.Add((a, r, w) => $"{a} quickly snaps an off-hand tendril strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} unexpectedly lashes out with an off-hand tendril, aiming for {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an off-hand tendril to slash at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} performs a swift off-hand tendril strike at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} rotates unexpectedly and strikes with an off-hand tendril at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} flicks an off-hand tendril quickly at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an unanticipated off-hand tendril strike from a side stance at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} twists slightly and strikes with an off-hand tendril from a lower posture towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts their weight subtly and snaps a quick off-hand tendril from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers an off-hand tendril strike from an adjusted stance, targeting {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} generates an off-hand tendril strike from an off-angle, aiming directly at {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} leans slightly and delivers an off-hand tendril strike from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} sets their feet in a shifted stance and explodes with an off-hand tendril strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} subtly clenches and snaps an off-hand tendril from a side angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} quickly steps in from the opposite side and delivers an off-hand tendril strike towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} finds an opening and sends an off-hand tendril strike from a less expected angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand tendril strike from a lowered stance targeting {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} concentrates and lashes out with an off-hand tendril from a side posture towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} primes and connects an off-hand tendril strike from an unconventional angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} braces and launches an off-hand tendril strike from the side towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts weight and drives an off-hand tendril strike from a lowered position towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand tendril strike from an off-angle towards {r}.");
                offhandDescriptions.Add((a, r, w) => $"{a} angles their body slightly and strikes with an off-hand tendril from the side towards {r}.");

                GeneralOffHandAttackDescriptionsForType.Add(typeof(Tendril), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Tendril)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => { return $"{a} lashes their tendril towards {FormatPossessive(r)} chest, aiming to slice through."; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly strikes at {r} with their tendril, seeking to tear through the chest."; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward, driving their tendril towards {FormatPossessive(r)} throat."; });
                descriptions.Add((a, r, w) => { return $"{a} curls their tendril and strikes, aiming to embed it into {FormatPossessive(r)} abdomen."; });
                descriptions.Add((a, r, w) => { return $"{a} leaps at {r}, trying to slash their tendril into their back."; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly slashes towards {FormatPossessive(r)} chest with their sharp tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} darts forward, tendril bared, aiming for {FormatPossessive(r)} neck."; });
                descriptions.Add((a, r, w) => { return $"{a} twists and drives their tendril towards {FormatPossessive(r)} ribs."; });
                descriptions.Add((a, r, w) => { return $"{a} lunges at {r}, attempting to slice their chest with the tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} slashes their tendril at {FormatPossessive(r)} side, trying to cause deep cuts."; });
                descriptions.Add((a, r, w) => { return $"{a} quickly strikes at {FormatPossessive(r)} head with their sharp tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} lunges and strikes at {FormatPossessive(r)} chest with their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} darts in, aiming to slash {r} with their sharp tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly slashes at {FormatPossessive(r)} neck with their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward and drives their tendril towards {FormatPossessive(r)} chest."; });
                descriptions.Add((a, r, w) => { return $"{a} strikes at {r} with their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} quickly slashes at {FormatPossessive(r)} chest with their sharp tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} twists and lunges, slashing at {r} with their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} strikes at {FormatPossessive(r)} chest with a quick slash of their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} drives their tendril towards {FormatPossessive(r)} chest with a quick thrust."; });
                descriptions.Add((a, r, w) => { return $"{a} slashes at {FormatPossessive(r)} head with their sharp tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} leaps and strikes at {FormatPossessive(r)} chest with their tendril."; });
                descriptions.Add((a, r, w) => { return $"{a} darts in with a quick slash at {FormatPossessive(r)} ribs with their tendril."; });

                HumanoidAttackDescriptionsForType.Add(typeof(Tendril), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptions()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Tendril)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly snaps an off-hand tendril strike towards {FormatPossessive(r)} neck."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} unexpectedly lashes with an off-hand tendril, aiming for {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an off-hand tendril to slash at {FormatPossessive(r)} throat."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} rotates unexpectedly and strikes with an off-hand tendril at {FormatPossessive(r)} ribs."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} flicks an off-hand tendril quickly at {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} twists slightly and strikes with an off-hand tendril from a lower posture towards {FormatPossessive(r)} abdomen."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their weight subtly and snaps a quick off-hand tendril from the side towards {FormatPossessive(r)} throat."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} delivers an off-hand tendril strike from an adjusted stance, targeting {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} sets their feet in a shifted stance and explodes with an off-hand tendril strike towards {FormatPossessive(r)} throat."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly clenches and snaps an off-hand tendril from a side angle towards {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} finds an opening and sends an off-hand tendril strike from a less expected angle towards {FormatPossessive(r)} neck."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand tendril strike from a lowered stance targeting {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} concentrates and lashes out with an off-hand tendril from a side posture towards {FormatPossessive(r)} throat."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} braces and launches an off-hand tendril strike from the side towards {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand tendril strike from an off-angle towards {FormatPossessive(r)} chest."; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} angles their body slightly and strikes with an off-hand tendril from the side towards {FormatPossessive(r)} neck."; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Tendril), offhandDescriptions);
            }
        }
    }
}
