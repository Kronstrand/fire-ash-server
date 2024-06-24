using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class RoomChange : Move
    {
        public RoomChange(Soul soul, Room room) : base("e", CreateDescription(room), room, CreateAction(soul, room))
        {
        }

        private static string CreateDescription(Room room)
        {
            return $"Enter {room.Name}.";
        }

        private static Action CreateAction(Soul soul, Room goToRoom)
        {
            return async () =>
            {
                Room xRoom = soul.Character.CurrentRoom;
                await soul.MoveCharToRoomAndSendDescriptionAsync(goToRoom);

                if (soul.Character.InCombat)
                    xRoom.FlagCombatMightBeResolved();
            };
        }
    }
}
