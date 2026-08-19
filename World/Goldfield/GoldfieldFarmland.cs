using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armoring;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldFarmland
    {
        public static Room Create(bool initProps, Room goldfieldSquare)
        {
            Room goldfieldFarmland = new Room(
                RoomKey.GoldfieldFarmland,
                "Goldfield Farmland",
                "The fields stretch toward the horizon, but the wheat is blackened and tangled. " +
                "Goldberry bushes twist unnaturally, their leaves yellowed and rotting. A faint stench of decay drifts on the wind."
            );
            goldfieldSquare.Light = Light.Bright;

            Item wheatPatch = new Item(
                "Wheat Patch",
                "Across the field",
                "Tall stalks of wheat rise unevenly, blackened and tangled in places, twisted by neglect and harsh seasons.",
                "goldfieldFarmland.wheatPatch"
            );
            goldfieldFarmland.AddItem(wheatPatch);

            Item goldberryBushInField = new Item(
                "Goldberry Bush",
                "Near the edge of the farmland",
                "A twisted bush with thick, tangled branches, its berries mottled and glistening with rot.",
                "goldfieldFarmland.goldberryBush"
            );
            goldberryBushInField.HoldsDescription = "tangled in the twisted branches of the";
            goldfieldFarmland.AddItem(goldberryBushInField);


            Item goldberry = new Item(
                "Wrinkled Goldberry",
                "A small golden berry, firm and wrinkled, formed by cruel seasons.",
                0.3
            );
            goldberryBushInField.AddItem((Item)goldberry.ShallowCopy());
            goldberryBushInField.AddItem((Item)goldberry.ShallowCopy());
            goldberryBushInField.AddItem((Item)goldberry.ShallowCopy());

            
            DateTime nextBerryTime = DateTime.UtcNow;

            goldberryBushInField.Update = () =>
            {
                if (DateTime.Now >= nextBerryTime)
                {
                    // Count current berries
                    int berryCount = goldberryBushInField.Items.Count(item => item.Name == "Wrinkled Goldberry");

                    if (berryCount < 3)
                    {
                        Item newBerry = (Item)goldberry.ShallowCopy();
                        goldberryBushInField.AddItem(newBerry);
                    }

                    // Schedule next check using the same Random instance
                    Random rand = new Random();
                    nextBerryTime = DateTime.UtcNow.AddMinutes(rand.Next(5, 16));
                }
            };

            Exit toSquareFromFarmland = new Exit(
                "A dirt path leads toward Goldfield, cobblestones faintly visible",
                "Beyond the path, the village square and its statue come into view.",
                goldfieldSquare,
                "GoldfieldFarmlandToSquare"
            );
            goldfieldFarmland.AddExit(toSquareFromFarmland);

            //cave
            Room cave = GoldfieldCave.Create(initProps, goldfieldFarmland);

            Exit toCave = new Exit(
                "Near a low mound of earth, a dark opening gapes like a wound in the ground",
                "The cave mouth is shadowed, smelling of damp stone and decay.",
                cave,
                "GoldfieldFarmlandToCave"
            );
            goldfieldFarmland.AddExit(toCave);

            //forest
            Room forestEdge = TwistedForestEdge.Create(initProps, goldfieldFarmland);

            Exit toForest = new Exit(
                "A twisted forest stretches ahead, dark and tangled",
                "A narrow path cuts through gnarled, clawing branches.",
                forestEdge,
                "GoldfieldFarmlandToForestEdge"
            );

            goldfieldFarmland.AddExit(toForest);

            //graveyard
            Room graveyard = GoldfieldGraveyard.Create(initProps, goldfieldFarmland);

            Exit toGraveyard = new Exit(
                "Beyond a broken stone fence",
                "A muddy trail leads toward an old graveyard swallowed by weeds.",
                graveyard,
                "GoldfieldFarmlandToGraveyard"
            );

            goldfieldFarmland.AddExit(toGraveyard);

            return goldfieldFarmland;
        }
    }
}