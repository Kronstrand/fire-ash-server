using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class BlackSiltSump
    {
        public static Room Create(bool initProps, Room forestCreekCrossing)
        {
            Room blackSiltSump = new Room(
                RoomKey.BlackSiltSump,
                "The Black-Silt Sump",
                "The creek gives up here, pooling into a wide, bubbling depression. " +
                "The water is thick and dark, moving with the heavy drag of liquid rot. " +
                "It laps lazily against banks of pale, ash-like silt. " +
                "A few gnarled roots poke out of the scum, looking tired."
            );

            blackSiltSump.Light = Light.Dim;

            Item ashenSilt = new Item(
                "Banks of Ashen Silt",
                "Lining the edges of the dark water",
                "Powdery and unnatural. It smells faintly of burnt metal, old grease, and neglect. " +
                "If you stepped in it, it would probably ruin your boots forever.",
                "blackSiltSump.ashenSilt"
            );
            blackSiltSump.AddItem(ashenSilt);

            // A piece of corrupted flora struggling in the sump
            Item tiredRoot = new Item(
                "Tired Root",
                "Jutting from the mud",
                "A blackened, swollen root that seems to be filtering the sluggish water. Thick, dark sap beads along its surface.",
                "blackSiltSump.tiredRoot"
            );
            blackSiltSump.AddItem(tiredRoot);

            // Exit heading back to the bridge
            Exit toCrossing = new Exit(
                "Upstream, where the silt thins out",
                "The blackened timbers of the wooden bridge are faintly visible through the dead trees.",
                forestCreekCrossing,
                "BlackSiltSumpToCrossing"
            );
            blackSiltSump.AddExit(toCrossing);

            if (!initProps)
                return blackSiltSump;

            return blackSiltSump;
        }
    }
}