using System;
using System.Net.Sockets;
using System.Net;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using fire_ash_server.World.BioMechWorld;
using System.Diagnostics.Metrics;
using fire_ash_server.Moves;
using System.Threading;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World
{
    [Serializable]
    internal class WorldSoul
    {
        public ThreadSafeList<Soul> Souls = new ThreadSafeList<Soul>();
        public ConcurrentDictionary<Thread, string> ThreadBufferText = new ConcurrentDictionary<Thread, string>();
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        public List<Faction> Factions = new List<Faction>();
        public List<Relationship> Relationships = new List<Relationship>();
        public List<Feat> Features = new List<Feat>();
        public BioMechCreator? World;

        public WorldSoul()
        {
            Room VoidRoom = new Room(Description(RoomKey.Void), "Void", "This is the Void.", true);
            AddRoom(VoidRoom);

            foreach (Enum factionKey in Enum.GetValues(typeof(FactionKey)))
            {
                Factions.Add(new Faction(Description(factionKey)));
            }
        }

        public Soul? GetSoul(Guid id)
        {
            foreach(Soul soul in Souls)
            {
                if (soul.Id == id)
                    return soul;
            }

            return null;
        }

        public Faction GetFaction(FactionKey key)
        {
            Faction? faction = GetFaction(Description(key));

            if (faction == null)
                throw new Exception($"FactionKey {Description(key)} has not been added to factions.");

            return faction;
        }

        public Faction? GetFaction(string name)
        {
            foreach (Faction faction in Factions) 
            {
                if (faction.Name == name)
                    return faction;
            }
            return null;
        }

        public void AddRoom(Room room)
        {
            Rooms.Add(room.RoomKey, room);
        }
        
        public Room GetRoom(RoomKey key)
        {
            return Rooms[Description(key)];
        }
    }

}
