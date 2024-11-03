using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Enums;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal static class SubterraneanPassage
    {
        public static Room Create(Room templeCourtyard)
        {
            // Define the new subterranean passage
            Room subterraneanPassage = new Room(
                "Subterranean Passage",
                "This narrow passage runs parallel beneath the temple courtyard. " +
                "The walls are carved from the same ancient stone, but the passage is rougher, " +
                "scarred by time. " +
                "The air is cool and carries the faint, earthy scent of moss and damp stone. " +
                "Muffled sounds from above occasionally drift down."
            );

            // Create exit from Subterranean Passage back to Temple Courtyard
            Exit toTempleCourtyardFromPassage = new Exit(
                "A small stone staircase ascends sharply to the temple courtyard.",
                templeCourtyard
            );
            subterraneanPassage.AddExit(toTempleCourtyardFromPassage);

            UndergroundStudy.Create(subterraneanPassage);

            return subterraneanPassage;
        }
    }
}