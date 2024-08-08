using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.World.BioMechWorld.MainHall;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld
{
    internal class NexusBridge
    {
       public NexusBridge(Room creationChamber)
       {
            Room nexusBridge = new Room(
                Description(RoomKey.NexusBridge),
                "Industrial Expanse",
                "A sense of unease washes over you as you step into a vast, industrial expanse bathed in dim, flickering lights. " +
                "A central metal bridge spans the room, slick and worn with rusted railings. " +
                "Overhead, a dense network of pipes and cables pulse with an eerie, greenish glow. " +
                "Below, the room drops into shadow, filled with the faint hum of unseen machinery."
            );


            Exit toBridgeFromCreationChamber = new Exit(
                "An imposing steel doorframe marks the exit, its rigid structure contrasting with the room's lifeblood. Beyond, a central metal bridge stretches into shadow.",
                nexusBridge);
            toBridgeFromCreationChamber.AddOnAfterMoveToEvent(
                (Soul soul, Prop exit) => {
                    _ = soul.SendAsync("A cold breeze wafts in from beyond the door, carrying a hint of metallic chill.");
                }, false);
            creationChamber.AddExit(toBridgeFromCreationChamber);



            // Adding exit to the Creation Chamber
            nexusBridge.AddExit(new Exit(
                "To the south", 
                "A doorway leads to the Creation Chamber.",
                creationChamber));

            //new rooms
            new CaretakerRoom(nexusBridge);
            IndustrialStaircase.Create(nexusBridge);
        }
    }
}
