using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class Say : Move
    {
        public Say(Soul soul, Character targetCharacter) : base(MoveKey.s.ToString(), $"Say")
        {
            Type = MoveType.MinorAction;
            Action = () =>
            {
                soul.Character.Speak(Payload);

                return Task.CompletedTask;
            };
        }
    }
}
