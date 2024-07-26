using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld
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

            // Adding exit to the Enclosed Room
            Exit toCareTakersRoomFromNexusBridge = new Exit(
                "In the middle of the bridge, branching eastward", 
                "A staircase ascends, leading to a small, enclosed room., " +
                "perhaps a place where someone oversees the machinery?",
                caretakerRoom);
            nexusBridge.AddExit(toCareTakersRoomFromNexusBridge);

            toCareTakersRoomFromNexusBridge.AddOnAfterMoveToEvent((Soul Soul) =>
            {
                _ = Soul.SendAsync(
                    "A biomechanical rat scurries out from the enclosed room, " +
                    "its metallic spine glinting as it vanishes into the shadows."); // the caretakers rat
                Soul.Character.CurrentRoom.BroadcastToSoulsInRoom("\"Aah Squee?! Where did you go now?\" " +
                    "a muffled, robotic-sounding voice emanates from the enclosed room.");
            }, 
            true);            

            // Back Room Description
            Room backRoom = new Room(
                Description(RoomKey.CaretakerBackRoom),
                "Caretaker's Backroom",
                "this is the back room"
            );

            Exit exitToBackRoom = new Exit(
                "to back room...",
                backRoom);

            // Adding exit to the Enclosed Room
            caretakerRoom.AddExit(exitToBackRoom);

            Character vexisTheCaretaker = new Character(
                "Vexis",
                "Vexis, a friendly yet eccentric figure. " +
                "Standing at five feet, his body is a fusion of rusted metal, gears, and organic tissue. " +
                "His limbs are equipped with various tools and gadgets, constantly buzzing with energy. " +
                "Vexis' face is an odd blend of amphibian and machine, featuring large, glowing green eyes and peculiar, protruding ears. " +
                "Despite his unsettling appearance, he exudes a warm, welcoming aura. " +
                "His voice, a distorted whisper with a hint of static, often carries a tone of curiosity and friendliness.",
                Race.Mecharion,
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
                (s) =>
                {
                    return
                    "From the look of Vexis' amphibian-like skin and protruding ears, you deduce that he was once a member of the Quorax. " +
                    "The Quorax are known for their subterranean habitat and acute hearing. " +
                    "Historically, the Quorax were nearly driven to extinction by the Mecharions in a brutal war over territory. " +
                    "The irony of finding a Quorax, transformed into a Mecharion caretaker, is not lost on you. " +
                    "This transformation symbolizes a twisted resolution of that ancient conflict.";
                },
                (s) =>
                {
                    return "You rack your brain but fail to recall any specific information about Vexis' origins. " +
                    "His strange appearance offer no clues to jog your memory.";
                }
            ));

            exitToBackRoom.OnBeforeExitEvent = (Soul soul) =>
            {
                if (exitToBackRoom.IsInGroupWith(vexisTheCaretaker) != true)
                    return false;

                _ = soul.SendAsync("Vexis is blocking the way...");
                return true;
            };
        }
    }
}
