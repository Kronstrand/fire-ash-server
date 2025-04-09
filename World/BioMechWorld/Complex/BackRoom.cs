using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.World.BioMechWorld.Temple;

namespace fire_ash_server.World.BioMechWorld.Complex
{
    internal class BackRoom
    {
        public static Room Create(Room caretakerRoom)
        {
            Room backRoom = new Room(
                "Back Room",
                "The room is a labyrinth of Vexis' peculiar creations, where bits of scrap metal and half-finished contraptions spill over every surface. " +
                "Floor-to-ceiling shelves dominate the walls, each crammed with mechanical oddities, softly glowing tubes, and ancient components stacked in chaotic harmony. " +
                "The hum of energy permeates the air, while the exposed wires and faintly pulsing organic growths suggest ongoing experiments. " +
                "This secluded workshop, clearly Vexis' personal domain, feels both alive and enigmatic. A place where his eccentric genius flourishes away from prying eyes."
            );

            Exit openDoorframeToCaretakerRoom = new Exit(
                "To the west",
                "Through a narrow passage, you catch glimpses of scattered tools and Vexis' unmistakable handiwork.",
                caretakerRoom
            );
            backRoom.AddExit(openDoorframeToCaretakerRoom);

            Item clutteredShelf = new Item(
                "Cluttered Shelves",
                "Lining the walls",
                "Tall, rickety shelves laden with a chaotic assortment of rusted gears, faintly glowing vials, and cryptic schematics. " +
                "Each shelf seems to defy gravity, the precariously balanced items threatening to topple with the slightest disturbance."
            );
            clutteredShelf.MakeUnpickupable()
        ; backRoom.AddItem(clutteredShelf);

            Item perpetualWireUntangler = new Item(
                "Perpetual Wire Untangler",
                "A compact, insectoid device with delicate, articulated arms designed to untangle the most hopelessly knotted wires. " +
                "Its chrome surface is worn from years of obsessive use, and etched into its side is the motto: 'Because a tidy workspace is a happy workspace.'",
                3
            );
            clutteredShelf.AddItem(perpetualWireUntangler);
            clutteredShelf.AddItem(ConsumableList.ARC2000());

            Room serpentsSpine = SerpentsSpine.Create(backRoom);

            Exit toSerpentsSpine = new Exit(
                "At the far end",
                "A small, rusted door, unremarkable and easy to miss.",
                serpentsSpine
            );
            backRoom.AddExit(toSerpentsSpine);

            return backRoom;
        }
    }
}
