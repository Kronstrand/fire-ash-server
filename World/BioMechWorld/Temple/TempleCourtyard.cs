using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using System.Numerics;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class TempleCourtyard
    {
        public static Room Create(Room undergroundCavePath)
        {
            Room templeCourtyard = new Room(
                RoomKey.TempleCourtyard,
                "Temple Courtyard",
                "The courtyard before the temple is a place of quiet reverence. " +
                "Weathered stone tiles cover the ground, cracked and overgrown with patches of moss. " +
                "An imposing statue of a coiled serpent, eight feet tall, stands vigil against the temple wall, " +
                "its dark eyes gleaming with an unsettling presence. " +
                "Fireflies float lazily through the air, their soft glow providing the only light in this otherwise shadowed space."
            );

            Exit toTempleCourtyard = new Exit(
                "A large tree root, twisted and warped into the shape of a serpent, coils around a dark opening ahead. " +
                "The root seems almost alive, as if some unnatural force has bent the form of nature itself. Beyond the root",
                "The opening is engulfed in shadow, with a small stone path leading deeper into the cave.",
                templeCourtyard
            );
            undergroundCavePath.AddExit(toTempleCourtyard);

            Exit toUndergroundCavePath = new Exit(
                "A narrow stone path leads out of the courtyard.",
                undergroundCavePath
            );
            templeCourtyard.AddExit(toUndergroundCavePath);

            // Create the statue
            Item serpentStatue = new Item(
                "Serpent Statue",
                "By the temple wall",
                "A massive stone statue of a coiled serpent, standing ten feet tall. Its eyes gleam darkly, as if watching all who approach."
            );
            serpentStatue.MakeUnpickupable(); // The statue is a permanent fixture
            templeCourtyard.AddItem(serpentStatue);

            // Create the fireflies (an atmospheric effect)
            Item fireflies = new Item(
                "Fireflies",
                "Around the entrance",
                "A small cluster of glowing fireflies, their greenish light flickering gently in the air."
            );
            fireflies.MakeUnpickupable(); // The fireflies are an ambient effect
            templeCourtyard.AddItem(fireflies);



            // Add an exit to the Temple Entrance
            /*Exit toTempleEntrance = new Exit(
                "Through the entrance",
                "A dark, imposing entrance to the ancient temple, flanked by tall pillars.",
                templeEntrance
            );
            templeCourtyard.AddExit(toTempleEntrance);*/

            Character lily = new Character(
                "Lily",
                "Lily's form is a haunting fusion of human and machine. Her once delicate features are now marred by the grotesque melding of flesh and metal. " +
                "Her eyes, once filled with life, are now a cold, unblinking mechanical gaze. She moves with a strange, unnatural grace, " +
                "her mechanical limbs occasionally sparking with electricity. The remnants of her humanity are seen in the occasional flicker of emotion that crosses her face, " +
                "but they are fleeting, overwhelmed by the cold efficiency of her mechanical parts.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                7,  // Strength
                12, // Dexterity
                7,  // Constitution
                14, // Intelligence
                15, // Wisdom
                11, // Charisma
                "Lily's broken form lies still, the once flickering signs of life within her now extinguished. " +
                "Her body, a twisted amalgamation of flesh and metal, is a grim testament to the failed experiment that she became."
            );

            lily.UniqueName = true;
            lily.HP = 12;
            lily.AddFeat(FeatKey.Stealth);
            lily.AddFeat(FeatKey.MeleeAttack);
            lily.AddFeat(FeatKey.RangedAttack);
            lily.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            lily.AddEquippedItem(InventorySlot.Ranged, WeaponList.YShapedSlingShot());

            // Dialogue Nodes
            DialogueNode lilyStartNode = new DialogueNode("W-who... wh-who... ar-are... y-you? Ev-everything... s-so... d-dark... wh-where... am I...?");
            DialogueNode lilyMemoryNode = new DialogueNode("I... I was... I th-think I... n-no, it's... g-gone... C-can't... rem-member... J-just... n-noise... and... p-pain...");
            DialogueNode lilyEliasLucidNode = new DialogueNode("E-Elias...? D-dad...? H-he... h-he left... m-me... all al-alone... D-did he... f-forget... me...");
            DialogueNode lilyPainNode = new DialogueNode("Ev-everything... b-broken... inside... W-why... c-can't I... feel...? C-cold... s-so c-cold... b-but... b-burning... t-too...");
            DialogueNode lilyQuestionNode = new DialogueNode("h-what... wh-why... I... I c-can't... D-did I... d-die...? Or... am I... s-still... l-living...?");
            DialogueNode lilyAliveResponseNode = new DialogueNode("A-alive...? B-but... I... I th-thought... m-maybe... th-this w-was... h-hell...");
            DialogueNode lilyUncertaintyResponseNode = new DialogueNode("D-don't... kn-know...");
            DialogueNode lilyFinalNode = new DialogueNode("Th-thank... y-you...? Wh-what... was... I... s-saying...? G-goodbye...?");

            // Initial Choices
            lilyStartNode.AddChoice("What happened to you?", lilyMemoryNode);
            lilyStartNode.AddChoice("Goodbye, Lily.", lilyFinalNode, true); // add last
            
            lilyMemoryNode.AddChoice("Why are you in pain?", lilyPainNode);
            lilyMemoryNode.AddChoice("You are Elias' daughter, do you remember your father?", lilyEliasLucidNode);

            // Choices for lilyEliasLucidNode
            lilyEliasLucidNode.AddChoice(
                "He didn't forget you, Lily.",
                new DialogueNode("H-he... he didn't...? B-but... wh-why... wh-why am I... s-so... a-alone...? H-he... pr-promised... to... s-save me...")
            );
            lilyEliasLucidNode.AddChoice(
                "It's time to let go, Lily.",
                new DialogueNode("L-let... go...? B-but... I... wh-what... do I... d-do...? H-he... he was... all I... h-had...")
            );
            lilyEliasLucidNode.AddChoice("Goodbye, Lily.", lilyFinalNode);

            // Choices for other nodes
            lilyPainNode.AddChoice("Are you okay?", lilyQuestionNode);
            lilyPainNode.AddChoice("Goodbye, Lily.", lilyFinalNode, true); // add last

            // Responses to Lily's Question
            lilyQuestionNode.AddChoice("You're still very much alive, Lily.", lilyAliveResponseNode);
            lilyQuestionNode.AddChoice(
                "I don't know...",
                (dm) => {
                    lily.BroadcastToSoulsInRoom($"{lily.Name} stares blankly into the void, her expression unchanging... as if she expected no other answer...");
                    return lilyUncertaintyResponseNode;
                });

            // Assign dialogue to Lily
            lily.CreateDialogueManager(lilyStartNode);
            lily.GoToRoom(templeCourtyard);


            //Eriska
            Character eriska = new Character(
                "Eriska",
                "Eriska stands shrouded in flowing, tattered robes that seem to absorb the light around her, creating an aura of unsettling calm. " +
                "Her wide-brimmed hat, adorned with strange metallic trinkets, casts a shadow over her face, leaving only her piercing eyes that glow with an unnatural light, " +
                "their depths swirling with an enigmatic mix of wisdom and madness. Tubes and wires snake out from beneath her garments, " +
                "occasionally sparking with a dim energy as though whispering secrets of the past.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                10,  // Strength
                14,  // Dexterity
                9,   // Constitution
                18,  // Intelligence
                16,  // Wisdom
                12,   // Charisma
                "Eriska's form lies still, her once glowing eyes now dark and empty. " +
                "Her hat has toppled to the ground, revealing a mess of lifeless wires and sinew. " +
                "The tattered robes, once flowing with an eerie grace, now drape over her inanimate form, silent and unmoving."
            );

            eriska.UniqueName = true;
            eriska.HP = 24;
            eriska.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);

            DialogueNode eriskaIntroNode = new DialogueNode(
                "I see you have met Lily... Such a fragile thing, isn't she? A pity, truly, what has become of her. But pity does little to mend what is broken."
            );

            DialogueNode eriskaLilyConditionNode = new DialogueNode(
                "It is far more complex than what those who rely solely on mechanics can fathom. " +
                "What ails her is not the malfunctioning of parts, but the dissonance of the spirit within the machine. " +
                "Her soul, if you will, is in turmoil, fragmented by the crude union of flesh and metal. " +
                "The others may see only what is broken on the surface, but I have delved deeper. " +
                "It is in this dissonance, this spiritual imbalance, that the true root of her suffering lies."
            );

            DialogueNode eriskaMissionNode = new DialogueNode(
                "To stabilize Lily's condition, we must conduct a ritual that realigns her spirit with her mechanical form. " +
                "For this, I need a specific ingredient. Something that can bridge the gap between the spiritual and the physical. " +
                "It's called the Serpent's Tear, a relic hidden deep within the Temple of Coiled Fate. " +
                "The temple is dangerous, its paths shifting and guarded by forces beyond the ordinary. " +
                "But without the Tear, Lily's suffering will only continue. I need your help to retrieve it. " +
                "If you obtain it, return here, and we can proceed.."
            ); //dead end

            DialogueNode eriskaVexisNode = new DialogueNode(
                "Vexis tried, in his way, to repair what was broken. He's a master of machines. " +
                "He replaced parts, adjusted mechanisms, and recalibrated circuits, all in vain. " +
                "After his repeated failures, something in Vexis broke as well. He took it as a personal failure, and I fear he has never been the same since. " +
                "He avoids her now, as if he cannot bear to face the embodiment of his failure. " +
                "But where machines have failed, there may yet be another way. A solution that requires a different kind of understanding."
            );

            DialogueNode eriskaSpiritualImbalanceNode = new DialogueNode(
                "When flesh and metal are forced together, it creates a harmony, or a disharmony in Lily's case. " +
                "Her essence, her very soul, is struggling against the cold, unyielding machinery that now encases it. " +
                "This struggle has caused a rift, a dissonance that reverberates through her entire being, " +
                "manifesting as the pain and confusion you witnessed. " +
                "Normally, such a rift might have settled over time, as the soul adapts to its new form. " +
                "But Lily's trauma, being torn from her family, rejected and abandoned before she was ready " +
                "has deepened this rift, perpetuating her suffering. " +
                "Her spirit cannot reconcile with the machine because the wound in her heart resonates with the imbalance, " +
                "keeping it open, festering. This is not something that can be repaired with tools or replaced with new parts. " +
                "No, this requires a much more... esoteric approach."
            );

            eriskaVexisNode.AddChoice("Why Couldn't he fix her?", eriskaSpiritualImbalanceNode);
            eriskaVexisNode.AddChoice("Do you have solution?", eriskaMissionNode);

            //Eriska has begun the conversation by focusing on Lily
            eriskaIntroNode.AddChoice("What exactly is wrong with Lily?", eriskaLilyConditionNode);
            eriskaIntroNode.AddChoice("You speak of mending, what do you intend to do?", eriskaMissionNode); //maybe add some in-between
            eriskaIntroNode.AddChoice("Lily's suffering is unbearable. Can she be saved?", eriskaVexisNode);


            //Eriska have spoken about her condition
            eriskaLilyConditionNode.AddChoice("What is this spiritual imbalance?", eriskaSpiritualImbalanceNode);

            DialogueNode eriskaEzekielNode = new DialogueNode(
                "Ezekiel... He has washed his hands of the matter. To him, Lily represents a failure. A flaw in the grand design he so obsessively pursues. " +
                "She defies the cold logic of the machine, a living reminder of the imperfection he cannot tolerate. " +
                "As for me, Ezekiel would rather have nothing to do with my methods, or with Lily herself. " +
                "He believes that whatever happens to her now is of no consequence. His focus is elsewhere, on perfecting the fusion of flesh and steel without such... distractions."
            ); //dead end..

            //Eriska has spoken more deeply about her condition
            eriskaSpiritualImbalanceNode.AddChoice("Does Ezekiel know you are trying to fix her?", eriskaEzekielNode);
            eriskaSpiritualImbalanceNode.AddChoice("What do you have in mind?", eriskaMissionNode);


            //Add something about her still being..

            eriska.CreateDialogueManager(eriskaIntroNode);
            eriska.GoToRoom(templeCourtyard);



            return templeCourtyard;
        }
    }
}
