using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    [Serializable]
    internal class RoomChange : Move
    {
        public RoomChange(Soul soul, Exit exit) : base(MoveKey.r.ToString(), CreateDescription(exit.GoToRoom), exit.GoToRoom, CreateAction(soul, exit))
        {
            IsMovement = true;
        }

        private static string CreateDescription(Room room)
        {
            return $"Enter {room.Name}.";
        }

        private static Func<Task> CreateAction(Soul soul, Exit exit)
        {
            return async () =>
            {                
                if (exit.OnBeforeExitEvent != null)
                {
                    bool isHandled = await exit.OnBeforeExitEvent(soul);
                    if (isHandled)
                        return;
                }

                await soul.MoveCharToRoomAndSendDescriptionAsync(exit.GoToRoom);
            };
        }
    }
}
