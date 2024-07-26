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

namespace fire_ash_server
{
    internal class Soul
    {
        private Character? character;
        public Socket? Socket;
        public bool IsDaemon;
        public ConcurrentDictionary<string, Move> AllPossibleMoves = new ConcurrentDictionary<string, Move>();
        public ConcurrentDictionary<string, Move> ShownPossibleMoves = new ConcurrentDictionary<string, Move>();
        public CancellationTokenSource CancelTokenSource = new CancellationTokenSource();

        public Soul(Socket soulSocket)
        {
            Socket = soulSocket;
            AddToWorldSoul();
        }

        public Soul(Character character)
        {
            Character = character;
            IsDaemon = true;
            AddToWorldSoul();
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
            if (possibleMoves == "")
                return;
            await SendAsync("$[pm]" + possibleMoves + "$[pmend]", SendOption.None);
        }

        public async Task SendInvalidInputAsync()
        {
            await SendAsync("$[invalid]");
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

        public void GeneratePossibleMoves()
        {
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

                    if (Character.IsInGroupWith(Character.LookAt) == false)
                        AddPossibleMove(new MoveTo(this, lookAtCharacter));

                    if (!Character.IsHidden())
                        if (lookAtCharacter.DialogueManager != null && Character.IsInGroupWith(Character.LookAt) == true)
                        {
                            AddPossibleMove(new Move(
                                "sp",
                                $"Speak to {lookAtCharacter.Name}",
                                () =>
                                {
                                    lookAtCharacter.DialogueManager.InitSpeakWith(Character);
                                }));
                        }
                }
                else if (Character.LookAt is Item)
                {
                    AddPossibleInvestigationOrLookMove(Character.LookAt);
                }
                else if (Character.LookAt is Room)
                {
                    Room lookAtRoom = (Room)Character.LookAt;
                    AddPossibleInvestigationOrLookMove(lookAtRoom);
                    foreach (Character otherChar in lookAtRoom.Characters.Where(character => character != Character && !character.IsHidden()))
                    {
                        AddPossibleMove(new LookAt(this, otherChar));
                    }
                }

                if (Character.LookAt.IsPickupable())
                {
                    Item item = (Item)Character.LookAt;

                    if (item.HeldByCharacter() != Character)
                        AddPossibleMove(new Grab(this, (Item)Character.LookAt)); //But you can pickup Props??

                    if (item.HeldByCharacter() == Character)
                        foreach (InventorySlot inventorySlot in item.CarriableByInventorySlots)
                            AddPossibleMove(new Equip(this, item, inventorySlot));
                }

                foreach (SkillCheck skillCheck in Character.LookAt.moves.Where(move => move.GetType() == typeof(SkillCheck)))
                {
                    AddPossibleUnusedMove(skillCheck.CreatePossibleMove(this, Character.LookAt));
                }

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
                    else
                        AddPossibleMove(new MoveTo(this, item));
                }
            }

            foreach (string featKey in Character.Feats)
            {
                Feat? feat = FeatCreater.Get(featKey, this, Character.LookAt);
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

            //Exits in current room
            foreach (Exit exit in Character.CurrentRoom.Exits.Where(exit => !exit.IsHidden()))
            {                           
                if (Character.IsInGroupWith(exit) == true)
                {
                    if (Character.LookAt == Character.CurrentRoom)
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
                                "fl",
                                $"Flee combat and enter {exit.GoToRoom.Name}.",
                                new SkillNumber(Skill.Acrobatics, DC),
                                true, //should be a nonpersonal process?
                                (Soul s) =>
                                {
                                    RoomChange roomChange = new RoomChange(this, exit);
                                    roomChange.Action();
                                    return null;
                                },
                                (Soul s) =>
                                {
                                    return $"{Character.Name} failed to flee combat...";
                                }));
                        }
                    }
                }
                else if (ReferenceEquals(Character.LookAt, Character.CurrentRoom))
                {                       
                    AddPossibleMove(new MoveTo(this, exit));
                }
            }           

            //Back..
            AddPossibleBackMove();

            //inventory
            AddPossibleMove(new LookInventory(this));
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
                        () =>
                        {
                            dialogueManager.SetCurrentNodeBasedOnChoice(choice);

                            if (dialogueManager.CurrentNode.Dialogue)                            
                                dialogueManager.SpeakCurrentNode();
                            if (!dialogueManager.CurrentNodeHasChoices())
                                dialogueManager.EndSpeakWith();
                        }
                        ));
        }

        private void AddPossibleBackMove()
        {
            int lookedAtIndex = Character.LookedAt.Count - 1;
            if (lookedAtIndex > 0 && Character.LookAt != null)
            {
                Move backMove = new Move(
                    "b",
                    "Back..",
                    async () =>
                    {
                        Prop xLookedAt = Character.LookAt;
                        Character.LookBack();
                        await SendAsync(Character.Name + " stops looking at " + xLookedAt.Name + ".");
                    });
                backMove.Type = MoveType.MinorAction;

                AddPossibleMove(backMove);
            }
        }

        private void AddPossibleInvestigationOrLookMove(Prop prop)
        {
            if (prop.HasHiddenItems() && AddPossibleUnusedMove(new Investigate(this, prop)))
                return;



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

                Move? nextMove = GetMoveFromInput(input);
                if (nextMove != null)
                {
                    if (execute)
                        Execute(ref nextMove);
                    await SendAsync(InputOk());
                    return nextMove;
                }
                else
                {
                    if (Socket == null)
                        return nextMove;
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

        public void Execute(ref Move move)
        {
            if (!Character.PropTargetIsValid(move))
                return;
            
            Character.RegisterUsedMoveOnProp(move);

            if (move.EnablesCombat)
            {
                if (Character.IsHidden())
                    Character.CurrentRoom.BroadcastToSoulsInRoom($"{Character.Name} reveals themselves from the shadows...");

                move.Action();

                if (Character.IsHidden())
                    Character.Unhide();

            }
            else
                move.Action();

            move.ExecutePostAction(Character);
         
            if (move.EnablesCombat)
            {
                Character? targetCharacer = null;
                if (move.Prop is Item)
                {
                    Item item = (Item)move.Prop;
                    Character? heldByCharacter = item.HeldByCharacter();
                    if (heldByCharacter != null || heldByCharacter != Character)
                    {
                        targetCharacer = heldByCharacter;
                    }
                }
                else if (move.Prop is Character)
                    targetCharacer = (Character)move.Prop;

                if (targetCharacer != null && move.EnablesCombat)
                    Character.EnableCombatWith = targetCharacer;
            }
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
            if (Character == null)
                throw new ArgumentNullException(nameof(Character), "Character cannot be null when adding possible move");

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

            //LastRoom.TestCombatIsResolved();

            await SendAsync(goToRoom.GetFullRoomDescription(Character));

            if (goToRoom.OnEnterEvent != null)
                goToRoom.OnEnterEvent(this);

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
                                                                (x.Value is MoveTo && x.Value.Prop is Character && Character.IsInHostileCombatWith((Character)x.Value.Prop))).ToDictionary();

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
                return relevantAttackMoves.ElementAt(rnd.Next(relevantAttackMoves.Count)).Value;
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
            Console.WriteLine("Soul has been been banished.");
        }
    }
}
