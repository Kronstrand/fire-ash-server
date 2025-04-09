using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Data;
using static fire_ash_server.Helpers;
using fire_ash_server.Props;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Dialogue;
using fire_ash_server.World;
using fire_ash_server.Abstract_Entities;
using System.Diagnostics;
using fire_ash_server.World.BioMechWorld;
using Newtonsoft.Json;

namespace fire_ash_server
{
    [Serializable]
    internal class Soul
    {
        private Character? character;
        public Guid Id;
        public bool IsDaemon;
        public ConcurrentDictionary<string, Move> AllPossibleMoves = new ConcurrentDictionary<string, Move>();
        public ConcurrentDictionary<string, Move> ShownPossibleMoves = new ConcurrentDictionary<string, Move>();
        public CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        public string BufferText = "";
        public bool AddToBufferText = false;
        public int InventoryToolTip = 0;
        public bool CompletedGame = false;

        public Soul(Socket soulSocket)
        {
            
            Id = Guid.NewGuid();
            Socket = soulSocket;
            AddToWorldSoul();
        }

        public Soul(Character character)
        {
            Id = Guid.NewGuid();
            Character = character;
            IsDaemon = true;
            AddToWorldSoul();
        }

        public Socket? Socket
        {
            get 
            {
                Program.Sockets.TryGetValue(Id, out Socket? socket);
                return socket;
            }
            set
            {
                if (value == null)
                    Program.Sockets.Remove(Id, out Socket? socket);
                else 
                    Program.Sockets[Id] = value;
            }
        }

        public Character Character
        {
            get
            {
                if (character == null) throw new ArgumentNullException(nameof(character), "Character cannot be null here..");
                return character;
            }
            set
            {
                character = value;
            }
        }

        public void InitToolTipCounters()
        {
            InventoryToolTip = 0;
        }

        public void AddToWorldSoul()
        {
            Program.WorldSoul.Souls.Add(this);
        }

        public async Task SendAsync(string messageToSend, bool isPersonal)
        {
            if (isPersonal)
                await SendAsync(messageToSend);
            else
                Character.CurrentRoom.BroadcastToSoulsInRoom(messageToSend);
        }

        public async Task SendAsync(string messageToSend)
        {
            await SendAsync(messageToSend, SendOption.None);
        }

        public async Task SendAsync(string messageToSend, SendOption sendOption)
        {
            if (Socket == null)// || Socket.Connected == false) //is probably daemon or disconnected
                return;

            /*if (sendOption == SendOption.None)
                messageToSend += " ";
            if (sendOption == SendOption.NewLine)
                messageToSend += "\n";
            else if (sendOption == SendOption.NewParagraph)
                messageToSend += "\n\n";*/

            messageToSend += "$[end]";

            byte[] buffer = Encoding.ASCII.GetBytes(messageToSend);

            await Socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None);
        }

        public async Task SendPossibleMovesAsync()
        {
            string possibleMoves = GetPossibleMovesAsString();
            await SendPossibleMovesAsync(possibleMoves);
        }

        public async Task SendPossibleMovesAsync(string possibleMoves)
        {
            if (possibleMoves == "")
                return;
            await SendAsync("$[pm]" + possibleMoves + "$[pmend]", SendOption.None);
        }

        public async Task SendInvalidInputAsync()
        {
            await SendAsync("$[invalid]");
        }

        public async Task SendChoiceFlagAsync()
        {
            await SendAsync("$[choice]");
        }

        public void CancelAndResetTokenSource()
        {
            CancelTokenSource.Cancel();
            CancelTokenSource = new CancellationTokenSource();
        }

        public async Task<string> ReceiveAsync()
        {
            if (Socket == null) throw new ArgumentNullException(nameof(Socket), "Socket cannot be null when sending to server");
            byte[] buffer = new byte[1024];

            CancelAndResetTokenSource();
            int received = await Socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None, CancelTokenSource.Token);
            if (received == 0)
            {
                Console.WriteLine("Manually throw exception since connection closed.");
                throw new SocketException((int)SocketError.ConnectionAborted);
            }

            string messageReceived = Encoding.ASCII.GetString(buffer, 0, received);
            Console.WriteLine("Received: " + messageReceived);
            return messageReceived;
        }

        public async Task<bool> AwaitYesNo()
        {
            await SendChoiceFlagAsync();
            string input = await ReceiveAsync();
            await SendAsync(InputOk());
            input = input.ToLower();
            return (input == "y" || input == "yes");
        }

        public async Task<string> AwaitInput(bool SetChoiceFlag)
        {
            if (SetChoiceFlag)
                await SendChoiceFlagAsync();
            string input = await ReceiveAsync();
            await SendAsync(InputOk());
            return input;
        }

        public async void GeneratePossibleMoves()
        {
            if (CompletedGame)
            {
                AddExitGameMove(false);
                return;
            }

            if (Character.Dead)
                return;

            if (Character.SpeakingTo != null && Character.SpeakingTo.DialogueManager != null && Character.SpeakingTo.DialogueManager.Initiater == Character)
            {
                DialogueManager dialogueManager = Character.SpeakingTo.DialogueManager;
                DialogueNode currentNode = dialogueManager.CurrentNode;
                for (int i = currentNode.Choices.Count() - 1; i >= 0; i--)
                {
                    DialogueChoice choice = currentNode.Choices[i];
                    if (dialogueManager.ChoiceIsValid(choice))
                    {
                        AddPossibleSpeakChoice(i, choice, dialogueManager);
                    }
                }
                return;
            }
            AddPossibleHiddenLookAtMove(Character);

            if (Character.LookAt != null)
            {
                AddPossibleMove(new LookAt(this));

                if (Character.LookAt is Character)
                {                  
                    Character lookAtCharacter = (Character)Character.LookAt;
                    if (lookAtCharacter.GetLightState(Character) != Light.Darkness)
                    {
                        bool? isInGroupWithTargert = Character.IsInGroupWith(Character.LookAt);

                        if (isInGroupWithTargert == false)
                            AddPossibleMove(new MoveTo(this, lookAtCharacter));
                        else if (lookAtCharacter.Dead && isInGroupWithTargert == true)
                            AddPossibleMove(new LookInventory(this,lookAtCharacter));
                        
                        

                        if (!Character.IsHidden() && isInGroupWithTargert == true)
                        {
                            if (lookAtCharacter.DialogueManager != null)
                                AddPossibleMove(new SpeakTo(this, lookAtCharacter));
                            if (lookAtCharacter.IsTrader)
                            {
                                AddPossibleMove(new BrowseGoods(this, lookAtCharacter));
                                AddPossibleMove(new ShowItemsToSell(this, lookAtCharacter));
                            }
                        }
                    }
                }
                else if (Character.LookAt is Inventory)
                {
                    Inventory inventory = (Inventory)Character.LookAt;
                    Character? tagetCharacter = inventory.HeldByCharacter();
                    if (tagetCharacter != null)
                        foreach(KeyValuePair<InventorySlot, Item> SlotAndItem in tagetCharacter.EquippedItems)
                            AddPossibleMove(new LookAt(this, SlotAndItem.Value));

                    foreach (Item item in inventory.Items)
                    {
                        if (item is Coins && inventory.HeldBy == Character.TradingWith)
                            continue;
                        AddPossibleMove(new LookAt(this, item));
                    }
                }
                else if (Character.LookAt is Item)
                {
                    Item item = (Item)Character.LookAt;

                    AddPossibleInvestigationOrLookMove(item);

                    if (Character.IsInGroupWith(item) == false && item.HeldByCharacter() == null && item.GetLightState(Character) != Light.Darkness && !item.Unreachable)
                        AddPossibleMove(new MoveTo(this, item));

                    if (Character.TradingWith != null && item.Sellable)
                    {
                        if (item.HeldByCharacter() == Character.TradingWith)
                            AddPossibleMove(new BuyItem(this, item, Character.TradingWith));
                        else if (item.HeldByCharacter() == Character)
                            AddPossibleMove(new SellItem(this, item, Character.TradingWith));
                    }

                    if (item.IsPickupable())
                    {
                        if (item.HeldByCharacter() != Character)
                            AddPossibleMove(new Grab(this, (Item)Character.LookAt)); //But you can pickup Props??
                        else
                        {
                            foreach (InventorySlot inventorySlot in item.CarriableByInventorySlots)
                            {
                                if (!(Character.EquippedItems.TryGetValue(inventorySlot, out var equippedItem) && equippedItem == item))
                                    AddPossibleMove(new Equip(this, item, inventorySlot));
                            }

                            if (item is Consumable)
                                AddPossibleMove(new Consume(this, (Consumable)item));
                            AddPossibleMove(new DropItem(this, item));
                        }
                    }

                    foreach(Effect effect in item.EquipEffects)
                        AddPossibleMove(new LookEffectDescription(this, effect.Name));
                }
                else if (Character.LookAt is Room)
                {
                    Room lookAtRoom = (Room)Character.LookAt;
                    AddPossibleInvestigationOrLookMove(lookAtRoom);
                    foreach (Character otherChar in lookAtRoom.Characters.Where(character =>
                                                                    character != Character && 
                                                                    !character.IsHidden() && 
                                                                    character.GetLightState(Character) != Light.Darkness))
                    {
                        AddPossibleMove(new LookAt(this, otherChar));
                    }
                }

                Light characterLookAtLightSate = Character.LookAt.GetLightState(Character);
                bool lookAtWithLight = !(characterLookAtLightSate == Light.Darkness && !Character.HasPointLight());
                if (lookAtWithLight)
                {
                    foreach (SkillCheck skillCheck in Character.LookAt.moves.Where(move => move.GetType() == typeof(SkillCheck)))
                    {
                        AddPossibleUnusedMove(skillCheck.CreatePossibleMove(this, Character.LookAt));
                    }
                }
                if (lookAtWithLight && Character.LookAt is not Inventory)
                {
                    foreach (Item item in Character.LookAt.Items.Where(item => !item.IsHidden()))
                    {
                        bool isClose = true;
                        if (item.HeldBy == Character.CurrentRoom)
                        {
                            isClose = Character.IsInGroupWith(item) != false;
                        }

                        if (isClose)
                        {
                            AddPossibleInvestigationOrLookMove(item);

                            if (item.HeldByCharacter() == null)
                            {
                                Grab grabMove = new Grab(this, item);
                                grabMove.Hidden = true;
                                AddPossibleMove(grabMove);
                            }
                        }
                        else if ((!isClose && item.Unreachable) || (item.DynamicDescription && item.GetLightState(character) == Light.Darkness && Character.HasPointLight()))
                        {
                            AddPossibleMove(new LookAt(this, item));
                        }
                        else if (!item.Unreachable)
                            if (item.DynamicDescription || item.GetLightState(character) != Light.Darkness)
                                AddPossibleMove(new MoveTo(this, item));
                    }
                }
                //looking at darkness with flashlight to see all props in group
                if (Character.LookAt.GetLightState(null, false) == Light.Darkness && Character.HasPointLight())
                {
                    Grouping? group = Character.LookAt.GetGrouping();
                    if (group != null)
                    {
                        foreach (Prop prop in group.Props)
                        {
                            if (Character.LookAt == prop)
                                continue;

                            AddPossibleMove(new LookAt(this, prop));
                        }
                    }                    
                }
            }

            foreach (string featKey in Character.Feats)
            {
                Feat? feat = Feats.Get(featKey, this, Character.LookAt);
                if (feat == null) continue;

                foreach (Move move in feat.Moves)
                {
                    if (move.IsValid(this) && IsValidCloseSingleTargetMove(move))
                    {
                        if (move is Attack && Character.LookAt is Character && Character.GetRelationShipTo((Character)Character.LookAt).GetStatus() == RelationshipStatus.good)
                            AddPossibleMove(move, true);
                        else
                            AddPossibleMove(move, false);
                    }
                }
            }

            foreach(Consumable consumable in Character.Inventory.Items.Where(i => i is Consumable))
            {
                Consume consume = new Consume(this, consumable);
                consume.Hidden = true;
                AddPossibleMove(consume);
            }

            //Exits in current room
            foreach (Exit exit in Character.CurrentRoom.Exits.Where(exit => !exit.IsHidden()))
            {                           
                if (Character.IsInGroupWith(exit) == true)
                {
                    if (Character.LookAt == Character.CurrentRoom) // or force (todo)
                        AddPossibleMove(new LookAt(this, exit));

                    if (ReferenceEquals(Character.LookAt, exit))
                    {
                        if (!Character.InCombat)
                            AddPossibleMove(new RoomChange(this, exit));
                        else
                        {
                            int countedEnemies = Character.CurrentRoom.Characters.Where(c => c.InCombat && c.LookAt == Character && c.IsInHostileCombatWith(Character)).Count();
                            int DC = 10 + countedEnemies;

                            AddPossibleMove(new SkillCheck(
                                this,
                                exit,
                                MoveKey.f.ToString(),
                                $"Flee combat and enter {exit.GoToRoom.Name}.",
                                new SkillNumber(Skill.Acrobatics, DC),
                                true, //should be a nonpersonal process?
                                async (Soul s) =>
                                {
                                    RoomChange roomChange = new RoomChange(this, exit);
                                    await roomChange.Action();
                                    return null;
                                },
                                async (Soul s) =>
                                {
                                    return $"{Character.Name} failed to flee combat...";
                                }));
                        }
                    }
                }
                else if (ReferenceEquals(Character.LookAt, Character.CurrentRoom))
                {
                    if (exit.GetLightState(Character) != Light.Darkness)
                        AddPossibleMove(new MoveTo(this, exit));
                }
            }

            //stop trading
            if (Character.TradingWith != null && Character.LookAt is Inventory)
                AddPossibleMove(new StopBrowseGoods(this));

            //Back..
            AddPossibleBackMove();

            //inventory
            AddPossibleMove(new LookInventory(this));
            //journal
            //AddPossibleMove(new CheckJournal(this));

            AddExitGameMove(true);

            //AddPossibleMove(new SaveGameState(this));
            //AddPossibleMove(new LoadGameState(this));
        }

        private void AddExitGameMove(bool hidden)
        {
            Move exitGameMove = new Move("x", "Exit Game", async () =>
            {
                await SendAsync("Do you really want to quit? (y/n)");
                if (await AwaitYesNo())
                    await SendAsync("$[quit]");
                else
                    await SendAsync("You decide to press on.");
            });
            exitGameMove.Hidden = hidden;
            AddPossibleMove(exitGameMove);
        }

        public void AddPossibleHiddenLookAtMove(Character character)
        {
            LookAt lookAtMe = new LookAt(this, character);
            lookAtMe.Hidden = true;
            AddPossibleMove(lookAtMe);
        }

        private void AddPossibleSpeakChoice(int i, DialogueChoice choice, DialogueManager dialogueManager)
        {
            AddPossibleMove(
                    new Move(
                        "" + i + 1,
                        choice.Text,
                        async () =>
                        {
                            dialogueManager.SetCurrentNodeBasedOnChoice(choice);                        
                            dialogueManager.SpeakCurrentNode();
                            if (!dialogueManager.CurrentNodeHasChoices())
                                dialogueManager.EndSpeakWith();
                        }
                        ));
        }

        private void AddPossibleBackMove()
        {

            if (AllPossibleMoves.ContainsKey(MoveKey.bg.ToString())) //use stop trading instead of back
                return;

            int lookedAtIndex = Character.LookedAt.Count - 1;
            if (!(lookedAtIndex > 0 && Character.LookAt != null))
                return;
            
            if (Character.TradingWith != null && Character.LookAt == Character.TradingWith.Inventory)
                return;

            Move backMove = new Move(
                MoveKey.b.ToString(),
                "Back..",
                async () =>
                {
                    Prop xLookedAt = Character.LookAt;
                    Character.LookBack();
                    await SendAsync(Character.Name + " stops looking " + xLookedAt.GetLightEffectedName("at ", "into the ", true, Character) + ".");
                });
            backMove.Type = MoveType.MinorAction;
            backMove.AllowedInTrade = true;

            AddPossibleMove(backMove);
            
        }

        private void AddPossibleInvestigationOrLookMove(Prop prop)
        {
            Light propLightState = prop.GetLightState(Character);
        
            if (propLightState != Light.Darkness)
            {
                if (prop.HasHiddenProps() && AddPossibleUnusedMove(new Investigate(this, prop)))
                    return;
            }

            if (!(prop is Room))
            {
                if (!prop.DynamicDescription && propLightState == Light.Darkness)
                    return;
                if (prop.DynamicDescription && propLightState == Light.Darkness && !Character.HasPointLight())
                    return;
            }

            if (prop != Character.LookAt)
                AddPossibleMove(new LookAt(this, prop));
        }

        public string GetPossibleMovesAsString()
        {
            string actionsAsStr = "";
            foreach (KeyValuePair<string, Move> kvp in ShownPossibleMoves.Where(kvp => !kvp.Value.Hidden).OrderBy(kvp => int.Parse(kvp.Key)))
            {
                if (actionsAsStr != "")
                    actionsAsStr += "\n";

                actionsAsStr += kvp.Key.ToUpper() + ") " + kvp.Value.Description;
            }
            return actionsAsStr;
        }

        public bool IsValidCloseSingleTargetMove(Move move)
        {
            if (move.Range == RangeType.CloseSingleTarget)
            {
                if (Character.LookAt == null)
                    return false;
                if (Character.IsInGroupWith(Character.LookAt) != true)
                    return false;
            }
            return true;
        }

        public async Task<Move?> ReceiveAndHandleMoveAsync(bool execute)
        {
            if (Socket == null)
                return null;

            while (true)
            {
                string input = await ReceiveAsync();
                input = input.ToLower();

                Move? moveToExecute = GetMoveFromInput(input);
                if (moveToExecute != null)
                {
                    if (execute)
                        //Execute(ref moveToExecute);
                        await moveToExecute.Execute(this);
                    await SendAsync(InputOk());
                    return moveToExecute;
                }
                else
                {
                    if (Socket == null)
                        return moveToExecute;
                    await SendInvalidInputAsync();
                }
            }
        }

        private Move? GetMoveFromInput(string input)
        {
            string loweredInput = input.ToLower();
            if (AllPossibleMoves.ContainsKey(loweredInput))
            {
                return AllPossibleMoves[loweredInput];
            }
            if (ShownPossibleMoves.ContainsKey(loweredInput))
            {
                return ShownPossibleMoves[loweredInput];
            }
            return null;
        }

        public void ClearMoves()
        {
            AllPossibleMoves.Clear();
            ShownPossibleMoves.Clear();
        }

        public async Task SendAndReceiveMoveOutOfCombatAsync()
        {
            try
            {
                if (Character.InCombat) return;
                await SendPossibleMovesAsync();
                if (Character.InCombat) throw new OperationCanceledException(); //should rearly happen
                await ReceiveAndHandleMoveAsync(true);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"{Character.Name} loop was interrupted by combat.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Banish();
            }
        }

        public bool AddPossibleUnusedMove(Move move)
        {
            if (!move.Repeatable && Character.HasUsedMoveOnProp(move))
                return false;

            AddPossibleMove(move);
            return true;
        }

        public void AddPossibleMove(Move move)
        {
            AddPossibleMove(move, false);
        }

        public void AddPossibleMove(Move move, bool forceHide)
        {
            if (move.IsMovement && Character.HasCondition(Condition.Rooted)) return;
            if (!move.AllowedInCombat && Character.CurrentRoom.InCombat) return;
            if (Character.TradingWith != null)
            {
                if (!move.AllowedInTrade) return;
                if (move.Hidden && !(move is LookInventory)) return;
                if (forceHide) return;
            }

            string possibleMoveKey = move.GetCompleteMoveKey();

            int number = 0;
            string loweredPossibleMoveKey = possibleMoveKey.ToLower();
            string moveKey;
            while (true)
            {
                moveKey = loweredPossibleMoveKey;

                if (number > 0)
                    moveKey += "#" + number;

                if (ReferenceEquals(AllPossibleMoves.GetOrAdd(moveKey, move), move)) //if object was added
                    break;

                number++;
            }

            if (move.Hidden || forceHide)
                return;

            int key = ShownPossibleMoves.Count() + 1;
            ShownPossibleMoves.GetOrAdd(key.ToString(), move);
        }

        public async Task MoveCharToRoomAndSendDescriptionAsync(RoomKey roomKey)
        {
            Room goToRoom = Program.WorldSoul.Rooms[Description(roomKey)];
            await MoveCharToRoomAndSendDescriptionAsync(goToRoom);
        }
        public async Task MoveCharToRoomAndSendDescriptionAsync(Room goToRoom)
        {
            Character.InCombat = goToRoom.InCombat;
            
            Character.GoToRoom(goToRoom);

            if (Character.LastRoom != null && Character.LastRoom.InCombat)
                Character.LastRoom.FlagCombatMightBeResolved();

            await SendAsync(goToRoom.GetDescription(Character, true));

            if (goToRoom.OnEnterEvent != null)
                goToRoom.OnEnterEvent(this);

            await SendAsync(goToRoom.GetAdditionalRoomDescription(Character));

            if (goToRoom.InCombat)
                goToRoom.EnableOrUpdateCombat(Character, null);
        }

        public Move? DaemonChoosesNextMove()
        {
            Random rnd = new Random();

            //make it choose stealth.....
            if (!(Character.LookAt is Character && Character.IsInHostileCombatWith((Character)Character.LookAt)))
            {
                Dictionary<string, Move> relevantLookMoves = new Dictionary<string, Move>();

                foreach (KeyValuePair<string, Move> kvp in AllPossibleMoves)
                {
                    if (kvp.Value is LookAt && kvp.Value.Prop is Character)
                    {
                        Character caracterToLookAt = (Character)kvp.Value.Prop;

                        if (caracterToLookAt.IsInHostileCombatWith(Character))
                        {
                            relevantLookMoves.Add(kvp.Key, kvp.Value);
                            continue;
                        }
                    }
                }
                if (relevantLookMoves.Count > 0)
                    return relevantLookMoves.ElementAt(rnd.Next(relevantLookMoves.Count)).Value;
                return null;
            }

            Dictionary<string, Move> relevantAttackMoves = AllPossibleMoves.Where(x => 
                                                                x.Value is Attack || 
                                                                (x.Value is MoveTo && x.Value.Prop is Character && Character.IsInHostileCombatWith((Character)x.Value.Prop) ||
                                                                x.Value is Consume)).ToDictionary();

            //add all feat moves from character
            foreach (KeyValuePair<string, Move> kvp in AllPossibleMoves)
            {
                /*bool isCharacterFeat = Character.Feats.SelectMany(f => f.Moves).Any(m => m.Key == kvp.Value.Key);
                if (isCharacterFeat)
                {
                    relevantAttackMoves.Add(kvp.Key, kvp.Value);
                }*/
            }


            if (relevantAttackMoves.Count > 0)
            {
                while (true)
                {
                    Move chosenMove = relevantAttackMoves.ElementAt(rnd.Next(relevantAttackMoves.Count)).Value;
                    if (chosenMove is Consume)
                    {
                        Random rand = new Random();
                        double roll = rand.NextDouble(); // Generates a random number between 0.0 and 1.0

                        if (roll <= 0.75) // 75% chance to skip consumable move
                            continue;

                        if (chosenMove.Prop != null)
                        {
                            if (chosenMove.Prop.Name == ConsumableList.BearTrapName)
                            {
                                continue;
                            }
                            else if (chosenMove.Prop.Name == ConsumableList.HealingPotionName)
                            {
                                if (Character.CurrentHP >= (Character.HP - Character.CurrentHP)) //only choose move if HP is below %50
                                    continue;
                            }
                            else if (chosenMove.Prop.Name == ConsumableList.ScrollofEntanglementName)
                            {
                                if (Character.lookAtBeforeInventory is Character)
                                {
                                    bool? isCloseTo = Character.IsInGroupWith(Character.lookAtBeforeInventory);
                                    if (isCloseTo == true || isCloseTo == null)
                                        continue;
                                }
                            }
                        }
                    }

                    return chosenMove;
                }
            }
            return null;
        }

        public void Banish()
        {
            IsDaemon = true;

            if (Socket != null)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket = null;
            }
            Console.WriteLine("Soul has been banished.");
        }
    }
}
