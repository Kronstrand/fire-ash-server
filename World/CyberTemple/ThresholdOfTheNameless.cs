using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld.Complex
{
    internal class ThreshholdOfTheNameless
    {
        public static Room Create()
        {
            Room thresholdOfTheNameless = new Room(
                Description(RoomKey.ThresholdOfTheNameless),
                "Threshold of the Nameless",
                "A vast, circular chamber carved from deep obsidian, studded with scattered silver - veined stones that catch the faintest light. " +
                "Its open roof reveals a boundless void, where distant stars drift silently through endless darkness. " +
                "Shifting glyphs glow faintly on the tall, silent pillars that encircle the perimeter. " +
                "Incense coils through the charged air, thick with silence and unseen presence. " +
                "At the center stands the Veiled Altar, draped in starwoven cloth, motionless and waiting. " +
                "A pale, spectral light spills down from the boundless void above in slow, rhythmic waves. " +
                "This is the still center, where thought dissolves and the soul prepares to cross the Abyss."
            );

            Item veiledAltar = new Item(
                "Veiled Altar",
                "At the heart of the chamber",
                "A silent altar stands draped in heavy, starwoven cloth that shimmers with faint, shifting patterns. " +
                "Its surface is obscured, the contents hidden beneath the veil, inviting neither haste nor irreverence. " +
                "The air around it is unnaturally still, as if time itself hesitates here. " +
                "The base of the altar bears no inscription, only smooth stone worn by unseen hands and unknown rituals."
            );
            thresholdOfTheNameless.AddItem(veiledAltar);

            Item crypticCodex = new Item(
                "Cryptic Codex",
                "Resting upon the Veiled Altar",
                "The book reads: \"Within the silent void beyond form and name, the Nameless stirs, an eternal witness to all beginnings and ends. " +
                "Here, where thought fades and time unwinds beneath the shadow of the black star, the soul must relinquish the illusions of self, " +
                "to embrace the crimson chalice that offers boundless freedom and transformation. " +
                "Beware the watchful gaze of the ancient god who stands at the gate of endings, whose scythe severs what must be undone. " +
                "Only through surrender to the dance of death and desire, to the sacred mysteries veiled in shadow, " +
                "can the seeker cross the threshold where light and darkness intertwine, and be reborn beyond the Abyss.\""
            );
            veiledAltar.AddItem(crypticCodex);

            return thresholdOfTheNameless;
        }
    }
}