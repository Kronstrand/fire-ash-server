using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Dialogue;

namespace fire_ash_server.World
{
    internal class CyberworldCreater
    {
        public CyberworldCreater(WorldSoul worldSoul)
        {


            worldSoul.Relationships.Add(
                new Relationship(
                    worldSoul.GetFaction(FactionKey.Corporates),
                    worldSoul.GetFaction(FactionKey.Resistance),
                    -20));

            worldSoul.Relationships.Add(
                new Relationship(
                    worldSoul.GetFaction(FactionKey.Players),
                    worldSoul.GetFaction(FactionKey.Resistance),
                    1));

            Room neonAlley = new Room(
                Description(RoomKey.NeonAlley),
                "Neon Alley",
                "The alley glows with the flickering light of neon signs, casting vibrant hues on the wet pavement. " +
                "Graffiti adorns the walls, a chaotic blend of rebellion and art, while the distant hum of machinery " +
                "echoes through the narrow passage. The air is thick with the smell of oil and metal, " +
                "intertwined with the occasional waft of street food from nearby vendors. " +
                "This place is a haven for those who move in the shadows, a crossroads of the hidden and the hunted.");
            neonAlley.AddItem(new Dagger(
                "Holographic Blade",
                "A sleek, high-tech blade that shimmers with a holographic edge, designed for both precision and style. " +
                "Its handle is wrapped in synthetic leather, providing a comfortable grip amidst the chaos of the city.")
                .Hide(5));

            Room abandonedArcade = new Room(
                Description(RoomKey.AbandonedArcade),
                "Abandoned Arcade",
                "Once a vibrant center of entertainment, the arcade now stands silent and forgotten. " +
                "The machines, covered in dust and cobwebs, still glow faintly with the remnants of their last games. " +
                "Broken screens and shattered glass litter the floor, while old posters cling to the walls, " +
                "faded reminders of a time when this place was alive with laughter and competition. " +
                "The air is stale, filled with the lingering scent of old electronics and the faint buzz of residual energy.");
            Item arcadeConsole = new Item(
                "Retro Arcade Console",
                "An old console, its once vibrant colors now dulled by time. The screen flickers with static, " +
                "occasionally displaying snippets of long-forgotten games. Its joystick and buttons, though worn, " +
                "still hold a sense of nostalgia, hinting at the countless hours spent in front of it.");
            arcadeConsole.MakeUnpickupable();

            abandonedArcade.AddItem(arcadeConsole);

                Item dataCrystal = new Item(
                "Data Crystal",
                "A small, transparent crystal that glows with an inner light. It pulses softly, " +
                "containing vast amounts of encrypted data. To the untrained eye, it seems like a simple trinket, " +
                "but those in the know would recognize its value and the secrets it holds."
                );
            dataCrystal.Hide(3);
            dataCrystal.AddMove(new SkillCheck(
                    null,
                    "Hack into the Data Crystal to extract its secrets.",
                    new SkillNumber(Skill.Hacking, 10),
                    true,
                    (Soul s) => {
                        return
                        "You successfully decrypt the data, revealing hidden schematics for a powerful piece of cyberware. " +
                        "These plans could give you a significant edge in the technological arms race of the city, " +
                        "or fetch a high price on the black market.";
                    },
                    (Soul s) => {
                        return
                        "Despite your efforts, the encryption holds firm. The secrets within the crystal remain locked away, " +
                        "leaving you with nothing but frustration and curiosity.";
                    }));
            arcadeConsole.AddItem(dataCrystal);

            neonAlley.AddExit(new Exit(
                "A narrow, shadowy passage leads from the neon-lit alley to the desolate remnants of an abandoned arcade.",
                abandonedArcade));

            abandonedArcade.AddExit(new Exit(
                "A dark, winding path emerges from the arcade, leading back to the vibrant yet foreboding neon alley.",
                neonAlley));

            Room neonPlaza = new Room(
                Description(RoomKey.NeonPlaza),
                "Neon Plaza",
                "The plaza is a bustling hub of activity, bathed in the glow of countless neon signs advertising everything from " +
                "cybernetic enhancements to exotic foods. Street vendors shout their wares, while the crowd moves like a living entity, " +
                "pulsing with the energy of the city. The scent of street food mingles with the sharp tang of ozone from nearby " +
                "power generators. In the center, a large holographic display broadcasts the latest news and advertisements, " +
                "casting a colorful light over the scene.");

            neonPlaza.AddExit(new Exit(
                "A bustling path lined with vendors and neon signs leads from the heart of the plaza to the shadowy neon alley.",
                neonAlley));
            neonAlley.AddExit(new Exit(
                "Following the vibrant lights and the sound of bustling activity, a path leads towards the Neon Plaza.",
                neonPlaza));

            Character Bytewhisper = new Character(
                "Bytewhisper",
                "Bytewhisper is a mysterious figure who roams the less-traveled paths of the Neon Plaza. " +
                "Draped in a cloak of shifting pixels, their appearance is as elusive as their motives. " +
                "Their eyes, hidden behind a visor of constantly updating data, scan their surroundings with an almost predatory focus. " +
                "Bytewhisper is known for their expertise in digital warfare, able to manipulate data and electronics with a mere thought. " +
                "They move with the grace of a shadow, their presence often unnoticed until it’s too late. " +
                "Despite their intimidating skill set, Bytewhisper is rumored to assist those who find themselves lost in the digital labyrinth of the city.",
                Race.Cyborg, // race
                8,  // strength - enhanced by cybernetic augmentations
                16, // dexterity - incredibly agile, especially in the digital realm
                10, // constitution - moderately tough, with cybernetic enhancements for resilience
                18, // intelligence - a genius in the realm of hacking and digital manipulation
                12, // wisdom - knowledgeable and perceptive, especially regarding technology
                14, // charisma - enigmatic and compelling, with a mysterious allure
                "In the neon-lit corners of the city, Bytewhisper's body lies still. " +
                "The cloak of pixels flickers weakly, and the visor that once brimmed with data is now dark and lifeless. " +
                "Their hands, once a blur of motion and command, now rest motionless, as if in a final, failed attempt to reach for the digital ether. " +
                "Even in death, they exude an air of mystery, a silent guardian of the city's cyber secrets.");
            Bytewhisper.HP = 20;
            Bytewhisper.AddFeat(FeatKey.MeleeAttack);
            Bytewhisper.GoToRoom(neonPlaza);
            Bytewhisper.Faction = worldSoul.GetFaction(FactionKey.Resistance);

            // Create dialogue nodes
            DialogueNode startNode = new DialogueNode("Greetings, traveler. What brings you to the Neon Plaza?");
            DialogueNode helpNode = new DialogueNode("I can assist you in navigating the digital labyrinth. What do you need help with?");
            DialogueNode infoNode = new DialogueNode("The city is a maze of secrets and dangers. Stay vigilant.");
            DialogueNode goodbyeNode = new DialogueNode("Farewell. Stay safe in the shadows.");

            // Add choices to nodes
            startNode.AddChoice("I need help.", (dm) => { return helpNode; });
            startNode.AddChoice("Tell me about the city.", infoNode);
            startNode.AddChoice("Goodbye.", goodbyeNode);

            helpNode.AddChoice("Tell me more about digital labyrinth.", infoNode);
            helpNode.AddChoice("Goodbye.", goodbyeNode);

            infoNode.AddChoice("Thank you. Goodbye.", goodbyeNode);

            // Assign dialogue to Bytewhisper
            Bytewhisper.CreateDialogueManager(startNode);

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
