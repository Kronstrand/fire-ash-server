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
            DialogueNode startNode = new DialogueNode("Ah, fresh from the Mother Machine's embrace! Welcome, welcome! I've been waiting for you. We have much to do, and little time.");
            DialogueNode whoAreYouNode = new DialogueNode("I am Ezekiel, the ever-watchful, the Mechanomancer extraordinaire! I've seen countless like you emerge, reborn from the machine's womb. Fascinating, isn't it?");
            DialogueNode whatHappenedNode1 = new DialogueNode("Ah, you were reborn! The Mother Machine took you, reshaped you, and now you are one of us. You are a Mecharion, forged from the union of flesh and steel.");
            DialogueNode motherMachineNode = new DialogueNode("Ah, the Mother Machine, a marvel of sinew and steel! She takes us, reshapes us. Some were sold, some were prisoners, but all are reborn here. Now, you're a Mecharion, part of this grand design.");
            DialogueNode attackNode = new DialogueNode("The facility is under siege! The Purists have come, a fanatical group that condemns our beautiful union of flesh and technology. They seek to destroy the Mother Machine and all her creations.");
            DialogueNode puristsNode = new DialogueNode("The Purists, zealots who believe our existence is an abomination. They think they can 'cleanse' us by tearing down everything we've become. Fools, the lot of them.");
            DialogueNode identityNode = new DialogueNode("Your story? Ha! Each of us has a unique tale, a reason we ended up here. But the Mother Machine doesn’t care for our pasts, only for what we become.");
            DialogueNode missionNode = new DialogueNode("Ah, my precious, we face dire times. The Purists, with their cold, unyielding ideology, have breached our sanctum. They seek to destroy the Mother Machine, our ancient lifeline that maintains the balance between technology and nature. To them, it’s a blasphemy they must purge to create their vision of purity. Without it, our world would wither, and chaos would reign.\n\nLeading them is the Vicar of Purity—once my closest ally, now my most bitter adversary. His name was Elias, a brilliant mind twisted by fanaticism. He believes that only by destroying the Mother Machine can he cleanse the world. His mastery over mind and matter makes him a perilous foe.\n\nWe must defend what we hold dear. Find Elias, disrupt his plans, and show him we will not be undone.");
            DialogueNode eliasNode = new DialogueNode("Elias and I were once visionaries, united by our desire to blend technology and nature harmoniously. We spent countless nights debating, designing, and dreaming. He was brilliant, passionate, and relentless in his pursuit of knowledge.\n\nBut then, tragedy struck. As we were working on the Mother Machine, Elias's daughter, Lily, fell gravely ill. Desperate to save her, Elias submitted her to the Mother Machine before we had fully tested its capabilities. He believed it could cure her, melding her illness away through a perfect synthesis of organic and synthetic life.\n\nHowever, the result was horrific. The machine malfunctioned, and instead of healing her, it twisted her into a grotesque fusion of flesh and metal. Consumed by guilt and grief, Elias became convinced that the Mother Machine was an abomination, a monstrous creation that must be destroyed.\n\nOur bond shattered when he declared his intention to eradicate it. I opposed him, believing it was the cornerstone of our world's harmony. He saw my resistance as a betrayal, and I, his fanaticism as madness. He left, vowing to return and cleanse our world by any means necessary. Now, he leads the Purists, driven by his tragic past and a warped sense of purpose.");
            DialogueNode goodbyeNode = new DialogueNode("Depart then, and may the spectral glow of the Machine God illuminate your path. We will meet again.");
            DialogueNode smallTalkNode1 = new DialogueNode("This place, it's more than just metal and circuits. It's alive, a sanctuary for those like us. The bond we share with the machine is profound.");
            DialogueNode smallTalkNode2 = new DialogueNode("Ah, you want to know about me? I was a scholar once, fascinated by the merger of flesh and technology. Now, I am a part of that merger. Strange how destiny works, isn't it?");

            // Main dialogue choices
            startNode.AddChoice("Who are you?", whoAreYouNode);
            startNode.AddChoice("What happened to me?", whatHappenedNode1);
            startNode.AddChoice("Why do I feel like something's wrong?", attackNode);
            startNode.AddChoice("Tell me about this place.", smallTalkNode1);        

            whoAreYouNode.OnAfterEvent = (dm) => {
                startNode.AddChoice("Tell me more about you.", smallTalkNode2);
                dm.CurrentNode = startNode;
            };

            whatHappenedNode1.OnAfterEvent = (dm) => {
                startNode.AddChoice("The Mother Machine?", motherMachineNode);
                dm.CurrentNode = startNode;
            };

            motherMachineNode.OnAfterEvent = (dm) => { 
                startNode.AddChoice("Was I sold or convicted?", identityNode); 
                dm.CurrentNode = startNode; 
            };
            
            identityNode.OnAfterEvent = (dm) => { dm.CurrentNode = startNode; };

            attackNode.OnAfterEvent = (dm) => {
                startNode.AddChoice("Who are the Purists?", puristsNode);
                startNode.AddChoice("The Mother Machine?", motherMachineNode);
                dm.CurrentNode = startNode;
            };

            puristsNode.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("What will you do about The Purists?", missionNode);
                dm.CurrentNode = startNode;
            };

            // Integrate small talk naturally into main dialogue choices
            DialogueNode smallTalkPositiveResponse1 = new DialogueNode("Ah, you understand! Our bond with the machine elevates us beyond mere mortals.");
            smallTalkPositiveResponse1.OnAfterEvent = (dm) => { dm.ImproveRelationship(); dm.CurrentNode = startNode; };

            DialogueNode smallTalkNegativeResponse1 = new DialogueNode("It's disappointing to hear you say that. Not everyone can see the beauty in our transformation.");
            smallTalkNegativeResponse1.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); dm.CurrentNode = startNode; };

            smallTalkNode1.AddChoice("I see the beauty in our transformation. The bond with the machine is indeed profound.", smallTalkPositiveResponse1);
            smallTalkNode1.AddChoice("This place feels cold and uninviting, more like a prison.", smallTalkNegativeResponse1);

            DialogueNode smallTalkPositiveResponse2 = new DialogueNode("Your appreciation for our journey warms my circuits. We are pioneers of a new age.");
            smallTalkPositiveResponse2.OnAfterEvent = (dm) => { dm.ImproveRelationship(); dm.CurrentNode = startNode; };

            DialogueNode smallTalkNegativeResponse2 = new DialogueNode("A shame you feel that way. Our path isn't for everyone, but it's a necessary evolution.");
            smallTalkNegativeResponse2.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); dm.CurrentNode = startNode; };

            smallTalkNode2.AddChoice("Your journey is inspiring. The merger of flesh and technology is a fascinating destiny.", smallTalkPositiveResponse2);
            smallTalkNode2.AddChoice("You sacrificed your humanity for this? It doesn't seem worth it.", smallTalkNegativeResponse2);

            // Allow player to say goodbye after learning about the mission
            missionNode.OnAfterEvent = (dm) =>
            {
                bool addAsLastChoice = true;
                startNode.AddChoice("How was Elias your closest ally – what happened?", eliasNode);
                startNode.AddChoice("Goodbye.", goodbyeNode, addAsLastChoice); //adding as last choice because it will end the dialogue
                dm.CurrentNode = startNode;
            };

            // Assign dialogue to Ezekiel the Mechanomancer
            ezekielTheMechanomancer.CreateDialogueManager(startNode);

        }
    }
}
