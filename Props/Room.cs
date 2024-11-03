using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.World;
using fire_ash_server.Moves;
using fire_ash_server.Abstract_Entities;
using static fire_ash_server.Helpers;
using fire_ash_server.Props.Items;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace fire_ash_server.Props
{
    internal class Room : Prop
    {
        public string RoomKey;
        public ThreadSafeList<Character> Characters = new ThreadSafeList<Character>();
        public ThreadSafeList<Grouping> Groupings = new ThreadSafeList<Grouping>();
        public ThreadSafeList<Relationship> RelationshipsInHostileCombat = new ThreadSafeList<Relationship>();
        public ThreadSafeList<Exit> Exits = new ThreadSafeList<Exit>();
        public Action<Soul>? OnEnterEvent;
        private List<Action> onCombatEndEvents = new List<Action>();
        private List<Action> onCombatEndEventsToBeRemoved = new List<Action>();
        public bool InCombat;
        private bool testCombatResolved;
        public bool AddCombatantsInCombatLoop;

        public Room(string roomKey,string name, string description) : base(name, description)
        {   
            RoomKey = roomKey;
            Light = Light.Bright;
            Program.WorldSoul.AddRoom(this);
        }

        public Room(RoomKey roomKey, string name, string description) : this(Description(roomKey), name, description)
        {
        }

        public Room(string name, string description) : this(name, name, description)
        {
        }

        public void AddExit(Exit exit)
        {
            Exits.Add(exit);
            exit.LocatedInRoom = this;
        }
        public string GetAdditionalRoomDescription(Character lookingCharacter)
        {
            string description = "";

            List<Exit> exitList = Exits.Where(e => !e.IsHidden()).ToList();

            for (int i = 0; i < exitList.Count; i++)
            {
                if (i != 0)
                    description += "; ";

                string exitDescription = exitList[i].GetDescription(lookingCharacter, false);

                if (i != exitList.Count - 1)
                    description += RemoveLastDot(exitDescription); //not last exit
                else
                    description += exitDescription; //last exit
            }
            
            string charactersAsString = ListPropsAsString(lookingCharacter);
            if (charactersAsString != "")
                description += "\n\n" + charactersAsString;

            string restOfItemsAsString = ListRestOfItemsAsString(lookingCharacter);
            if (restOfItemsAsString != "")
                description += "\n\n" + restOfItemsAsString;

            return description;
        }

        private string ListRestOfItemsAsString(Character lookingCharacter)
        {
            List<string> outputStrings = new List<string>();
            string output = "";
            string stringContext = "the ground";

            var groups = lookingCharacter.CurrentRoom.Groupings.Where(g => g.GetLightState(lookingCharacter) != Light.Darkness);
            foreach (Grouping group in groups)
            {
                bool characterFound = false;
                List<Prop> unpickupable = new List<Prop>();
                List<Item> itemsToPick = new List<Item>();
                foreach (Prop prop in group.Props.Where(p => !p.IsHidden()))
                {
                    if (prop is Character && prop != lookingCharacter)
                    {
                        characterFound = true;
                    }
                    else if (prop is Item)
                    {
                        if (prop.IsPickupable())
                            itemsToPick.Add((Item)prop);
                        else
                            unpickupable.Add(prop);
                    }
                    else if (prop is Exit)
                    {
                        unpickupable.Add(prop);
                    }
                }

                if (!characterFound && itemsToPick.Any())
                {
                    if (!unpickupable.Any())
                    {
                        if (itemsToPick.Count == 1)
                            outputStrings.Add($"{itemsToPick.First().Name} lies by itself on ${stringContext}");
                        else
                            outputStrings.Add($"In a pile on ${stringContext} lies " + ListToString(itemsToPick));
                    }
                    else
                    {
                        outputStrings.Add($"Besides {unpickupable.First().Name} lies " + ListToString(itemsToPick));
                    }
                }
            }
           

            if (lookingCharacter.CurrentRoom.GetLightState(lookingCharacter) != Light.Darkness)
            {
                List<Item> ungroupedItems = lookingCharacter.CurrentRoom.Items.Where(i => i.IsPickupable() && i.GetGrouping(lookingCharacter.CurrentRoom) == null && !i.IsHidden()).ToList();

                if (ungroupedItems.Count == 1)
                    outputStrings.Add($"{ungroupedItems.First().Name} lies by itself on ${stringContext}");
                else if (ungroupedItems.Count > 1)
                    outputStrings.Add($"{ListToString(ungroupedItems)} lies scattered on ${stringContext}");
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
        

            private string ListItemsAsString(Character lookingCharacter, Prop? groupProp)
        {
            Grouping? grouping = null;
            if (groupProp != null)
                grouping = groupProp.GetGrouping(lookingCharacter.CurrentRoom);

            List<Item> items = lookingCharacter.CurrentRoom.Items.Where(e => !e.IsHidden() && e.IsPickupable() && e.GetGrouping(lookingCharacter.CurrentRoom) == grouping).ToList();

            return ListToString(items);
        }

        private string ListPropsAsString(Character lookingCharacter)
        {
            List<Tuple<Prop?, List<Character>>> groupedCharactersByProp = new List<Tuple<Prop?, List<Character>>>();

            List<Character> allGroupedCharacters = new List<Character>();
 
            foreach (Grouping grouping in Groupings)
            {
                Prop? stageItem = null;
                List<Character> groupedCharacters = new List<Character>();
                bool groupIsShroudedInDarkness = grouping.GetLightState(lookingCharacter) == Light.Darkness;
                foreach (Prop prop in grouping.Props)
                {
                    if (groupIsShroudedInDarkness)
                    {
                        if (prop is Character)
                            if (!Equals((Character)prop, lookingCharacter))
                                allGroupedCharacters.Add((Character)prop);
                        continue;
                    }

                    if (!prop.IsPickupable() && stageItem == null && !prop.IsHidden())
                        if (prop is Item || prop is Exit)
                                stageItem = prop;


                    if (prop is Character)
                        if (!Equals((Character)prop, lookingCharacter))
                        {
                            allGroupedCharacters.Add((Character)prop);
                            if (!prop.IsHidden())
                                groupedCharacters.Add((Character)prop);
                        }
                        else
                            stageItem = prop;

                }
                if (groupedCharacters.Any())
                    groupedCharactersByProp.Add(Tuple.Create(stageItem, groupedCharacters));
            }

            List<Character> ungroupedcharacters = Characters.Where(c => !allGroupedCharacters.Contains(c)).ToList();

            List<GroupedCountedProp> groupedCountedProps = new List<GroupedCountedProp>();
            foreach (Tuple<Prop?, List<Character>> propCharacters in groupedCharactersByProp)
            {

                GroupedCountedProp groupedCountedProp = new GroupedCountedProp(propCharacters.Item1);
                groupedCountedProps.Add(groupedCountedProp);

                foreach(Character groupedCharacter in propCharacters.Item2)
                {
                    groupedCountedProp.AddToCountedCharacters(groupedCharacter);
                }
            }

            if (ungroupedcharacters.Any())
            {
                GroupedCountedProp groupedCountedProp = new GroupedCountedProp(null);
                foreach (Character character in ungroupedcharacters)
                {
                    if (Equals(character, lookingCharacter))
                        continue;
                    if (character.GetLightState(lookingCharacter) == Light.Darkness)
                        continue;
                    if (character.IsHidden())
                        continue;

                    groupedCountedProp.AddToCountedCharacters(character);
                }
                if (groupedCountedProp.CountedCharacters.Count > 0)
                    groupedCountedProps.Add(groupedCountedProp);
            }

            if (groupedCountedProps.Count() < 1)
                return "";

            int numberOfItems = 0;
            int outerLoopCounter = 0;

            string output = "As you look around you see";     
            foreach (GroupedCountedProp groupedCountedProp in groupedCountedProps)
            {
                outerLoopCounter++;
                int i = 0;
                numberOfItems = groupedCountedProp.CountedCharacters.Count();
                output += " ";
                foreach (KeyValuePair<string, CountedCharacter> countedName in groupedCountedProp.CountedCharacters)
                {

                    string name = countedName.Key;

                    bool isLastItem = false;
                    //first item
                    if (i == 0)
                    {
                        output += GetCountedElement(countedName.Value, name);
                        if (numberOfItems == 1)
                            isLastItem = true;
                    }
                    //not last item
                    else if (i + 1 != numberOfItems && numberOfItems != 1)
                    {
                        output += $", {GetCountedElement(countedName.Value, name)}";

                    }
                    //last item
                    else
                    {
                        output += $", and {GetCountedElement(countedName.Value, name)}";
                        isLastItem = true;

                    }

                    if (isLastItem)
                    {
                        if (groupedCountedProp.Prop == lookingCharacter)
                            output += " close at hand";
                        else if (groupedCountedProp.Prop != null)
                            output += " at the " + groupedCountedProp.Prop.Name;
                        else if (groupedCountedProp.Prop == null)
                            output += $" is also here";

                        string itemsAsString = ListItemsAsString(lookingCharacter, groupedCountedProp.Prop);
                        if (itemsAsString != "")
                            output += " where on the ground lies " + itemsAsString;

                        if (outerLoopCounter != groupedCountedProps.Count())
                            output += ';';
                        else
                            output += '.';
                    }
                    
                    i++;
                }
            }
            return output;
        }

        private string AddAtThe(string? groupName)
        {
            if (groupName != null)
                return " at the " + groupName;
            return "";
        }
        

        public void FlagCombatMightBeResolved()
        {
            if (InCombat)
                testCombatResolved = true;
        }
        public bool TestCombatReslovedIsFlagged()
        {
            return testCombatResolved;
        }

        public void EnableOrUpdateCombat(Character enabledBy, Character? enemy)
        {
            if (enemy != null)
            {
                enabledBy.AddRelatedRelationshipToCombat(enemy);
            }

            AddHostileRelationshipsToCombat();

            bool addedLivingCharacter = false;

            List<Faction> factionsInCombat = GetFactionInCombat();
            foreach (Character character in Characters)
            {
                if (character.InCombat || character.IsHidden() || !factionsInCombat.Contains(character.Faction))
                    continue;

                character.InCombat = true;

                if (character != enabledBy && !character.Dead)
                    addedLivingCharacter = true;            

                if (character.Soul.IsDaemon)
                    if (character.IsInHostileCombatWith(enabledBy))
                        character.SetLookAt(enabledBy);
            }
            AddCombatantsInCombatLoop = true;

            if (InCombat)
                return;
            else if (!InCombat && !addedLivingCharacter) //targets are dead and combat is resolved (before it started)
            {
                DisableCombat(false);
                return;
            }

            enabledBy.BroadcastToSoulsInRoom("Combat breaks out...");
            _ = RunCombatLoopForRoom(enabledBy);
        }

        private List<Faction> GetFactionInCombat()
        {
            List<Faction> factionsInCombat = new List<Faction>();

            foreach (Relationship relationship in RelationshipsInHostileCombat)
            {
                if (!factionsInCombat.Contains(relationship.Faction1))
                    factionsInCombat.Add(relationship.Faction1);

                if (!factionsInCombat.Contains(relationship.Faction2))
                    factionsInCombat.Add(relationship.Faction2);
            }

            return factionsInCombat;
        }

        private void AddHostileRelationshipsToCombat()
        {
            List<Faction> factionsAlive = Characters.Where(c => !c.Dead)
                                    .Select(c => c.Faction)
                                    .Distinct()
                                    .ToList();

            foreach (Relationship relationship in Program.WorldSoul.Relationships)
            {
                if (factionsAlive.Contains(relationship.Faction1) && factionsAlive.Contains(relationship.Faction2))
                {
                    if (!relationship.IsHostile())
                        continue;

                    if (!RelationshipsInHostileCombat.Contains(relationship))
                        RelationshipsInHostileCombat.Add(relationship);
                }
            }
        }

        public void DisableCombat(bool broadcast)
        {
            InCombat = false;

            foreach (Character character in Characters.Where(c => c.InCombat))
            {
                character.InCombat = false;
            }
            if (broadcast)
                BroadcastToSoulsInRoom($"Combat is resolved.");
            Console.WriteLine("Combat has ended.");
            
            RunOnAfterCombatEvents();
        }

        public void TestCombatIsResolved()
        {
            if (!InCombat)
                return;

            if (CombatIsResolved())
                DisableCombat(true);
        }

        private bool CombatIsResolved()
        {
            var charactersInCombat = Characters.Where(c => c.InCombat && !c.Dead).ToList();
            var factionsInCombat = charactersInCombat.Select(c => c.Faction).Distinct().ToList();

            // Collect all relevant relationships once
            var relevantRelationships = RelationshipsInHostileCombat
                .Where(r => factionsInCombat.Contains(r.Faction1) || factionsInCombat.Contains(r.Faction2))
                .ToList();

            // Check each character and their opposing factions
            foreach (var character in charactersInCombat)
            {
                foreach(Relationship rel in relevantRelationships.Where(r =>
                        (r.Faction1 == character.Faction && factionsInCombat.Contains(r.Faction2)) ||
                        (r.Faction2 == character.Faction && factionsInCombat.Contains(r.Faction1))))
                {
                    if (rel.Faction1 != rel.Faction2)
                        return false;
                    else
                        if (charactersInCombat.Any(c => c != character && c.Faction == rel.Faction1)) //makes sure he is not just fighting himself
                            return false;
                }
            }

            return true;
        }

        private List<Character> GetOrderedCombatants(Dictionary<Character, int> initiativeRolls)
        {
            List<Character> combatCharacters = Characters.Where(c => c.InCombat).ToList();
            if (combatCharacters.Count == 0)
                return new List<Character>();

            foreach (Character character in combatCharacters)
            {
                if (!initiativeRolls.ContainsKey(character))
                {
                    initiativeRolls.Add(character, character.RollInitiative());
                }
            }

            return combatCharacters.OrderByDescending(character => initiativeRolls[character]).ToList();
        }

        private void BroadcastInitiative(List<Character> orderedCombatChars, List<Character> xOrderedCombatChars, Dictionary<Character, int> initiativeRolls, bool firstRound)
        {
            string initiative = "Initiative:";
            foreach (Character combatChar in orderedCombatChars)
            {
                int roll = initiativeRolls[combatChar];
                initiative += $"\n";
                if (roll < 10)
                    initiative += $" {roll}  {combatChar.Name}";
                else 
                    initiative += $"{roll} {combatChar.Name}";
                if (!firstRound && !xOrderedCombatChars.Contains(combatChar)) //dosnt work, since char is never removed from list...
                    initiative += " (Joined combat)";
            }
            BroadcastToSoulsInRoom(initiative);
        }

        public async Task RunCombatLoopForRoom(Character enabledBy)
        {
            Character? initiater = enabledBy;
            InCombat = true;

            Dictionary<Character, int> initiativeRolls = new Dictionary<Character, int>();
            List<Character> orderedCombatChars = new List<Character>();
            List<Character> activeOrderedCombatCharsInRound = new List<Character>();
            AddCombatantsInCombatLoop = true;
            int round = 1;
            while (InCombat)
            {
                if (AddCombatantsInCombatLoop)
                {
                    orderedCombatChars = GetOrderedCombatants(initiativeRolls);
                    if (orderedCombatChars.Count <= 1)
                        if (CombatIsResolved())
                        {
                            bool shouldBroadcast = round > 1;
                            DisableCombat(shouldBroadcast);
                            return;
                        }

                    AddCombatantsInCombatLoop = false;

                    foreach(Character combatCaracter in orderedCombatChars)
                    {
                        if (combatCaracter != initiater)                        
                            await combatCaracter.Interrupt();
                    }
                    
                    foreach (Character tradingCharacterNotInCombat in Characters.Where(c => c.TradingWith != null && !c.InCombat))
                    {
                        if (tradingCharacterNotInCombat != initiater)
                            await tradingCharacterNotInCombat.Interrupt();
                    }
                    initiater = null;

                    BroadcastInitiative(orderedCombatChars, activeOrderedCombatCharsInRound, initiativeRolls, (round == 1));
                }

                Console.WriteLine($"Combat round {round} in room {Name}.");
                foreach (Character character in orderedCombatChars)
                {
                    Console.WriteLine(character.Name + " is in combat.");
                }

                //activeOrderedCombatCharsInRound = orderedCombatChars.Where(c => c.InCombat).ToList();
                foreach (Character character in orderedCombatChars)
                {
                    bool playerTurnBroadcasted = false;
                    
                    bool ActionUsed = false;
                    while (!ActionUsed && character.InCombat && character.CurrentRoom == this)
                    {
                        character.Soul.ClearMoves();
                        character.Soul.GeneratePossibleMoves();
                        if (character.Soul.ShownPossibleMoves.Count == 0)
                            throw new Exception("We need moves...");
                        if (!character.Soul.IsDaemon)
                        {
                            try
                            {                          
                                await character.Soul.SendPossibleMovesAsync();
                                if (!playerTurnBroadcasted)
                                {
                                    BroadcastToSoulsInRoom($"{character.Name} has the initiative...");
                                    playerTurnBroadcasted = true;
                                }
                                Move? nextMove = await character.Soul.ReceiveAndHandleMoveAsync(false);
                                ActionUsed = ExecuteCombatAction(character, nextMove);
                                character.TryEnableCombat();
                            }
                            catch (OperationCanceledException)
                            {
                                Console.WriteLine($"{character.Name} combat loop was interrupted by player loop.");
                                //do nothing, this should rarely happen. (if ever)
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                                character.Soul.Banish();  
                            }
                        }
                        else
                        {
                            Move? nextMove = character.Soul.DaemonChoosesNextMove();
                            if (nextMove != null && nextMove.Type != MoveType.MinorAction)
                                await Task.Delay(1500);
                            ActionUsed = ExecuteCombatAction(character, nextMove);
                            character.TryEnableCombat();
                        }                       
                    }
                    if (TestCombatReslovedIsFlagged())
                        if (CombatIsResolved())
                        {
                            DisableCombat(true);
                            return;
                        }
                }
                round++;
            }
        }

        private void RunOnAfterCombatEvents()
        {
            foreach (Action afterCombatEvent in onCombatEndEvents)
                afterCombatEvent();

            onCombatEndEvents.RemoveAll(e => onCombatEndEventsToBeRemoved.Contains(e));
            onCombatEndEventsToBeRemoved.Clear();
        }

        public void AddOnAfterCombatEvent(Action action)
        {
            onCombatEndEvents.Add(action);
            onCombatEndEventsToBeRemoved.Add(action);
        }

        private bool ExecuteCombatAction(Character character, Move? move)
        {          
            if (move == null)
                return true;

            character.Soul.Execute(ref move);
            
            bool actionUsed = (move.Type != MoveType.MinorAction);

            return actionUsed;
        }

        public void BroadcastToSoulsInRoom(string message)
        {
            BroadcastToSoulsInRoom(null, message, null);
        }

        public void BroadcastToSoulsInRoom(string message, Character? excludeChar)
        {
            BroadcastToSoulsInRoom(null, message, excludeChar);
        }

        public void BroadcastToSoulsInRoom(Character? character, string message, Character? excludeChar)
        {
            if (character != null)
            {
                string? buffer;
                if (Program.WorldSoul.ThreadBufferText.TryGetValue(Thread.CurrentThread, out buffer) && !string.IsNullOrEmpty(buffer))
                {
                    message = buffer + message;
                    RemoveBufferTextForThread();
                    Console.WriteLine("Buffer used and cleared at " + DateTime.Now);
                }
                else
                    Console.WriteLine("send without buffer at " + DateTime.Now);

                if (character.IsHidden())
                {
                    _ = character.Soul.SendAsync(message);
                    return;
                }
            }

            foreach (Character characterInRoom in Characters)
            {
                if (excludeChar == characterInRoom)
                    continue;

                if (characterInRoom.Soul != null)
                {
                    _ = characterInRoom.Soul.SendAsync(message);
                }
            }
        }
    }
}
