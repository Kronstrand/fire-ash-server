using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Weapons;
using System.Linq.Expressions;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class SmallRitualChamber
    {
        public static Room Create(Room ancientHall)
        {
            Room serpentSanctuary = new Room(
                "Small Ritual Chamber",
                "A small, foreboding chamber filled with an eerie stillness. A stone altar sits prominently in the center, worn and ancient, " +
                "bearing deep scratches and carvings that speak of forgotten rituals. The walls are rough, jagged rock. " +
                "The remains of several serpents lie coiled around the altar, their lifeless bodies marked with deep slashes and gashes, " +
                "as though they were sacrificed in a sinister and bloody rite."
            );

            Item altar = new Item(
                "Stone Altar",
                "In the center of the room",
                "A timeworn stone altar, pitted and cracked with age. It is adorned with ancient symbols, hinting at dark rituals performed long ago. " +
                "A cup-like offering bowl is placed atop the altar, empty but for dust and memories. Dark stains have seeped into the stone, possibly remnants of past sacrifices."
            );
            serpentSanctuary.AddItem(altar);

            Item crevice = new Item(
                "Crevice",
                "In the far back of the chamber",
                "A jagged crevice splits the wall, its dark opening twisting out of sight. " +
                "The uneven edges seem worn, as if smoothed by something passing in and out over time."
            );
            crevice.Light = Light.Darkness;
            crevice.MakeUnpickupable();
            crevice.DynamicDescription = true; //if the description is not part the room description and need to change, typically based on the light property
            crevice.DarknessOverride = true;
            serpentSanctuary.AddItem(crevice);

            Character serpentDevil = MonsterCreator.SerpentDevil();
            serpentDevil.GoToRoom(serpentSanctuary);
            serpentDevil.MoveToGroup(crevice);

            Item serpentsTear = new Item(
                "Serpent's Tear",
                "Within this crystalline droplet swirls a liquid of shifting hues, " +
                "from deep emerald to burnished gold. The Serpent's Tear is a fragment of ancient wisdom, " +
                "containing the foresight of an age where flesh and metal merge into a singular existence.",
                500);
            serpentDevil.AddToInventory(serpentsTear);

            Exit staircaseToAncientHall = new Exit(
                "Opposite the altar",
                "A steep stone staircase leads upwards, carved directly into the rock.",
                ancientHall
            );
            serpentSanctuary.AddExit(staircaseToAncientHall);

            Exit ancientHallToStaircase = new Exit(
                "To the north",
                "Through a carved hole in the cave wall, a staircase descends into a small ritual chamber, its rough stone steps worn and treacherous.",
                serpentSanctuary
            );
            ancientHall.AddExit(ancientHallToStaircase);

            return serpentSanctuary;
        }
    }
}
