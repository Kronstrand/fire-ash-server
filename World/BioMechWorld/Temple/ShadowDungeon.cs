using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.Enums;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class ShadowDungeon
    {
        public static Room Create(Room serpentSanctum)
        {
            Room shadowDungeon = new Room(
                "Shadow Dungeon",
                "This dark and desolate dungeon is filled with a heavy sense of despair. The stone walls are covered with years of grime, and long, rusted chains hang from the ceiling and walls. " +
                "Faint echoes can be heard, perhaps the whispers of past captives. The air is thick and cold, carrying a metallic scent that suggests the room's sinister history. " +
                "A series of locked cells line the eastern wall, their iron bars twisted and corroded."
            );
            shadowDungeon.Light = Light.Darkness;

            Exit serpentSanctumToShadowDungeon = new Exit(
                "To the northwest",
                "A narrow, dark corridor leads deeper into a dark dungeon.",
                shadowDungeon
            );
            serpentSanctumToShadowDungeon.Hide(5);
            serpentSanctum.AddExit(serpentSanctumToShadowDungeon);            

            Exit shadowDungeonToSerpentSanctum = new Exit(
                "By the southeastern corner",
                "A narrow, dark corridor leads towards the serpent sanctum.",
                serpentSanctum
            );
            shadowDungeon.AddExit(shadowDungeonToSerpentSanctum);

            Item hangingChains = new Item(
                "Hanging Chains",
                "Suspended from the ceiling and walls",
                "Rusted iron chains hang ominously from above, their ends frayed or still clasped with rusted shackles. " +
                "They sway slightly, as if moved by an unseen force, and their clinking adds an eerie rhythm to the dungeon's oppressive silence."
            );
            shadowDungeon.AddItem(hangingChains);

            Item prisonCells = new Item(
                "Prison Cells",
                "Along the western wall",
                "A row of barred cells, each with a heavy iron door that appears nearly fused to the stone from rust. " +
                "The interiors reveal outlines of what might have been crude bedding or discarded belongings of long-forgotten prisoners."
            );
            shadowDungeon.AddItem(prisonCells);

            Item sacrificialNote = new Item(
                "Sacrificial Note",
                "A blood-stained note reads: " + "\n\n" +
                "The blade was raised, the chants echoing like a storm in my mind. I felt the world slip away, my vision dimming as the serpent's eyes bore into me. " +
                "But then-chaos. A crackling light erupted from the altar, and the priests screamed. Something they didn't foresee, something... broke their ritual.\n\n" +
                "I staggered back here, my body alive but hollow. The whispers-they haven't stopped, growing louder, closer, as if they know I shouldn't still be breathing. " +
                "This place... it's alive, watching, waiting for the next offering.\n\n" +
                "If you've come this far, turn back. Don't let it take you too. The Serpent hungers still, and its gaze does not forgive.\n\n" +
                "- Erynn, An Unfinished Sacrifice.",
                0.1
            );
            sacrificialNote.Hide(6);
            prisonCells.AddItem(sacrificialNote);

            return shadowDungeon;
        }
    }
}
