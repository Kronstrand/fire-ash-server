using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class MoveTo : Move
    {
        public MoveTo(Soul soul, Prop targetProp) : base("m", $"Move to {targetProp.Name}", targetProp)
        {
            Action = () =>
            {
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} moves towards {targetProp.Name}");
                soul.Character.MoveToGroup(targetProp);
            };
        }

    }
}
