using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Dialogue;

namespace fire_ash_server.World.BioMechWorld
{
    internal class CreationChamber
    {
        private Room creationChamber;
        private Item emptyIncubatorPod;

        public CreationChamber() 
        {
            creationChamber = new Room(
                Description(RoomKey.CreationChamber),
                "Creation Chamber",
                "The Creation Chamber is a sprawling facility filled with biomechanical incubation pods, each pulsating with an eerie, otherworldly energy. " +
                "The walls are a grotesque fusion of metal and flesh, with organic tubes and cables snaking across the surface, " +
                "pumping luminous, viscous fluids that glow with an unsettling, greenish hue. " +
                "In the center of the chamber stands the Mother-Machine, a towering, spidery construct of steel and sinew. " +
                "Its many arms, a blend of mechanical precision and organic fluidity, constantly twitch and move, " +
                "tending to the various incubation pods that line the room. " +
                "These pods, filled with a thick, amniotic fluid, house the nascent forms of new humans, " +
                "their silhouettes barely visible through the murky liquid. " +
                "The air is thick with the smell of antiseptic and the underlying scent of decay, " +
                "a constant reminder of the unnatural processes at work. " +
                "Soft, rhythmic humming fills the room, punctuated by the occasional hiss of escaping steam and the mechanical whirring of the Mother-Machine. " +
                "This chamber is both a birthplace and a factory, a grotesque testament to the blending of technology and biology."
            );

            emptyIncubatorPod = new Item(
                "Incubator Pod",
                "The incubator pod stands vacant, a fusion of organic and mechanical elements. " +
                "Its translucent, flesh-like walls glisten with a faint, greenish residue from the drained amniotic fluid. " +
                "Smooth, metallic surfaces interlace with the organic material, giving the pod an otherworldly appearance. " +
                "Mechanical arms and cables still snake from the ceiling and walls, connected to the pod and twitching slightly, " +
                "as if confused by the sudden absence of their occupant.",
                0
            );
            emptyIncubatorPod.MakeUnpickupable();

            // Add the empty incubator pod to the creation chamber
            creationChamber.AddItem(emptyIncubatorPod);

            Character motherMachine = new Character(
                "Mother-Machine",
                "The Mother-Machine is an awe-inspiring yet terrifying fusion of advanced technology and organic matter, " +
                "taking the form of a colossal spider. Standing at over ten feet tall, its main body is a mass of cables, gears, and pulsating flesh. " +
                "Multiple arms, some ending in delicate surgical instruments and others in multi-jointed appendages, extend from its torso, constantly adjusting and manipulating the incubation pods that surround it. " +
                "Its 'face' is a blank, metallic surface, with clusters of sensors and organic eyes that seem to peer into the very soul of anyone who dares to look. " +
                "The machine exudes a palpable sense of intelligence and malevolence, as if it possesses a will of its own. " +
                "The creation of new humans is both an act of precision and brutality, with the Mother-Machine ensuring each creation is perfected to its unsettling standards. " +
                "Its constant, rhythmic movements and the occasional sound of metal scraping together create a disturbing symphony that fills the chamber.",
                Kindred.Mecharion,
                CreatureType.Construct,
                18, // strength
                12, // dexterity
                15, // constitution
                15, // intelligence
                11, // wisdom
                8,   // charisma
                "The towering form of the Mother-Machine lies still, its multitude of arms now lifelessly hanging, and the symphony of metal scraping has fallen silent. " +
                "The blank, metallic surface of its 'face' now reflects a haunting stillness, and the once pulsating flesh within its mass of cables and gears is now eerily motionless. " +
                "The incubation pods, untouched, mark the end of its precise and brutal creations, leaving an unsettling silence in the chamber."
            );

            motherMachine.UniqueName = true;
            motherMachine.HP = 70;
            motherMachine.AddFeat(FeatKey.DualWield);
            motherMachine.DefaultHand = new InsectClaw();
            motherMachine.GoToRoom(creationChamber);
            motherMachine.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            motherMachine.DynamicDescription = false;

            // Create dialogue nodes for the Mother-Machine
            DialogueNode mm_startNode = new DialogueNode("Ah, a child returns... speak... listen...");
            DialogueNode mm_whoAreYouNode = new DialogueNode("I... Mother-Machine... weaver of life from metal dreams...");
            DialogueNode mm_whatHappenedNode = new DialogueNode("Reborn... flesh and wire in union divine...");
            DialogueNode mm_uneaseNode = new DialogueNode("Unease... new mind awakens and consciousness stirs...");
            DialogueNode mm_goodbyeNode = new DialogueNode("Depart... echoes of union guide you...");

            mm_startNode.AddChoice("Why do I feel uneasy?", mm_uneaseNode);
            mm_startNode.AddChoice("What happened to me?", mm_whatHappenedNode);
            mm_startNode.AddChoice("Who are you?", mm_whoAreYouNode);
            mm_startNode.AddChoice("Goodbye.", mm_goodbyeNode, true);

            // Assign dialogue to the Mother-Machine
            motherMachine.CreateDialogueManager(mm_startNode);

            new NexusBridge(creationChamber);
        }

        public Room CreateIncubator()
        {
            //Incubator Pod
            Room incubator = new Room(
                "Incubator" + GetNextId(),
                "Incubator",
                "You awaken in the confines of a small, enclosed space. The interior is soft and warm, " +
                "lined with a flesh-like material that pulsates gently, mimicking the rhythm of a heartbeat. " +
                "The walls are covered with a translucent membrane, through which you can see the shadowy outlines " +
                "of mechanical arms tending to other enclosures. The air inside is humid " +
                "and carries the faint scent of antiseptic mixed with a more organic, almost comforting aroma. " +
                "Soft, rhythmic humming fills the space, blending with the distant, muffled sounds of the larger chamber beyond. " +
                "You feel both cradled and imprisoned, as the walls seem to respond to your slightest movement, " +
                "constricting slightly before relaxing again."
            );

            Action<Soul> incubatorRelease = (s) =>
            {
                if (s.Character.CurrentRoom != incubator)
                    return;

                _ = s.SendAsync($"Suddenly, the floor beneath {s.Character.Name} begins to shift and open. " +
                    $"The membrane tears away, and {s.Character.Name} is flushed out of the enclosed space in a rush of fluids, " +
                    $"tumbling into the larger chamber beyond. The fluid is warm and viscous, carrying {s.Character.Name} with surprising force. " +
                    $"As {s.Character.Name} emerges, they find themselves lying on a slick, metallic surface, " +
                    $"surrounded by the eerie, mechanical environment of the larger chamber.");
                _ = s.MoveCharToRoomAndSendDescriptionAsync(creationChamber);
                s.Character.MoveToGroup(emptyIncubatorPod);

                /*s.Character.Journal.AddNewEntry(
                    JournalKey.TheMecharions, 
                    "I've escaped the pod. The chamber I'm in is vast, " + 
                    "filled with more incubation pods, " + 
                    "all connected to an intricate network of cables and machinery. " + 
                    "A towering machine looms in the center, tending to the pods. " + 
                    "I need to figure out where I am and why I was created here.");*/
            };

            Item umbilicalTube = new Item(
                "Umbilical Tube",
                "The umbilical tube is a disturbing technology, connecting you to the enclosure. " +
                "It pulses with a faint, rhythmic energy, as if it has a life of its own. " +
                "One end is securely attached to your abdomen, the connection feeling both invasive and strangely comforting. " +
                "The other end disappears into the wall of the enclosure, merging seamlessly with the surrounding membrane. " +
                "This tube not only provides nourishment and sustenance, but also seems to monitor your vital signs, " +
                "with occasional pulses indicating a transfer of information.",
                0
            );
            umbilicalTube.MakeUnpickupable();

            incubator.AddItem(umbilicalTube);
            incubator.OnEnterEvent = (s) =>
            {
                s.Character.MoveToGroup(umbilicalTube);
                if (s.Socket == null)
                    return;

                /*s.Character.Journal.AddNewEntry(
                    JournalKey.TheMecharions, 
                    "I woke inside a strange incubation pod, half metal, half flesh. " + 
                    "The air is thick with the sterile scent of machinery and fluids. " + 
                    "I need to find a way out of this place.");*/
            };

            Character ocularSentinel = new Character(
                 "Ocular Sentinel",
                 "The Ocular Sentinel is a long, sinuous creature of pulsating mechanical components. " +
                 "At one end of the sentinel is a single, large, unblinking eye that constantly scans its surroundings with a menacing red glow. " +
                 "The sentry itself moves with a disturbing, serpentine grace, and is capable of lashing out with surprising speed and precision. " +
                 "The probe is covered in a mix of organic tendrils and metallic wires, making it both resilient and flexible. " +
                 "It emits a low, droning hum, adding to its eerie presence.",
                 Kindred.Mecharion,
                 CreatureType.Construct,
                 6,  // strength
                 10, // dexterity
                 9, // constitution
                 12, // intelligence
                 10, // wisdom
                 8,  // charisma
                 "The Ocular Sentinel lies in a twisted heap, its pulsating mechanical components now motionless. " +
                 "The single, large eye at one end of the tube is dim and lifeless, no longer scanning its surroundings. " +
                 "The tube, once moving with serpentine grace, is now still, its organic tendrils and metallic wires lying limp. " +
                 "The low, droning hum that once emanated from it has ceased."
            );

            ocularSentinel.UniqueName = true;
            ocularSentinel.HP = 2;
            ocularSentinel.AddFeat(FeatKey.MeleeAttack);
            ocularSentinel.DefaultHand = new Tendril();
            ocularSentinel.Faction = Program.WorldSoul.GetFaction(FactionKey.TechnomancersDefenceSystem);
            ocularSentinel.IsInfluencer = false;

            Action<Soul> releaseSentry = (s) =>
            {
                s.Character.BroadcastToSoulsInRoom(
                $"As {s.Character.Name} struggles with the umbilical tube, an alarm sounds and a panel in the wall slides open. " +
                $"From the darkness, a {ocularSentinel.Name} emerges, its single, red eye locking onto {s.Character.Name}. " +
                "The living tube moves with a disturbing, serpentine grace, ready to attack.");
                ocularSentinel.GoToRoom(incubator);
                ocularSentinel.MoveToGroup(s.Character);

                incubator.AddOnAfterCombatEvent(() =>
                {
                    incubatorRelease(s);
                });
            };

            int tubeMoveUsed = 0;
            umbilicalTube.AddMove(new SkillCheck(
                null,
                "Attempt to remove the umbilical tube.",
                new SkillNumber(Skill.Intelligence, 13),
                false,
                async (s) =>
                {
                    s.Character.BroadcastToSoulsInRoom($"With a focused mind, {s.Character.Name} carefully examines the tube connected to their abdomen. " +
                    $"{s.Character.Name} notices the intricate mechanical elements, " +
                    "and with precise understanding, manages to disconnect it without causing harm. " +
                    "The tube detaches with a soft hiss, and they feel a slight release of pressure as it comes free.");
                    incubatorRelease(s);
                    return "";
                },
                async (s) =>
                {
                    s.Character.BroadcastToSoulsInRoom(
                        $"The attempts of {s.Character.Name} to understand the connection between the tube and their body are in vain. " +
                        $"The mechanical elements are too complex to decipher, " +
                        $"and {s.Character.Name} is unable to remove it.");

                    tubeMoveUsed++;
                    if (tubeMoveUsed == 1)
                        releaseSentry(s);
                    else if ((tubeMoveUsed == 2))
                        s.Character.SetEnableCombatWith(ocularSentinel);

                    return "";
                }
            ));
            umbilicalTube.AddMove(new SkillCheck(
                null,
                "Attempt to remove the umbilical tube by force.",
                new SkillNumber(Skill.Strength, 12),
                false,
                async (s) =>
                {
                    s.Character.BroadcastToSoulsInRoom(
                        $"{s.Character.Name} grips the tube firmly and yanks it away with a surge of brute strength. " +
                        "The connection resists at first, but sheer force overpowers it. " +
                        "The tube tears free with a violent jerk, and a brief sting of pain is felt before a rush of relief as it detaches. " +
                        "Blood trickles from the wound left behind.");
                    s.Character.TakeDamage(new Damage(new Roll(new Die(1, 1), 0, RollType.DamageRoll, s.Character), DamageType.None), umbilicalTube.Name); // Player takes damage regardless of success
                    incubatorRelease(s);
                    return "";
                },
                async (s) =>
                {
                    s.Character.BroadcastToSoulsInRoom(
                        $"{s.Character.Name} grasps the tube and pulls with all their might, but the connection holds firm. " +
                        "The blend of organic and mechanical elements proves too resilient, " +
                        $"and the effort leaves {s.Character.Name} exhausted with the tube still firmly attached. " +
                        "Blood trickles from where the tube meets the skin.");
                    s.Character.TakeDamage(new Damage(new Roll(new Die(1, 1), 0, RollType.DamageRoll, s.Character), DamageType.None), umbilicalTube.Name);

                    tubeMoveUsed++;
                    if (tubeMoveUsed == 1)
                        releaseSentry(s);
                    else if ((tubeMoveUsed == 2))
                        s.Character.SetEnableCombatWith(ocularSentinel);

                    return "";
                }
            ));

            return incubator;
        }
    }
}
