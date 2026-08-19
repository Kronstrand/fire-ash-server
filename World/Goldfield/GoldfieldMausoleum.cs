using System.Threading;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using Microsoft.AspNetCore.SignalR;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldMausoleum
    {
        public static Room Create(bool initProps, Room graveyard)
        {
            Room mausoleum = new Room(
                RoomKey.GoldfieldMausoleum,
                "Mausoleum",
                "The air inside is cold and heavy with the scent of damp stone and rot. " +
                "Cracked burial alcoves line the walls, several broken open long ago. " +
                "Dust blankets the floor, disturbed only by scattered footprints and drag marks."
            );

            mausoleum.Light = Light.Darkness;

            Item sarcophagus = new Item(
                "Stone Sarcophagus",
                "At the center of the mausoleum",
                "An ancient stone sarcophagus rests upon a raised platform, its lid cracked slightly open.",
                "goldfieldMausoleum.sarcophagus"
            );

            mausoleum.AddItem(sarcophagus);

            Exit toGraveyard = new Exit(
                "Through the open iron gate",
                "Weak gray light spills from the graveyard outside.",
                graveyard,
                "GoldfieldMausoleumToGraveyard"
            );

            mausoleum.AddExit(toGraveyard);

            

            Character skeleton = MonsterCreator.Skeleton();
            skeleton.GoToRoom(mausoleum);
            skeleton.AddToInventory(ConsumableList.BookOfTwoWeapons());
            skeleton.AddFeat(FeatKey.DualWield);

            mausoleum.CreateRespawningMonster(MonsterCreator.Skeleton, 5, 30, 1, null);

            if (!initProps)
                return mausoleum;

            return mausoleum;
        }
    }
}