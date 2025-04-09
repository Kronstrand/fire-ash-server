using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armor;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class UndergroundStudy
    {
        public static Room Create(Room subterraneanPassage)
        {
            // Define the new underground study
            Room undergroundStudy = new Room(
                RoomKey.UndergroundStudy,
                "Underground Study",
                "The underground study is hidden beneath the temple courtyard. " +
                "Its walls are lined with tall, dust-covered bookshelves, filled with thick, leather-bound tomes and ancient scrolls. " +
                "An iron chandelier hangs from the ceiling, " +
                "its flickering candles casting long, dancing shadows over the cold stone floor. " +
                "The air is damp, thick with the scent of mildew and forgotten knowledge. " +
                "A wooden desk sits against the south-facing wall."
            );
            undergroundStudy.Light = Light.Bright;

            Exit toUndergroundStudy = new Exit(
                "After a set of southward stairs",
                "An open doorway leads east into an underground study.",
                undergroundStudy
            );
            subterraneanPassage.AddExit(toUndergroundStudy);

            Item desk = new Item(
                "Wooden Desk",
                "At the far end of the room, beneath a weathered stone archway",
                "A wooden desk sits cluttered with manuscripts, " +
                "with a single unlit candle resting on its surface."
                );
            undergroundStudy.AddItem(desk);

            // Create exit from Underground Study back to Subterranean Passage
            Exit toSubterraneanPassageFromStudy = new Exit(
                "An open doorway leads east out into an Subterranean Passage.",
                subterraneanPassage
            );
            undergroundStudy.AddExit(toSubterraneanPassageFromStudy);

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
            eriska.IsTrader = true;
            eriska.tradeModifier = 0.2;
            eriska.HP = 24;
            eriska.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);

            eriska.AddToInventory(ArmorList.NocturnalOptics());
            eriska.AddToInventory(ConsumableList.HealthPotion());
            eriska.AddToInventory(ConsumableList.HealthPotion());
            eriska.AddToInventory(ConsumableList.HealthPotion());
            eriska.AddToInventory(ConsumableList.BookOfHealth());
            eriska.AddToInventory(new Coins(200, 32));


            eriska.AddOnAfterMoveToEvent((Soul soul) =>
            {
                if (eriska.Dead)
                    return Task.FromResult(false);
                if (soul.Character.IsHidden())
                    return Task.FromResult(false);
                if (soul.Character.HP == soul.Character.CurrentHP)
                    return Task.FromResult(false);

                soul.Character.CurrentRoom.BroadcastToSoulsInRoom(
                    $"With a slow gesture, Eriska traces a pattern in the air, and faint ribbons of light drift toward {soul.Character.Name}. " +
                    $"The energy settles over {FormatPossessive(soul.Character.Name)} wounds, " +
                    $"stitching them closed with a soft, fleeting warmth before fading into nothing.");
                soul.Character.GainLife(soul.Character.HP - soul.Character.CurrentHP);

                return Task.FromResult(true);
            },
            false);

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

            // Eriska's response to the player's disdain
            DialogueNode eriskaDisdainResponseNode = new DialogueNode(
                "You might see it that way, but what's done cannot be undone. " +
                "The world has moved beyond simple flesh and bone. The fusion of metal and spirit is the future. " +
                "Lily's suffering isn't a failure of the transformation itself but a failure to achieve harmony in it. " +
                "We must work with what she has become, not what she once was. The past is irrelevant. Only the solution matters."
            );

            DialogueNode eriskaMissionNode = new DialogueNode(
               "To stabilize Lily's condition, we must conduct a ritual that realigns her spirit with her mechanical form. " +
               "For this, I need a specific ingredient. Something that can bridge the gap between the spiritual and the physical. " +
               "It's called the Serpent's Tear, a relic hidden deep within the Temple of Coiled Fate. " +
               "The temple is dangerous, its paths shifting and guarded by forces beyond the ordinary. " +
               "But without the Tear, Lily's suffering will only continue. I need your help to retrieve it. " +
               "If you obtain it, return here, and we can proceed.."
           ); //dead end

            // Continue conversation with other choices leading back to main dialogue
            eriskaDisdainResponseNode.AddChoice("What do you suggest then?", eriskaMissionNode);

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
            eriskaLilyConditionNode.AddChoice("Maybe the root of the problem is that she was ever made into a Mecharion at all.", eriskaDisdainResponseNode); // Player's disdainful response choice

            DialogueNode eriskaEzekielNode = new DialogueNode(
                "Ezekiel... He has washed his hands of the matter. To him, Lily represents a failure. A flaw in the grand design he so obsessively pursues. " +
                "She defies the cold logic of the machine, a living reminder of the imperfection he cannot tolerate. " +
                "As for me, Ezekiel would rather have nothing to do with my methods, or with Lily herself. " +
                "He believes that whatever happens to her now is of no consequence. His focus is elsewhere, on perfecting the fusion of flesh and steel without such... distractions."
            );

            //Eriska has spoken more deeply about her condition
            eriskaSpiritualImbalanceNode.AddChoice("Does Ezekiel know you are trying to fix her?", eriskaEzekielNode);
            eriskaSpiritualImbalanceNode.AddChoice("What do you have in mind?", eriskaMissionNode);


            //Add something about her still being..

            DialogueNode eriskaAgeExplanationNode = new DialogueNode(
                "Ah, you've noticed. Indeed, Lily appears young, untouched by the passage of time. This is one of the peculiarities of our kind, the Mecharions. " +
                "Once mechanized, we do not age as we did when we were merely flesh. Time's ravages are a distant concern, irrelevant to the amalgam of metal and sinew. " +
                "Lily, despite the years that have passed, remains as she was when first intertwined with the machine. " +
                "A child in appearance, yet carrying the weight of decades within her heart."
            );

            //Eriske talked about Ezikiel
            eriskaEzekielNode.AddChoice("But Lily looks so young. Wasn't she mechanized decades ago?", eriskaAgeExplanationNode);

            DialogueNode eriskaGeneralAgingNode = new DialogueNode(
                "Yes, all Mecharions are freed from the natural progression of time. Our bodies do not wither, nor do our minds grow frail. " +
                "Instead, we are left to endure, unchanged, until some external force or internal failure disrupts the delicate balance that sustains us. " +
                "It is a blessing for some, a curse for others. In Lily's case, her physical form remains unaltered, but the suffering within her spirit is timeless."
            );

            // Choices to further explore Lily's condition or return to the mission
            eriskaAgeExplanationNode.AddChoice("Does this mean all Mecharions don't age?", eriskaGeneralAgingNode);
            eriskaAgeExplanationNode.AddChoice("If she can't age, does that mean she will never heal?", eriskaMissionNode); // Imbalance note already used

            DialogueNode eriskaPhilosophyNode = new DialogueNode(
                "Time... it becomes a curious thing when it no longer holds sway over your body. For many, it is a relentless march, a constant reminder of mortality. " +
                "For us, it is more like a silent river, flowing around us, not through us. We observe its effects on the world, but we remain outside its grasp. " +
                "This perspective changes us, makes us see life differently. Our struggles become eternal, our victories timeless. It can be both liberating and terrifying."
            );

            DialogueNode eriskaMadnessNode = new DialogueNode(
                "Indeed, it does. Time becomes a cruel master when its passage no longer has meaning. For some Mecharions, the endless years stretch on like an eternal night, " +
                "and their minds, unable to cope with the vast expanse of unchanging existence, begin to unravel. They become lost in the void, " +
                "their thoughts circling endlessly until madness takes root. It is a slow, creeping insanity, one that erodes the self bit by bit, " +
                "until all that remains is a hollow shell, filled with echoes of the past and fears of the future." +

                "When a Mecharion falls into this abyss, there is little that can be done. The only mercy we can offer is to end their existence, " +
                "to free them from the torment of their own minds. It is a sad and desperate act, but one that is necessary. " +
                "For to let them continue would be to condemn them to an eternity of suffering, a fate far worse than any death." +

                "I believe that what ails Lily is not unlike the madness that afflicts these lost souls. A rift, a disharmony between the spirit and the form. " +
                "If we could find a way to heal her, to bring balance to her being, then perhaps... perhaps we could find a way to help those who have been consumed by their own minds. " +
                "It is a faint hope, but it is one I cling to. For in saving Lily, we might find the key to saving ourselves from the darkness that lies within."
            );

            // Choices to transition back to other nodes or deeper discussions
            eriskaGeneralAgingNode.AddChoice("How does this affect your view on time?", eriskaPhilosophyNode);
            eriskaGeneralAgingNode.AddChoice("This sounds like it could lead to a form of madness.", eriskaMadnessNode);

            DialogueNode eriskaPersonalInsightsNode = new DialogueNode(
                "My suffering... an astute question. You see, for Mecharions, death is not a looming inevitability, but a choice. " +
                "A choice we must consciously make. We are not bound by time, nor by the decay that comes with it. Our bodies do not age, do not wither. " +
                "But that doesn't mean we are free from pain, or from the burdens that time would normally ease with its passing. " +
                "Each day is a reminder of our stagnation, of the endless existence that lies before us. Some of us, like Ezekiel, seek solace in purpose, " +
                "in the perfection of our craft. Others, like myself, find the weight of choice pressing down with each passing moment. " +
                "The burden of deciding when to let go, when to release oneself from this unending life, it is a torment of its own kind. " +
                "For many, the decision never comes, and they live on in a limbo of existence, neither truly alive nor dead." +

                "Lily... she gives me a reason to focus, to channel my thoughts into something other than the void that awaits me. " +
                "Her suffering distracts me from my own, a mirror reflecting my desire to mend, to fix what is broken. " +
                "Perhaps, in saving her, I can find a semblance of peace. Or maybe I am merely delaying the inevitable. " +
                "In her eyes, I see a struggle similar to my own, a soul trapped in a cage of metal, yearning for release. " +
                "And yet, I cannot help but wonder... am I saving her for her sake, or for mine? In the end, the line blurs, " +
                "and I find myself lost in the shadows of my own making. But I still think it is the right thing to do."
            );

            DialogueNode eriskaEternalSufferingNode = new DialogueNode(
                "Not if I can help it. Lily's suffering doesn't have to be endless. There are ways to ease her pain, perhaps even restore her mind. " +
                "We just need to find the right approach, the right... solution. Her condition isn't hopeless. With the proper intervention, " +
                "we can prevent her from facing an eternity of torment. It's simply a matter of time and finding the right method."
            );
            eriskaEternalSufferingNode.AddChoice("Do you have a solution in mind?", eriskaMissionNode);

            DialogueChoice askAboutEriskaSuffering = new DialogueChoice("And what of your own suffering, Eriska?", eriskaPersonalInsightsNode); //instatiate it ensures we don't get the choice multiple times, when used, since that is not allowed.
            eriskaPhilosophyNode.AddChoice("I think I see what you're saying. Does that mean Lily could suffer like this forever?", eriskaEternalSufferingNode);
            eriskaPhilosophyNode.AddChoice(askAboutEriskaSuffering);

            eriskaPersonalInsightsNode.AddChoice("How will you save her?", eriskaMissionNode);

            // Choices to further explore the concept of madness or transition to other nodes
            eriskaMadnessNode.AddChoice("Tell me more about how you plan to heal Lily.", eriskaMissionNode);

            eriska.GoToRoom(undergroundStudy);
            eriska.MoveToGroup(desk);

            string bookDescription =
            "This ancient tome is bound in cracked, leathered skin, its cover adorned with the faded image of a serpent. " +
            "The pages within are brittle and yellowed with age, but the ink remains clear, as if the words themselves resist the passage of time. " +
            "A marked passage reads: " + "\n\n" +

            "Serpent's Tear" + "\n\n" +

            "Within the hallowed walls of the Temple of Coiled Fate lies the Serpent's Tear. " +
            "Legend tells that the Tear was shed when the Serpent foresaw the coming of an age where flesh and metal would merge, " +
            "disrupting the natural order." +
            "The Tear holds the essence of the Serpent's wisdom, " +
            "a bridge between the physical and spiritual realms. " +
            "It is said that only those who understand " +
            "the true balance of these forces may wield its power, " +
            "capable of mending the deepest of rifts within a soul." + "\n\n" +

            "The Chrono Serpent" + "\n\n" +

            "Guarding the Serpent's Tear is the Chrono Serpent, a being woven from the very fabric of time. " +
            "This ethereal creature is a manifestation of the eternal cycle, its form constantly shifting between the past, present, and future. " +
            "The Chrono Serpent possesses the power to manipulate time within the temple, slowing or accelerating its flow to protect the Tear. " +
            "It is said that the serpent's gaze can see all possible futures, and it will allow only those who are truly worthy to approach the Tear. " +
            "Those who fail its test are said to be lost in time, their fate forever sealed within the coils of the Serpent's domain.";

            Item book = new Item("An Account of the Nine Serpents", bookDescription, 13);
            desk.AddItem(book);
            desk.AddItem(WeaponList.WhiteCandle());

            bool SerpentsTearHandedOverToEriska = false;

            DialogueNode eriskaTearReceivedNode = new DialogueNode(
                "You've done it. The Serpent's Tear... it is beautiful, isn't it? A fragment of something ancient, something powerful. " +
                "This relic holds the essence of balance, a perfect equilibrium of opposing forces. It is exactly what we need to begin the ritual."
            );

            DialogueNode eriskaRitualExplanationNode = new DialogueNode(
                "The ritual will not be simple, nor will it be without risk. The Serpent's Tear is a conduit, a bridge, but I must guide it, " +
                "channeling its energy to realign Lily's spirit with her mechanical form. The process will test her, and it will test me. " +
                "There is no guarantee of success. If I falter, the imbalance could worsen, leaving her in even greater agony. " +
                "But if we succeed, she may finally know peace."
            );

            DialogueNode eriskaRiskAcknowledgmentNode = new DialogueNode(
                "I wish there were another way, but there isn't. Her suffering will only grow if we do nothing. " +
                "The Tear is our one chance to restore harmony to her being. I must see this through, no matter the cost. " +
                "If you have doubts, now is the time to voice them, for once the ritual begins, there is no turning back."
            );

            // New node where the player decides not to give the Tear
            DialogueNode playerDeclinesRitualNode = new DialogueNode(
                "I understand your hesitation. The path ahead is fraught with uncertainty. " +
                "I will not force your hand, but know that Lily's pain will persist, " +
                "and this opportunity may not come again. Should you change your mind, return to me, and we can reconsider."
            );

            DialogueNode eriskaFinalPreparationNode = new DialogueNode("");
            eriskaFinalPreparationNode.OnAfterEvent = (DialogueManager dm) => { SerpentsTearHandedOverToEriska = true; };

            // Adjust eriskaTearReceivedNode choices if needed
            eriskaTearReceivedNode.AddChoice("What happens now?", eriskaRitualExplanationNode);
            eriskaTearReceivedNode.AddChoice("I'm having second thoughts about this.", eriskaRiskAcknowledgmentNode);

            // Update choices in eriskaRiskAcknowledgmentNode
            eriskaRiskAcknowledgmentNode.AddChoice("Let's begin. [Hand over Serpent's Tear]", eriskaFinalPreparationNode);
            eriskaRiskAcknowledgmentNode.AddChoice("I can't go through with this. I'm sorry.", playerDeclinesRitualNode);

            // Update eriskaRitualExplanationNode to remove direct progression to final preparation        
            eriskaRitualExplanationNode.AddChoice("I understand the risks. [Hand over Serpent's Tear]", eriskaFinalPreparationNode);
            eriskaRitualExplanationNode.AddChoice("What risks are involved?", eriskaRiskAcknowledgmentNode);

            eriska.CreateDialogueManager(eriskaTearReceivedNode);

            eriska.OnBeforeSpeakTo = (Soul soul, Character SpeakToCharacter) => 
            {
                if (soul.Character.Inventory.ContainsItemWithName("Serpent's Tear"))
                    eriska.CreateDialogueManager(eriskaTearReceivedNode);
                else
                    eriska.CreateDialogueManager(eriskaIntroNode);
            };

            eriska.OnAfterSpeakTo = (Soul soul, Character SpeakToCharacter) =>
            {
                if (SerpentsTearHandedOverToEriska)
                {
                    _ = soul.SendAsync(GetEndText());
                    soul.CompletedGame = true;
                }
            };

            /*Item st = new Item("Serpent's Tear", "lol", 0);
            undergroundStudy.AddItem(st);*/

            return undergroundStudy;
        }

        private static string GetEndText()
        {
            string sceneDescription =
                    "Eriska cradles the Serpent's Tear, its eerie green light reflecting off her metallic skin.\n" +
                    "She nods solemnly, her voice steady but tinged with apprehension. " +
                    "She says: \"Come. It is time to end this suffering, one way or another.\"\n\n" +

                    "Lily walks beside Eriska, her fragile form dwarfed by the towering walls of the sanctum. " +
                    "She clutches her arm, her movements stiff, her expression one of quiet resignation. " +
                    "You follow closely as they ascend the staircase into the temple, finding their way to the Serpent Sanctum.\n\n" +

                    "The room's oppressive atmosphere settles over the group. The massive serpent statue looms above, its stone gaze both inviting and menacing. " +
                    "Around the altar, the ritual bowls seem to hum faintly, as if aware of the ancient energies about to be awakened. " +
                    "The air feels heavier, charged with an unseen force.\n\n" +

                    "Eriska places the Serpent's Tear in a socket at the center of the altar. The gem glows brighter, casting long shadows that dance across the walls. " +
                    "She begins to chant, her voice melodic yet alien, weaving an incantation that reverberates through the room.\n\n" +

                    "Lily steps forward hesitantly. Eriska gestures for her to kneel before the altar.\n\n" +

                    "Eriska whispers: \"This will hurt,\" her voice soft but unyielding. \"But it must.\"\n\n" +

                    "You watch as the Tear's light envelops Lily, the glow intensifying until it becomes blinding. " +
                    "A low hum grows into a deafening roar, and the room itself seems to tremble. " +
                    "Lily screams, a sound that is both human and something far more primal.\n\n" +

                    "The light recedes, and the silence that follows is absolute. Lily rises from the altar, her frail form replaced by something both beautiful and terrifying. " +
                    "Her skin shimmers with an iridescent sheen, veins of molten light coursing beneath. " +
                    "Her eyes, now glowing orbs, pierce the room with an unearthly gaze. " +
                    "She exudes a presence that feels too large for the space, as if the sanctum struggles to contain her.\n\n" +

                    "You step back instinctively, your breath caught in your throat. " +
                    "Lily says: \"I see it all now,\" her tone devoid of the fragility it once held. " +
                    "\"What was done to me, what was stolen. I was a child... I was helpless... And you-\"\n\n" +

                    "She turns to Eriska, her expression unreadable. For a moment, it seems like she might embrace her savior. " +
                    "Instead, her hand moves in a blur, and a surge of energy erupts from her palm. " +
                    "Eriska collapses, her body crumpling to the floor, lifeless.\n\n" +

                    "Lily says: \"-you made me this.\"\n\n" +

                    "Lily strides toward the exit of the sanctum, her every step reverberating through the ground. " +
                    "You hesitate, caught between the urge to intervene and the paralyzing weight of what you have just witnessed.\n\n" +

                    "She pauses at the threshold, glancing over her shoulder. " +
                    "Lily says: \"Come,\" she commands. \"There is more to be done.\"\n\n" +

                    "You follow, helpless to stop the events unfolding. In the main hall, Ezekiel stands waiting, his imposing frame shadowed by the flickering lights. " +
                    "He takes one look at Lily and his confident grin falters.\n\n" +

                    "Ezekiel murmurs: \"Lily?\" disbelief mingling with awe. \"What have you become?\"\n\n" +

                    "Lily's response is swift and merciless. She raises a hand, and Ezekiel is wrenched forward by an invisible force. " +
                    "His body contorts as light pierces through his cybernetic implants. His scream is short-lived.\n\n" +

                    "Lily whispers: \"For your perfection,\" as Ezekiel falls, his lifeless form a twisted shadow of its former self.\n\n" +

                    "The stronghold is in chaos. Mecharions scatter as Lily dismantles the facility with precise, unrelenting force. " +
                    "You follow her through the destruction, each step revealing the depths of her newfound power.\n\n" +

                    "At last, they reach Elias, his expression is stoic, though a flicker of regret crosses his face as he looks at Lily.\n\n" +

                    "Elias says: \"I wanted to save you,\" his voice quiet. \"What I did was wrong, but this… This is not salvation.\"\n\n" +

                    "Lily hesitates for the first time, her glowing eyes dimming slightly. Her voice softens, and for a moment, the child she once was seems to resurface.\n\n" +

                    "Lily says: \"Father,\" her tone almost tender. But the light in her chest flares, and her voice hardens. \"You abandoned me.\"" +
                    "She raises her hand, and Elias drops to his knees. His body convulses, and with a final gasp, he collapses, his lifeless eyes staring into the void.\n\n" +

                    "Lily stands amidst the ruins of the stronghold, her expression inscrutable. She turns to you, the light within her flickering as if struggling to stay contained.\n\n" +

                    "Lily says: \"I have no place here. Not among the living, not among the machines.\"\n\n" +

                    "She steps onto the remains of a dismantled platform, her form glowing brighter and brighter until it is impossible to look at her directly. " +
                    "With a final surge of light, she vanishes, leaving you alone in the silent wreckage.\n\n" +

                    "The stronghold lies in ruins. The Mecharions are scattered, their leaders gone. " +
                    "And Lily, no longer bound by flesh, machine, or mortality, has become something beyond comprehension.\n\n" +

                    "As you survey the devastation, a single thought lingers: Was this justice, or merely another form of revenge?";

            return sceneDescription;
        }
    }
}
