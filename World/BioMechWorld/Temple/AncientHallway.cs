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
    internal class AncientHallway
    {
        public static Room Create(Room wellRoom)
        {
            Room ancientHall = new Room(
                RoomKey.AncientHallway,
                "Ancient Hallway",
                "Massive stone columns rise from the ground and stand like ancient sentinels, dividing the hall. " +
                "The cave walls, visible between the columns, are rugged and uneven, adding to the sense of untamed depth in this underground space. " +
                "The air is cold and still, with a subtle hint of moisture, suggesting the depths of this underground domain."
            );

            Item stoneColumns = new Item(
                "Towering Columns",
                "Alongside the hall",
                "Massive stone columns covered in ancient carvings stand throughout the room. " +
                "Their surfaces are chipped, but they still evoke a sense of grandeur, as if bearing witness to a lost era."
            );
            ancientHall.AddItem(stoneColumns);

            Exit wellRoomToAncientHall = new Exit(
                "To the northeast",
                "A doorway leading into an ancient hall, its rough-hewn surfaces bearing the marks of time and neglect.",
                ancientHall
            );
            wellRoom.AddExit(wellRoomToAncientHall);

            Exit ancientHallToWellRoom = new Exit(
                "To the southwest",
                "The faint outline of a doorway is visible, framed by jagged, timeworn stone.",
                wellRoom
            );
            ancientHall.AddExit(ancientHallToWellRoom);

            Room zigzaggingStairway = ZigzaggingStairway.Create(ancientHall);
            SmallRitualChamber.Create(ancientHall);

            Exit ancientHallToZigzaggingStairway = new Exit(
                "To the northeast",
                "A gently ascending staircase bordered by arching stone pillars, " +
                "continuing upward into a cavernous tunnel.",
                zigzaggingStairway
            );
            ancientHall.AddExit(ancientHallToZigzaggingStairway);

            Character skel1 = MonsterCreator.Skeleton();
            skel1.GoToRoom(ancientHall);
            skel1.MoveToGroup(ancientHallToZigzaggingStairway);

            Character skel2 = MonsterCreator.Skeleton();
            skel2.GoToRoom(ancientHall);
            
            return ancientHall;
        }
    }
}
