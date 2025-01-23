using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class GrandTempleAscent
    {
        public static Room Create(Room courtyard)
        {
            Room grandTempleAscent = new Room(
                "Grand Temple Ascent",
                "An imposing stone chamber, with towering columns lining the walls, each one etched with worn, " +
                "ancient symbols. The floor is uneven, with cracks revealing roots that have forced their way through the stone. " +
                "The air is thick with the scent of age and decay."
            );
            grandTempleAscent.Light = Light.Dim;

            // from courtyard
            Exit toTempleEntrance = new Exit(
                "Flanked by tall pillars",
                "An imposing entrance leads to an ancient temple.",
                grandTempleAscent
            );
            courtyard.AddExit(toTempleEntrance);

            grandTempleAscent.AddExit(
                new Exit(
                    "At the bottom of the stairs",
                    "Framed by vines and overgrown foliage, a weathered stone doorway, leading out of the temple to the courtyard.",
                    courtyard
            ));

            Room wellRoom =  WellRoom.Create(grandTempleAscent);

            Exit toMainTemple = new Exit(
                "In the center of the camber",
                "A grand staircase ascends toward towards the entrance to the main temple - a sanctum threashold, " +
                "its steps worn from centuries of passage.",
                wellRoom);
            grandTempleAscent.AddExit(toMainTemple);

            /*Character shadecreeper = MonsterCreator.Shadecreeper();
            shadecreeper.GoToRoom(grandTempleAscent);
            shadecreeper.MoveToGroup(toMainTemple);*/

            return grandTempleAscent;
        }
    }
}
