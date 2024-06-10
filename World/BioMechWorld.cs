using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

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
                "This chamber is both a birthplace and a factory, a grotesque testament to the blending of technology and biology in this dystopian world."
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
                "Ezekiel the Mechanomancer",
                "Ezekiel the Mechanomancer is a twisted fusion of man and machine, a high priest of the cult of the Machine God. " +
                "Standing at an imposing seven feet tall, his body is a grotesque amalgamation of cybernetic implants and organic tissue. " +
                "His left arm is a massive, multi-functional appendage equipped with surgical tools, weapons, and strange, arcane devices. " +
                "Ezekiel's face is a patchwork of metal plates and organic skin, with one eye replaced by a multi-lensed optic that glows with an ominous red light. " +
                "His other eye is a cold, piercing blue, hinting at the remnants of his humanity. " +
                "He wears a long, tattered robe adorned with glowing runes and circuitry, and his deep, gravelly voice echoes with a mechanical timbre. " +
                "Ezekiel is both revered and feared by his followers, known for his ruthless pursuit of knowledge and power. " +
                "He seeks to merge the divine and the mechanical, creating a new form of life that transcends both.",
            Race.Mecharion,
            14, //strenght
            10, //dexterity
            11, //constitution
            15, //intelligence
            13, //wisdom
            9,  //charisma
                "Ezekiel's imposing seven-foot frame lies still, " +
                "his body a grotesque amalgamation of cybernetic implants and organic tissue. His massive, " +
                "multi-functional left arm, equipped with surgical tools, weapons, and arcane devices, " +
                "rests lifelessly. His face, a patchwork of metal plates and organic skin, is frozen in death, " +
                "one eye a dimly glowing red optic, the other a cold, lifeless blue. His long, tattered robe, " +
                "adorned with dimmed runes and circuitry, drapes over his form. Once revered and feared, " +
                "Ezekiel now lies silent, a testament to his ruthless pursuit of knowledge and power."
            );
            ezekielTheMechanomancer.UniqueName = true;
            ezekielTheMechanomancer.HP = 30;
            ezekielTheMechanomancer.AddFeat(FeatKey.MeleeAttack);
            ezekielTheMechanomancer.GoToRoom(mainCorridor);
            ezekielTheMechanomancer.Faction = worldSoul.GetFaction(FactionKey.Technomancers);

            // Create dialogue nodes for Ezekiel the Mechanomancer
            DialogueNode startNode = new DialogueNode("Welcome to the new dawn, creations of the Mother-Machine. What do you seek?");
            DialogueNode helpNode = new DialogueNode("I can guide you through the darkness. What do you need help with?");
            DialogueNode infoNode = new DialogueNode("The city is a labyrinth of neon and shadow, filled with ancient terrors and secrets. Stay vigilant.");
            DialogueNode goodbyeNode = new DialogueNode("Farewell. May the light of the Machine God guide you.");
            DialogueNode motherMachineNode = new DialogueNode("The Mother-Machine is the divine womb, merging flesh and steel to create life. It is both our creator and protector.");
            DialogueNode dangersNode = new DialogueNode("The city is rife with dangers: rogue AI, eldritch entities lurking in the shadows, and cults vying for power. Trust no one.");

            // Add choices to nodes
            startNode.AddChoice("Tell me about the Mother-Machine.", motherMachineNode);
            startNode.AddChoice("What dangers await in the city?", dangersNode);
            startNode.AddChoice("I need help.", helpNode);
            startNode.AddChoice("Goodbye.", goodbyeNode);

            helpNode.AddChoice("Tell me more about the city.", infoNode);
            helpNode.AddChoice("Goodbye.", goodbyeNode);

            infoNode.AddChoice("Thank you. Goodbye.", goodbyeNode);

            motherMachineNode.AddChoice("What dangers await in the city?", dangersNode);
            motherMachineNode.AddChoice("Goodbye.", goodbyeNode);

            dangersNode.AddChoice("Tell me about the Mother-Machine.", motherMachineNode);
            dangersNode.AddChoice("Goodbye.", goodbyeNode);

            // Assign dialogue to Ezekiel the Mechanomancer
            ezekielTheMechanomancer.CreateDialogueManager(startNode);
        }
    }
}
