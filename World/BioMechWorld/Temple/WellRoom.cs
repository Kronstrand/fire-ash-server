using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armoring;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class WellRoom
    {
        public static Room Create(Room grandTempleAscent)
        {
            Room wellRoom = new Room(
                "Sanctum Threshold",
                "At the center of the room lies an empty basin, a smooth, circular structure where water once flowed. " +
                "Now dry and worn with time, it serves as a silent relic of a forgotten past. " +
                "The stillness in the room is palpable, the air heavy with the weight of abandonment, " +
                "amplifying the quiet presence of the space.");

            Exit wellRoomToTempleAscent = new Exit(
                "To the south",
                "Through the entrance, a wide staircase begins its descent.",
                grandTempleAscent
            );
            wellRoom.AddExit(wellRoomToTempleAscent);

            Item emptyBasin = new Item(
                "Hollow Basin",
                "At the center of the room",
                "This empty basin, once filled with flowing water, is a smooth, circular structure worn by time. " +
                "Its surface is cracked and weathered, a remnant of its forgotten purpose.");
            wellRoom.AddItem(emptyBasin);

            Character giantSnake = MonsterCreator.GiantSnake();
            giantSnake.GoToRoom(wellRoom);
            giantSnake.MoveToGroup(emptyBasin);

            Armor wardensScales = ArmorList.WardensScales();
            wellRoom.AddItem(wardensScales);
            wardensScales.MoveToGroup(emptyBasin);

            SerpentSanctum.Create(wellRoom);
            AncientHallway.Create(wellRoom);

            return wellRoom;
        }
    }
}
