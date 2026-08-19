using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.Goldfield
{
    internal class RotBlightSteading
    {
        public static Room Create(bool initProps, Room shallows)
        {
            Room steading = new Room(
                RoomKey.RotBlightSteading,
                "The Rot-Blight Steading",
                "A fortified orc camp crouches along sluggish creek waters winding through mire, " +
                "built out of woven brambles and hides with a grim sort of domesticity. " +
                "Massive clay cauldrons sit over smoldering fires along the bank beyond the camp's walls, " +
                "sending up thick, choking plumes that mingle with damp mist rising from the dark waters. " +
                "The air is blisteringly hot, heavy with a suffocating, almost sweet reek of things rotting far too fast.");
            steading.Light = Light.Dim;

            Item clayVats = new Item(
                "Simmering Clay Vats",
                "Perched along the edge of the creekbed",
                "Massive earthen cauldrons sitting over smoldering fires. Inside, a miserable stew of blackened wheat, withered berries, and unidentifiable carcasses bubbles away, overflowing directly into the creek. It's an efficient, ugly operation.",
                "rotBlightSteading.clayVats"
            );
            steading.AddItem(clayVats);

            Room pulpit = BoneStrewnPulpit.Create(initProps, steading);

            Exit toPulpit = new Exit(
                "Uphill",
                "A crude, steep path beaten into the ridge leads up toward a windy pulpit clattering with bone ornaments.",
                pulpit,
                "RotBlightSteadingToPulpit"
            );
            steading.AddExit(toPulpit);

            Exit toShallows = new Exit(
               "Downstream",
               "The sluggish water creeps north, spreading into a stretch of steaming, shallow mire.",
               shallows,
               "RotBlightSteadingToShallows"
);
            steading.AddExit(toShallows);

            Room hollowTree = HollowTree.Create(initProps, steading);

            Exit toHollowTree = new Exit(
                "Upstream",
                "Beyond the camp, sluggish waters vanish into the enormous hollow trunk of a fallen tree.",
                hollowTree,
                "RotBlightSteadingToHollowedTree"
            );
            toHollowTree.State.IsOpen = false;
            toHollowTree.State.VisableClosedDiscription = "The entrance to the hollowed tree is bared by a locked iron gate.";
            steading.AddExit(toHollowTree);

            if (!initProps)
                return steading;

            Character rotMinderThrum = new Character(
                "Thrum",
                "Thrum is a hulking, grey-green orc whose massive frame is covered by a stained, scorched leather apron. " +
                "Deep scars cross his bare chest, and thick, blunt tusks protrude from his lower jaw. He carries himself " +
                "with a profound, sluggish exhaustion, looking entirely unmoved by the world around him.",
                Kindred.Orc,
                CreatureType.Humanoid,
                14, 8, 13, 9, 10, 8,
                "The corpse of Thrum lies sprawled here, heavy and entirely still. His large, scarred hands are open " +
                "and empty. Even in death, his face holds a flat, frozen expression of mild inconvenience."
            );

            rotMinderThrum.UniqueName = true;
            rotMinderThrum.Title = "Rot Minder";
            rotMinderThrum.HP = 21;
            rotMinderThrum.AddFeat(FeatKey.MeleeAttack);
            rotMinderThrum.Faction = Program.WorldSoul.GetFaction(FactionKey.KettleKeepers);
            rotMinderThrum.SetDialogue(DialogueKey.RotMinder);
            rotMinderThrum.GoToRoom(steading);
            rotMinderThrum.MoveToGroup(clayVats);

            return steading;
        }
    }
}