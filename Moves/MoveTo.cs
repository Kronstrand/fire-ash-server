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
        public MoveTo(Soul soul, Prop targetProp) : base("m", $"Move to {CreateName(targetProp)}", targetProp)
        {
            Range = RangeType.RangeSingleTarget;

            Action = async () =>
            {                
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} moves to {CreateName(targetProp)}.");
                soul.Character.MoveToGroup(targetProp);
                if (soul.Character.LookAt != targetProp)
                {
                    soul.Character.SetLookAt(targetProp);
                    await LookAt.LookAtAction(soul, targetProp);
                }

                targetProp.RunOnAfterMoveToEvents(soul);
            };
        }

        public static string CreateName(Prop prop)
        {
            if (prop is Exit)
                return ((Exit)prop).GoToRoom.Name + " Entrance";

            return prop.Name;
        }


    }
}
