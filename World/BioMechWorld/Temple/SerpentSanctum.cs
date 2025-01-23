using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class SerpentSanctum
    {
        public static Room Create(Room wellRoom)
        {
            Room serpentSanctum = new Room(
                "Serpent Sanctum",
                "The room is dominated by a massive serpentine sculpture that coils from the floor, arching upward to disappear into the ceiling. " +
                "Its head look down at the room with an unsettling presence, filling the chamber with an air of dark reverence. " +
                "At the base of the serpent sculpture lies an ominous altar adorned with ritualistic engravings, " +
                "surrounded by several large stone bowls that hint at past ceremonies." +
                "The walls are covered in carved reliefs depicting serpentine forms and unsettling rituals"
            );
            Exit wellRoomToSerpentSanctum = new Exit(
                "To the northwest",
                "Through a doorway, a small staircase descends into a grander hall. Above the archway, the words 'Serpent Sanctum' are etched.",
                serpentSanctum
            );
            wellRoom.AddExit(wellRoomToSerpentSanctum);

            Exit serpentSanctumToWellRoom = new Exit(
                "By the southestern wall",
                "A small set of stairs ascends through an arched opening in the wall to a chamber above.",
                wellRoom
            );

            Item serpentineAltar = new Item(
                "Serpent Altar",
                "At the center of the room",
                "A grand, ominous altar sits at the base of the giant serpent statue. " +
                "The altar is adorned with intricate engravings and ritualistic objects, its purpose lost to history but still emanating an aura of significance. " +
                "The texture of the stone is aged and rough, darkened by time and the reverence of those who once stood here."
            );
            serpentSanctum.AddItem(serpentineAltar);

            Item ritualBowls = new Item(
                "Ritual Bowls",
                "Placed around the altar",
                "Several large stone bowls are arranged around the altar, each one carved with symbols that match those on the walls. " +
                "They appear empty now, but traces of a dark residue cling to their surfaces, hinting at past ceremonial uses."
            );
            serpentSanctum.AddItem(ritualBowls);

            Item carvedWallReliefs = new Item(
                "Carved Wall Reliefs",
                "Encircling the room",
                "The walls of the sanctum are covered with carvings that depict serpentine forms intertwined with human figures. " +
                "The scenes are strange and unsettling, suggesting rituals of devotion, sacrifice, or perhaps something even more arcane. " +
                "The figures are faint due to the passage of time, but their story seems to still haunt the sanctum."
            );
            serpentSanctum.AddItem(carvedWallReliefs);

            ShadowDungeon.Create(serpentSanctum);

            return serpentSanctum;
        }
    }
}
