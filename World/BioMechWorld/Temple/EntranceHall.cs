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
    internal class EntranceHall
    {
        public static Room Create(Room courtyard)
        {

            Room entranceHall = new Room(
                "Grand Temple Ascent",
                "An imposing stone chamber, with towering columns lining the walls, each one etched with worn, " +
                "ancient symbols. The floor is uneven, with cracks revealing roots that have forced their way through the stone. " +
                "The air is thick with the scent of age and decay, and the faint echo of dripping water can be heard in the distance. "
            );
            entranceHall.Light = Light.Dim;

            // from courtyard
            Exit toTempleEntrance = new Exit(
                "Flanked by tall pillars",
                "An imposing entrance leads to an ancient temple.",
                entranceHall
            );
            courtyard.AddExit(toTempleEntrance);

            //move image here:
            entranceHall.AddExit(new Exit(
                "In the center of the camber",
                "A grand staircase ascends toward the main temple, " +
                "its steps worn from centuries of passage " +
                "leading further into the unknown depths.",
                entranceHall)); //fix

            entranceHall.AddExit(
                new Exit(
                    "At the bottom of the stairs",
                    "Framed by vines and overgrown foliage, a weathered stone doorway, leading out of the temple to the courtyard.",
                    courtyard
            ));

            Character shadecreeper = MonsterCreator.GiantSnake();
            shadecreeper.GoToRoom(entranceHall);
            shadecreeper.MoveToGroup(toTempleEntrance);

            return entranceHall;
        }
    }
}
