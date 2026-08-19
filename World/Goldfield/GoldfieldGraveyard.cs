using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldGraveyard
    {
        public static Room Create(bool initProps, Room goldfieldFarmland)
        {
            Room graveyard = new Room(
                RoomKey.GoldfieldGraveyard,
                "Graveyard",
                "Crooked gravestones lean from the earth at odd angles, many split or sunken deep into the mud. " +
                "Dead grass sways in the cold wind, and the air smells of damp soil and decay."
            );

            graveyard.Light = Light.Dim;

            Item gravestones = new Item(
                "Gravestones",
                "Scattered throughout the graveyard",
                "Most of the stones are weathered beyond recognition, their names eroded by rain and time. " +
                "A few still bear faint carvings of wheat sheaves and prayers to Lorath.",
                "goldfieldGraveyard.gravestones"
            );
            graveyard.AddItem(gravestones);

            //Farmland
            Exit toFarmland = new Exit(
                "Beyond the graves",
                "A muddy trail winds toward the farmlands.",
                goldfieldFarmland,
                "GoldfieldGraveyardToFarmland"
            );
            graveyard.AddExit(toFarmland);

            //Mousoleum
            Room mausoleum = GoldfieldMausoleum.Create(initProps, graveyard);

            Exit toMausoleum = new Exit(
            "Near the far edge of the graveyard",
            "The mausoleum stands with its iron gate hanging open.",
            mausoleum,
            "GoldfieldFarmlandToMausoleum"
            );
            graveyard.AddExit(toMausoleum);

            graveyard.CreateRespawningMonster(MonsterCreator.Skeleton, 3, 10, 1, gravestones);

            if (!initProps)
                return graveyard;

            return graveyard;
        }
    }
}