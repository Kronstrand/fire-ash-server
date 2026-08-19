using
    System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;

namespace fire_ash_server
{
    internal class Flag
    {
        public FlagKey Type;
        public FactionKey FactionKey;
        public string RoomKey;
        public DateTime Time;
        public Flag() { }

        public Flag(FlagKey type, FactionKey faction, string roomKey)
        {
            Type = type;
            FactionKey = faction;
            RoomKey = roomKey;
            Time = DateTime.UtcNow;
        }
    }
}