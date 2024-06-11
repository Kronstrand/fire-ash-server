using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using fire_ash_server.Moves;

namespace fire_ash_server.World
{
    internal class BioMechWorld
    {
        public BioMechWorld(WorldSoul worldSoul)
        {
            // Creation Chamber Room
            Room creationChamber = new Room(
                RoomKey.CreationChamber,
                "Creation Chamber",
                "The Creation Chamber is a sprawling, biomechanical womb, pulsating with an eerie, otherworldly energy. " +
                "The walls are a grotesque fusion of metal and flesh, with organic tubes and cables snaking across the surface, " +
                "pumping luminous, viscous fluids that glow with an unsettling, greenish hue. " +
                "In the center of the chamber stands the Mother-Machine, a towering, nightmarish construct of steel and sinew. " +
                "Its many arms, a blend of mechanical precision and organic fluidity, constantly twitch and move, " +
                "tending to the various incubation pods that line the room. " +
                "These pods, filled with a thick, amniotic fluid, house the nascent forms of new humans, " +
                "their silhouettes barely visible through the murky liquid. " +
                "The air is thick with the smell of antiseptic and the underlying scent of decay, " +
                "a constant reminder of the unnatural processes at work. " +
                "Soft, rhythmic humming fills the room, punctuated by the occasional hiss of escaping steam and the mechanical whirring of the Mother-Machine. " +
                "This chamber is both a birthplace and a factory, a grotesque testament to the blending of technology and biology."
            );

            Item motherMachine = new Item(
                "Mother-Machine",
                "The Mother-Machine is an awe-inspiring yet terrifying fusion of advanced technology and organic matter. " +
                "Standing at over ten feet tall, its main body is a mass of cables, gears, and pulsating flesh. " +
                "Multiple arms, some ending in delicate surgical instruments and others in multi-jointed appendages, " +
                "extend from its torso, constantly adjusting and manipulating the incubation pods that surround it. " +
                "Its 'face' is a blank, metallic surface, with clusters of sensors and organic eyes that seem to peer into the very soul of anyone who dares to look. " +
                "The machine exudes a palpable sense of intelligence and malevolence, as if it possesses a will of its own. " +
                "The creation of new humans is both an act of precision and brutality, with the Mother-Machine ensuring each creation is perfected to its unsettling standards. " +
                "Its constant, rhythmic movements and the occasional sound of bone and metal scraping together create a disturbing symphony that fills the chamber."
            );
            motherMachine.MakeUnpickupable();

            creationChamber.AddItem(motherMachine);

            // Main Corridor Room
            Room mainCorridor = new Room(
                roomKey: RoomKey.MainCorridor,
                "Main Corridor",
                "The Main Corridor is a long, dimly lit passage that stretches into darkness. " +
                "The walls are covered with a lattice of exposed pipes and cables, some of which ooze a viscous, black fluid. " +
                "Flickering neon lights struggle to illuminate the corridor, casting eerie shadows that seem to move on their own. " +
                "The air is thick with the smell of oil and something acrid, almost metallic. " +
                "A series of doors line the corridor, each one marked with cryptic symbols and warning signs. " +
                "The floor is grated metal, and the sound of dripping water echoes ominously. " +
                "It feels as if the corridor itself is alive, watching and waiting for something."
            );

            // Adding exit to the Main Corridor
            creationChamber.AddExit(new Exit(
                "A narrow, dimly lit corridor leads out of the Creation Chamber, " +
                "its walls pulsating with the same eerie energy as the chamber itself.",
                mainCorridor));

            mainCorridor.AddExit(new Exit(
                "The pulsating glow from the Creation Chamber spills into the corridor, " +
                "leading you back to the source of new life.",
                mainCorridor));

            // Adding a unique character in the Main Corridor
            Character ezekielTheMechanomancer = new Character(
                "Ezekiel",
                "Ezekiel is a twisted fusion of man and machine, a high priest of the cult of the Machine God. " +
                "Standing at an imposing seven feet tall, his body is a grotesque amalgamation of cybernetic implants and organic tissue. " +
                "His left arm is a massive, multi-functional appendage equipped with surgical tools, weapons, and strange, arcane devices. " +
                "Ezekiel's face is a patchwork of metal plates and organic skin, with only a single eye gazing into meatspace. " +
                "His other eyes? Only God and Ezekiel himself know in which dimensions these are prying." +
                "He wears a long, tattered robe adorned with glowing runes and circuitry, and his deep, gravelly voice echoes with a mechanical timbre.",
            Race.Mecharion,
            14, //strenght
            10, //dexterity
            11, //constitution
            15, //intelligence
            13, //wisdom
            9,  //charisma
                "Ezekiel's imposing seven-foot frame lies still, " +
                "his massive, " +
                "multi-functional left arm, equipped with surgical tools, weapons, and arcane devices, " +
                "rests lifelessly. His face, a patchwork of metal plates and organic skin, is frozen in death. " +
                "His long, tattered robe, " +
                "adorned with dimmed runes and circuitry, drapes over his form. Once revered and feared, " +
                "Ezekiel now lies silent, a testament to his ruthless pursuit of knowledge and power."
            );
            ezekielTheMechanomancer.UniqueName = true;
            ezekielTheMechanomancer.HP = 30;
            ezekielTheMechanomancer.AddFeat(FeatKey.MeleeAttack);
            ezekielTheMechanomancer.GoToRoom(mainCorridor);
            ezekielTheMechanomancer.Faction = worldSoul.GetFaction(FactionKey.Technomancers);

            ezekielTheMechanomancer.AddMove(new SkillCheck(
                    null,
                    "Recall lore about Ezekiel.",
                    new SkillNumber(Skill.Religion, 8),
                    (Soul s) => {
                        return
                        "Ezekiel the Mechanomancer, a high priest of the cult of the Machine God, " +
                        "stands tall and imposing. He is both revered and feared by his followers. " +
                        "Known for his ruthless pursuit of knowledge and power, " +
                        "Ezekiel seeks to merge the divine and the mechanical, " +
                        "creating a new form of life that transcends both. His cult, " +
                        "known as the Cult of Technomancers, is dedicated to worshiping the Machine God, " +
                        "believing in the ultimate union of flesh and technology. " +
                        "Ezekiel's position as a leader is cemented by his formidable presence " +
                        "and unwavering dedication to this singular vision.";
                    },
                    (Soul s) => {
                        if (s.Character.Race == Race.Mecharion)
                        {
                            return "Your newborn memory is failing you, " +
                            "the details of this strange and ominous figure slipping through the cracks " +
                            "of your freshly hatched mind. The knowledge remains just out of reach.";
                        }
                        return "Your mind draws a blank, the details of this particular religion eluding your memory. " +
                        "You struggle to recall any relevant information about him.";
                    }
                    ));

            // Create dialogue nodes for Ezekiel the Mechanomancer
            DialogueNode startNode = new DialogueNode("Ah, a fresh cog in the grand machine! What conundrum twists your gears?");
            DialogueNode helpNode = new DialogueNode("Assistance from me, a mere whisperer in the void? What vexation plagues you?");
            DialogueNode infoNode = new DialogueNode("This facility is a labyrinthine enigma, filled with whispers of the past and shadows of the future. Step lightly.");
            DialogueNode goodbyeNode = new DialogueNode("Depart then, and may the spectral glow of the Machine God illuminate your path.");
            DialogueNode motherMachineNode = new DialogueNode("The Mother-Machine, she who melds sinew and steel, the primordial artificer of our kind. Behold her majesty!");
            DialogueNode dangersNode = new DialogueNode("Beware the lurking phantoms: rogue AIs, eldritch specters, and the ceaseless machinations of power-hungry cultists. Trust not the shadows.");
            DialogueNode unknownForceNode = new DialogueNode("An unknown force besieges us, an echo from the void. Can you unravel this cryptic menace?");
            DialogueNode visionNode = new DialogueNode("Through my vision device, I have glimpsed them. Shadows incarnate, flitting just beyond the veil. We must act with alacrity and stealth.");
            DialogueNode identityNode = new DialogueNode("You, reborn in the crucible of the Mother-Machine. Your past, a mere prologue to this mechanized rebirth. Focus on your newfound purpose.");

            startNode.AddChoice("Tell me about the Mother-Machine.", motherMachineNode);
            startNode.AddChoice("What dangers await in the facility?", dangersNode);
            startNode.AddChoice("I need help.", helpNode);
            startNode.AddChoice("Who am I? Why am I here?", identityNode);
            startNode.AddChoice("Goodbye.", goodbyeNode);

            helpNode.AddChoice("The facility is under attack by an unknown force. I need to find out who it is.", unknownForceNode);
            helpNode.AddChoice("Tell me more about the facility.", infoNode);
            helpNode.AddChoice("Goodbye.", goodbyeNode);

            unknownForceNode.AddChoice("What do you know about the attackers?", visionNode);
            unknownForceNode.AddChoice("Goodbye.", goodbyeNode);

            visionNode.AddChoice("Thank you. Goodbye.", goodbyeNode);

            infoNode.AddChoice("Thank you. Goodbye.", goodbyeNode);

            motherMachineNode.AddChoice("What dangers await in the facility?", dangersNode);
            motherMachineNode.AddChoice("Goodbye.", goodbyeNode);

            dangersNode.AddChoice("Tell me about the Mother-Machine.", motherMachineNode);
            dangersNode.AddChoice("Goodbye.", goodbyeNode);

            identityNode.AddChoice("Tell me about the Mother-Machine.", motherMachineNode);
            identityNode.AddChoice("What dangers await in the facility?", dangersNode);
            identityNode.AddChoice("I need help.", helpNode);
            identityNode.AddChoice("Goodbye.", goodbyeNode);

            // Assign dialogue to Ezekiel the Mechanomancer
            ezekielTheMechanomancer.CreateDialogueManager(startNode);
        }
    }
}
