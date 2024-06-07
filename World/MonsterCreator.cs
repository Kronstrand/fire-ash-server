using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World
{
    internal class MonsterCreator
    {

        public static Character CreateShadecreeper()
        {
            Character Shadecreeper = new Character(
                "Shadecreeper",
                "Shadecreeper is a small, cunning humanoid that dwells in the dimmest parts of the Whispering Forest. " +
                "Its skin is a murky grey, allowing it to meld into the shadows with ease. With sharp, " +
                "alert eyes that gleam in the darkness, it watches quietly, blending almost seamlessly with the underbrush. " +
                "The Shadecreeper is notoriously mischievous, known for its stealthy movements and a knack for avoiding detection. " +
                "While not inherently evil, it possesses a playful yet eerie demeanor, " +
                "often leading travelers astray with misleading whispers and echoing footsteps. " +
                "Its slender fingers are adept at picking locks and setting traps, " +
                "making it a tricky foe or an invaluable, albeit unpredictable, ally.",
                Race.Human, // race
                8,  // strength - not very strong, relies on cunning and agility
                15, // dexterity - extremely agile, quick in its shadowy environment
                10, // constitution - resilient enough to survive in the forest, but not particularly tough
                12, // intelligence - clever and resourceful, skilled in forest survival
                12, // wisdom - highly perceptive, in tune with the forest's hidden paths
                6,  // charisma - its eerie, mysterious nature can be unsettling, yet intriguing
                "Still as a fallen leaf, the Shadecreeper lies crumpled amongst the shadowed roots. " +
                "The murky grey of its skin, once a perfect camouflage, now seems dull and lifeless.  " +
                "Its once-gleaming eyes are vacant, sightless orbs reflecting no light.  " +
                "The playful malice it once possessed has vanished, replaced by an unsettling stillness.  " +
                "Even in death, a faint sense of mischief lingers - a single, slender finger twitches ever so slightly, " +
                "a chilling reminder of the tricks it played in life.");
            Shadecreeper.HP = 6;
            Shadecreeper.AddFeat(FeatKey.Stealth);
            Shadecreeper.AddFeat(FeatKey.MeleeAttack);
            Shadecreeper.Inventory.AddItem(new Item("Emerald", "A Green Emerald."));

            if (!Character.hitReactions.ContainsKey(Shadecreeper.Name))
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
                Character.hitReactions.Add(Shadecreeper.Name, hitDescriptions);
            }

            return Shadecreeper;
        }
    }
}
