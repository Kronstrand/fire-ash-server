using System;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World.BioMechWorld.Temple;
using fire_ash_server.World.Goldfield;
using static fire_ash_server.World.ConsumableList;

namespace fire_ash_server.World
{
    static class Dialogues
    {
        public static Dictionary<DialogueKey, Func<DialogueNode>> Registry = new Dictionary<DialogueKey, Func<DialogueNode>>();

        public static void InitDicts()
        {
            Registry.Add(DialogueKey.CaretakerTempleOfLorath, CaretakerTempleOfLorath);
            Registry.Add(DialogueKey.RotMinder, RotMinder);
            Registry.Add(DialogueKey.Shaman, Shaman);
        }

        private static DialogueNode Shaman()
        {
            // The stressed-out opener
            DialogueNode n1 = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} slams his fist down onto a stack of wet parchment, causing his bone necklaces to clatter violently. " +
                       $"\"Do you mind?! I am trying to calculate the decay-velocity of the lower wetlands and Thrum hasn't sent up his afternoon log! " +
                       $"The Doctrine of the Great Rot expects a five percent increase in stagnation this quarter, and I am working with amateurs!\"";
            });

            // Node: Inquiring about the Doctrine / Management
            DialogueNode n2_doctrine = new DialogueNode((dm) =>
            {
                return $"\"What is it? It's administrative efficiency!\" {dm.SpeakingCharacter.Name} waves his quill wildly, leaving a trail of black dots in the air. " +
                       $"\"Solthera is dead. The old ecosystem is unsupported legacy code! We are transitioning the valley to an automated, mold-forward infrastructure. " +
                       $"But do the higher-ups supply more paddles? No! Do they fix the vat alignment? No! Just 'Gorg, optimize the rot!'\"";
            });

            // Node: Complaining about Thrum
            DialogueNode n2_thrum = new DialogueNode((dm) =>
            {
                return $"\"Thrum? Don't get me started on Thrum!\" {dm.SpeakingCharacter.Name} pulls at his hair, making his ornaments shake. " +
                       $"\"The man has zero vertical mobility drive. I tell him to stir clockwise for optimal spore distribution, and he just stares at me. " +
                       $"He's been 'quiet quitting' since the first age! If he weren't the only one who could survive the vat fumes, I'd outsource him to the goblins.\"";
            });

            // Node: Player threatens him
            DialogueNode n2_threat = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} freezes, looking at your weapon, then lets out a manic, high-pitched laugh. " +
                       $"\"Oh, brilliant. Go ahead! Kill the middleman! Let's see how the audit goes when the High Cultists arrive and find no one " +
                       $"has logged the carcass-to-muck ratios! Do you know how much paperwork an assassination causes? The supply chain will freeze!\"";
            });

            // Linking options to Node 1
            n1.AddChoice("What exactly is the 'Doctrine of the Great Rot'?", n2_doctrine);
            n1.AddChoice("Why don't you go down there and make Thrum do his job?", n2_thrum);
            n1.AddChoice("I'm here to put an end to your pollution, Shaman.", n2_threat);

            // Deep dive into the Bureaucracy
            DialogueNode n3_audit = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} scoffs, muttering under his breath as he scribbles a note. " +
                       $"\"The world ended, {OrcInsult(dm)}. Everyone is just trying to keep the lights on. The humans in Goldfield pretend they're still noble knights; " +
                       $"we catalog fungus. At least our metrics are honest. Now, if you're not going to hand me a ledger, get out of my office. The wind is ruining my margins.\"";
            });
            n2_doctrine.AddChoice("You're treating a holy apocalypse like a corporate audit.", n3_audit);

            DialogueNode n3_thrum_content = new DialogueNode((dm) =>
            {
                return $"\"Content?!\" {dm.SpeakingCharacter.Name} gasps, his quill snapping cleanly in his ink-stained fist. " +
                       $"\"That is exactly the problem! He has zero growth mindset! The whole valley is collapsing under systemic rot, " +
                       $"and his professional development goals are 'stir and sleep'?! If everyone had his complete lack of corporate synergy, " +
                       $"the Great Decay wouldn't even make it past the outer marshes by winter!\"";
            });
            n2_thrum.AddChoice("Honestly, I talked to him. He seems content just stirring.", n3_thrum_content);

            return n1;
        }

        private static string OrcInsult(DialogueManager dm)
        {
            if (dm.SpeakingCharacter.SpeakingTo != null)
            {
                if (dm.SpeakingCharacter.SpeakingTo.Kindred == Kindred.Human)
                    return "smooth-skin";
                else if (dm.SpeakingCharacter.SpeakingTo.Faction != dm.SpeakingCharacter.Faction)
                    return "outsider";
            }
            return "princess";
        }

        private static DialogueNode RotMinder()
        {
            // The deadpan opener
            DialogueNode n1 = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} doesn't look up from the cauldron. He just leans heavily on his paddle and sighs. " +
                       $"\"If you're going to kill me, do it clean. I don't want to bleed on the apron. The grease is impossible to get out.\"";
            });

            // Node: Inquiring about the vats / The downstream pollution
            DialogueNode n2_vats = new DialogueNode((dm) =>
            {
                return $"\"Boiling the blight-mash.\" {dm.SpeakingCharacter.Name} gives the sludge a lazy, wet stir. " +
                       $"\"The shamans like to preach about the Doctrine of the Great Rot up on the hill, but I'm the one who has to stir " +
                       $"the garbage down here so it flows downstream. Don't go into logistics.\"";
            });

            // Node: Epic confrontation / Tearing down the camp
            DialogueNode n2_camp = new DialogueNode((dm) =>
            {
                return $"\"Tear it down?\" {dm.SpeakingCharacter.Name} snorts, wiping sweat from his heavy brow. \"Go ahead. The walls are just woven brambles " +
                       $"and rot anyway. Might save me the trouble of sweeping. Just don't knock the vat over, or the shaman will make " +
                       $"me re-boil the whole batch.\"";
            });

            // Node: Direct hostility / Intimidation
            DialogueNode n2_threat = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} slowly blinks his tired eyes at you. \"Buddy, my lower back is gone and I've been smelling boiled " +
                       $"carcasses since sunrise. You think a sword scares me? Go ahead. It's an early clock-out.\"";
            });

            // Hooking the choices to the first node
            n1.AddChoice("What exactly is stewing in these cauldrons?", n2_vats);
            n1.AddChoice("I am here to tear this camp to the ground.", n2_camp);
            n1.AddChoice("Stand aside, orc. Your end has come.", n2_threat);

            DialogueNode n3_purpose = new DialogueNode((dm) =>
            {
                return $"\"The grand plan? Ask the boys with the bone necklaces up top,\" {dm.SpeakingCharacter.Name} grunts, leaning his weight back onto the paddle. " +
                       $"\"Something about breaking the valley's spirit from the roots up. Personally, I think they just like the smell. " +
                       $"I don't get paid to think. I get paid in moldy hardtack to keep the fire hot.\"";
            });

            DialogueNode n3_quit = new DialogueNode((dm) =>
            {
                return $"{dm.SpeakingCharacter.Name} looks at you for a long, flat beat, then looks back at the vat. " +
                       $"\"And go where? The other camps are exactly like this one, just with pointier spikes and worse hours. " +
                       $"At least down here by the creek, nobody expects me to march. I stir, I sleep, I repeat. There are worse gigs.\"";
            });

            n2_vats.AddChoice("Why poison the downstream water supply on purpose?", n3_purpose);
            n2_vats.AddChoice("You know you don't have to stay here and do this, right?", n3_quit);

            return n1;
        }

        private static DialogueNode CaretakerTempleOfLorath()
        {
            DialogueNode n1 = new DialogueNode(
                 "The harvest has not failed. It has only changed its way of coming."
             );

            DialogueNode n2_faith = new DialogueNode(
                "Yes... yes. Lorath will guide us."
            );

            DialogueNode n2_discomfort = new DialogueNode(
                "It is not reassuring because I cannot tell what the harvest is becoming. But do not take that worry to the town."
            );

            DialogueNode n2_blessing = new DialogueNode(
                "Of course, my child."
            );
            n2_blessing.OnAfterEvent = (dm) =>
            {
                Effect effect = new Effect();
                effect.LightRadiusModifer = Light.Dim;
                effect.rollModifiers.Add(new RollModifier(RollType.Attack, 1));
                BuffDebuff buff = new BuffDebuff("Blessed by Lorath", 1200, effect);
                if (dm.SpeakingCharacter.SpeakingTo != null)
                {
                    dm.SpeakingCharacter.SpeakingTo.AddBuffDebuff(buff);
                    dm.SpeakingCharacter.CurrentRoom.BroadcastToSoulsInRoom(
                        $"{dm.SpeakingCharacter.Name} places his hands gently upon {dm.SpeakingCharacter.SpeakingTo.Name}'s shoulders and murmurs: \"Bless this one, in whom we place our trust, to carry out your divine will in your holy name.\"" +
                        $"\n* {dm.SpeakingCharacter.SpeakingTo.Name} feels a quiet blessing of Lorath settle upon them. *");
                }
            };

            DialogueNode n2_beg = new DialogueNode("I am sorry, my child. The temple has little to spare."); 
            
            n2_beg.OnBeforeEvent = (dm) => { 
                if (dm.SpeakingCharacter.SpeakingTo == null) 
                    return; 
                double totalGold = dm.SpeakingCharacter.GetTotalCoinValue(); 
                if (totalGold > 10) 
                { 
                    dm.SpeakingCharacter.TransferCoinTo(dm.SpeakingCharacter.SpeakingTo, 10, 0); 
                    dm.CurrentNode = new DialogueNode("Of course, my child. Take this, and may Lorath see you through."); 
                } 
            };


            DialogueNode n2_give = new DialogueNode(
                "That is generous, my child. Lorath will surely put it to good use."
            );

            n2_give.OnBeforeEvent = (dm) =>
            {
                if (dm.SpeakingCharacter.SpeakingTo == null)
                    return;

                double totalGold = dm.SpeakingCharacter.SpeakingTo.GetTotalCoinValue();

                if (totalGold >= 20)
                {
                    dm.SpeakingCharacter.SpeakingTo.TransferCoinTo(
                        dm.SpeakingCharacter,
                        20,
                        0
                    );

                    dm.CurrentNode = new DialogueNode(
                        "Thank you, my child. May Lorath remember your generosity."
                    );
                }
            };

            n1.AddChoice("That is not reassuring.", n2_discomfort);
            n1.AddChoice("Lorath will see it through.", n2_faith);
            n1.AddChoice("Father, would you grant me a blessing in these trying times?", n2_blessing);
            n1.AddChoice("Could you spare a coin for a poor soul?", n2_beg);

            DialogueNode missingBook = new DialogueNode(
                "The book has gone missing!"
            );


            DialogueNode initNode = new DialogueNode("");
            initNode.OnBeforeEvent = (dm) =>
            {

                Item? alter = dm.SpeakingCharacter.CurrentRoom.GetItemById(Ids.templeOfLorath_altar);
                if (alter != null)
                { 
                    bool hasBook = alter.Items.Any(i => i.Name == Names.TheTeachingsofLorath);

                    if (hasBook)
                    {
                        dm.CurrentNode = n1;
                        if (dm.SpeakingCharacter.SpeakingTo != null && dm.SpeakingCharacter.SpeakingTo.GetTotalCoinValue() >= 20)                       
                            dm.CurrentNode.AddChoice("I would like to give 20 gold to the temple.", n2_give);

                        return;
                    }
                }

                dm.CurrentNode = missingBook;
            };

            return initNode;
        }
    }
}