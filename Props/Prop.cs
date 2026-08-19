using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(Item), "item")]
    [JsonDerivedType(typeof(Character), "character")]
    [JsonDerivedType(typeof(Exit), "exit")]
    [JsonDerivedType(typeof(Room), "room")]
    [JsonDerivedType(typeof(Inventory), "inventory")]
    internal abstract class Prop
    {
        [JsonIgnore]    public Action Update;
        [JsonInclude]   public string Id { get; set; }
        [JsonInclude]   public bool WorldProp {  get; set; }
        [JsonInclude]   public string Name { get; set; }
        [JsonInclude]   public string Description { get;  set; }
        [JsonInclude]   public string? ContextDescription;
        [JsonInclude]   public string HoldsDescription = "lying on the";
        [JsonInclude]   private bool pickupable;
        [JsonInclude]   private bool hidden;
        [JsonInclude]   public PropSubtype Subtype = PropSubtype.None;
        [JsonInclude]   public Light Light { private get; set; } = Light.None;
        [JsonInclude]   public bool DarknessOverride { get; set; } = false;
        [JsonInclude]   public bool DynamicDescription = false;
        [JsonInclude]   public PropState State { get; set; } = PropState.Default;
        [JsonInclude]   public bool Unreachable = false;
        [JsonInclude]   public int HiddenDC { get; set; }
        [JsonInclude]   public FactionKey? BelongsToFaction;
        [JsonInclude]   public List<ItemRespawn> ItemRespawns = new List<ItemRespawn>();

        [JsonIgnore]    public ThreadSafeList<Item> Items = new ThreadSafeList<Item>();
        [JsonPropertyName("Items")]
        [JsonInclude]   public List<Item> ItemsSerializable
                        {
                            get => Items.ToList();
                            set => Items = new ThreadSafeList<Item>(value);
                        }

        [JsonIgnore]    public ThreadSafeList<Effect> Effects = new ThreadSafeList<Effect>();
        [JsonPropertyName("Effects")]
        [JsonInclude]   public List<Effect> EffectsSerializable
                        {
                            get => Effects.ToList();
                            set => Effects = new ThreadSafeList<Effect>(value);
                        }

        [JsonIgnore]    public ThreadSafeList<Move> moves = new ThreadSafeList<Move>();
        [JsonIgnore]    public Action<Soul>? OnAfterLookAt;
        [JsonIgnore]    private ThreadSafeList<Func<Soul, Task<bool>>> OnBeforeMoveFromEvents = new ThreadSafeList<Func<Soul, Task<bool>>>();
        [JsonIgnore]    private ThreadSafeList<Func<Soul, Task<bool>>> OnBeforeMoveFromEventsToBeRemoved = new ThreadSafeList<Func<Soul, Task<bool>>>();
        
        [JsonIgnore]    private ThreadSafeList<EventKey> OnAfterPickUpEvents = new ThreadSafeList<EventKey>();
        [JsonPropertyName("OnAfterPickUpEvents")]
        [JsonInclude]   public List<EventKey> OnAfterPickUpEventsSerializable
        {
                            get => OnAfterPickUpEvents.ToList();
                            set => OnAfterPickUpEvents = new ThreadSafeList<EventKey>(value);
        }
        [JsonIgnore] private ThreadSafeList<EventKey> OnAfterPickUpEventsToBeRemoved = new ThreadSafeList<EventKey>();
        [JsonPropertyName("OnAfterPickUpEventsToBeRemoved")]
        [JsonInclude]
        public List<EventKey> OnAfterPickUpEventsToBeRemovedSerializable
        {
            get => OnAfterPickUpEventsToBeRemoved.ToList();
            set => OnAfterPickUpEventsToBeRemoved = new ThreadSafeList<EventKey>(value);
        }


        [JsonIgnore]    private ThreadSafeList<EventKey> OnAfterMoveToEvents = new ThreadSafeList<EventKey>();
        [JsonPropertyName("OnAfterMoveToEvents")]
        [JsonInclude]   public List<EventKey> OnAfterMoveToEventsSerializable
                        {
                            get => OnAfterMoveToEvents.ToList();
                            set => OnAfterMoveToEvents = new ThreadSafeList<EventKey>(value);
                        }



        [JsonIgnore] private ThreadSafeList<EventKey> OnAfterMoveToEventsToBeRemoved = new ThreadSafeList<EventKey>();
        [JsonPropertyName("OnAfterMoveToEventsToBeRemoved")]
        [JsonInclude]   public List<EventKey> OnAfterMoveToEventsToBeRemovedSerializable
                        {
                            get => OnAfterMoveToEventsToBeRemoved.ToList();
                            set => OnAfterMoveToEventsToBeRemoved = new ThreadSafeList<EventKey>(value);
                        }

        [JsonIgnore] public ThreadSafeList<Flag> Flags = new ThreadSafeList<Flag>();
        [JsonPropertyName("Flags")]
        [JsonInclude]
        public List<Flag> FlagsSerializable
        {
            get => Flags.ToList();
            set => Flags = new ThreadSafeList<Flag>(value);
        }

        [JsonIgnore]    public List<Prop> propsInImage = new List<Prop>();

        public Prop() { }

        public Prop(string name, string description, string id)
        {            
            Name = name;
            Description = description;
            Id = id;
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
            {
                description = "There is darkness.";
            }
            if (lookingCharacter.LookAt != this && ContextDescription != null)
                description = ContextDescription + ", " + ToLowerFirstChar(description);

            string exitImagePrefix = "";
            if (this is Character)
            {
                Character character = (Character)this;
                if (character.Description == "")
                {
                    Weapon mainHand = character.GetMainHand();
                    Weapon offHand = character.GetOffHand();
                    Weapon? ranged = character.GetRangedWeapon();

                    description = $"{character.Name}";

                    // Check if the character has any weapons equipped
                    bool hasMainHand = mainHand != character.DefaultHand;
                    bool hasOffHand = offHand != character.DefaultHand;
                    bool hasRanged = ranged != null;

                    if (hasMainHand || hasOffHand || hasRanged)
                    {
                        if (character.Dead)
                            description += $", a dead {character.GetType()}, equipped with";
                        else
                            description += $", a {character.Kindred}, equipped with";

                        // Handle main hand and off-hand weapon description
                        if (hasMainHand && hasOffHand)
                        {
                            description += $" {mainHand.Name} and {offHand.Name}";
                        }
                        else if (hasMainHand)
                        {
                            description += $" {mainHand.Name}";
                        }
                        else if (hasOffHand)
                        {
                            description += $" {offHand.Name} as off-hand";
                        }

                        // Handle ranged weapon description
                        if (ranged != null)
                        {
                            if (hasMainHand || hasOffHand)
                            {
                                description += $", along with {ranged.Name}";
                            }
                            else
                            {
                                description += $" {ranged.Name}";
                            }
                        }
                    }
                    else
                    {
                        // If no weapons are equipped, provide a generic fallback description
                        description += $", an unarmed {character.Kindred}";
                    }

                    if (character.Dead)
                        description += ", lies on the ground";
                    description += ".";

                    return description;
                }
                if (character.Dead)
                    return character.DeathDescription;
            }
            else if (this is Exit)
            {
                Exit exit = (Exit)this;

                if (lookingCharacter != null && lookingCharacter.LastRoom == exit.GoToRoom)
                    description = "Where you came from, " + ToLowerFirstChar(description);

                if (!exit.State.IsOpen && exit.State.VisableClosedDiscription != "")
                    description += $" {exit.State.VisableClosedDiscription}";

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

            //show only image if all props in image are ok
            foreach(Prop imageProp in propsInImage)
            {
                if (lookingCharacter != null && lookingCharacter.CurrentRoom != imageProp.GetRoomLocation())
                    return description;

                if (imageProp is Character)
                {
                    Character imageCharProp = (Character)imageProp;
                    if (imageCharProp.Dead)
                        return description;
                }
            }

            string Imagename = (exitImagePrefix + Name).ToLower().Replace(" ", "");

            return Img(Imagename) + description;
        }

        public void AddItem(Item item)
        {
            item.LastHeldBy = item.HeldBy;
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
            Prop copy = (Prop)MemberwiseClone();
            copy.Id = Guid.NewGuid().ToString();
            return copy;
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

        public async Task<bool> RunOnBeforeMoveFromEvents(Soul soul)
        {
            bool interruptMove = false;
            foreach (Func<Soul, Task<bool>> beforeMoveEvent in OnBeforeMoveFromEvents)
                if (await beforeMoveEvent(soul))
                    interruptMove = true;

            OnBeforeMoveFromEvents.RemoveAll(OnBeforeMoveFromEventsToBeRemoved);
            OnBeforeMoveFromEventsToBeRemoved.Clear();

            return interruptMove;
        }

        public void AddOnBeforeMoveFromEvent(Func<Soul, Task<bool>> action, bool runOnce)
        {
            OnBeforeMoveFromEvents.Add(action);
            if (runOnce)
                OnBeforeMoveFromEventsToBeRemoved.Add(action);
        }

        public async Task RunOnAfterPickUpEvents(Soul soul)
        {
            await Events.RunEvents(soul, this, OnAfterPickUpEvents, OnAfterPickUpEventsToBeRemoved);
            
            /*
            List<EventKey> pickUpEventsThatRanSuccessfuly = new List<EventKey>();
            foreach (EventKey eventKey in OnAfterPickUpEvents)
            {
                Func<Soul, Prop, Task<bool>>? pickUpEvent;
                string key = Description(eventKey);
                Events.events.TryGetValue(key, out pickUpEvent);
                if (pickUpEvent == null)
                    continue;
                if (await pickUpEvent(soul, this))
                    pickUpEventsThatRanSuccessfuly.Add(eventKey);
            }

            foreach (EventKey evnt in pickUpEventsThatRanSuccessfuly)
            {
                if (OnAfterPickUpEventsToBeRemoved.Contains(evnt))
                {
                    OnAfterPickUpEvents.Remove(evnt);
                    OnAfterPickUpEventsToBeRemoved.Remove(evnt);
                }
            }
            */
        }

        public async Task RunOnAfterMoveToEvents(Soul soul)
        {
            await Events.RunEvents(soul, this, OnAfterMoveToEvents, OnAfterMoveToEventsToBeRemoved);

            /*
            List<EventKey> afterMoveEventsThatRanSuccessfuly = new List<EventKey>();
            foreach (EventKey afterMoveEventKey in OnAfterMoveToEvents)
            {
                Func<Soul, Prop, Task<bool>>? moveToEvent;
                string key = Description(afterMoveEventKey);
                Events.events.TryGetValue(key, out moveToEvent);
                if (moveToEvent == null)
                    continue;
                if(await moveToEvent(soul,this))
                    afterMoveEventsThatRanSuccessfuly.Add(afterMoveEventKey);
            }

            foreach(EventKey evnt in afterMoveEventsThatRanSuccessfuly)
            {
                if (OnAfterMoveToEventsToBeRemoved.Contains(evnt))
                {
                    OnAfterMoveToEvents.Remove(evnt);
                    OnAfterMoveToEventsToBeRemoved.Remove(evnt);
                }
            }
            */
        }
        public void AddOnAfterPickUpEvent(EventKey key, bool runOnce)
        {
            OnAfterPickUpEvents.Add(key);
            if (runOnce)
                OnAfterPickUpEventsToBeRemoved.Add(key);
        }

        public void AddOnAfterMoveToEvent(EventKey key, bool runOnce)
        {
            OnAfterMoveToEvents.Add(key);
            if (runOnce)
                OnAfterMoveToEventsToBeRemoved.Add(key);
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

            Prop prop = this;

            //if prop is an item in an inventory, use the character holding it as prop for group
            if (this is Item)
            {
                Item item = (Item)this;
                if (item.HeldBy is Inventory)
                {
                    Inventory inventoryHoldingItem = (Inventory)item.HeldBy;
                    if (inventoryHoldingItem.HeldBy != null)
                        prop = inventoryHoldingItem.HeldBy; //this would be a character
                }
            }

            foreach (Grouping group in currentRoom.Groupings)
                if (group.Props.Contains(prop))
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

            if (this is Room)
            {
                Room room = (Room)this;
                foreach (Prop prop in room.GetPropsInRoom())
                {
                    Light propLight = prop.GetPropLight();
                    if (propLight > light)
                        light = propLight;
                    if (light == Light.Bright)
                        return light;
                }
            }

            return light;
        }

        public List<Effect> GetAllEfects()
        {
            if (this is Character)
                return ((Character)this).GetAllEffectsIncludingFeatsAndBuffs();

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
                    foreach (Effect effect in character.GetAllEffectsIncludingFeatsAndBuffs())
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

        public void AddFlag(Flag flag)
        {
            Flags.RemoveAll(f => 
                f.Type == flag.Type && 
                f.FactionKey == flag.FactionKey && 
                f.RoomKey == flag.RoomKey);                
            Flags.Add(flag);
        }

        public string ListItemsAsString(Character lookingCharacter)
        {
            List<string> outputStrings = new List<string>();
            string output = "";

            Prop lookingAtProp;
            if (lookingCharacter.LookAt != null)
                lookingAtProp = lookingCharacter.LookAt;
            else
                return "";

            if (lookingAtProp.GetLightState(lookingCharacter) != Light.Darkness)
            {
                List<Item> items = lookingAtProp.Items.Where(i => i.IsPickupable() && !i.IsHidden()).ToList();
                
                if (items.Count > 0)
                {
                    string verb = (items.Count == 1 && !items[0].IsPlural) ? "is" : "are";
                    outputStrings.Add($"{ListToString(items)} {verb} {lookingAtProp.HoldsDescription} {lookingAtProp.Name}");
                }
            }

            for (int i = 0; i < outputStrings.Count; i++)
            {
                output += outputStrings[i];
                if (i == outputStrings.Count - 1)
                    output += ".";
                else
                    output += "; ";
            }

            return output;
        }

        public Item? GetItemById(string id)
        {
            return Items.Where(i => i.Id == id).FirstOrDefault();
        }
    }
}
