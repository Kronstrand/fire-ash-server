using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class UnStealth : Move
    {
        public UnStealth(Soul soul) : base ("sh", "Leave shadows.")
        {
            Character character = soul.Character;
            Action = () =>
            {
                soul.Character.Unhide();
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} steps out of the shadows...");
            };
        }

        public override bool IsValid(Soul soul)
        {
            return soul.Character.IsHidden();
        }
    }
}
