using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;

namespace fire_ash_server.World.BioMechWorld.MainHall
{
    internal class Ezekiel
    {
        public static Character Create(Character elara, DialogueNode elaraStartNode)
        {
            Character ezekielTheMechanomancer = new Character(
                "Ezekiel",
                "Ezekiel's appearance is a chilling fusion of human and machine, with cybernetic parts grotesquely integrated into his body. His face, frozen in a sinister grin, is a ghastly combination of metal and skin. Draped in old, scarred armor adorned with a variety of menacing tools and weapons, Ezekiel's very presence instills terror and demands respect.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                14, //strength
                10, //dexterity
                11, //constitution
                16, //intelligence
                13, //wisdom
                9,  //charisma
                //death description
                "Ezekiel's lifeless body lies still, a grotesque mix of cybernetic implants and organic tissue. " +
                "His face, once marked by a disturbing grin, is frozen in death. " +
                "His heavy, worn armor, adorned with numerous tools and weapons, rests motionless. " +
                "Once revered and feared, Ezekiel now lies silent, a testament to his ruthless pursuit of knowledge and power."
                );
            ezekielTheMechanomancer.UniqueName = true;
            ezekielTheMechanomancer.HP = 30;
            ezekielTheMechanomancer.AddFeat(FeatKey.MeleeAttack);

            ezekielTheMechanomancer.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);

            ezekielTheMechanomancer.AddMove(new SkillCheck(
                    null,
                    "Recall lore about Ezekiel.",
                    new SkillNumber(Skill.Religion, 8),
                    true,
                    async (s) =>
                    {
                        return
                        "Ezekiel the Mechanomancer, an enigmatic and commanding figure, is whispered to be a high priest of the so-called 'Cult of Technomancers,' " +
                        "a term coined by his detractors to vilify his vision. To his followers, he is the guiding force of the Mecharions, " +
                        "a people who embrace the unity of flesh and technology as the next stage of evolution.";
                    },
                    async (s) =>
                    {
                        if (s.Character.Kindred == Kindred.Mecharion)
                        {
                            return "Your newborn memory is failing you, " +
                            "the details of this strange and ominous figure slipping through the cracks " +
                            "of your freshly hatched mind. The knowledge remains just out of reach.";
                        }
                        return "Your mind draws a blank, the details of this particular religion eluding your memory. " +
                        "You struggle to recall any relevant information about him.";
                    }
                    ));

            // Assign dialogue to Ezekiel the Mechanomancer
            ezekielTheMechanomancer.CreateDialogueManager(
                    InitEzekielDialogue(elara, elaraStartNode));
            
            ezekielTheMechanomancer.OnAfterSpeakTo = (Soul soul, Character character) => 
            { 
                ezekielTheMechanomancer.CreateDialogueManager(
                    InitEzekielDialogue(elara, elaraStartNode)); 
            };

            return ezekielTheMechanomancer;
        }

        private static DialogueNode InitEzekielDialogue(Character elara, DialogueNode elaraStartNode)
        {
            // Create dialogue nodes for Ezekiel the Mechanomancer
            DialogueNode startNode = new DialogueNode("Ah, fresh from the Mother-Machine's embrace! Welcome, welcome! We have much to do, and little time.");
            DialogueNode whoAreYouNode = new DialogueNode("Yes, I am the conductor of this grand symphony of synthetic life. Known as Ezekiel, I crafted and govern this haven of progress. It fills me with pride to see the fruits of my labor, beings like you, emerging from our work. Intriguing, isn't it?");
            DialogueNode whatHappenedNode1 = new DialogueNode("Ah, you were reborn! The Mother-Machine took you, reshaped you, and now you are one of us. You are a Mecharion, forged from the union of flesh and steel.");
            DialogueNode motherMachineNode = new DialogueNode("Ah, the Mother-Machine, a marvel of sinew and steel! She takes us, reshapes us. Some were sold, some were prisoners, but all are reborn here. Now, you're a Mecharion, part of this grand design.");
            DialogueNode attackNode = new DialogueNode("The facility is under siege! The Purists have come, a fanatical group that condemns our beautiful union of flesh and technology. They seek to destroy the Mother-Machine and all her creations.");
            DialogueNode puristsNode = new DialogueNode("The Purists, zealots who believe our existence is an abomination. They think they can 'cleanse' us by tearing down everything we've become. Fools, the lot of them.");
            DialogueNode identityNode = new DialogueNode("Your story? Ha! Each of us has a unique tale, a reason we ended up here. But the Mother-Machine doesn't care for our pasts, only for what we become.");
            DialogueNode missionNode = new DialogueNode("Ah, my precious, we face dire times. The Purists, with their cold, unyielding ideology, have breached our sanctum. They seek to destroy the Mother-Machine, our ancient lifeline that maintains the balance between technology and nature. To them, it's a blasphemy they must purge to create their vision of purity. Without it, our world would wither, and chaos would reign.\n\nLeading them is the Vicar of Purity. Once my closest ally, now my most bitter adversary. His name is Elias, a brilliant mind twisted by fanaticism. He believes that only by destroying the Mother-Machine can he cleanse the world. His mastery over mind and matter makes him a perilous foe.\n\nWe must defend what we hold dear. Find Elias, disrupt his plans, and show him we will not be undone.");
            DialogueNode eliasNode = new DialogueNode("Elias and I were once visionaries, united by our desire to blend technology and nature harmoniously. We spent countless nights debating, designing, and dreaming. He was brilliant, passionate, and relentless in his pursuit of knowledge.\n\nBut then, tragedy struck. As we were working on the Mother-Machine, Elias's daughter, Lily, fell gravely ill. Desperate to save her, Elias submitted her to the Mother-Machine before we had fully tested its capabilities. He believed it could cure her, melding her illness away through a perfect synthesis of organic and synthetic life.\n\nHowever, the result was horrific. The machine malfunctioned, and instead of healing her, it twisted her into a grotesque fusion of flesh and metal. Consumed by guilt and grief, Elias became convinced that the Mother-Machine was an abomination, a monstrous creation that must be destroyed.\n\nOur bond shattered when he declared his intention to eradicate it. I opposed him, believing it was the cornerstone of our world's harmony. He saw my resistance as a betrayal, and I, his fanaticism as madness. He left, vowing to return and cleanse our world by any means necessary. Now, he leads the Purists, driven by his tragic past and a warped sense of purpose.");
            //DialogueNode goodbyeNode = new DialogueNode("Depart then, and may the spectral glow of the Machine God illuminate your path. We will meet again.");
            DialogueNode goodbyeNode = new DialogueNode("Before you go, speak with Elara, she have details about how you can help defeat the Purists.");
            goodbyeNode.OnAfterEvent = (DialogueManager dm) =>
            {
                // Assign dialogue to Elara the Defender
                elara.CreateDialogueManager(elaraStartNode);
            };
            DialogueNode smallTalkNode1 = new DialogueNode(
                "This place is no ordinary structure of metal and circuits; it is a hive, teeming with life, a sanctuary crafted by my own hands. " +
                "Here, the essence of our bond with the machine is palpable. The walls throb with the energy of new life being forged. " +
                "Each conduit pulses with the promise of creation, and every circuit sings with potential. We are not merely inhabitants; we are the lifeblood of this biomechanical nest. " +
                "Within this hive, we shed our mortal limitations, merging seamlessly with the organic and the mechanical. " +
                "In this sacred space, amidst the soothing hum of the machinery and the steady heartbeat of the living walls, we birth new forms. " +
                "This is our cradle, our refuge, where the union of flesh and technology thrives in perfect harmony. " +
                "It is here that I take the greatest pride, in nurturing the next generation of our kind."
            );
            DialogueNode smallTalkNode2 = new DialogueNode("Ah, I was Professor Ezekiel! I taught biotechnology at Havenbrook University. My days were consumed by lectures on the intricate dance between flesh and technology, and my nights were spent in the lab, delving into the mysteries of life itself. My experiments eventually led to my expulsion from the university. Now, in a twist of fate, I have become a living embodiment of those very experiments. Strange how destiny weaves its web, isn't it?");

            // Main dialogue choices

            startNode.AddChoice("We are at war?", attackNode);
            startNode.AddChoice("Tell me about this place.", smallTalkNode1);
            startNode.AddChoice("What happened to me?", whatHappenedNode1);
            startNode.AddChoice("So you are the leader of this place?", whoAreYouNode);

            smallTalkNode1.OnAfterEvent = (dm) =>
            {
                dm.CurrentNode = startNode;
            };

            whoAreYouNode.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("Who were you before you were mechanized?", smallTalkNode2);
                dm.CurrentNode = startNode;
            };

            whatHappenedNode1.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("The Mother-Machine?", motherMachineNode);
                dm.CurrentNode = startNode;
            };

            motherMachineNode.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("Was I sold or convicted?", identityNode);
                dm.CurrentNode = startNode;
            };

            identityNode.OnAfterEvent = (dm) => { dm.CurrentNode = startNode; };

            attackNode.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("Who are the Purists?", puristsNode);
                startNode.AddChoice("The Mother-Machine?", motherMachineNode);
                dm.CurrentNode = startNode;
            };

            puristsNode.OnAfterEvent = (dm) =>
            {
                startNode.AddChoice("What will you do about The Purists?", missionNode);
                dm.CurrentNode = startNode;
            };

            DialogueNode smallTalkPositiveResponse2 = new DialogueNode("Your appreciation for our journey warms my circuits. We are pioneers of a new age.");
            smallTalkPositiveResponse2.OnAfterEvent = (dm) => { dm.ImproveRelationship(); dm.CurrentNode = startNode; };

            DialogueNode smallTalkNegativeResponse2 = new DialogueNode("A shame you feel that way. Our path isn't for everyone, but it's a necessary evolution.");
            smallTalkNegativeResponse2.OnAfterEvent = (dm) => { dm.DecreaseRelationship(); dm.CurrentNode = startNode; };

            smallTalkNode2.AddChoice("You sacrificed your humanity for this? It doesn't seem worth it.", smallTalkNegativeResponse2);
            smallTalkNode2.AddChoice("Your journey is inspiring. The merger of flesh and technology is a fascinating destiny.", smallTalkPositiveResponse2);


            // Allow player to say goodbye after learning about the mission
            missionNode.OnAfterEvent = (dm) =>
            {
                bool addAsLastChoice = true;
                startNode.AddChoice("How was Elias your closest ally. What happened?", eliasNode);
                startNode.AddChoice("Goodbye.", goodbyeNode, addAsLastChoice); //adding as last choice because it will end the dialogue
                dm.CurrentNode = startNode;
            };

            eliasNode.OnAfterEvent = (dm) =>
            {
                dm.CurrentNode = startNode;
            };

            return startNode;
        }
    }
}
