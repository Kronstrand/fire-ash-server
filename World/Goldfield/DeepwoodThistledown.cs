using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class DeepwoodThistledown
    {
        public static Room Create(bool initProps, Room overgrownForestPath)
        {
            Room deepwood = new Room(
                RoomKey.DeepwoodThistledown,
                "Deepwood Thistledown",
                "Gnarled tree trunks crowd together beneath a heavy ceiling of leaves. " +
                "Choking patches of tall thistle and pale ferns border the dirt track, " +
                "their dry stalks rustling with every faint draft. " +
                "The air here is cool, still, and heavy."
            );

            deepwood.Light = Light.Dim;

            Item thistlePatches = new Item(
                "Tall Thistle Patches",
                "bordering the dirt track",
                "Sharp, pale green stems topped with faded, powdery burrs. " +
                "They grow in stubborn clusters that bound the clear dirt trail.",
                "deepwoodThistledown.thistlePatches"
            );
            deepwood.AddItem(thistlePatches);

            // Exit back toward the forest path
            Exit toForestPath = new Exit(
                "Along the thinner trees",
                "The thickets loosen slightly where the trail leads through lighter canopy.",
                overgrownForestPath,
                "DeepwoodToForestPath"
            );
            deepwood.AddExit(toForestPath);

            // Create and link the Grate Wall room
            Room grateWallRoom = GrateWall.Create(initProps, deepwood);

            // Exit heading toward the ancient structure
            Exit toGrateWall = new Exit(
                "Through the thickets",
                "Where the track begins to fade, a gap in the heavy branches reveals a glimpse of ancient stonework ahead.",
                grateWallRoom,
                "DeepwoodToGrateWall"
            );
            deepwood.AddExit(toGrateWall);

            if (!initProps)
                return deepwood;

            return deepwood;
        }
    }
}