using System;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World.BioMechWorld;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World
{
    internal class WorldSoul
    {
        public Room? World;
        public ConcurrentDictionary<Thread, string> ThreadBufferText = new ConcurrentDictionary<Thread, string>();
        public ConcurrentDictionary<string, Character> SoulstonedCharacters = new ConcurrentDictionary<string, Character>();
        public Dictionary<string, Room> Rooms = new Dictionary<string, Room>();
        public List<Faction> Factions = new List<Faction>();
        public List<Relationship> Relationships = new List<Relationship>();
        public List<Feat> Features = new List<Feat>();
        public ThreadSafeList<Soul> Souls = new ThreadSafeList<Soul>();
        

        /*[JsonPropertyName("Souls")]
        public List<Soul> SoulsSerializable
        {
            get => Souls.ToList();
            set => Souls = new ThreadSafeList<Soul>(value);
        }*/

        //public BioMechCreator? World;
        //public CyberTempleCreator? World;

        public WorldSoul()
        {
            // JSON only — NO LOGIC
        }

        public void InitWorldSoul()
        {
            Room voidRoom = new Room(
                Description(RoomKey.Void),
                "Void",
                "This is the Void.",
                true
            );
            AddRoom(voidRoom);

            foreach (FactionKey factionKey in Enum.GetValues(typeof(FactionKey)))
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
            if (!Rooms.ContainsKey(room.RoomKey))
            {
                Rooms.Add(room.RoomKey, room);
            }
            else
            {
                Console.WriteLine($"Room {room.RoomKey} was already in world sould dictonary.");
            }

        }
        
        public Room GetRoom(RoomKey key)
        {
            return Rooms[Description(key)];
        }

        public Room GetRoom(string roomKeyString)
        {
            return Rooms[roomKeyString];
        }

        public string GetRoomKey(Room room)
        {
            return Rooms.FirstOrDefault(kvp => kvp.Value == room).Key;
        }

        public List<Item> GetAllItems()
        {
            List<Item> items = new();

            foreach (Room room in Rooms.Values)
            {
                CollectItemsRecursive(items, room);

                foreach (Character character in room.Characters)
                {
                    CollectItemsRecursive(items, character);
                    CollectItemsRecursive(items, character.Inventory);
                    foreach(Item equippedItem in character.EquippedItems.Values)
                    {
                        CollectItemsRecursive(items, equippedItem);
                    }
                }
            }

            return items;
        }

        private void CollectItemsRecursive(List<Item> items, Prop prop)
        {
            if (prop is Item)
                items.Add((Item)prop);

            foreach (Item item in prop.Items)
            {
                CollectItemsRecursive(items, item);
            }
        }
    }

}
