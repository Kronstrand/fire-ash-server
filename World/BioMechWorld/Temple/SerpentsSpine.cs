using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Enums;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal static class SerpentsSpine
    {
        public static Room Create(Room backRoom)
        {
            Room SerpentsSpine = new Room(
                RoomKey.SerpentsSpine,
                "Serpent's Spine",
                "A colossal, serpentine bridge descends into a vast, shadowed cavern. " +
                "The slick, weathered stone is riddled with cracks and patches of lichen. " +
                "Faint light seeps through the rocky walls, casting eerie shadows that dance along the path. " +
                "Above, a massive, ribbed tube descends from the industrial complex, its surface pulsating with a faint, " +
                "mechanical rhythm as it snakes its way into the abyss below, connecting to something unseen.");

            Exit toBackRoom = new Exit(
                "At the top, within a small cave",
                "A rusted door leads to the Back Room.",
                backRoom
            );
            SerpentsSpine.AddExit(toBackRoom);

            BrokenBridge.Create(SerpentsSpine);



            return SerpentsSpine;
        }
    }
}
