using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Fist : Weapon
    {
        public Fist() : base("Fist", "Fist", new Die(1, 2), DamageType.Bludgeoning)
        {
            CreateAttackDescriptions();
            CreateOffHandAttackDescriptions();
        }

        public static void CreateAttackDescriptions()
        {
            if (!AttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => { return $"{a} clenches their fist, driving a forceful punch towards {r}'s midsection"; });
                descriptions.Add((a, r, w) => { return $"{a} throws a powerful right hook, aiming for {r}'s temple"; });
                descriptions.Add((a, r, w) => { return $"{a} executes a spinning backhand punch, aiming at {r}'s cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} winds up and hurls a devastating overhand right at {r}'s head"; });
                descriptions.Add((a, r, w) => { return $"{a} pivots, channeling momentum into a crushing blow to {r}'s side"; });
                descriptions.Add((a, r, w) => { return $"{a} clenches their other fist, delivering a sharp jab towards {r}'s shoulder"; });
                descriptions.Add((a, r, w) => { return $"{a} rears back and launches a swift uppercut aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their stance and throws a strong straight punch to {r}'s throat"; });
                descriptions.Add((a, r, w) => { return $"{a} coils up and releases a forceful hook to {r}'s ribcage"; });
                descriptions.Add((a, r, w) => { return $"{a} adjusts their footing and delivers a rapid punch to {r}'s temple"; });
                descriptions.Add((a, r, w) => { return $"{a} flexes their arm, launching a brutal cross to {r}'s cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} gathers strength and propels a vicious jab directly at {r}'s jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} leans forward, firing a quick straight punch at {r}'s midsection"; });
                descriptions.Add((a, r, w) => { return $"{a} sets their feet and unleashes a powerful uppercut to {r}'s chin"; });
                descriptions.Add((a, r, w) => { return $"{a} tightens their fist, swinging a fierce hook to {r}'s side"; });
                descriptions.Add((a, r, w) => { return $"{a} quickly steps in and delivers a solid jab to {r}'s nose"; });
                descriptions.Add((a, r, w) => { return $"{a} finds an opening and sends a heavy straight punch to {r}'s forehead"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward with a forceful uppercut targeting {r}'s abdomen"; });
                descriptions.Add((a, r, w) => { return $"{a} concentrates and fires a rapid hook to {r}'s jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} primes and strikes with a devastating right cross to {r}'s cheek"; });
                descriptions.Add((a, r, w) => { return $"{a} braces and slams a powerful jab into {r}'s solar plexus"; });
                descriptions.Add((a, r, w) => { return $"{a} shifts weight and unleashes a sharp right hook to {r}'s rib"; });
                descriptions.Add((a, r, w) => { return $"{a} advances and hammers a quick uppercut to {r}'s lower jaw"; });
                descriptions.Add((a, r, w) => { return $"{a} angles their body and delivers a crushing punch to {r}'s collarbone"; });

                AttackDescriptionsForType.Add(typeof(Fist), descriptions);
            }
        }

        public static void CreateOffHandAttackDescriptions()
        {
            if (!OffHandAttackDescriptionsForType.ContainsKey(typeof(Fist)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly snaps an off-hand jab towards {r}'s midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} unexpectedly swings an off-hand hook, aiming for {r}'s temple"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an off-hand reverse slap, targeting {r}'s cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} performs a swift off-hand overhand strike at {r}'s head"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} rotates unexpectedly and throws an off-hand backhand strike to {r}'s side"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} flicks an off-hand jab quickly at {r}'s shoulder"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses an unanticipated off-hand straight punch from a side stance at {r}'s throat"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} twists slightly and strikes with an off-hand hook from a lower posture to {r}'s ribcage"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts their weight subtly and pops a quick off-hand punch from the side to {r}'s temple"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} delivers an off-hand cross from an adjusted stance, targeting {r}'s cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} generates an off-hand jab from an off-angle, aiming directly at {r}'s jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} leans slightly and delivers an off-hand straight punch from the side to {r}'s midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} sets their feet in a shifted stance and explodes with an off-hand uppercut to {r}'s chin"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly clenches and swings an off-hand hook from a side angle to {r}'s side"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly steps in from the opposite side and delivers an off-hand jab to {r}'s nose"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} finds an opening and sends an off-hand straight punch from a less expected angle to {r}'s forehead"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand uppercut from a lowered stance targeting {r}'s abdomen"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} concentrates and lashes out with an off-hand hook from a side posture to {r}'s jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} primes and connects an off-hand cross from an unconventional angle to {r}'s cheek"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} braces and launches an off-hand jab from the side into {r}'s solar plexus"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} shifts weight and drives an off-hand hook from a lowered position to {r}'s rib"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} advances with an off-hand uppercut from an off-angle to {r}'s lower jaw"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} angles their body slightly and strikes with an off-hand punch from the side to {r}'s collarbone"; });

                OffHandAttackDescriptionsForType.Add(typeof(Fist), offhandDescriptions);
            }
        }

    }
}
