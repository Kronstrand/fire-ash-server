using System.Net.Mail;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armoring;
using fire_ash_server.World.BioMechWorld.Complex;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Formats.Asn1.AsnWriter;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class TempleOfLorath
    {

        /*
In the earliest age, the world was governed by Solthera, the sun god whose presence unified all natural cycles into a single harmonious rhythm. Over time, however, this unity came to be seen not as balance, but as constraint. A growing rebellion rejected Solthera’s imposed harmony, breaking the rituals and structures that maintained global synchronization.

The result was not immediate collapse, but fragmentation. This moment is recorded as DS: the Death of Solthera — not the destruction of a god, but the loss of shared alignment across the world.

In the centuries that followed, local systems emerged to restore stability in isolated regions. These worked for a time, creating a fragmented but functional world. Yet without global synchronization, these systems slowly drifted apart.

Now the world persists, but no longer in agreement with itself. Growth, decay, and predation still exist — but no longer in rhythm.
         */
        public static Room Create(bool initProps, Room goldfieldSquare)
        {
            // --- Temple of Lorath (Main Hall) ---
            Room templeOfLorath = new Room(
                RoomKey.TempleOfLorath,
                "Temple Hall",
                "A stone altar dominates the center of the hall, cracked and darkened with age. " +
                "The surrounding walls are carved with fields and wheat, their details softened where stone has been worn down. " +
                "Benches sit along the edges of the room, slightly uneven on the flagstone floor."
            );

            Item altar = new Item(
                "Stone Altar",
                "At the center of the hall",
                "A heavy stone altar, darkened with age and cracked along its surface. Small grains of dried corn are caught in the fissures.",
                Ids.templeOfLorath_altar
            );
            templeOfLorath.AddItem(altar);

            Item benches = new Item(
                "Wooden Benches",
                "Along the sides of the hall",
                "Simple wooden benches, aged and warped, their surfaces softened by years of use and quiet attention.",
                "templeOfLorath.benches"
            );
            templeOfLorath.AddItem(benches);

            // --- Inner Chamber ---
            Room innerChamber = InnerChamber.Create(initProps, templeOfLorath);

            Exit toInnerChamber = new Exit(
                "A narrow passage leads beyond the altar into a quieter chamber", 
                "Cooler, dimmer air drifts from within.", 
                innerChamber,
                "TempleOfLorathToInnerChamber");
            templeOfLorath.AddExit(toInnerChamber);

            // --- Exit back to Goldfield Square ---
            Exit toSquare = new Exit(
                "The hall opens onto Goldfield Square",
                "The cracked cobblestones and the weathered statue come into view.",
                goldfieldSquare,
                "TempleOfLorathToSquare"
            );
            templeOfLorath.AddExit(toSquare);

            if (!initProps)
                return templeOfLorath;

            // Caretaker NPC
            Character caretaker = new Character(
                "Eldron",
                "A gaunt man in faded, patched robes, with a thin, slightly stooped frame and a lined, worn face. Steady, watchful eyes give him a quiet intensity.",
                Kindred.Human,
                CreatureType.Humanoid,
                9, 8, 10, 12, 14, 11,
                "Eldron lies motionless, robes rumpled and frame slack, eyes closed in quiet repose."
            );
            caretaker.UniqueName = true;
            caretaker.Title = "Servant of Lorath";
            caretaker.HP = 12;
            caretaker.AddFeat(FeatKey.MeleeAttack);
            caretaker.Faction = Program.WorldSoul.GetFaction(FactionKey.Goldfield);
            caretaker.GoToRoom(templeOfLorath);
            caretaker.MoveToGroup(altar);
            caretaker.SetDialogue(DialogueKey.CaretakerTempleOfLorath);
            caretaker.AddToInventory(new Coins(100,0));

            //Items
            Head wolfSkull = new Head(
                Names.WolfSkull, 
                "The skull of a wolf bearing the sigil of Lorath. " +
                "The ancient mark speaks of the bond between the wild and the cultivated, " +
                "between the untamed forest and the tended field.", 
                30);
            caretaker.AddToInventory(wolfSkull);
            caretaker.AddToInventory(ConsumableList.ResurrectionStone());

            string bookDescription =
                "From the Teachings of Lorath, as spoken in the Fields of First Harvest:\n\n" +

                "Do not mistake growth for blessing.\n" +
                "Do not mistake abundance for rightness.\n\n" +

                "The field is not given to excess, but to purpose.\n" +
                "And what grows without measure will turn against itself.\n\n" +

                "You are not commanded to create growth, but to honor it when it is ready.\n" +
                "For to act before the time is pride, and to delay beyond it is waste.\n\n" +

                "The harvest is sacred not because it is great, but because it is right.\n\n" +

                "Therefore be diligent.\n" +
                "Therefore be restrained.\n" +
                "Therefore be faithful in what you take and what you leave.\n\n" +

                "For Lorath does not bless the careless hand,\n" +
                "nor the field that is ignored,\n" +
                "nor the harvest that is forgotten.";


            Item book = new Item(Names.TheTeachingsofLorath, bookDescription, 30);
            book.IsPlural = true;           
            book.BelongsToFaction = FactionKey.Goldfield;
            book.AddOnAfterPickUpEvent(EventKey.PickUpBookOfLorath, false);
            book.AddOnAfterPickUpEvent(EventKey.TriggerStealFlag, false);
            altar.AddItem(book);

            return templeOfLorath;

        }
    }

    internal class InnerChamber
    {
        public static Room Create(bool initProps, Room templeOfLorath)
        {
            Room innerChamber = new Room(
                RoomKey.TempleOfLorathInnerChamber,
                "Inner Chamber",
                "This smaller chamber has a lower ceiling, carved beams still showing fine craftsmanship. " +
                "Faint frescoes depict rituals and offerings, dulled by dust but touched in spots by recent hands. " +
                "A central pedestal holds a ceremonial bowl, tarnished but polished in the areas often used by the devout. " +
                "The scent of old incense lingers faintly, a subtle reminder of devotion."
            );

            Item pedestal = new Item(
                "Ceremonial Pedestal",
                "At the center of the chamber",
                "A short pedestal holding a bronze bowl, darkened with age but smoothed in parts from repeated use.",
                "innerChamber.pedestal"
            );
            innerChamber.AddItem(pedestal);

            // --- Crypt ---
            Room crypt = Crypt.Create(initProps, innerChamber);

            Exit toCrypt = new Exit(
                "A trapdoor in the floor leads down into a shadowed crypt",
                "A faint chill and damp stone smell rises from below.",
                crypt,
                "TempleOfLorathInnerChamberToCrypt"
            );
            innerChamber.AddExit(toCrypt);

            Exit toTemple = new Exit(
                "A passage leads to the main hall",
                "The light and faint aroma of offerings drift back from the Temple of Lorath.",
                templeOfLorath,
                "TempleOfLorathInnerChamberToMainHall"
            );
            innerChamber.AddExit(toTemple);

            return innerChamber;
        }
    }

    internal class Crypt
    {
        public static Room Create(bool initProps, Room innerChamber)
        {
            Room crypt = new Room(
                RoomKey.TempleOfLorathShrineCrypt,
                "Crypt",
                "A quiet chamber beneath the temple, lined with stone sarcophagi. " +
                "Some ceremonial items lie broken, others carefully placed by hands still respectful. " +
                "The damp air carries a solemn stillness, a reminder that even here faith lingers."
            );

            Item sarcophagus = new Item(
                "Stone Sarcophagus",
                "Along the walls",
                "A stone sarcophagus, cracked in places but clearly crafted with care. " +
                "Some contain fragments of ceremonial items, as if the faithful still honor those entombed here.",
                "crypt.sarcophagus"
            );
            crypt.AddItem(sarcophagus);

            Exit toInnerChamber = new Exit(
                "A trapdoor leads back up to the Inner Chamber",
                "Faint light filters down from above.",
                innerChamber
            );
            crypt.AddExit(toInnerChamber);

            return crypt;
        }
    }
}
