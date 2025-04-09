using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    [Serializable]
    internal class Fist : Weapon
    {
        public Fist() : base("Fist", "Fist", new Die(1, 1), DamageType.Bludgeoning, 0)
        {
            CreateGeneralAttackDescriptions();
            CreateGeneralOffHandAttackDescriptions();
            CreateHumanoidAttackDescriptions();
            CreateHumanoidOffHandAttackDescriptions();
        }

        public static void CreateGeneralAttackDescriptions()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => $"{a} delivers a forceful punch towards {r}");
                descriptions.Add((a, r, w) => $"{a} throws a powerful hook at {r}");
                descriptions.Add((a, r, w) => $"{a} executes a spinning backhand punch directed at {r}");
                descriptions.Add((a, r, w) => $"{a} hurls a devastating overhand right towards {r}");
                descriptions.Add((a, r, w) => $"{a} channels momentum into a crushing blow to {r}");
                descriptions.Add((a, r, w) => $"{a} delivers a sharp jab towards {r}");
                descriptions.Add((a, r, w) => $"{a} rears back and launches a swift uppercut at {r}");
                descriptions.Add((a, r, w) => $"{a} throws a strong straight punch at {r}");
                descriptions.Add((a, r, w) => $"{a} releases a forceful hook towards {r}");
                descriptions.Add((a, r, w) => $"{a} adjusts their footing and delivers a rapid punch at {r}");
                descriptions.Add((a, r, w) => $"{a} launches a brutal cross towards {r}");
                descriptions.Add((a, r, w) => $"{a} propels a vicious jab directly at {r}");
                descriptions.Add((a, r, w) => $"{a} fires a quick straight punch at {r}");
                descriptions.Add((a, r, w) => $"{a} unleashes a powerful uppercut at {r}");
                descriptions.Add((a, r, w) => $"{a} swings a fierce hook towards {r}");
                descriptions.Add((a, r, w) => $"{a} delivers a solid jab at {r}");
                descriptions.Add((a, r, w) => $"{a} sends a heavy straight punch towards {r}");
                descriptions.Add((a, r, w) => $"{a} lunges forward with a forceful uppercut at {r}");
                descriptions.Add((a, r, w) => $"{a} fires a rapid hook at {r}");
                descriptions.Add((a, r, w) => $"{a} strikes with a devastating right cross towards {r}");
                descriptions.Add((a, r, w) => $"{a} slams a powerful jab into {r}");
                descriptions.Add((a, r, w) => $"{a} unleashes a sharp hook towards {r}");
                descriptions.Add((a, r, w) => $"{a} hammers a quick uppercut at {r}");
                descriptions.Add((a, r, w) => $"{a} delivers a crushing punch to {r}");

                GeneralAttackDescriptionsForType.Add(typeof(Fist), descriptions);
            }
        }

        public static void CreateGeneralOffHandAttackDescriptions()
        {
            if (!GeneralOffHandAttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => $"{a} snaps an off-hand jab towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} unexpectedly swings an off-hand hook at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} uses an off-hand reverse slap directed at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} performs a swift off-hand overhand strike towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} rotates and throws an off-hand backhand strike at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} flicks an off-hand jab quickly towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} strikes with an off-hand straight punch from a side stance at {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} twists and delivers an off-hand hook towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts weight and pops a quick off-hand punch from the side towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} delivers an off-hand cross from an adjusted stance towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} generates an off-hand jab from an off-angle towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} leans and delivers an off-hand straight punch from the side towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} sets their feet and explodes with an off-hand uppercut towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} clenches and swings an off-hand hook from a side angle towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} steps in and delivers an off-hand jab towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} finds an opening and sends an off-hand straight punch towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand uppercut towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} lashes out with an off-hand hook towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} primes and connects an off-hand cross towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} launches an off-hand jab towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} shifts weight and drives an off-hand hook towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} advances with an off-hand uppercut towards {r}");
                offhandDescriptions.Add((a, r, w) => $"{a} angles their body and strikes with an off-hand punch towards {r}");

                GeneralOffHandAttackDescriptionsForType.Add(typeof(Fist), offhandDescriptions);
            }
        }

        public static void CreateHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => { return $"{a} clenches their fist, driving a forceful punch towards {FormatPossessive(r)} midsection"; });
                descriptions.Add((a, r, w) => { return $"{a} throws a powerful right hook, aiming for {FormatPossessive(r)} temple"; });
                descriptions.Add((a, r, w) => { return $"{a} executes a spinning backhand punch, aiming at {FormatPossessive(r)} cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} winds up and hurls a devastating overhand right at {FormatPossessive(r)} head"; });
                descriptions.Add((a, r, w) => { return $"{a} pivots, channeling momentum into a crushing blow to {FormatPossessive(r)} side"; });
                descriptions.Add((a, r, w) => { return $"{a} clenches their other fist, delivering a sharp jab towards {FormatPossessive(r)} shoulder"; });
                descriptions.Add((a, r, w) => { return $"{a} rears back and launches a swift uppercut aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their stance and throws a strong straight punch to {FormatPossessive(r)} throat"; });
                descriptions.Add((a, r, w) => { return $"{a} coils up and releases a forceful hook to {FormatPossessive(r)} ribcage"; });
                descriptions.Add((a, r, w) => { return $"{a} adjusts their footing and delivers a rapid punch to {FormatPossessive(r)} temple"; });
                descriptions.Add((a, r, w) => { return $"{a} flexes their arm, launching a brutal cross to {FormatPossessive(r)} cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} gathers strength and propels a vicious jab directly at {FormatPossessive(r)} jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} leans forward, firing a quick straight punch at {FormatPossessive(r)} midsection"; });
                descriptions.Add((a, r, w) => { return $"{a} sets their feet and unleashes a powerful uppercut to {FormatPossessive(r)} chin"; });
                descriptions.Add((a, r, w) => { return $"{a} tightens their fist, swinging a fierce hook to {FormatPossessive(r)} side"; });
                descriptions.Add((a, r, w) => { return $"{a} quickly steps in and delivers a solid jab to {FormatPossessive(r)} nose"; });
                descriptions.Add((a, r, w) => { return $"{a} finds an opening and sends a heavy straight punch to {FormatPossessive(r)} forehead"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward with a forceful uppercut targeting {FormatPossessive(r)} abdomen"; });
                descriptions.Add((a, r, w) => { return $"{a} concentrates and fires a rapid hook to {FormatPossessive(r)} jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} primes and strikes with a devastating right cross to {FormatPossessive(r)} cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} braces and slams a powerful jab into {FormatPossessive(r)} solar plexus"; });
                descriptions.Add((a, r, w) => { return $"{a} shifts weight and unleashes a sharp right hook to {FormatPossessive(r)} rib"; });
                descriptions.Add((a, r, w) => { return $"{a} advances and hammers a quick uppercut to {FormatPossessive(r)} lower jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} angles their body and delivers a crushing punch to {FormatPossessive(r)} collarbone"; });

                HumanoidAttackDescriptionsForType.Add(typeof(Fist), descriptions);
            }
        }

        public static void CreateHumanoidOffHandAttackDescriptions()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly snaps an off-hand jab towards {FormatPossessive(r)} midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} unexpectedly swings an off-hand hook, aiming for {FormatPossessive(r)} temple"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an off-hand reverse slap, targeting {FormatPossessive(r)} cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} performs a swift off-hand overhand strike at {FormatPossessive(r)} head"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} rotates unexpectedly and throws an off-hand backhand strike to {FormatPossessive(r)} side"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} flicks an off-hand jab quickly at {FormatPossessive(r)} shoulder"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an unanticipated off-hand straight punch from a side stance at {FormatPossessive(r)} throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} twists slightly and strikes with an off-hand hook from a lower posture to {FormatPossessive(r)} ribcage"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their weight subtly and pops a quick off-hand punch from the side to {FormatPossessive(r)} temple"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} delivers an off-hand cross from an adjusted stance, targeting {FormatPossessive(r)} cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} generates an off-hand jab from an off-angle, aiming directly at {FormatPossessive(r)} jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} leans slightly and delivers an off-hand straight punch from the side to {FormatPossessive(r)} midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} sets their feet in a shifted stance and explodes with an off-hand uppercut to {FormatPossessive(r)} chin"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly clenches and swings an off-hand hook from a side angle to {FormatPossessive(r)} side"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly steps in from the opposite side and delivers an off-hand jab to {FormatPossessive(r)} nose"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} finds an opening and sends an off-hand straight punch from a less expected angle to {FormatPossessive(r)} forehead"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand uppercut from a lowered stance targeting {FormatPossessive(r)} abdomen"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} concentrates and lashes out with an off-hand hook from a side posture to {FormatPossessive(r)} jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} primes and connects an off-hand cross from an unconventional angle to {FormatPossessive(r)} cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} braces and launches an off-hand jab from the side into {FormatPossessive(r)} solar plexus"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts weight and drives an off-hand hook from a lowered position to {FormatPossessive(r)} rib"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand uppercut from an off-angle to {FormatPossessive(r)} lower jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} angles their body slightly and strikes with an off-hand punch from the side to {FormatPossessive(r)} collarbone"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Fist), offhandDescriptions);
            }
        }

    }
}
