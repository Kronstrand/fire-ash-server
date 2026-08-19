using fire_ash_server;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World;
using fire_ash_server.Moves;
using System.Runtime.CompilerServices;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldCave
    {
        public static Room Create(bool initProps, Room goldfieldFarmland)
        {
            Room goldfieldCave = new Room(
                RoomKey.GoldfieldCave,
                "Small Cave",
                "This shallow cave feels claimed. The air is cool and carries a musky scent, " +
                "and the ground shows signs of repeated use."
            );

            Item bonePile = new Item(
                "Pile of Bones",
                "Near the rear of the cave",
                "A jumble of old bones, some gnawed and broken, lies heaped against the wall, a sign of a predator's presence.",
                "goldfieldCave.bonePile"
            );
            bonePile.Light = Light.Darkness;
            bonePile.DynamicDescription = true;
            bonePile.DarknessOverride = true;
            goldfieldCave.AddItem(bonePile);

            Item denHollow = new Item(
                "Den Hollow",
                "Along one side of the cave",
                "A worn depression in the stone and earth, shaped by something that rests here often.",
                "goldfieldCave.denHollow"
            );
            denHollow.Light = Light.Darkness;
            denHollow.DynamicDescription = true;
            denHollow.DarknessOverride = true;
            goldfieldCave.AddItem(denHollow);

            Exit toFarmland = new Exit(
                "The cave entrance leads out toward the open fields",
                "Golden farmland stretches beyond the mouth of the cave.",
                goldfieldFarmland,
                "GoldfieldCaveToFarmland"
            );
            goldfieldCave.AddExit(toFarmland);

            goldfieldCave.CreateRespawningMonster(MonsterCreator.Wolf, 5, 17, 1, null);

            if (!initProps)
                return goldfieldCave;

            return goldfieldCave;
        }
    }
}