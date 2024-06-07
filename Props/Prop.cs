using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props
{
    internal abstract class Prop
    {
        public string Name { get; set; }
        public string Description { get;  set; }
        public int FurtherDescriptionDC { get; set; }
        private bool pickupable;
        private bool hidden;
        public int HiddenDC { get; set; }

        public ThreadSafeList<Item> Items = new ThreadSafeList<Item>();
        public ThreadSafeList<Move> moves = new ThreadSafeList<Move>();

        public List<Effect> Effects = new List<Effect>();

        public Prop(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void RemoveAllEffects(EffectKey effectKey)
        {
            RemoveAllEffects(Description(effectKey));
        }
        public void RemoveAllEffects(string effectName)
        {
            Effects.RemoveAll(e => e.Name == effectName);
        }

        public bool HasHiddenItems()
        {
            return Items.Where(item => item.IsHidden()).Any();
        }

        public string GetDescription()
        {
            if (GetType() == typeof(Character))
            {
                Character character = (Character)this;
                if (character.Dead)
                    return character.DeathDescription;
            }
            return Description + Img(Name.ToLower().Replace(" ", ""));
        }

        public void AddItem(Item item)
        {
            item.ClearHeldBy();
            Items.Add(item);
            item.HeldBy = this;
        }

        public Prop AddMove(Move move)
        {
            moves.Add(move);
            return this;
        }

        public Prop Hide(int DC)
        {
            hidden = true;
            HiddenDC = DC;
            return this;
        }

        public Prop Unhide()
        {
            if (this is Character)
            {
                Character character = (Character)this;
                character.RemoveAllEffects(EffectKey.Stealth);
            }

            if (!hidden)
                return this;

            hidden = false;
            return this;
        }

        public bool IsHidden()
        {
            Prop prop = this;
            while (true)
            {
                if (prop.hidden)
                    return true;
                else if (prop is Item)
                {
                    Item item = (Item)prop;
                    if (item.HeldBy == null)
                        return false;
                    prop = item.HeldBy;
                }
                else if (prop is Exit)
                {
                    return prop.hidden;
                }
                else
                    return false;
            }
        }

        public Prop MakePickable()
        {
            pickupable = true;
            return this;
        }

        public Prop MakeUnpickupable()
        {
            pickupable = false;
            return this;
        }

        public bool IsPickupable() 
        {
            return pickupable;
        }


        public Prop ShallowCopy()
        {
            return (Prop)this.MemberwiseClone();
        }

        public List<Item> FoundItems(int result)
        {
            return Items.Where(item => item.IsHidden() && item.HiddenDC <= result).ToList();
        }

        public Prop? GetPropPosition()
        {
            if (this is Character)
            {
                return ((Character)this).CurrentRoom;
            }
            else if (this is Item)
            {
                return ((Item)this).HeldBy;
            }
            else if (this is Exit)
            {
                return ((Exit)this).LocatedInRoom;
            }
            return null;
        }

        public List<Character> GetCharactersLookingAt()
        {
            List<Character> lookingChars = new List<Character>();
            Room? room = GetRoomLocation();
            if (room == null)
                return lookingChars;
            
            foreach(Character character in room.Characters)
            {
                if (character.LookAt == this)
                {
                    lookingChars.Add(character);
                } 
            }
            return lookingChars;
        }

        public Room? GetRoomLocation()
        {
            Room? room = null;
            if (this is Room)
            {
                room = (Room)this;
            }
            if (this is Character)
            {
                room = ((Character)this).CurrentRoom;
            }
            else if (this is Item)
            {
                room = ((Item)this).LocatedInRoom();
            }
            else if (this is Exit)
            {
                room = ((Exit)this).LocatedInRoom;
            }
            return room;
        }

        public Room? GetEmidiateRoomLocation()
        {
            Room? room = null;
            if (this is Character)
            {
                room = ((Character)this).CurrentRoom;
            }
            else if (this is Item)
            {
                Item item = ((Item)this);
                if (item.HeldBy is Room)
                    return (Room)item.HeldBy;
            }
            else if (this is Exit)
            {
                room = ((Exit)this).LocatedInRoom;
            }
            return room;
        }

        public bool IsInRoomOrIsRoom(Room room)
        {
            if (this == room)
                return true;
            return GetRoomLocation() == room;
        }

        public bool MoveToGroup(Prop prop)
        {
            Room? room1 = GetEmidiateRoomLocation();
            if (room1 == null)
                return false;
            Room? room2 = prop.GetEmidiateRoomLocation();
            
            if (room2 == null || room1 != room2)
                return false;

            RemoveFromCurrentGrouping();

            foreach (Grouping group in room1.Groupings)
            {
                if (group.Characters.Contains(prop))
                {
                    group.Characters.Add(this);
                    return true;
                }
            }

            room1.Groupings.Add(
                new Grouping(this, prop));
            
            return true;
        }

        public void RemoveFromCurrentGrouping()
        {
            Room? room = GetRoomLocation();
            if (room == null) 
                return;

            for (int i = room.Groupings.Count - 1; i >= 0; i--)
            {
                Grouping group = room.Groupings.GetAt(i);
                if (group.Characters.Contains(this))
                {
                    group.Characters.Remove(this);
                    if (group.Characters.Count == 0)
                    {
                        room.Groupings.RemoveAt(i);
                    }
                    break;
                }
            }
        }

        public bool? IsInGroupWith(Prop prop)
        {
            Room? currentRoom = GetEmidiateRoomLocation();
            if (currentRoom == null)
                return null;

            foreach (Grouping grouping in currentRoom.Groupings)
            {
                if (grouping.Characters.Contains(this))
                {
                    if (grouping.Characters.Contains(prop))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
