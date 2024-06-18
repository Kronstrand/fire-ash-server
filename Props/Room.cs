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

namespace fire_ash_server.Props
{
    internal class Room : Prop
    {
        public RoomKey RoomKey;
        public ThreadSafeList<Character> Characters = new ThreadSafeList<Character>();
        public ThreadSafeList<Grouping> Groupings = new ThreadSafeList<Grouping>();
        public ThreadSafeList<Relationship> RelationshipsInHostileCombat = new ThreadSafeList<Relationship>();
        public ThreadSafeList<Exit> Exits = new ThreadSafeList<Exit>();
        public Action<Soul>? OnEnterEvent;
        public bool InCombat;
        private bool testCombatResolved;
        public bool AddCombatantsInCombatLoop;

        public Room(RoomKey roomKey,string name, string description) : base(name, description)
        {   
            RoomKey = roomKey;
            Program.WorldSoul.AddRoom(this);
        }

        public void AddExit(Exit exit)
        {
            Exits.Add(exit);
            exit.LocatedInRoom = this;
        }
        public string GetFullRoomDescription(Character excludeCharacter)
        {
            string description = GetDescription();

            foreach (Exit exit in Exits)
            {
                description += exit.GetDescription();
            }       
            
            description += ListCharactersAsString(excludeCharacter);

            return description;
        }

        private string ListCharactersAsString(Character excludeCharacter)
        {
            Dictionary<string, Tuple<int, bool>> countedNamesInRoom = new Dictionary<string, Tuple<int, bool>> ();
            foreach(Character character in Characters)
            {
                if (Equals(character, excludeCharacter))
                    continue;

                string name = character.Name;
                if (countedNamesInRoom.ContainsKey(name))
                    countedNamesInRoom[name] = Tuple.Create(countedNamesInRoom[name].Item1 + 1, countedNamesInRoom[name].Item2);
                else
                    countedNamesInRoom.Add(name, Tuple.Create(1, character.UniqueName));
            }

            if (countedNamesInRoom.Count() < 1)
                return "";

            string output = "As you look around you see ";
            int lengthOfList = countedNamesInRoom.Count;
            for (int i = 0; i < lengthOfList; i++)
            {
                //first item
                if (i == 0)
                {
                    output += GetCountedElement(countedNamesInRoom, i);
                }
                //not last item
                else if (i + 1 != lengthOfList)
                {
                    output += $", {GetCountedElement(countedNamesInRoom, i)}";
                }
                //last item
                else
                {
                    output += $", and {GetCountedElement(countedNamesInRoom, i)}.";
                }
            }
            return output;
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
                if (enemy.Dead)
                    return;

                enabledBy.AddRelatedRelationshipToCombat(enemy);
            }

            AddHostileRelationshipsToCombat();

            List<Faction> factionsInCombat = GetFactionInCombat();
            foreach (Character character in Characters)
            {
                if (character.InCombat || character.IsHidden() || !factionsInCombat.Contains(character.Faction))
                    continue;

                character.InCombat = true;

                if (character.Soul.IsDaemon)
                    if (character.IsInHostileCombatWith(enabledBy))
                        character.SetLookAt(enabledBy);
            }
            AddCombatantsInCombatLoop = true;

            if (InCombat)
                return;

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
                        if (charactersInCombat.Any(c => c != character && c.Faction == rel.Faction1)) //make sure he is not just fighting himself
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
                //if (character.Soul.Socket != null && character.CharacterLoopIsActive)
                    //character.Soul.CancelTokenSource.Cancel();
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
                        if (combatCaracter.Soul.Socket != null && combatCaracter != initiater)
                        {
                            await combatCaracter.Soul.SendAsync("$[cancel]");
                            combatCaracter.Soul.CancelAndResetTokenSource();
                            initiater = null;
                        }
                    }

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
                if (character.IsHidden())
                {
                        _ = character.Soul.SendAsync(message);
                    return;
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
