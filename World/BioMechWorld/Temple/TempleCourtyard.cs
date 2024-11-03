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
using fire_ash_server.Moves;
using System.Runtime.CompilerServices;

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
                "An imposing statue of a coiled serpent, ten feet tall, stands vigil against the temple wall, " +
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

            GrandTempleAscent.Create(templeCourtyard);
            Room subterraneanPassage = SubterraneanPassage.Create(templeCourtyard);

            // Create exit from Temple Courtyard to Subterranean Passage
            Exit toSubterraneanPassage = new Exit(
                "At the eastern edge of the courtyard",
                "A small stone staircase descends sharply into a subterranean passage.",
                subterraneanPassage
            );
            templeCourtyard.AddExit(toSubterraneanPassage);

            //Lilly
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
            lilyFinalNode.OnAfterEvent = (DialogueManager dm) => {
                if (lily.IsInGroupWith(toSubterraneanPassage) == true)
                {
                    MoveTo move = new MoveTo(lily.Soul, serpentStatue);
                    move.Action();
                }
            };

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

            lily.MoveToGroup(toSubterraneanPassage);

            toSubterraneanPassage.OnBeforeExitEvent = (Soul soul) =>
            {
                if (toSubterraneanPassage.IsInGroupWith(lily) != true)
                    return false;

                _ = soul.SendAsync("Lily is blocking the way...");
                return true;
            };

            return templeCourtyard;
        }
    }
}
