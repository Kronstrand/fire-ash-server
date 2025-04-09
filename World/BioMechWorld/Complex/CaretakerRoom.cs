using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;
using static fire_ash_server.Helpers;
using static fire_ash_server.World.BioMechWorld.GlobalVariables;

namespace fire_ash_server.World.BioMechWorld.Complex
{
    internal class CaretakerRoom
    {
        public CaretakerRoom(Room nexusBridge)
        {
            // Enclosed Room Description
            Room caretakerRoom = new Room(
                Description(RoomKey.CaretakerRoom),
                "Enclosed Room",
                "This enclosed room is a chaotic workshop, a sanctuary of mechanical spare parts and tools scattered haphazardly across every surface. " +
                "The room is eerily quiet, the stillness only occasionally disturbed by distant echoes of mechanical hums and faint dripping sounds. " +
                "A desolate ambiance pervades the space, evoking a sense of abandonment and solitude."
            );
            caretakerRoom.Light = Light.Bright;

            // Adding exit to the Enclosed Room
            Exit toCareTakersRoomFromNexusBridge = new Exit(
                "In the middle of the bridge, branching eastward",
                "A staircase ascends, leading to a small, enclosed room.",
                caretakerRoom);
            nexusBridge.AddExit(toCareTakersRoomFromNexusBridge);

            toCareTakersRoomFromNexusBridge.AddOnAfterMoveToEvent((Soul) =>
            {
                _ = Soul.SendAsync(
                    "A biomechanical rat scurries out from the enclosed room, " +
                    "its metallic spine glinting as it vanishes into the shadows."); // the caretakers rat
                Soul.Character.CurrentRoom.BroadcastToSoulsInRoom("\"Aah Squee?! Where did you go now?\" " +
                    "a muffled, robotic-sounding voice emanates from the enclosed room.");

                return Task.FromResult(true);
            },
            true);

            Exit toNexusBridgeFromCaretakerRoom = new Exit(
                "A doorway opens into a sprawling industrial expanse",
                "Through the open doorway, you catch glimpses of the Nexus Bridge",
                nexusBridge);
            caretakerRoom.AddExit(toNexusBridgeFromCaretakerRoom);

            Club ironPipe = new Club(
                "Rustclad Pipe",
                "A battered iron pipe from Vexis' workshop, perfect for those who prefer their solutions simple and blunt.",
                1
            );
            nexusBridge.AddItem(ironPipe);
            ironPipe.MoveToGroup(toCareTakersRoomFromNexusBridge);

            Room backRoom = BackRoom.Create(caretakerRoom);

            Exit exitToBackRoom = new Exit(
                "A narrow passage leads into the dim confines of a back room.",
                backRoom
            );

            // Adding exit to the Enclosed Room
            caretakerRoom.AddExit(exitToBackRoom);

            ref Character vexisTheCaretaker = ref Program.GlobalVariables.vexisTheCaretaker;

            vexisTheCaretaker = new Character(
                "Vexis",
                "Vexis, a friendly yet eccentric figure. " +
                "Standing at five feet, his body is a fusion of rusted metal, gears, and organic tissue. " +
                "His limbs are equipped with various tools and gadgets, constantly buzzing with energy. " +
                "Vexis' face is an odd blend of amphibian and machine, featuring large, glowing green eyes and peculiar, protruding ears. " +
                "Despite his unsettling appearance, he exudes a warm, welcoming aura. " +
                "His voice, a distorted whisper with a hint of static, often carries a tone of curiosity and friendliness.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                14, //strength
                9, //dexterity
                13, //constitution
                14, //intelligence
                13, //wisdom
                8,  //charisma
                "Vexis lies motionless, his multi-functional limbs now still. " +
                "His glowing green eyes are dim, and his peculiar, protruding ears droop lifelessly. " +
                "The once-buzzing tools and gadgets are silent, leaving behind a sense of eerie calm. " +
                "Even in death, there's a lingering warmth to his form, a testament to his friendly nature."
            );
            vexisTheCaretaker.UniqueName = true;
            vexisTheCaretaker.HP = 30;
            vexisTheCaretaker.AddFeat(FeatKey.MeleeAttack);
            vexisTheCaretaker.GoToRoom(caretakerRoom);
            vexisTheCaretaker.MoveToGroup(exitToBackRoom);
            vexisTheCaretaker.SetFaction(FactionKey.Technomancers);

            vexisTheCaretaker.AddMove(new SkillCheck(
                null,
                "Recall lore about Vexis' origins.",
                new SkillNumber(Skill.History, 12),
                true,
                async (s) =>
                {
                    return
                    "From the look of Vexis' amphibian-like skin and protruding ears, you deduce that he was once a member of the Quorax. " +
                    "The Quorax are known for their subterranean habitat and acute hearing. " +
                    "Historically, the Quorax were nearly driven to extinction by the Mecharions in a brutal war over territory. " +
                    "The irony of finding a Quorax, transformed into a Mecharion caretaker, is not lost on you. " +
                    "This transformation symbolizes a twisted resolution of that ancient conflict.";
                },
                async (s) =>
                {
                    return "You rack your brain but fail to recall any specific information about Vexis' origins. " +
                    "His strange appearance offer no clues to jog your memory.";
                }
            ));

            DialogueNode vexisIntroNode = new DialogueNode(
                "Squee? Where have you scampered off to...? Ah, hello there! I don't think we've met."
);

            // 2. Who Are You?
            DialogueNode vexisWhoAreYouNode = new DialogueNode(
                "I'm Vexis, the caretaker around here. So you've just emerged from the Mother Machine, eh? " +
                "Everything must feel a bit off right now. Don't fret; you'll adjust in time."
            );

            // 3. Caretaker Duties
            DialogueNode vexisCaretakerDutiesNode = new DialogueNode(
                "Mostly tinkering and tending. If a valve's about to burst or a circuit's sparking, " +
                "someone's gotta fix it before it causes trouble. We've all got our roles to play."
            );

            // 4. Beyond the Back Room?
            DialogueNode vexisBeyondBackroomNode = new DialogueNode(
                "Oh, that? It's just... well, nothing a newcomer needs to concern themselves with. " +
                "Trust me, it's not a place you want to wander without permission."
            );

            // 5. Evasiveness / Why Not?
            DialogueNode vexisEvasivenessNode = new DialogueNode(
                "I'm not trying to be cryptic, but I have my orders. If you really want to know more, " +
                "you'll have to speak to Ezekiel. He decides who goes through and who doesn't. Sorry."
            );

            // 6. Ezekiel & Purists Node
            DialogueNode vexisEzekielPuristsNode = new DialogueNode(
                "Ezekiel's the guiding hand here, sharp mind and keen vision. " +
                "He's always pushing the boundaries of what flesh and steel can accomplish together. " +
                "Not everyone sees it that way, though. The Purists have barricaded us in, calling our existence a blight on nature. " +
                "They haven't broken through yet, but if they ever do... well, I'd rather not find out what happens next."
            );

            // 7. Goodbye
            DialogueNode vexisGoodbyeNode = new DialogueNode(
                "Alright, I won't keep you. If you see Squee, send him scurrying back my way. " +
                "Stay safe out there, friend."
            );

            DialogueNode vexisMotherMachineNode = new DialogueNode(
                "No, you've got it backward, friend. The Mother Machine created me, not the other way around. " +
                "The original design came from Ezekiel long ago. I'm just a caretaker, " +
                "helping keep it all running as intended."
            );

            DialogueNode vexisPuristsForewarningNode = new DialogueNode(
                "I'd rather not stir that hornet's nest. They despise everything the Mother-Machine stands for, " +
                "and everyone linked to it. If you really want the details, seek out Ezekiel. " +
                "He knows more than I'm comfortable sharing."
            );

            // Intro Node -> choices
            vexisIntroNode.AddChoice("Goodbye.", vexisGoodbyeNode);
            vexisIntroNode.AddChoice("What do you do here?", vexisCaretakerDutiesNode);
            vexisIntroNode.AddChoice("Who are you?", vexisWhoAreYouNode);

            // Who Are You? Node -> next possible choices
            vexisWhoAreYouNode.AddChoice("Alright, I'll be going.", vexisGoodbyeNode);
            vexisWhoAreYouNode.AddChoice("What's beyond that back room?", vexisBeyondBackroomNode);
            vexisWhoAreYouNode.AddChoice("Did you create the Mother-Machine?", vexisMotherMachineNode);
            vexisWhoAreYouNode.AddChoice("What do you do here?", vexisCaretakerDutiesNode);

            vexisMotherMachineNode.AddChoice("Alright, I'll talk to Ezekiel.", vexisGoodbyeNode);
            vexisMotherMachineNode.AddChoice("What's beyond that back room?", vexisBeyondBackroomNode);
            vexisMotherMachineNode.AddChoice("Tell me about Ezekiel.", vexisEzekielPuristsNode);

            // Caretaker Duties Node -> next possible choices
            vexisCaretakerDutiesNode.AddChoice("I will let you get back to it, then.", vexisGoodbyeNode);
            vexisCaretakerDutiesNode.AddChoice("What's beyond that back room?", vexisBeyondBackroomNode);


            // Beyond the Back Room? Node -> next choices
            vexisBeyondBackroomNode.AddChoice("Fine. Goodbye.", vexisGoodbyeNode);
            vexisBeyondBackroomNode.AddChoice("Why not let me see for myself?", vexisEvasivenessNode);


            // Evasiveness Node -> next
            vexisEvasivenessNode.AddChoice("Fine. Goodbye.", vexisGoodbyeNode);
            vexisEvasivenessNode.AddChoice("Tell me about Ezekiel.", vexisEzekielPuristsNode);


            // Ezekiel & Purists Node -> exit or end
            vexisEzekielPuristsNode.AddChoice("I see. I'll find Ezekiel then.", vexisGoodbyeNode);
            vexisEzekielPuristsNode.AddChoice("The Purists?", vexisPuristsForewarningNode);

            vexisEzekielPuristsNode.AddChoice("I see. I'll find Ezekiel then.", vexisGoodbyeNode);

            // Finally, assign this dialogue to Vexis
            vexisTheCaretaker.CreateDialogueManager(vexisIntroNode);

            exitToBackRoom.OnBeforeExitEvent = async (soul) =>
            {
                if (exitToBackRoom.IsInGroupWith(Program.GlobalVariables.vexisTheCaretaker) != true)
                    return false;

                _ = soul.SendAsync("Vexis is blocking the way...");
                return true;
            };

            ref DialogueNode vexisTemplePermissionNode = ref Program.GlobalVariables.vexisTemplePermissionNode;

            // player pass through
            vexisTemplePermissionNode = new DialogueNode(
                "Those eyes of yours have a purpose now, eh? " +
                "I'm guessing Ezekiel or Elara finally sent you my way. " +
                "I'll admit, I'm not keen on letting folks wander past this door, " +
                "but if they've given the all-clear, then who am I to stand in your path?"
            );
            //vexisTheCaretaker.CreateDialogueManager(vexisTemplePermissionNode);

            DialogueNode vexisTemplePermissionSerious = new DialogueNode(
                "So it's true. Well, I won't mince words: " +
                "That back room leads to a winding corridor, " +
                "and eventually down into depths best left alone if you ask me. " +
                "But it's not my place to refuse you anymore.");

            DialogueNode vexisTemplePermissionHumorous = new DialogueNode(
                "Ha! If you pull that off, I'll personally stitch your name into my next tune-up log. " +
                "Though I'd hate to see any slimy beast trying to fix a busted valve, " +
                "imagine the mess they'd leave!"
                );

            // Serious Choice
            vexisTemplePermissionNode.AddChoice(
                "Yes, they've asked me to go below. Will you let me pass?",
                vexisTemplePermissionSerious
            );
            // Humorous Choice
            vexisTemplePermissionNode.AddChoice(
                "Cheer up! Maybe we could train any lurking horrors to do caretaker chores for you?",
                vexisTemplePermissionHumorous
            );

            Action<DialogueManager> VexisGetOutOfTheWay = async (dm) =>
            {
                if (Program.GlobalVariables.vexisTheCaretaker.IsInGroupWith(exitToBackRoom) == true)
                {
                    MoveTo move = new MoveTo(Program.GlobalVariables.vexisTheCaretaker.Soul, toNexusBridgeFromCaretakerRoom);
                    await move.Action();
                }
            };
            vexisTemplePermissionSerious.OnAfterEvent = VexisGetOutOfTheWay;
            vexisTemplePermissionHumorous.OnAfterEvent = VexisGetOutOfTheWay;
        }
    }
}
