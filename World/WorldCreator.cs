using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Moves;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World
{
    internal class WorldCreator
    {
        public WorldCreator(WorldSoul worldSoul) 
        {
            foreach(Enum factionKey in Enum.GetValues(typeof(FactionKey)))
            {
                worldSoul.Factions.Add(new Faction(Description(factionKey)));
            }

            worldSoul.Relationships.Add(
                new Relationship(
                    worldSoul.GetFaction(FactionKey.LightShades), 
                    worldSoul.GetFaction(FactionKey.Wilders), 
                    -10));

            new Room(RoomKey.Void,"Void", "This is the void");

            Room wolfCave = new Room(
                RoomKey.WolfCave,
                "Wolf Cave",
                "In the shadowy confines of the cave, the distinct, " +
                "earthy scent of wolves permeates the cool, stagnant air, " +
                "hinting at their recent presence. " +
                "The uneven ground beneath is littered with tufts of fur, paw prints, " +
                "and gnawed bones, remnants of the cave's recent inhabitants. " +
                "The walls, rough and damp, are adorned with ancient mineral formations that glisten faintly, " +
                "their intricate patterns providing the only relief to the enveloping darkness. " +
                "The profound silence is almost tangible, broken only occasionally by the distant, " +
                "muffled sound of dripping water, reinforcing the cave's desolate, forgotten atmosphere.");

            Room abandonedShrine = new Room(
                RoomKey.AbandonedWolfShrine,
                "Abandoned Shrine",
                "Veiled in a thick blanket of moss and ivy, the crumbling stone walls whisper tales of forgotten deities. " +
                "Statues, eroded by time, stand as silent guardians over the long-deserted sanctum, their features worn away, " +
                "leaving their once divine identities to speculation. " +
                "The air is heavy with the scent of damp earth and decayed wood, mingling with a faint, lingering essence of incense, " +
                "a ghostly remnant of ancient ceremonies. " +
                "Fallen leaves carpet the ground, concealing the fractured remnants of sacred offerings and ritualistic relics. " +
                "In the center, an altar, scarred by time and neglect, holds a mysterious, ageless power, " +
                "its surface scattered with offerings from those who dare to remember the old ways.");
            Item wolfAlter = (Item) new Item(
                "Wolf Alter",
                "A palpable silence envelops the alter, a respectful hush that seems to ward off the uninitiated, " +
                "making the air feel thick with unspoken secrets and lost histories.").
                MakeUnpickupable();
            
            abandonedShrine.AddItem(wolfAlter);

            wolfAlter.AddItem((Item)new Item(
                "Celestial Wolf Idol",
                "A profound idol depicting a wolf god, small enough to rest within the confines of your grasp, " +
                "sculpted from an otherworldly stone that pulsates with an eerie, unworldly warmth. " +
                "Its visage is a blend of terror and awe, with eyes that seem to gaze into the void, " +
                "mouth agape as if to devour the stars. The intricate markings suggest a connection to the cosmos, " +
                "imbued with an ancient power that whispers of forgotten rites and the inevitable return of its eldritch divinity. " +
                "Its presence is unsettling, as it exudes a sense of ancient wisdom and an impending sense of cosmic dread.")
                .Hide(3)
                .AddMove(new SkillCheck(
                    "r",
                    "Recall lore about the Celestial Wolf Idol.",
                    new SkillNumber(Skill.Religion, 8), 
                    () => { return
                        "This idol depicts Lunaris, a celestial entity deeply intertwined with the moon's mystique. " +
                        "Resonating with the lunar spirit, its essence mirrors the cosmic ebb and flow. Lunaris, " +
                        "known as the Void Howler, embodies the moon's silent command over the night sky, " +
                        "with an ethereal howl that echoes the ancient rhythms of time and space. " +
                        "Followers of Lunaris were once famed for the Night of Silver Shadows, " +
                        "a night when the moon's light turned silver and their synchronized howling rituals reportedly shifted the tides of reality, " +
                        "unveiling hidden truths and altering the course of celestial events. The idol, " +
                        "crafted from luminescent stone, glows softly, a testament to the deep connection Lunaris shares with the nocturnal cosmos, " +
                        "promising enlightenment to those who decipher its celestial whispers."; },
                    () => { return 
                        "Despite your efforts, the mysteries of the idol remain elusive. Its silent, " +
                        "inscrutable gaze offers no insights, " +
                        "leaving you with more questions than answers."; }
                    )));
            wolfAlter.AddItem(new Item(
                "Runed Compass",
                "An ornate compass, its rim engraved with symbols that resonate with magical energy. " +
                "Rather than pointing north, the needle spins slowly, only coming to a halt when unseen forces align, " +
                "suggesting it guides not through geography, but fate itself.")
                .Hide(11));

            wolfCave.AddExit(new Exit(
                "Beyond the wolf-infested shadows, " +
                "a narrow, winding path leads to the remnants of an abandoned shrine.", 
                abandonedShrine));

            abandonedShrine.AddExit(new Exit(
                "A barely discernible path, shrouded in an eerie silence, retreats from the shrine, " +
                "snaking through the dense underbrush and leading back to the ominous, " +
                "gloom-laden depths of the Wolf Cave.",
                wolfCave));

            Room whisperingForest = new Room(
                RoomKey.WhisperingForest,
                "Whispering Forest",
                "The forest stands dense and ancient, a labyrinth of towering trees whose leaves whisper secrets of old. " +
                "The light here is a dappled symphony, casting shadows that seem to dance with an ethereal quality. " +
                "Every step is cushioned by a thick layer of moss, muting your passage through this venerable grove. " +
                "Occasionally, the silver flash of a stream breaks the monotony of greens and browns, its water clear and cold, " +
                "singing melodies of the untamed wild. It's easy to lose oneself in the beauty, yet each whispered breeze, " +
                "each rustle in the underbrush, hints at watching eyes and lurking dangers. Ancient stones, covered in runes " +
                "that flicker with an otherworldly glow, are scattered throughout, suggesting this place is more than a simple forest - " +
                "it's a sanctuary of ancient magics, where the line between the mundane and the mystical blurs.");
  
            whisperingForest.AddExit(new Exit(
                "A moss-covered path, barely visible beneath the cloak of nature, leads from the heart of the forest to the shadows of a secluded cave.",
                wolfCave));
            wolfCave.AddExit(new Exit(
                "Following the scent of fresh air and the distant call of birds, a path reveals itself, winding its way towards the Whispering Forest.",
                whisperingForest));

            Item stoneknife = new Dagger(
                "Crude Stone Knife",
                "A crudely fashioned stone knife, " +
                "its jagged blade chipped from flint, " +
                "bound to a rough wooden handle with aged sinew, " +
                "exuding a primal, utilitarian essence.")
                .Hide(5);
            whisperingForest.AddItem(stoneknife);

            Character shadecreeper = MonsterCreator.CreateShadecreeper();
            shadecreeper.GoToRoom(whisperingForest);
            Character shadecreeper2 = MonsterCreator.CreateShadecreeper();
            shadecreeper2.GoToRoom(whisperingForest);

            shadecreeper.MoveToGroup(shadecreeper2);

            Character dawnwhisper = new Character(
                "Dawnwhisper",
                "Dawnwhisper is a lithe, ethereal being found in the brighter parts of the Whispering Forest, " +
                "where sunlight filters through the canopy. Its skin shimmers with a soft, golden hue, reminiscent of the morning sun. " +
                "Its eyes, large and luminous, radiate a warm, inviting glow, contrasting sharply with the shadowy Shadecreeper. " +
                "Dawnwhisper is known for its benevolent nature, guiding lost travelers back to safety with gentle, melodious whispers. " +
                "This creature moves with grace and fluidity, its presence often heralding safety and peace. " +
                "Despite its gentle demeanor, it possesses a formidable ability to harness light, using it to heal allies or dazzle foes. " +
                "Its fingers, though delicate, are capable of weaving complex enchantments, protecting the forest from those who mean harm.",
                Race.Elf, // race
                7,  // strength - not strong, relies on magical abilities and wisdom
                14, // dexterity - graceful and quick, especially in well-lit areas
                9,  // constitution - not very tough, but has mystical protections
                14, // intelligence - highly intelligent, knowledgeable about forest lore and light magic
                15, // wisdom - deeply insightful, with a strong connection to the natural and spiritual realm
                13, // charisma - its warm, radiant nature is reassuring and compelling
                "Under the sunlit patches of the forest, the Dawnwhisper rests peacefully in death. " +
                "Its shimmering golden skin, once a beacon of warmth and safety, now fades to a pale, serene glow. " +
                "The light in its eyes has dimmed, yet a softness remains, as if it still watches over the woods it loved. " +
                "Even in stillness, the Dawnwhisper exudes a sense of tranquility, its hands folded as if in final prayer, " +
                "ensuring the forest’s light never truly fades.");
            dawnwhisper.HP = 15;
            dawnwhisper.AddFeat(FeatKey.DualWield);
            dawnwhisper.GoToRoom(whisperingForest);
            dawnwhisper.Faction = worldSoul.GetFaction(FactionKey.LightShades);

            /*Character monster = new Character(
                "Name",
                "Description",
                Race.Human, //race
                10, //strength
                10, //Dexterity
                10, //Constitution
                10, //Intelligence
                10, //Wisom
                10, //Charisma
                "Death Description");*/
        }
    }
}
