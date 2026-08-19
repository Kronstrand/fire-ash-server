using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class GrateWall
    {
        public static Room Create(bool initProps, Room deepwood)
        {
            Room grateWallRoom = new Room(
                RoomKey.GrateWall,
                "The Grate Wall",
                "A towering wall of weathered stone rises out of the quiet shade, stretching high into the canopy overhead. " +
                "Its massive masonry forms an unbroken barrier that continues off in both directions as far as the eye can see. " +
                "Near the center, a stone archway is cut directly into the ancient wall."
            );

            grateWallRoom.Light = Light.Dim;

            // Exit heading back toward the deepwood thickets
            Exit toDeepwood = new Exit(
                "Toward the dense thickets",
                "The faint remnants of the dirt trail emerge toward thick woods.",
                deepwood,
                "GrateWallToDeepwood"
            );
            grateWallRoom.AddExit(toDeepwood);

            // Locked/closed exit framed within the archway
            Exit toBeyondWall = new Exit(
                "Through the stone archway",
                "A heavy metal grate is anchored firmly inside the passage.",
                grateWallRoom, // Destination set to itself until future content is connected
                "GrateWallToBeyond"
            );
            toBeyondWall.State.IsOpen = false;
            toBeyondWall.State.VisableClosedDiscription = "The gate is locked by thick iron bars.";
            grateWallRoom.AddExit(toBeyondWall);

            if (!initProps)
                return grateWallRoom;

            return grateWallRoom;
        }
    }
}