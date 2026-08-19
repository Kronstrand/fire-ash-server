using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World.Goldfield;

internal class ForestCreekCrossing
{
    public static Room Create(bool initProps, Room forestEdge)
    {
        Room creekCrossing = new Room(
            RoomKey.ForestCreekCrossing,
            "Forest Creek Crossing",
            "A sluggish creek cuts through the trees like a ribbon of black ink. " +
            "A wooden bridge spans the water, its timbers blackened, slick, and rotting in perfect silence. " +
            "It feels less like a crossing and more like a place where someone stopped to think about turning back."
        );

        Item beneathBridge = new Item(
            "Beneath the Wooden Bridge",
            "Shadowed beneath the rotting timbers",
            "Thick wooden support beams sink deep into the black creek bed. " +
            "Silt, fallen leaves, and tangle-weed collect around the base of the timber, lost in quiet, cold shadow.",
            "creekCrossing.beneathBridge"
        );
        creekCrossing.AddItem(beneathBridge);

        Room overgrownPath = OvergrownForestPath.Create(initProps, creekCrossing);

        Exit toOvergrownPath = new Exit(
            "Across the wooden bridge",
            "The rotting planks of the bridge lead toward an overgrown forest path.",
            overgrownPath,
            "ForestCreekToOvergrownPath"
        );
        creekCrossing.AddExit(toOvergrownPath);

        Exit toForestEdge = new Exit(
            "Where the forest loosens", 
            "The edge of the forest appears.", 
            forestEdge,
            "ForestCreekToForestEdge"
            );

        creekCrossing.AddExit(toForestEdge);

        Room sump = BlackSiltSump.Create(initProps, creekCrossing);

        Exit toSump = new Exit(
            "Downstream",
            "The trees choke out the light where the water slows into a dark sump.",
            sump,
            "ForestCreekToSump"
        );
        creekCrossing.AddExit(toSump);

        Room shallows = Shallows.Create(initProps, creekCrossing);

        Exit toShallows = new Exit(
            "Upstream",
            "A muddy path tracks north along the water. In the distance, the creek visibly widens and flattens into foggy shallows.",
            shallows,
            "ForestCreekToShallows"
        );
        creekCrossing.AddExit(toShallows);

        if (!initProps)
            return creekCrossing;

        return creekCrossing;
    }
}
