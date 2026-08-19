using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class TwistedForestEdge
    {
        public static Room Create(bool initProps, Room goldfieldFarmland)
        {
            Room forestEdge = new Room(
                RoomKey.TwistedForestEdge,
                "Twisted Forest Edge",
                "A narrow, barely discernible path winds through gnarled trees. " +
                "Branches twist unnaturally, their bark blackened and scarred, and thick underbrush threatens to snare the unwary. " +
                "The forest feels alive in its resistance, every movement measured against unseen dangers."
            );

            // Exit to Farmland
            Exit toFarmland = new Exit(
                "Where the twisted trees loosen",
                "The path opens onto ruined fields of blackened wheat and sagging goldberry bushes.",
                goldfieldFarmland,
                "TwistedForestEdgeToFarmland"
            );
            forestEdge.AddExit(toFarmland);

            //creek
            Room creekCrossing = ForestCreekCrossing.Create(initProps, forestEdge);

            Exit toCreek = new Exit(
                "Deeper into the forest", 
                "Through the tangled underbrush, a creek murmurs faintly.", 
                creekCrossing,
                "TwistedForestEdgeToCreek");
            forestEdge.AddExit(toCreek);

            if (!initProps)
                return forestEdge;

            return forestEdge;
        }
    }
}