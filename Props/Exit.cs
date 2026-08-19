using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using fire_ash_server.Abstract_Entities;

namespace fire_ash_server.Props
{
    internal class Exit : Prop
    {
        [JsonIgnore]    public Room GoToRoom;
        [JsonInclude]   public string GoToRoomKey;
        [JsonIgnore]    public Room? LocatedInRoom;
        [JsonIgnore]    public Func<Soul, Task<bool>>? OnBeforeExitEvent;
        [JsonIgnore]    public ExitState State;

        public Exit() { }

        //to be deleted
        public Exit(string description, Room room) : base(CreateName(room), description, CreateName(room) + "-" + Guid.NewGuid().ToString())
        {
            WorldProp = true;
            GoToRoom = room;
            GoToRoomKey = room.RoomKey;
            State = new ExitState("PersistenceNotSupported");

        }
        //to be deleted
        public Exit(string context, string description, Room room) : base(CreateName(room), description, CreateName(room) + "-" + Guid.NewGuid().ToString())
        {
            WorldProp = true;
            GoToRoom = room;
            GoToRoomKey = room.RoomKey;
            ContextDescription = context;
            State = new ExitState("PersistenceNotSupported");
        }
        public Exit(string description, Room room, string id) : base(CreateName(room), description, id)
        {
            WorldProp = true;
            GoToRoom = room;
            GoToRoomKey = room.RoomKey;
            State = new ExitState(id);
        }
        public Exit(string context, string description, Room room, string id) : base(CreateName(room), description, id)
        {
            WorldProp = true;
            GoToRoom = room;
            GoToRoomKey = room.RoomKey;
            ContextDescription = context;
            State = new ExitState(id);
        }

        private static string CreateName(Room room)
        {
            return $"Entrance to {room.Name}";
        }
    }
}
