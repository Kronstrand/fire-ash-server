using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Props
{
    internal class Exit : Prop
    {
        public Room GoToRoom;
        public Room? LocatedInRoom;
        public bool MinorExit;
        public Func<Soul, bool>? OnBeforeExitEvent;
        public Exit(string description, Room goToRoom) : base(CreateName(goToRoom), description)
        {
            GoToRoom = goToRoom;
        }
        public Exit(string context, string description, Room goToRoom) : base(CreateName(goToRoom), description)
        {
            GoToRoom = goToRoom;
            ContextDescription = context;
        }

        private static string CreateName(Room room)
        {
            return $"Entrance to {room.Name}";
        }
    }
}
