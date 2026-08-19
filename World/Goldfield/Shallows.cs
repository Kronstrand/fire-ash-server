using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class Shallows
    {
        public static Room Create(bool initProps, Room forestCreekCrossing)
        {
            Room shallows = new Room(
                RoomKey.Shallows,
                "The Shallows",
                "The water spreads into a wide stretch of steaming shallows where sluggish waters weave between patches of mire. " +
                "A row of splintered wooden stakes juts from the saturated ground, hung with sagging, unidentifiable hides and small, " +
                "ruined trinkets left behind like desperate bargains."
            );

            shallows.Light = Light.Dim;

            Item greasyShallows = new Item(
                "Greasy Shallows",
                "Swirling lazily near the muddy banks",
                "Thick ribbons of dark, stagnant scum drift aimlessly, " +
                "snaking around half-submerged bundles of bone and faded cloth. " +
                "Beneath the surface, strands of foul fungal growth sway back and forth like hair caught in a drain.",
                "shallows.greasyShallows"
            );
            shallows.AddItem(greasyShallows);

            // Exit heading south toward the bridge crossing
            Exit toCrossing = new Exit(
                "Following the bank downstream",
                "The muddy ground rises, leading toward the wooden bridge.",
                forestCreekCrossing,
                "ShallowsToForestCreekCrossing"
            );
            shallows.AddExit(toCrossing);

            Room steading = RotBlightSteading.Create(initProps, shallows);

            Exit toSteading = new Exit(
            "Upstream",
            "Smoke drifts across the water from massive clay cauldrons simmering along the bank, in front of looming timber walls.",
            steading,
            "ShallowsToRotBlightSteading"
        );
            shallows.AddExit(toSteading);

            if (!initProps)
                return shallows;

            return shallows;
        }
    }
}