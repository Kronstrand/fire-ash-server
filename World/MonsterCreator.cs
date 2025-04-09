using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World.BioMechWorld;
using fire_ash_server.World.BioMechWorld.Temple;

namespace fire_ash_server.World
{
    internal static class MonsterCreator
    {
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
            Character skeleton = new Character(
                "Skeleton",
                "A humanoid figure made entirely of bones, moving with an unnatural clatter. " +
                "Its hollow eye sockets glow faintly with an eerie light, and it creaks with each motion, driven by a silent, unseen force.",
                Kindred.Undead,
                CreatureType.Humanoid, // Skeletal but retains a humanoid structure
                10,  // strength - average strength for an undead animated by dark forces
                8,   // dexterity - slightly awkward, its movements stiff and jerky
                12,  // constitution - sturdy in death, capable of withstanding minor damage
                5,   // intelligence - limited intelligence, functioning on basic instincts
                7,   // wisdom - not very aware, simply following basic commands
                5,   // charisma - devoid of any personal charm or humanity
                "The skeleton crumbles into a pile of brittle bones as the animating force leaves it. Its empty eye sockets stare into nothing, lifeless once again."
            );

            skeleton.AddEquippedItem(InventorySlot.MainHand, WeaponList.RustedSword());
            skeleton.AddEquippedItem(InventorySlot.OffHand, ArmorList.WornWoodenShield());

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
                Kindred.Serpentine,
                CreatureType.Beast, // Reptilian type due to its serpentine form and cold-blooded nature
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
                Kindred.Demon,
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

    }
}
