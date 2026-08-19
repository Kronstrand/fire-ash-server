using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class HollowTree
    {
        public static Room Create(bool initProps, Room steading)
        {
            Room hollowTreePassage = new Room(
                RoomKey.HollowTree,
                "The Hollowed Tree",
                "TODO" //this is where the water runs upwards from the land of the dead
            );
            return hollowTreePassage;

        }
    }
}