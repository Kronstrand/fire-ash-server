using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

internal class BoneStrewnPulpit
{
    public static Room Create(bool initProps, Room steading)
    {
        Room pulpit = new Room(
            RoomKey.BoneStrewnPulpit,
            "The Bone-Strewn Pulpit",
            "A windy, exposed ridge overlooking the creekbed below. Stolen human church pews, scarred by hatchets and caked in muck, have been dragged up here to form a crude perimeter against the wind. At the center sits a heavy stone altar piled high with damp parchment, bleeding ink, and jars of dark, pressurized sludge. The frantic clatter of bone ornaments cuts through the howling gale."
        );

        Item moldyLedgers = new Item(
            "Moldy Ledgers",
            "Scattered across the defaced altar",
            "Damp, waterlogged books bound in peeling hide. The pages are covered in manic, ink-smeared columns cataloging 'Sludge Density,' 'Sore Count,' and 'Quarterly Spore Yield.' Several margins contain frustrated notes about Thrum's lack of punctuality.",
            "boneStrewnPulpit.moldyLedgers"
        );
        pulpit.AddItem(moldyLedgers);

        // Connect back down to Thrum
        Exit toSteading = new Exit(
            "Downhill",
            "A steep, slick path of mud and loose shale slides back down toward the steading's smoke plumes.",
            steading,
            "BoneStrewnPulpitToSteading"
        );
        pulpit.AddExit(toSteading);

        if (!initProps)
            return pulpit;

        Character shamanGorg = new Character(
            "Gorg",
            "Gorg is a wire-thin, twitchy orc whose eyes are wide with an intense, sleep-deprived panic. " +
            "He wears a tattered robe weighed down by dozens of heavy bone necklaces that clatter like wind chimes " +
            "every time he gestures frantically at his paperwork.",
            Kindred.Orc,
            CreatureType.Humanoid,
            10, 12, 11, 14, 13, 9,
            "The crumpled form of Gorg lies motionless. His bone necklaces are shattered, " +
            "and his ink-stained fingers are finally still. Even in death, his brow is frozen in a deeply stressed, " +
            "agonized furrow."
        );

        shamanGorg.UniqueName = true;
        shamanGorg.Title = "Blight Overseer";
        shamanGorg.HP = 16; // Less physical than Thrum, relies on mental stats
        shamanGorg.Faction = Program.WorldSoul.GetFaction(FactionKey.KettleKeepers);
        shamanGorg.SetDialogue(DialogueKey.Shaman);
        shamanGorg.GoToRoom(pulpit);
        shamanGorg.MoveToGroup(moldyLedgers);

        return pulpit;
    }
}