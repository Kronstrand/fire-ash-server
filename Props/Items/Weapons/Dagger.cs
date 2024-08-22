using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Dagger : Weapon
    {
        public Dagger(string name, string description) : base(name, description, new Die(1, 4), DamageType.Piercing)
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
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Dagger)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => { return $"{a} strikes with {w.Name}, aiming directly at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} swings {w.Name} decisively towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} thrusts {w.Name} with precision at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} slashes through the air with {w.Name}, targeting {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} lunges forward, {w.Name} extended towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} executes a swift attack with {w.Name}, directed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} maneuvers {w.Name} in a swift motion, aiming for {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} delivers a calculated strike with {w.Name} towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} makes a quick jab with {w.Name}, aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} moves with agility, thrusting {w.Name} at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} directs {w.Name} skillfully towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} wields {w.Name} expertly, attacking {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} strikes swiftly, {w.Name} aimed directly at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} swings {w.Name} in a powerful arc towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} stabs with {w.Name}, aiming to pierce {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} uses {w.Name} to deliver a forceful blow at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} attacks decisively, {w.Name} poised towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} brandishes {w.Name} with intent, attacking {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} aims {w.Name} directly at {r}, ready to strike"; });
                descriptions.Add((a, r, w) => { return $"{a} thrusts {w.Name} forward, aiming for {r}"; });
                GeneralAttackDescriptionsForType.Add(typeof(Dagger), descriptions);
            }
        }

        public static void SetGeneralOffHandAttackDescriptionsForType()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Dagger)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} stealthily extends their off-hand, {w.Name} slicing a quick, sharp line towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly shifts their off-hand {w.Name}, aiming a precise, low stab towards {FormatPossessive(r)} abdomen"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} catches {r} off-guard with a sudden off-hand jab, the {w.Name} aiming for a tender spot"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} with a slight twist, their off-hand {w.Name} cuts an elusive path towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses their off-hand to flick the {w.Name} in a deceptive, angular strike towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly switches their {w.Name} to the off-hand, delivering a stabbing thrust unexpectedly towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} feints with the main hand, while the off-hand {w.Name} slices horizontally towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} keeps their {w.Name} low and unseen, then suddenly drives it upward in an off-hand thrust"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} redirects {FormatPossessive(r)} attention before the off-hand {w.Name} makes a silent plea for contact"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} manipulates the shadows, the off-hand {w.Name} emerging with a deadly intent towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} maintains a relaxed posture before the off-hand {w.Name} lunges in a swift, piercing motion"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} with a calculated pause, then a swift off-hand motion, the {w.Name} aims directly at {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} ensures their off-hand {w.Name} moves in a tight spiral, driving menacingly towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} employs a quick sidestep, their off-hand {w.Name} tracing a lethal half-circle"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} lets the {w.Name} in their off-hand whisper a threat, barely audible before it strikes towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} cradles the {w.Name} in the off-hand before it springs forth in a blink, aiming for {FormatPossessive(r)} midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} offers a slight nod, the prelude to the off-hand {w.Name}'s rapid journey towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} masks the movement of their off-hand, the {w.Name} then darting towards {r} with lethal precision"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Dagger), offhandDescriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Dagger)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => { return $"{a} deftly twirls their {w.Name}, casting a silver arc towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} swiftly drives their {w.Name} forward, as if painting a stroke of pain towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} gracefully lunges, their {w.Name} slicing through the air like a whisper towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} with a dancer's grace, pirouettes, their {w.Name} drawing a lethal line towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} flicks their wrist delicately, the {w.Name} darting like a viper's tongue towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} performs a balletic sweep with their {w.Name}, aiming a silent but deadly thrust at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} sends their {w.Name} gliding smoothly, carving an elegant arc aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} with a sly twist of their hand, the {w.Name} slips through the air, seeking {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unfurls their arm, the {w.Name} tracing a deadly spiral on its path to {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steps with a shadow's silence, {w.Name} poised for a swift, unseen strike towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} slices the air with their {w.Name}, a glint of silver streaking towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} arcs their {w.Name} with a flourish, the blade humming a deadly tune towards {r}."; });
                descriptions.Add((a, r, w) => { return $"{a} angles the {w.Name} with precision, its tip promising pain as it points at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} sways lightly before striking, the {w.Name}'s blade dancing dangerously close to {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} mimics the fall of a guillotine, their {w.Name} plummeting towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} carves a crescent with their {w.Name}, targeting the space just before {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} punctuates the air with a sharp stab, the {w.Name} aiming directly at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} twirls the {w.Name} expertly, its edge slicing towards {r} with deadly intent"; });
                descriptions.Add((a, r, w) => { return $"{a} pulls back momentarily, then propels their {w.Name} in a fierce jab towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} traces a chilling path with their {w.Name}, its cold steel seeking {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} with a sudden burst of clarity, thrusts their {w.Name} directly at {r}, like fate drawing a straight line"; });
                descriptions.Add((a, r, w) => { return $"{a} hides the {w.Name}'s intent until the last moment, when it springs towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} uses the light to disguise their movement, the {w.Name} appearing suddenly on its path to {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} mirrors the strike of a clock's hand at midnight, their {w.Name} punctually targeting {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} advances stealthily, their {w.Name} cutting an inevitable swath towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} releases a flurry of quick stabs, the {w.Name} dancing a lethal ballet towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} draws a short, brutal line with their {w.Name}, aiming to etch it upon {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} leans into a feint, swiftly correcting course with a piercing thrust towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} guides their {w.Name} in a low, hunting sweep, aiming to hamstring {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} orchestrates a cunning arc with their {w.Name}, its blade singing towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} hurls their body forward, {w.Name} leading like a spearhead aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} surprises {r} with a sudden reversal, the {w.Name} snapping back in a reverse grip"; });
                descriptions.Add((a, r, w) => { return $"{a} sweeps the {w.Name} in a deceptive curve, cloaking its true target until it nears {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} crafts a sharp angle with the {w.Name}, its blade plotting a precise route to {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} with a burst of agility, drives the {w.Name} in a vertical slash down towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} enacts a swift parry with one hand, the other thrusting the {w.Name} towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} veils their intent with a cloak of feints before the {w.Name} strikes towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} sets the {w.Name} ablaze with motion, casting a deadly shadow before striking {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unleashes a rapid succession of thrusts, each one a promise of peril to {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} displays a masterful grip, the {w.Name} spiraling towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} elicits a silent gasp from the air, the {w.Name} slicing a hushed threat at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} choreographs a quick lunge, {w.Name} aimed squarely at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} maneuvers with the subtlety of smoke, their {w.Name} manifesting suddenly at {FormatPossessive(r)} flank"; });
                descriptions.Add((a, r, w) => { return $"{a} deploys the {w.Name} with surgical precision, its tip tracing a deadly trajectory towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} employs a swift twist of their wrist, the {w.Name} diving daringly towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} executes a nimble sidestep, redirecting their {w.Name}'s bite towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} orchestrates the momentum of a spin, {w.Name} swiping fiercely at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} embodies the silence of the night, their {w.Name} strike as sudden as a shadow crossing {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} conjures an unexpected thrust, the {w.Name}'s point driving fiercely towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} closes the distance with a calculated step, their {w.Name} tracing a deadly line to {r}"; });
                HumanoidAttackDescriptionsForType.Add(typeof(Dagger), descriptions);
            }
        }

        public static void SetHumanoidOffHandAttackDescriptionsForType()
        {
            if (!HumanoidOffHandAttackDescriptionsForType.ContainsKey(typeof(Dagger)))
            {
                List<Func<string, string, Weapon, string>> offhandDescriptions = new List<Func<string, string, Weapon, string>>();
                offhandDescriptions.Add((a, r, w) => { return $"{a} stealthily extends their off-hand, {w.Name} slicing a quick, sharp line towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} subtly shifts their off-hand {w.Name}, aiming a precise, low stab towards {FormatPossessive(r)} abdomen"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} catches {r} off-guard with a sudden off-hand jab, the {w.Name} aiming for a tender spot"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} with a slight twist, their off-hand {w.Name} cuts an elusive path towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} uses their off-hand to flick the {w.Name} in a deceptive, angular strike towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} quickly switches their {w.Name} to the off-hand, delivering a stabbing thrust unexpectedly towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} feints with the main hand, while the off-hand {w.Name} slices horizontally towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} keeps their {w.Name} low and unseen, then suddenly drives it upward in an off-hand thrust"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} redirects {FormatPossessive(r)} attention before the off-hand {w.Name} makes a silent plea for contact"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} manipulates the shadows, the off-hand {w.Name} emerging with a deadly intent towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} maintains a relaxed posture before the off-hand {w.Name} lunges in a swift, piercing motion"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} with a calculated pause, then a swift off-hand motion, the {w.Name} aims directly at {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} ensures their off-hand {w.Name} moves in a tight spiral, driving menacingly towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} employs a quick sidestep, their off-hand {w.Name} tracing a lethal half-circle"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} lets the {w.Name} in their off-hand whisper a threat, barely audible before it strikes towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} cradles the {w.Name} in the off-hand before it springs forth in a blink, aiming for {FormatPossessive(r)} midsection"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} offers a slight nod, the prelude to the off-hand {w.Name}'s rapid journey towards {r}"; });
                offhandDescriptions.Add((a, r, w) => { return $"{a} masks the movement of their off-hand, the {w.Name} then darting towards {r} with lethal precision"; });

                HumanoidOffHandAttackDescriptionsForType.Add(typeof(Dagger), offhandDescriptions);
            }
        }
    }
}
