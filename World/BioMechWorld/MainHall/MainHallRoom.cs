using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld.MainHall
{
    internal class MainHallRoom
    {
        public MainHallRoom(Room bazar)
        {
            string song = @"
            In the halls where shadows creep,
            Silent gears in darkness weep.
            Flesh and steel, a mighty dance,
            In the void, our power's trance.

            Cold as iron, hearts of chrome,
            Rulers of the Mecharion home.
            Crafted strong, we rise anew,
            Mecharions, proud and true.

            Wires pulse with power's might,
            Through the endless, silent night.
            With iron will, our hearts ignite,
            Our anthem's flame, eternal light.

            Cold as iron, hearts of chrome,
            Rulers of the Mecharion home.
            Crafted strong, we rise anew,
            Mecharions, proud and true.

            Binary souls, our destiny,
            Mechanized supremacy.
            Whispers of our past persist,
            In realms of iron mist.

            Cold as iron, hearts of chrome,
            Rulers of the Mecharion home.
            Crafted strong, we rise anew,
            Mecharions, proud and true.

            In the end, all flesh must yield,
            To the anthem of the Mecharion field.
            Forever bound, in gears and bone,
            This proud lament, eternal throne.
            ";

            // Main Hall Description
            Room mainHall = new Room(
                Description(RoomKey.MainHall),
                "Heart of the Nexus",
                "A sense of awe overwhelms you. Towering columns rise towards a vaulted ceiling obscured by shadows. " +
                "The air is thick with animated chatter and the occasional clink of mechanical parts. " +
                "Intricate carvings depicting ancient Mecharion lore adorn the walls and columns, telling tales of a civilization that marries the mechanical with the organic in a dark, eerie harmony. " +
                "The hall is a social hub where the Mecharions gather to exchange stories. " +
                "At the far end of the hall, a raised stage stands ready for speakers to address the assembly or for occasional performances to entertain the Mecharions. " +
                "The atmosphere is heavy with an undercurrent of ancient wisdom, a place where the past and future converge in an unsettling, yet fascinating blend."
            );

            mainHall.OnEnterEvent = (soul) =>
            {
                mainHall.OnEnterEvent = null;
                mainHall.BroadcastToSoulsInRoom(song); // should be splitted up, maybe add timer.
            };

            mainHall.OnEnterEvent = (soul) =>
            {
                mainHall.OnEnterEvent = null;

                // Ezekiel's entrance
                string ezekielEntrance =
                "As you step into the hall, the Mecharions gather around a raised stage at the far end, " +
                "where a figure, known as Ezekiel, emerges from the shadows. " +
                "With a commanding presence, the Ezekiel strides purposefully to the center of the stage. " +
                "He raises his arms, and the room falls silent in anticipation.";

                // Introduction of the song
                string songIntroduction =
                "His voice, a blend of mechanical resonance and commanding presence, fills the hall: \"Brothers and sisters, in this time of great peril, we gather to fortify our spirits and remember our strength. " +
                "First, let us honor our ancestors and draw inspiration from their resilience. " +
                "Join me in the song of our lineage, The Lament of Steel and Bone, as we prepare our souls to face the Purist threat.\" " +
                "With a fluid motion, he signals the start, and the hall resonates with the harmonious and haunting melody sung by the Mecharions.";

                // Ezekiel's speech about the situation
                string ezekielSpeech =
                "As the final notes of the song fade into the silence, Ezekiel steps forward once more. " +
                "\"My fellow Mecharions\", he continues, his voice solemn, \"we face a grave threat. The Purists have breached our outer defenses and now press against our barricades. " +
                "But we are not defeated. Even now, preparations for a final frontal assault are underway. " +
                "We will meet them head-on and show them the strength and resolve of the Mecharions. Stand ready, for our moment of reckoning approaches.\" " +
                "A wave of fervent applause and triumphant cheers erupts from the audience, their determination and resolve echoing through the hall.";

                // Broadcast the event to souls in the room
                mainHall.BroadcastToSoulsInRoom(ezekielEntrance + "\n\n" + songIntroduction + "\n\n" + song + "\n\n" + ezekielSpeech);
            };

            // bazar -> nexus
            Exit mainHallExit = new Exit(
                "To the east",
                "At the end of the bazaar, a larger, more central area of the facility reveals itself.",
                mainHall
            );
            bazar.AddExit(mainHallExit);

            // mainhall -> bazar
            mainHall.AddExit(new Exit(
                "To the west",
                "An open pathway leading to a bustling bazar filled with stalls and vendors.",
                bazar
                ));

            Room corridor2A = new Room(
                Description(RoomKey.Corridor2A),
                "Corridor 2A",
                "A narrow, dimly lit corridor stretches out before you. " +
                "The walls are lined with conduits and hissing steam pipes, " +
                "creating a claustrophobic atmosphere. "
                );

            Exit reinforcedSteelDoor = new Exit(
                "In the northeast corner",
                "A massive, reinforced steel door stands, secured with an intricate system of hydraulic braces and clamps.",
                corridor2A);
            mainHall.AddExit(reinforcedSteelDoor);

            // Define the stage item
            Item stage = new Item(
                "Stage",
                "At the far end of the hall",
                "A raised platform presenting itself as a stage " +
                "made of polished metal, " +
                "adorned with bio-organic patterns. " +
                "It serves as a focal point for gatherings, speeches, and performances."
            );

            // Add the stage item to the Main Hall
            mainHall.AddItem(stage);

            //Characters
            Character ezekielTheMechanomancer = Ezekiel.Create();
            ezekielTheMechanomancer.GoToRoom(mainHall);
            ezekielTheMechanomancer.MoveToGroup(stage);

            Character elaraTheDefender = Elara.Create();
            elaraTheDefender.GoToRoom(mainHall);
            elaraTheDefender.MoveToGroup(stage);
        }
    }
}
