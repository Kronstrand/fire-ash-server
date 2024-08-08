using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;

namespace fire_ash_server.World.BioMechWorld
{
    internal class BackAlley
    {
        public static Room Create(Room industrialStaircase)
        {
            Room backAlley = new Room(
                "Back Alley",
                "Back Alley",
                "You find yourself in a narrow, shadowy alley. The air here is damp and thick with the scent of decay. " +
                "Grime covers the walls, and the ground is littered with discarded mechanical parts. " +
                "Flickering lanterns cast eerie shadows, making the alley appear even more foreboding. " +
                "Tangled wires hang overhead, occasionally sparking with electricity. " +
                "Faint sounds of machinery and distant voices echo through the alley, creating an unsettling atmosphere. " +
                "It feels like a place where secrets are kept and hidden dealings occur."
            );

            // staircase -> Back Alley
            Exit staircaseToAlley = new Exit(
                "At the top of the staircase",
                "A metal door leads to what appears to be some kind of underground back alley.",
                backAlley
            );
            industrialStaircase.AddExit(staircaseToAlley);


            // Back Alley -> Industrial Staircase
            Exit backAlleyToStaircaseExit = new Exit(
                "To the south",
                "A metal door, leading to an industrial staircase.",
                industrialStaircase
            );
            backAlley.AddExit(backAlleyToStaircaseExit);

            new Bazar(backAlley);

            return backAlley;
        }
    }
}
