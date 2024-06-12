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
            DialogueNode startNode = new DialogueNode("Ah, fresh from the Mother Machine's embrace! Welcome, welcome! I've been waiting for you. We have much to do, and little time!");
            DialogueNode whoAreYouNode = new DialogueNode("I am Ezekiel, the ever-watchful, the Mechanomancer extraordinaire! I've seen countless like you emerge, reborn from the machine's womb. Fascinating, isn't it?");
            DialogueNode motherMachineNode = new DialogueNode("Ah, the Mother Machine, a marvel of sinew and steel! She takes us, reshapes us. Some were sold, some were prisoners, but all are reborn here. Now, you're a Mecharion, part of this grand design!");
            DialogueNode attackNode = new DialogueNode("The facility is under siege! The Purists have come, a fanatical group that condemns our beautiful union of flesh and technology. They seek to destroy the Mother Machine and all her creations!");
            DialogueNode puristsNode = new DialogueNode("The Purists, zealots who believe our existence is an abomination. They think they can 'cleanse' us by tearing down everything we've become. Fools, the lot of them!");
            DialogueNode identityNode = new DialogueNode("Your story? Ha! Each of us has a unique tale, a reason we ended up here. But the Mother Machine doesn’t care for our pasts, only for what we become.");
            DialogueNode missionNode = new DialogueNode("I need your help, dear one! The Purists must be stopped. Find the core and shut it down if you can. The risks are immense, but so are the rewards!");
            DialogueNode goodbyeNode = new DialogueNode("Depart then, and may the spectral glow of the Machine God illuminate your path. We will meet again!");
            DialogueNode smallTalkNode1 = new DialogueNode("This place, it's more than just metal and circuits. It's alive, a sanctuary for those like us. The bond we share with the machine is profound.");
            DialogueNode smallTalkNode2 = new DialogueNode("I've been here for what feels like an eternity. The transformations, the experiments... they've changed me in ways I can't fully explain. But there's beauty in this evolution.");
            DialogueNode smallTalkNode3 = new DialogueNode("Ah, you want to know about me? I was a scholar once, fascinated by the merger of flesh and technology. Now, I am a part of that merger. Strange how destiny works, isn't it?");
            DialogueNode smallTalkNode4 = new DialogueNode("The Purists think they can save us by destroying us. They don't understand the harmony we've found here. Their ignorance is their greatest weapon and our greatest threat.");

            // Main dialogue choices
            startNode.AddChoice("Who are you?", whoAreYouNode);
            startNode.AddChoice("What happened to me?", motherMachineNode);
            startNode.AddChoice("Why do I feel like something's wrong?", attackNode);
            startNode.AddChoice("What's the mission?", missionNode);
            startNode.AddChoice("Tell me about this place.", smallTalkNode1);

            whoAreYouNode.AddChoice("What happened to me?", motherMachineNode);
            whoAreYouNode.AddChoice("Why do I feel like something's wrong?", attackNode);
            whoAreYouNode.AddChoice("What's the mission?", missionNode);
            whoAreYouNode.AddChoice("Tell me more about you.", smallTalkNode3);

            motherMachineNode.AddChoice("Was I sold or convicted?", identityNode);

            attackNode.AddChoice("Who are the Purists?", puristsNode);
            attackNode.AddChoice("What's the mission?", missionNode);

            identityNode.AddChoice("What dangers await in the facility?", attackNode);
            identityNode.AddChoice("I need help.", missionNode);

            // Small talk choices indicating alignment with Ezekiel's values
            DialogueNode smallTalkResponse1 = new DialogueNode("Ah, you understand! Yes, it's a magnificent evolution. We are part of something greater.");
            smallTalkNode1.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };                     
            DialogueNode smallTalkResponse2 = new DialogueNode("Exactly! Embrace the change. We are stronger together in this new form.");
            smallTalkResponse2.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };                    
            DialogueNode smallTalkResponse3 = new DialogueNode("Thank you! It's a destiny we all share and must protect.");
            smallTalkResponse3.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };
            DialogueNode smallTalkResponse4 = new DialogueNode("Indeed, their ignorance is dangerous. Our harmony is our strength.");
            smallTalkResponse4.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };            
            DialogueNode smallTalkResponse5 = new DialogueNode("You grasp the essence of our transformation. It's a dance of metal and flesh.");
            smallTalkResponse5.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };           
            DialogueNode smallTalkResponse6 = new DialogueNode("Precisely, our evolution is a testament to the beauty of progress.");
            smallTalkResponse6.OnAfterEvent = (dm) => { dm.ImproveRelationship(); };           
            DialogueNode smallTalkResponseNegative1 = new DialogueNode("Oh, I see. Not everyone can appreciate the magnificence of our transformation.");
            smallTalkResponseNegative1.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };           
            DialogueNode smallTalkResponseNegative2 = new DialogueNode("It's a shame you don't see the beauty in what we've become.");
            smallTalkResponseNegative2.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };           
            DialogueNode smallTalkResponseNegative3 = new DialogueNode("Pity, I thought you'd understand the significance of our evolution.");
            smallTalkResponseNegative3.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };
            DialogueNode smallTalkResponseNegative4 = new DialogueNode("How disappointing. The harmony we've achieved is beyond mere understanding.");
            smallTalkResponseNegative4.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };         
            DialogueNode smallTalkResponseNegative5 = new DialogueNode("Skepticism is natural, but it hinders true comprehension.");
            smallTalkResponseNegative5.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };           
            DialogueNode smallTalkResponseNegative6 = new DialogueNode("Such a view is limiting, but I won't force you to see beyond it.");
            smallTalkResponseNegative6.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); };

            smallTalkNode1.AddChoice("I see the beauty in our transformation. The bond with the machine is indeed profound.", smallTalkResponse1); // Relationship Improves
            smallTalkNode1.AddChoice("This place feels cold and uninviting, more like a prison.", smallTalkResponseNegative1); // Relationship Decreases

            smallTalkNode2.AddChoice("There's beauty in our evolution. Our changes make us stronger.", smallTalkResponse2); // Relationship Improves
            smallTalkNode2.AddChoice("These changes feel unnatural. We're losing ourselves.", smallTalkResponseNegative2); // Relationship Decreases

            smallTalkNode3.AddChoice("Your journey is inspiring. The merger of flesh and technology is a fascinating destiny.", smallTalkResponse3); // Relationship Improves
            smallTalkNode3.AddChoice("You sacrificed your humanity for this? It doesn't seem worth it.", smallTalkResponseNegative3); // Relationship Decreases

            smallTalkNode4.AddChoice("The Purists are misguided. They can't see the harmony we've achieved.", smallTalkResponse4); // Relationship Improves
            smallTalkNode4.AddChoice("Maybe the Purists are right. This doesn't feel like harmony.", smallTalkResponseNegative4); // Relationship Decreases

            // Continuation after small talk
            smallTalkResponse1.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponse2.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponse3.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponse4.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponse5.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponse6.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative1.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative2.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative3.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative4.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative5.AddChoice("Goodbye.", goodbyeNode);
            smallTalkResponseNegative6.AddChoice("Goodbye.", goodbyeNode);

            puristsNode.AddChoice("What's the mission?", missionNode);

            // Only allow the player to say goodbye after learning about the mission
            missionNode.AddChoice("Goodbye.", goodbyeNode);

            // Assign dialogue to Ezekiel the Mechanomancer
            ezekielTheMechanomancer.CreateDialogueManager(startNode);
        }
    }
}
