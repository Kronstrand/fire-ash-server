using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class OvergrownForestPath
    {
        public static Room Create(bool initProps, Room forestCreekCrossing)
        {
            Room forestPath = new Room(
                RoomKey.OvergrownForestPath,
                "Overgrown Forest Path",
                "Dense oak branches interweave overhead, casting a heavy, mossy gloom across the dirt trail. " +
                "The ground is soft and damp, packed down by old, forgotten footfalls. " +
                "Pale bark on the surrounding trunks bears deep, deliberate gashes, left long ago."
            );
            forestPath.Light = Light.Dim;

            Item carvedTrunks = new Item(
                "Scarred Oak Trunks",
                "Flanking the narrow trail",
                "Grooves cut deep into the wood have healed into pale, raised welts. " +
                "They form old directional blazes left by long-absent travelers.",
                "overgrownForestPath.carvedTrunks"
            );
            forestPath.AddItem(carvedTrunks);

            // Exit back to the creek crossing
            Exit toCreek = new Exit(
                "Where the trees thin out",
                "The dirt trail slants down toward the creek and its blackened wooden bridge.",
                forestCreekCrossing,
                "ForestPathToCreekCrossing"
            );
            forestPath.AddExit(toCreek);

            // Create and link the intermediate forest room
            Room deepwood = DeepwoodThistledown.Create(initProps, forestPath);

            Exit toDeepwood = new Exit(
                "Deeper into the dense woods",
                "The trees press closer together as the trail continues into the quiet woods.",
                deepwood,
                "ForestPathToDeepwood"
            );
            forestPath.AddExit(toDeepwood);

            if (!initProps)
                return forestPath;

            return forestPath;
        }
    }
}