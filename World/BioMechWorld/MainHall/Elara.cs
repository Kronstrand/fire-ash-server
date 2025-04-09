using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.BioMechWorld.MainHall
{
    internal class Elara
    {
        public static Character Create(out DialogueNode elaraStartNode)
        {
            Character elaraTheDefender = new Character(
                "Elara",
                "Elara stands tall, her presence commanding and resolute. " +
                "At first glance, she appears to be a normal human, " +
                "but as you look closer, you notice the subtle gleam of her metallic skin, " +
                "a clear indication that her original skin has been replaced. " +
                "Elara's demeanor is both fierce and protective. " +
                "She is the shield that guards this stronghold.",
            Kindred.Mecharion,
            CreatureType.Humanoid,
            15, //strength
            14, //dexterity
            13, //constitution
            12, //intelligence
            15, //wisdom
            11, //charisma
                "Elara's tall, imposing figure lies motionless. " +
                "The once subtle gleam of her replaced metallic skin now stands out in stark contrast to the cold, lifeless form she has become. " +
                "Elara's face, a blend of human and machine, is frozen in a final expression of resolve. " +
                "Her body, once a perfect fusion of organic and mechanical, now rests as a testament to her ultimate sacrifice in defending her people."
            );
            elaraTheDefender.UniqueName = true;
            elaraTheDefender.HP = 40;
            elaraTheDefender.AddFeat(FeatKey.MeleeAttack);
            elaraTheDefender.AddFeat(FeatKey.RangedAttack);
            elaraTheDefender.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            elaraTheDefender.AddToInventory(new Coins(500, 10));

            // Create dialogue nodes for Elara the Defender
            elaraStartNode = new DialogueNode("I am Elara, the Defender of this enclave. Time is against us, and I have no one else to spare. I need your help.");
            DialogueNode elaraTunnelMission = new DialogueNode("Fighting the Purists head-on would be a bloodbath. There are some old tunnels that offer us a hidden route, one that can take us straight to the enemy's heart. I need you to locate these tunnels.");

            DialogueNode elaraTunnelsExplanationNode = new DialogueNode(
                "These tunnels are remnants of a forgotten age, long sealed off and buried under the weight of history. " +
                "This facility was built on the ruins of an ancient temple dedicated to Zathar, the old snake god. " +
                "Zathar was worshipped as the deity of wisdom, stealth, and transformation. When the Macharion built this facility, they closed the entrance to the temple, and no one has been there since."
            );

            DialogueNode elaraSeekVexisNode = new DialogueNode("To find these tunnels, you must seek out Vexis, the caretaker. Vexis knows the way into these ancient passages. Find him in the Caretaker's Chamber, close to the Creation Chamber. When you locate the tunnels, you'll need to find your way through and clear them for safe passage.");
            DialogueNode elaraEndNode = new DialogueNode("");       

            elaraStartNode.AddChoice("How so?", elaraTunnelMission);

            elaraTunnelMission.AddChoice("Any idea where to find the entrance?", elaraSeekVexisNode);
            elaraTunnelMission.AddChoice("What were these tunnels used for?", elaraTunnelsExplanationNode);

            elaraTunnelsExplanationNode.AddChoice("Any idea where to find them?", elaraSeekVexisNode);

            elaraSeekVexisNode.AddChoice("I see.", elaraEndNode);

            bool gaveGold = false;
            elaraEndNode.OnAfterEvent = (DialogueManager dm) =>
            {
                if (!gaveGold)
                {
                    if (elaraTheDefender.SpeakingTo != null)
                    {
                        int goldAmount = 150;
                        elaraTheDefender.BroadcastToSoulsInRoom(
                            $"{elaraTheDefender.Name} hands {goldAmount} gold coins to {elaraTheDefender.SpeakingTo.Name}. " +
                            "With a knowing smile, she says, \"Spend this wisely. The bazar has more than just gear.\"");
                        elaraTheDefender.TransferCoinTo(elaraTheDefender.SpeakingTo, goldAmount, 0);
                        
                        gaveGold = true;
                    }
                }

                Program.GlobalVariables.vexisTheCaretaker.CreateDialogueManager(Program.GlobalVariables.vexisTemplePermissionNode);
            };

            // Assign dialogue to Elara is done by Ezekiel

            return elaraTheDefender;
        }
    }
}
