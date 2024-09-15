using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items;
using fire_ash_server.World;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props
{
    internal abstract class Prop
    {
        public string Name { get; set; }
        public string Description { get;  set; }
        public string? ContextDescription;
        private bool pickupable;
        private bool hidden;
        public Light Light { private get; set; } = Light.None;
        public bool DarknessOverride { get; set; } = false;
        public bool DynamicDescription = false;
        public bool Unreachable = false;
        public int HiddenDC { get; set; }

        public ThreadSafeList<Item> Items = new ThreadSafeList<Item>();
        public ThreadSafeList<Move> moves = new ThreadSafeList<Move>();

        public ThreadSafeList<Effect> Effects = new ThreadSafeList<Effect>();

        private ThreadSafeList<Func<Soul, bool>> OnBeforeMoveFromEvents = new ThreadSafeList<Func<Soul, bool>>();
        private ThreadSafeList<Func<Soul, bool>> OnBeforeMoveFromEventsToBeRemoved = new ThreadSafeList<Func<Soul, bool>>();

        private ThreadSafeList<Action<Soul, Prop>> OnAfterMoveToEvents = new ThreadSafeList<Action<Soul, Prop>>();
        private ThreadSafeList<Action<Soul, Prop>> OnAfterMoveToEventsToBeRemoved = new ThreadSafeList<Action<Soul, Prop>>();

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
            for (int i = Effects.Count - 1; i >= 0; i--)
            {
                if (Effects.GetAt(i).Name == effectName)
                    Effects.RemoveAt(i);
            }
        }

        public bool HasHiddenProps()
        {
            if (Items.Where(item => item.IsHidden()).Any())
                return true;
            if (this is Room && ((Room)this).Exits.Where(item => item.IsHidden()).Any())
                return true;

            return false;
        }

        public string GetDescription(Character lookingCharacter)
        {
            return GetDescription(lookingCharacter, true);
        }

        public string GetDescription(Character lookingCharacter, bool showImage)
        {
            string description = Description;
            if (GetLightState(lookingCharacter) == Light.Darkness)
                description = "There is darkness.";
            if (lookingCharacter.LookAt != this && ContextDescription != null)
                description = ContextDescription + ", " + ToLowerFirstChar(description);

            string exitImagePrefix = "";
            if (this is Character)
            {
                Character character = (Character)this;
                if (character.Dead)
                    return character.DeathDescription;
            }
            else if (this is Exit)
            {
                Exit exit = (Exit)this;

                if (lookingCharacter != null && lookingCharacter.LastRoom == exit.GoToRoom)
                    description = "Where you came from, " + ToLowerFirstChar(description);

                if (exit.LocatedInRoom != null)
                    exitImagePrefix = exit.LocatedInRoom.Name;
            }
            else if (this is Room)
            {
                Room room = (Room)this;

                if (room.GetLightState(lookingCharacter) == Light.Darkness)
                    showImage = false;

                List<Item> dynamicItems = room.Items.Where(i => i.DynamicDescription).ToList();
                foreach (Item item in dynamicItems)
                {
                    description += " " + item.GetDescription(lookingCharacter, false);
                }
            }

            if (!showImage)
                return description;

            string Imagename = (exitImagePrefix + Name).ToLower().Replace(" ", "");

            return Img(Imagename) + description;
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
            return (Prop)MemberwiseClone();
        }

        public List<Prop> FoundItems(int result)
        {
            List<Prop> props = Items.Where(item => item.IsHidden() && item.HiddenDC <= result).Select(item => (Prop)item).ToList();
            if (this is Room)
            {
                List<Prop> exits = ((Room)this).Exits.Where(exit => exit.IsHidden() && exit.HiddenDC <= result).Select(item => (Prop)item).ToList();
                props.AddRange(exits);
            }
            return props;
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
                if (group.Props.Contains(prop))
                {
                    group.Props.Add(this);
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
                if (group.Props.Contains(this))
                {
                    group.Props.Remove(this);
                    if (group.Props.Count == 0)
                    {
                        room.Groupings.RemoveAt(i);
                    }
                    break;
                }
            }
        }

        public Prop? GetGroundLevelProp()
        {

            if (this is Room)
            {
                return null;
            }
            else if (this is Item)
            {
                Item item = (Item)this;
                if (item.HeldBy is Room)
                    return item;
                else if (item.HeldBy == null)
                    return null;
                return item.HeldBy.GetGroundLevelProp();
            }
            return this;
        }

        public bool? IsInGroupWith(Prop prop)
        {
            Prop? prop1 = this.GetGroundLevelProp();
            Prop? prop2 = prop.GetGroundLevelProp();

            if (prop1 == null || prop2 == null)
                return null;

            Room? currentRoom = GetRoomLocation();
            if (currentRoom == null)
                return null;

            foreach (Grouping grouping in currentRoom.Groupings)
            {
                if (grouping.Props.Contains(prop1))
                {
                    if (grouping.Props.Contains(prop2))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        public bool RunOnBeforeMoveFromEvents(Soul soul)
        {
            bool interruptMove = false;
            foreach (Func<Soul, bool> beforeMoveEvent in OnBeforeMoveFromEvents)
                if (beforeMoveEvent(soul))
                    interruptMove = true;

            OnBeforeMoveFromEvents.RemoveAll(OnBeforeMoveFromEventsToBeRemoved);
            OnBeforeMoveFromEventsToBeRemoved.Clear();

            return interruptMove;
        }

        public void AddOnBeforeMoveFromEvent(Func<Soul, bool> action, bool runOnce)
        {
            OnBeforeMoveFromEvents.Add(action);
            if (runOnce)
                OnBeforeMoveFromEventsToBeRemoved.Add(action);
        }

        public void RunOnAfterMoveToEvents(Soul soul)
        {
            foreach (Action<Soul,Prop> afterMoveEvent in OnAfterMoveToEvents)
                afterMoveEvent(soul, this);

            OnAfterMoveToEvents.RemoveAll(OnAfterMoveToEventsToBeRemoved);
            OnAfterMoveToEventsToBeRemoved.Clear();
        }

        public void AddOnAfterMoveToEvent(Action<Soul,Prop> action, bool runOnce)
        {
            OnAfterMoveToEvents.Add(action);
            if (runOnce)
                OnAfterMoveToEventsToBeRemoved.Add(action);
        }

        public string GetLightEffectedName(string preTextWithLigh, string preTextWithDarkness, Character? lookingCharacter)
        {
            return GetLightEffectedName(preTextWithLigh, preTextWithDarkness, false, lookingCharacter);
        }
        public string GetLightEffectedName(string preTextWithLigh, string preTextWithDarkness, bool excludeContext, Character? lookingCharacter)
        {
            if (DynamicDescription && GetLightState(lookingCharacter) == Light.Darkness)
            {
                if (ContextDescription == null) throw new Exception($"{Name} has no context description");
                string darkResult = preTextWithDarkness + "darkness";
                if (!excludeContext)
                    darkResult += $", {ToLowerFirstChar(ContextDescription)}";
                return darkResult;
            }
            else
                return preTextWithLigh + Name;
        }

        public Grouping? GetGrouping()
        {
            return GetGrouping(null);
        }
        public Grouping? GetGrouping(Room? currentRoom)
        {
            if (currentRoom == null)
            {
                currentRoom = GetRoomLocation();
                if (currentRoom == null)
                    return null;
            }

            foreach (Grouping group in currentRoom.Groupings)
                if (group.Props.Contains(this))
                    return group;

            return null;
        }

        public Light GetPropLight()
        {
            Light light = Light;

            foreach(Effect effect in GetAllEfects())
            {
                if (effect.LightRadiusModifer > light)
                    light = effect.LightRadiusModifer;
            }

            return light;
        }

        public List<Effect> GetAllEfects()
        {
            if (this is Character)
                return ((Character)this).GetAllEffectsIncludingFeats();

            return Effects.ToList();
        }

        public bool HasEffect(EffectKey effectKey)
        {
            List <Effect> effects = GetAllEfects();
            string effectName = Description(effectKey);
            return effects.Where(e => e.Name == effectName).Any();
        }
        public Light GetLightState(Character? characterLooking)
        {
            return GetLightState(characterLooking, true);
        }
        public Light GetLightState(Character? characterLooking, bool includeLightPointers)
        {
            Light currentLightSate = Light.Bright;
            if (characterLooking != null && characterLooking.HasEffect(EffectKey.Darkvision))
                return currentLightSate;
            
            //room ligh is base light
            Room? currentRoom = GetRoomLocation();
            if (currentRoom != null)
                currentLightSate = currentRoom.GetPropLight();

            if (this is Room)
            {
                Room thisRoom = (Room)this;

                if (thisRoom.Light == Light.Bright)
                    return thisRoom.Light;

                List<Prop> unprocecessedProps = thisRoom.Characters
                                                        .Concat<Prop>(thisRoom.Items)
                                                        .Concat(thisRoom.Exits)
                                                        .Where(p => !p.DarknessOverride)
                                                        .ToList();

                while (unprocecessedProps.Count > 0)
                {
                    Prop unprocecessedProp = unprocecessedProps[0];
                    unprocecessedProps.RemoveAt(0);

                    Light lightContenter = Light.None;
                    Grouping? grouping = unprocecessedProp.GetGrouping();
                    if (grouping != null)
                    {
                        lightContenter = grouping.GetLightState(null);

                        foreach (Prop groupedProp in grouping.Props)
                            unprocecessedProps.Remove(groupedProp);
                    }
                    else
                        lightContenter = unprocecessedProp.GetLightState(null);

                    if (lightContenter > currentLightSate)
                    {
                        currentLightSate = lightContenter;
                        if (currentLightSate == Light.Bright)
                            return currentLightSate;
                    }
                }
                return currentLightSate;
            }

            Grouping? group = GetGrouping(currentRoom);
            if (group != null)
            {
                //does any lightsource in the group darken it?
                foreach (Prop dakrProp in group.Props.Where(p => p.DarknessOverride))
                {
                    Light darkPropLight = dakrProp.GetPropLight();
                    if (darkPropLight < currentLightSate)
                        currentLightSate = darkPropLight;
                }
                if (currentLightSate == Light.Bright)
                    return currentLightSate;

                //does any lightsource in the group light it up?
                foreach (Prop prop in group.Props.Where(p => p.DarknessOverride == false))
                {
                    Light propLight = prop.GetPropLight();
                    if (propLight > currentLightSate)
                        currentLightSate = propLight;
                }
            }
            //prop is not in group
            else
            {
                Light propLight = GetPropLight();
                if (DarknessOverride)
                {
                    
                    if (propLight < currentLightSate)
                        currentLightSate = propLight;
                }
                else
                {
                    if (propLight > currentLightSate)
                        currentLightSate = propLight;
                }
            }

            if (currentLightSate == Light.Bright)
                return currentLightSate;

            if (!includeLightPointers)
                return currentLightSate;

            // add light from light pointers
            if (currentRoom == null)
                return currentLightSate;

            List<Character> charactersWithPointLight = currentRoom.Characters.Where(c => c.HasPointLight()).ToList();
            if (!charactersWithPointLight.Any())
                return currentLightSate;

            if (group != null)
            {
                foreach (Prop prop in group.Props)
                    currentLightSate = prop.AddLookedAtLightSource(currentLightSate, charactersWithPointLight);
            }
            else
                currentLightSate = AddLookedAtLightSource(currentLightSate, charactersWithPointLight);

            return currentLightSate;
        }

        private Light AddLookedAtLightSource(Light currentLightSate, List<Character> characters)
        {
            foreach (Character character in characters)
            {
                if (character.LookAt == this)
                    foreach (Effect effect in character.GetAllEffectsIncludingFeats())
                    {
                        if (effect.LightPointerModifer == Light.Bright)
                            return Light.Bright;
                        else if (effect.LightPointerModifer > currentLightSate)
                            currentLightSate = effect.LightPointerModifer;
                    }
            }
            return currentLightSate;
        }

        public bool IsHeldByDeadCharacter()
        {
            if (this is Item)
            {
                Item item = (Item)this;
                Character? heldByCharacter = item.HeldByCharacter();
                if (heldByCharacter != null && heldByCharacter.Dead)
                    return true;
            }
            return false;

        }
    }
}
