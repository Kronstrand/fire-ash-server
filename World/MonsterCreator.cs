using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armoring;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World.BioMechWorld.Temple;
using fire_ash_server.World.Goldfield;

namespace fire_ash_server.World
{
    static class MonsterCreator
    {

        public static List<string> skeletonNames = new()
{
            "Cracked", "Splintered", "Shattered",
            "Fractured", "Broken", "Jawless", "Headless",
            "Dust covered", "Grave worn", "Tomb worn",
            "Battle worn", "War torn", "Blade marked",
            "Arrow pierced", "Rib broken", "Jaw broken",
            "Skull cracked", "Spine snapped", "Pelvis shattered",
            "Finger missing", "Handless", "Footless",
            "Hollow eyed", "Staggering", "Slack jawed",
            "Empty eyed", "Dark eyed", "Glowing eyed",
            "Rattling", "Clattering", "Loose jointed",
            "Misaligned", "Disjointed", "Reassembled",
            "Poorly assembled", "Half formed",
            "Mud encrusted", "Moss covered",
            "Lichen covered", "Ash covered", "Soot stained",
            "Burned bone", "Charred bone", "Blackened bone",
            "Bleached bone", "Ivory white", "Yellowed bone",
            "Ancient", "Forgotten", "Unburied",
            "Unearthed", "Desecrated", "Rot stained",
            "Wailing", "Silent", "Restless",
            "Rusted chained", "Cave stained", 
        };

        public static List<string> wolfNames = new()
        {
            "Scarred", "Broken tailed", "One eared",
            "Torn eared", "Crooked jawed", "Fang missing",
            "Mottled furred", "Ragged furred", "Pale furred",
            "Green eyed", "Amber eyed",
            "Cloud eyed", "Bloodshot eyed",
            "Yellow eyed", "Black eyed", "Twisted pawed",
            "Split eared", "Eye scarred", "Muzzle scarred",
            "Burn scarred", "Long muzzled", "Short muzzled",
            "Thick coated", "Thin coated", "Brindled",
            "Dark masked", "Ridge furred",
            "Faded furred", "Patchy furred", "Uneven furred",
            "Clumped furred", "Matt furred", "Shaggy furred",
            "Smooth furred", "Coarse furred", "Tuft eared",
            "Bent eared", "Folded eared", "Nicked eared",
            "Cracked nosed", "Split nosed", "Scar lined",
            "Grizzled muzzled", "Dust stained", "Tailless",
            "Mud spattered", "Grime streaked", "Soot marked",
            "White streaked", "Dark streaked", "Flecked furred",
            "Speckled furred", "Limping", "Lean framed", "Lopsided jawed",
            "Albino", "White furred", "Black furred", "Gray furred",
            "Brown furred", "Red furred", "Cream furred",
            "Golden furred", "Dark furred", "Jaw scarred"
        };

        public static Character Shadecreeper()
        {
            Character shadecreeper = new Character(
                "Shadecreeper",
                "A small, elusive figure with large, " +
                "reflective eyes. Its fur is matted and its movements are unnervingly quiet, " +
                "as though it has perfected the art of avoiding attention.",
                Kindred.Fay,
                CreatureType.Humanoid, // Humanoid type due to their upright stance and use of tools
                8,  // strength - physically weak, relies more on agility and stealth
                14, // dexterity - high dexterity for stealth and archery
                10, // constitution - average toughness, can survive in harsh environments
                7,  // intelligence - limited intelligence, primarily instinct-driven
                12, // wisdom - has a natural cunning and awareness of its surroundings
                8,  // charisma - not charismatic, tends to induce fear or unease
                "The Shadecreeper lies still, its shadowy form now lifeless. Even in death, its presence seems to cast a darkness around it, " +
                "as if the shadows cling to its body."
            );
            shadecreeper.AddEquippedItem(InventorySlot.MainHand, WeaponList.StoneKnife());
            shadecreeper.AddEquippedItem(InventorySlot.Ranged, WeaponList.TribalShortBow());

            // Set the creature's health
            shadecreeper.HP = 7;

            // Add special feats or abilities
            shadecreeper.AddFeat(FeatKey.Stealth); // Allows them to move undetected
            shadecreeper.AddFeat(FeatKey.DarkVision); // Can see clearly in low-light conditions
            shadecreeper.AddFeat(FeatKey.MeleeAttack);
            shadecreeper.AddFeat(FeatKey.RangedAttack);
            shadecreeper.AddToInventory(TempleTrinketList.GetRandom());
            shadecreeper.AddToInventory(Coins.GenerateCoins(40));

            if (!Character.hitReactions.ContainsKey(shadecreeper.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>();
                hitDescriptions.Add((a, r) => { return $"{r} clenches its teeth, a pained grunt escaping through its sharp breath."; });
                hitDescriptions.Add((a, r) => { return $"{r} shudders, the shock of the blow momentarily contorting its shadowy form."; });
                hitDescriptions.Add((a, r) => { return $"{r} squints in pain, a low growl rumbling from its throat."; });
                hitDescriptions.Add((a, r) => { return $"{r} stiffens, a harsh gasp breaking the silence as it absorbs the impact."; });
                hitDescriptions.Add((a, r) => { return $"{r} jerks back, eyes wide with surprise and pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} lets out a sharp yelp, the sound echoing off the forest trees."; });
                hitDescriptions.Add((a, r) => { return $"{r} arches its back in pain, a silent hiss shaping its mouth."; });
                hitDescriptions.Add((a, r) => { return $"{r} flinches hard, its eyes briefly flashing a fierce glow of anger."; });
                hitDescriptions.Add((a, r) => { return $"{r} growls lowly, bearing its teeth in a spontaneous snarl."; });
                hitDescriptions.Add((a, r) => { return $"{r} stumbles slightly, catching itself without a sound, pain flickering in its eyes."; });
                hitDescriptions.Add((a, r) => { return $"{r} emits a pained squeak, almost childlike, as it recoils."; });
                hitDescriptions.Add((a, r) => { return $"{r} grits its teeth, emitting a muffled moan as it steadies its stance."; });
                hitDescriptions.Add((a, r) => { return $"{r} blinks rapidly, disoriented by the blow, a growl of frustration escaping."; });
                hitDescriptions.Add((a, r) => { return $"{r} curls inward momentarily, a whimper of discomfort evident."; });
                hitDescriptions.Add((a, r) => { return $"{r} jerks its head back, a pained expression flashing across its face."; });
                hitDescriptions.Add((a, r) => { return $"{r} winces severely, a strained noise leaking from its lips."; });
                hitDescriptions.Add((a, r) => { return $"{r} shivers, a soft groan marking the moment of impact."; });
                hitDescriptions.Add((a, r) => { return $"{r} recoils, lips parted in a silent scream, eyes momentarily losing their gleam."; });
                hitDescriptions.Add((a, r) => { return $"{r} grimaces, a quick intake of breath signaling its discomfort."; });
                hitDescriptions.Add((a, r) => { return $"{r} locks eyes with {a}, a pained snarl distorting its features as it bears the hit."; });
                hitDescriptions.Add((a, r) => { return $"{r} glares at {a}, emitting a strained grunt as the impact resonates through its body."; });
                hitDescriptions.Add((a, r) => { return $"{r} recoils from {a}'s strike, a sharp hiss escaping as it stares back fiercely."; });
                hitDescriptions.Add((a, r) => { return $"{r} winces at {a}, a low growl filling the air as it absorbs the pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} briefly loses focus on {a}, a gasp of pain cutting through the tension."; });
                hitDescriptions.Add((a, r) => { return $"{r} stumbles backward from {a}'s blow, eyes narrowing in anger and pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} grits its teeth at {a}, a pained roar echoing as it regains its composure."; });
                hitDescriptions.Add((a, r) => { return $"{r} shudders under {a}'s assault, quickly regaining its eerie stare."; });
                hitDescriptions.Add((a, r) => { return $"{r} meets {a}'s gaze with a painful grimace, holding its ground despite the hit."; });
                Character.hitReactions.Add(shadecreeper.Name, hitDescriptions);
            }

            return shadecreeper;
        }

        public static Character Skeleton()
        {
            var rnd = Random.Shared;
            int i = rnd.Next(skeletonNames.Count);

            Character skeleton = new Character(
                $"{skeletonNames[i]} Skeleton",
                "A humanoid figure made entirely of bones, moving with an unnatural clatter, driven by a silent, unseen force.",
                Kindred.None,
                CreatureType.Undead,
                10,  // strength - average strength for an undead animated by dark forces
                8,   // dexterity - slightly awkward, its movements stiff and jerky
                12,  // constitution - sturdy in death, capable of withstanding minor damage
                5,   // intelligence - limited intelligence, functioning on basic instincts
                7,   // wisdom - not very aware, simply following basic commands
                5,   // charisma - devoid of any personal charm or humanity
                $"$The {skeletonNames[i]} Skeleton lies motionless, its structure reduced to a faint humanoid shape."
            );

            //skeleton.AddEquippedItem(InventorySlot.MainHand, WeaponList.RustedSword());
            //skeleton.AddEquippedItem(InventorySlot.OffHand, ArmorList.WornWoodenShield());

            // Set the creature's health
            skeleton.HP = 8;

            // Add special feats or abilities
            skeleton.AddFeat(FeatKey.MeleeAttack);
            //skeleton.AddFeat(FeatKey.DarkVision); // Enables it to see in the dark, typical for undead creatures

            // Optional: Add some simple loot or effects
            skeleton.AddToInventory(Coins.GenerateCoins(5));

            // Skeleton hit reactions
            if (!Character.hitReactions.ContainsKey(skeleton.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>();
                hitDescriptions.Add((a, r) => { return $"{r}'s bones rattle loudly as the blow strikes, but it remains unfazed."; });
                hitDescriptions.Add((a, r) => { return $"{r} jerks unnaturally from the impact, its bones clattering with the force."; });
                hitDescriptions.Add((a, r) => { return $"{r} stumbles briefly, but the dark force animating it pulls it upright again."; });
                hitDescriptions.Add((a, r) => { return $"{r} creaks as the attack hits, but its skeletal form shows little sign of damage."; });
                hitDescriptions.Add((a, r) => { return $"{r} shudders as the blow lands, a few bones cracking under the pressure."; });
                hitDescriptions.Add((a, r) => { return $"{r} clatters as the attack strikes, sending a fine dust of bone particles into the air."; });
                hitDescriptions.Add((a, r) => { return $"{r}'s ribcage compresses slightly, emitting a faint cracking noise."; });
                hitDescriptions.Add((a, r) => { return $"{r} loses a bone with the force of the hit, but the dark magic holds it together."; });
                hitDescriptions.Add((a, r) => { return $"{r} sways slightly, its bones clicking together in an eerie, hollow sound."; });
                hitDescriptions.Add((a, r) => { return $"{r}'s skull twists awkwardly from the blow, the eerie glow in its eyes flickering."; });
                hitDescriptions.Add((a, r) => { return $"{r} stumbles, a leg bone rattling loose, but it keeps moving."; });
                hitDescriptions.Add((a, r) => { return $"{r} clinks loudly as the impact reverberates through its skeletal form."; });
                hitDescriptions.Add((a, r) => { return $"{r}'s spine arches unnaturally, creaking under the pressure of the strike."; });
                hitDescriptions.Add((a, r) => { return $"{r} briefly collapses into a pile of bones, only to reassemble with unnatural speed."; });
                hitDescriptions.Add((a, r) => { return $"{r} flinches as part of its ribcage shatters, the dark magic trying to hold it together."; });

                Character.hitReactions.Add(skeleton.Name, hitDescriptions);
            }

            return skeleton;
        }

        public static Character GiantSnake()
        {
            Character giantSnake = new Character(
                "Giant Snake",
                "A massive serpent with shimmering scales and slit-like eyes that glow with a predatory hunger. " +
                "Its coiled form ripples with powerful muscle, and its movements are unnervingly silent, " +
                "as though it stalks its prey with lethal precision.",
                Kindred.None,
                CreatureType.Beast,
                13,  // strength - high physical power for constriction and biting
                12,  // dexterity - above-average dexterity for swift, fluid movements
                11,  // constitution - resilient, capable of withstanding attacks
                7,   // intelligence - low intelligence, driven by primal instincts
                10,  // wisdom - moderate wisdom, relying on keen senses to hunt
                8,   // charisma - not charismatic, but imposes fear through presence alone
                "The massive serpent's body lies motionless, its once-glimmering eyes now dull. " +
                "Even in death, its coiled form appears ready to strike, and the eerie silence in its wake " +
                "leaves a lasting sense of unease in the air."
            );
            
            // Set the creature's health
            giantSnake.HP = 12;

            // Add special feats or abilities
            //giantSnake.AddFeat(FeatKey.Constriction); // Can immobilize its prey with a powerful squeeze
            giantSnake.AddFeat(FeatKey.DarkVision); // Can see clearly in low-light conditions
            giantSnake.AddFeat(FeatKey.BiteAttack); // Strong melee attack from its bite
            giantSnake.AddEquippedItem(InventorySlot.Teeth, new VenomousSnakeBite());

            if (!Character.hitReactions.ContainsKey(giantSnake.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>();
                hitDescriptions.Add((a, r) => { return $"{r} hisses sharply, its coiled body rippling in pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} recoils slightly, the impact causing its massive body to quiver."; });
                hitDescriptions.Add((a, r) => { return $"{r} lets out a deep, guttural hiss, its eyes narrowing in pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} shudders, the scales on its body bristling as it absorbs the hit."; });
                hitDescriptions.Add((a, r) => { return $"{r} jerks back, the force of the blow rippling through its muscular coils."; });
                hitDescriptions.Add((a, r) => { return $"{r} emits a low growl, venom dripping from its fangs in response to the pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} stiffens, its long body tensing as it endures the attack."; });
                hitDescriptions.Add((a, r) => { return $"{r} flinches, a sharp hiss escaping as its tail flicks angrily."; });
                hitDescriptions.Add((a, r) => { return $"{r} briefly recoils, its massive form twitching as it braces against the pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} rears up slightly, its eyes flashing with a mixture of anger and pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} lets out a sharp, furious hiss, its body coiling tighter in reaction."; });
                hitDescriptions.Add((a, r) => { return $"{r} momentarily pulls back, its gleaming fangs bared in a silent snarl."; });
                hitDescriptions.Add((a, r) => { return $"{r} glares fiercely at {a}, its eyes burning with a cold, venomous fury."; });
                hitDescriptions.Add((a, r) => { return $"{r} shudders under {a}'s strike, its body writhing in a serpentine manner."; });
                hitDescriptions.Add((a, r) => { return $"{r} hisses sharply, its scales rustling with irritation and pain."; });
                hitDescriptions.Add((a, r) => { return $"{r} recoils from {a}'s blow, a vicious hiss rumbling from deep within."; });
                hitDescriptions.Add((a, r) => { return $"{r} flinches as {a} strikes, its body twisting in discomfort."; });
                hitDescriptions.Add((a, r) => { return $"{r} meets {a}'s gaze, its eyes gleaming with cold, predatory anger."; });
                hitDescriptions.Add((a, r) => { return $"{r} curls inward briefly, a harsh rasp marking its reaction to the strike."; });
                Character.hitReactions.Add(giantSnake.Name, hitDescriptions);
            }

            return giantSnake;
        }

        public static Character SerpentDevil()
        {
            Character serpentDevil = new Character(
                "Serpent Devil",
                "A nightmarish fusion of man and serpent, the Serpent Devil towers with its grotesque form of scaled flesh, " +
                "a venomous tail coiled and ready to strike. Its glowing, slitted eyes radiate malice. " +
                "The beast moves with unnatural fluidity, blending demonic cunning with predatory instinct.",
                Kindred.None,
                CreatureType.Monstrosity, // Monstrous hybrid of serpent and demon
                15,  // strength - immense physical power for crushing and ripping apart prey
                12,  // dexterity - swift and serpentine movements, capable of quick strikes
                14,  // constitution - resilient against attacks, fortified by demonic energy
                10,  // intelligence - cunning and strategic, with a predatory intellect
                12,  // wisdom - heightened awareness of its surroundings
                8,   // charisma - terrifying rather than charming, its presence inspires dread
                "The Serpent Devil collapses into a heap of twisted scales and claws, its venom pooling in dark puddles around its coiled form. " +
                "Even in death, its malevolent gaze seems to linger, searing terror into the minds of those who dared face it."
            );

            // Set the creature's health
            serpentDevil.HP = 17;
            serpentDevil.DefaultHand = new BeastClaw();
            serpentDevil.AddFeat(FeatKey.DarkVision); // Can see clearly in low-light conditions
            serpentDevil.AddFeat(FeatKey.DualWield);
            serpentDevil.AddFeat(FeatKey.BiteAttack); //snakebit

            serpentDevil.AddEquippedItem(InventorySlot.Teeth, new TailSnakeBite());

            // Optional: Add loot or inventory
            serpentDevil.AddToInventory(TempleTrinketList.GetRandom()); // Mystical loot item

            // Serpent Devil hit reactions
            if (!Character.hitReactions.ContainsKey(serpentDevil.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>();
                hitDescriptions.Add((a, r) => { return $"{r} snarls, its venom-coated fangs snapping dangerously at {a}."; });
                hitDescriptions.Add((a, r) => { return $"{r} recoils briefly, its scaled tail thrashing with deadly intent."; });
                hitDescriptions.Add((a, r) => { return $"{r} hisses furiously, its glowing eyes narrowing with pain and anger."; });
                hitDescriptions.Add((a, r) => { return $"{r} stiffens, its muscular coils rippling as it absorbs the blow."; });
                hitDescriptions.Add((a, r) => { return $"{r} lashes out instinctively, its claws scraping against the ground in frustration."; });
                hitDescriptions.Add((a, r) => { return $"{r} emits a guttural growl, venom dripping from its elongated fangs."; });
                hitDescriptions.Add((a, r) => { return $"{r} momentarily retreats, its serpentine body twisting with unnerving grace."; });
                hitDescriptions.Add((a, r) => { return $"{r} bares its fangs at {a}, venom seeping as it coils tighter."; });
                hitDescriptions.Add((a, r) => { return $"{r} snarls, its forked tongue flicking angrily as it readjusts."; });
                hitDescriptions.Add((a, r) => { return $"{r} thrashes violently, its barbed tail whipping through the air in fury."; });
                hitDescriptions.Add((a, r) => { return $"{r} momentarily falters, its demonic form bristling with restrained rage."; });
                hitDescriptions.Add((a, r) => { return $"{r} emits a guttural hiss, its tail striking the ground with a deafening crack."; });

                Character.hitReactions.Add(serpentDevil.Name, hitDescriptions);
            }

            return serpentDevil;
        }

        public static Character Ratrocity()
        {
            Character ratrocity = new Character(
                "Ratrocity",
                "A grotesque amalgamation of rat and machine, its patchy fur barely conceals the exposed gears and wires along its back. " +
                "Its glowing red eyes flicker with an unsettling intelligence, and its jagged, metal-tipped claws scrape ominously against the ground.",
                Kindred.None,
                CreatureType.Monstrosity, // Hybrid creature, not fully beast or machine
                10,  // strength - capable of powerful swipes
                12,  // dexterity - quick and nimble, despite its unnatural build
                8,  // constitution - resilient, with reinforced mechanical parts
                6,   // intelligence - driven by primal and programmed instincts
                9,  // wisdom - sharp survival instincts
                7,   // charisma - induces fear rather than charm
                "A Ratrocity collapsed into a heap of fur and scrap, its mechanical components sparking weakly."
            );

            BeastClaw beastClaw = new BeastClaw();
            beastClaw.DamageDie = new Die(1, 2);
            beastClaw.VendorValue = 0.6;

            ratrocity.AddEquippedItem(InventorySlot.MainHand, beastClaw);
            ratrocity.HP = 2;

            // Abilities
            ratrocity.AddFeat(FeatKey.MeleeAttack);
            ratrocity.AddFeat(FeatKey.DarkVision); // Perfect for lurking in shadows

            // Hit reactions
            if (!Character.hitReactions.ContainsKey(ratrocity.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>
                    {
                        (a, r) => $"{r} lets out a screech, sparks flying as its metal parts jolt from the impact.",
                        (a, r) => $"{r} staggers briefly, its claws scraping against the floor with an ear-piercing screech.",
                        (a, r) => $"{r} jerks its head, a mechanical whir accompanying its low growl of pain.",
                        (a, r) => $"{r} emits a guttural hiss, its gears grinding audibly as it absorbs the hit.",
                        (a, r) => $"{r} snarls, its red eyes flickering violently as sparks cascade from its exposed wiring.",
                        (a, r) => $"{r} skitters backward, leaving a trail of jagged scratches on the ground as it regains balance.",
                        (a, r) => $"{r} shudders, its patchy fur bristling as a burst of steam hisses from its mechanical joints.",
                        (a, r) => $"{r} lets out a metallic screech, the sound resonating like nails on rusted steel.",
                        (a, r) => $"{r} slams its tail into the ground with a loud clang, trying to steady its twitching frame.",
                        (a, r) => $"{r} emits a distorted growl, its jagged claws flexing as it shakes off the blow.",
                        (a, r) => $"{r} sparks wildly, its damaged servos emitting a high-pitched whine of protest.",
                        (a, r) => $"{r} snarls as one of its mechanical limbs jams briefly, forcing it to lurch awkwardly.",
                        (a, r) => $"{r} reels from the hit, its tail whipping around in a sharp, metallic arc.",
                        (a, r) => $"{r} growls deeply, the faint sound of grinding gears adding an unsettling undertone."
                    };
                Character.hitReactions.Add(ratrocity.Name, hitDescriptions);
            }

            return ratrocity;
        }

        public static Character Wolf()
        {
            return Wolf(10);
        }
        public static Character Wolf(int hp)
        {
            var rnd = Random.Shared;
            int i = rnd.Next(wolfNames.Count);

            Character wolf = new Character(
                $"{wolfNames[i]} Wolf",
                $"A {wolfNames[i]} wolf, carrying the quiet tension of a wild predator. " +
                "It moves with deliberate control, every motion precise, as if nothing is wasted.",
                Kindred.None,
                CreatureType.Beast,
                12,  // strength
                15,  // dexterity
                10,  // constitution
                6,   // intelligence
                10,  // wisdom
                7,   // charisma
                $"The {wolfNames[i]} wolf lies motionless, eyes dulled and unresponsive. " +
                "Even in death, the body holds the taut silence of a predator, jaws set in a final snarl."
            );

            // Set health
            wolf.HP = hp;

            // Claws
            wolf.DefaultHand = new BeastClaw();

            Head wolfSkull = new Head(
                   Names.WolfSkull,
                   "The skull of a wolf, a silent remnant of a fierce and untamed spirit.",
                   10);
            wolfSkull.SetIsBodyPart();
            ItemPopulation.TryAddLimitedItem(wolfSkull, wolf);

            //Wolf skull
            //Head wolfSkull = new Head(Names.WolfSkull, "The skull of a wolf, a silent remnant of a fierce and untamed spirit.", 10);
            //wolf.AddToInventory(wolfSkull);
            //wolfSkull.SetIsBodyPart();

            // Abilities
            wolf.AddFeat(FeatKey.MeleeAttack);
            wolf.AddFeat(FeatKey.DarkVision);

            // AI
            wolf.BehaviorKey = BehaviorKey.CaveWolf;

            // Hit reactions
            if (!Character.hitReactions.ContainsKey(wolf.Name))
            {
                List<Func<string, string, string>> hitDescriptions = new List<Func<string, string, string>>();
                hitDescriptions.Add((a, r) => $"{r} snaps violently, fangs bared, a low growl of rage rumbling from its throat.");
                hitDescriptions.Add((a, r) => $"{r} lunges forward, eyes glowing with primal fury, claws digging into the ground.");
                hitDescriptions.Add((a, r) => $"{r} snarls fiercely, fur bristling, lips pulled back in a wicked snarl at {a}.");
                hitDescriptions.Add((a, r) => $"{r} yelps sharply, teeth clashing together as it pivots, eyes fixed on {a} with predatory intent.");
                hitDescriptions.Add((a, r) => $"{r} stiffens violently, hackles raised, a sharp growl vibrating through its chest.");
                hitDescriptions.Add((a, r) => $"{r} lets out a guttural, almost feral howl, baring its teeth at {a}.");
                hitDescriptions.Add((a, r) => $"{r} snaps its jaws dangerously, saliva flecking the air as it recoils from {a}'s attack.");
                hitDescriptions.Add((a, r) => $"{r} shivers, fur standing on end, claws scraping the ground as it glares at {a}.");
                hitDescriptions.Add((a, r) => $"{r} lunges and twists sharply, teeth aiming for {a}'s hand, eyes burning with malice.");
                hitDescriptions.Add((a, r) => $"{r} growls low and threateningly, muscles tensing as it circles {a} warily.");
                hitDescriptions.Add((a, r) => $"{r} snaps and shuffles back, hackles bristling, eyes flashing with raw aggression.");
                hitDescriptions.Add((a, r) => $"{r} bares its teeth, letting out a harsh, chilling growl that echoes menace.");
                hitDescriptions.Add((a, r) => $"{r} twitches its ears sharply, a predator fully alert to every move {a} makes.");
                hitDescriptions.Add((a, r) => $"{r} crouches low for a split second, eyes narrowing, ready to strike viciously.");
                hitDescriptions.Add((a, r) => $"{r} snarls, a wet, rattling sound escaping as it braces for {a}'s next move.");
                hitDescriptions.Add((a, r) => $"{r} spins violently, fangs snapping inches from {a}, fur bristling in a frenzy.");
                hitDescriptions.Add((a, r) => $"{r} growls deeply, muscles rippling under matted fur, eyes locked in feral focus.");
                hitDescriptions.Add((a, r) => $"{r} flinches and snaps its jaws repeatedly, radiating barely contained aggression.");
                hitDescriptions.Add((a, r) => $"{r} twists its body violently, claws raking the ground, teeth aimed at {a} in a vicious snarl.");
                hitDescriptions.Add((a, r) => $"{r} shivers violently, a low, threatening growl vibrating from deep in its throat.");

                Character.hitReactions.Add(wolf.Name, hitDescriptions);
            }

            return wolf;
        }

    }
}
