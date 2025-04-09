using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;

namespace fire_ash_server.World.BioMechWorld.Complex
{
    internal class IndustrialStaircase
    {
        public static Room Create(Room nexusBridge)
        {
            // Define the industrial staircase as a room
            Room industrialStaircase = new Room(
                "Industrial Staircase",
                "Industrial Staircase",
                "A rusty, industrial staircase made of metal grates and pipes. The air is cooler here, with the distant hum of machinery reverberating through the structure. " +
                "Dim lights flicker intermittently, casting earie shadows on the metal steps."
            );

            // Staircase -> Nexus Bridge
            Exit staircaseToBridgeExit = new Exit(
                "At the bottom of the staircase",
                "An open metal door, leading to a bridge-like complex.",
                nexusBridge
            );
            industrialStaircase.AddExit(staircaseToBridgeExit);

            // Nexus Bridge -> Staircase
            Exit bridgeToStaircase = new Exit(
                "At the northern end of the bridge",
                "An open metal door, leading to a staircase going upwards.",
                industrialStaircase
            );
            nexusBridge.AddExit(bridgeToStaircase);

            BackAlley.Create(industrialStaircase);

            return industrialStaircase;
        }
    }
}
