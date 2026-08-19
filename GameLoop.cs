using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
//using System.Net.Sockets;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World;
using fire_ash_server.World.BioMechWorld;
using fire_ash_server.World.Goldfield;
using Microsoft.Extensions.Options;
using static fire_ash_server.Helpers;

namespace fire_ash_server
{
    internal class GameLoop
    {     
        public async Task Open(int port, string[] args)
        {
            PrintLogo();
            
            ConsumableList.InitConsumableDicts();
            ItemFactory.InitDicts();
            Events.InitEvents();
            Behavior.InitBehavior();
            Dialogues.InitDicts();

            Program.WorldSoul.World = GoldfieldCreator.Create(
                !File.Exists(
                    Path.Combine(Program.SaveFolder, "soulstoned_NPCs.json")));

            //Program.WorldSoul.World = new BioMechCreator();
            //Program.WorldSoul.World = new CyberTempleCreator();


            LoadPersistedWorld();
            //Program.WorldTick.StartWorldEventQueue();
            Program.WorldTick.StartRoomsLoop();

            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.UseWebSockets();

            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    Console.WriteLine("WebSocket connected");

                     await NewSoul(webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            });

            Console.WriteLine("Starting server on port " + port);
            await app.RunAsync($"http://0.0.0.0:{port}");
        }

        public void CleanUpThreadBufferText()
        {
            foreach (Thread thread in Program.WorldSoul.ThreadBufferText.Keys.ToList())
            {
                if (!thread.IsAlive)
                {
                    Program.WorldSoul.ThreadBufferText.TryRemove(thread, out _);
                }
            }
        }

        //public void NewSoul(Socket soulSocket)
        public async Task NewSoul(WebSocket soulSocket)
        {
            Soul soul = new Soul(soulSocket);
            await EnterGame(soul);
        }

        private async Task EnterGame(Soul soul)
        {
            try
            {
                Console.WriteLine("A soul entered the world.");
                _ = soul.ReceiveLoop();

                string path = Path.Combine(Program.SaveFolder, "player.json");

                if (false/*File.Exists(path)*/)
                {
                    string loadedJson = File.ReadAllText(path);

                    // Deserialize
                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = false,
                        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                    };

                    soul.Character = JsonSerializer.Deserialize<Character>(loadedJson, options);
                    soul.Character.Soul = soul;
                    Console.WriteLine("Character Loaded");
                }
                else
                {
                    soul.Character = new Character(soul, "Player" + Program.WorldSoul.Souls.Count);
                    /*soul.Character.Strength = 14;
                    soul.Character.Dexterity = 13;
                    soul.Character.Constition = 11;
                    soul.Character.Intelligence = 12;
                    soul.Character.Wisdom = 10;
                    soul.Character.Charisma = 10;*/
                    //soul.Character.AddFeat(FeatKey.Stealth);
                    soul.Character.AddFeat(FeatKey.MeleeAttack);
                    //soul.Character.AddFeat(FeatKey.DualWield);
                    soul.Character.AddFeat(FeatKey.RangedAttack);
                    //soul.Character.AddToInventory(new Coins(20, 0));

                    /*soul.Character.AddFeat(FeatKey.Stealth);
                    soul.Character.AddToInventory(ConsumableList.ARC2000());
                    soul.Character.AddToInventory(WeaponList.ColtARFifteen());
                    soul.Character.AddToInventory(WeaponList.Machete());
                    soul.Character.AddToInventory(ArmorList.MetalShield());
                    soul.Character.AddToInventory(ArmorList.DriftersVest());

                    soul.Character.AddToInventory(ConsumableList.ScrollOfEntanglement());
                    soul.Character.AddToInventory(ConsumableList.BearTrap());
                    soul.Character.AddToInventory(WeaponList.HolographicBlade());
                    soul.Character.AddToInventory(WeaponList.LuminarBaton());
                    soul.Character.AddToInventory(ArmorList.NocturnalOptics());
                    soul.Character.AddToInventory(ArmorList.DriftersVest());
                    soul.Character.AddToInventory(new Coins(7000, 30));*/

                    await soul.SendAsync("Welcome to Fire & Ashes: Death of Solthera.");

                    await soul.SendAsync("The world continues in the aftermath. " +
                        "Its regions no longer share a common rhythm. Each has settled into its own way of enduring.\n\n" +
                        "You begin in Goldfield, where human settlements and farmlands preserve the appearance of a world that has already ended. " +
                        "People trade, tend failing crops, and repeat old prayers with quiet, fragile insistence, pretending the old rules still apply. " +
                        "It is a place of anxious dignity, where everyone knows the soil is fading, but no one dares to say it aloud.");

                                        /*
                                        await soul.SendAsync(
                                           "Welcome to Fire & Ashes: Death of Solthera.");

                                        await soul.SendAsync(
                                            "The world continues in the aftermath of the Death of Solthera.\n\n" +

                                            "Its regions no longer share a rhythm. Each follows its own distinct order.\n\n" +

                                            "In Goldfield, human settlements and farmlands preserve the appearances of a world that has already ended. People trade, tend failing crops, and repeat old prayers with a quiet, fragile insistence, pretending the old rules still apply. It is a place of anxious dignity, where everyone knows the soil is fading but no one dares to say it aloud.\n\n" +

                                            "In Rot-Blight, orc-held steads and industrial decay are bound into grim routine along the wetlands. Heat, manual toil, and rot are part of daily life there, folded into the same repeated processes that keep things functioning.\n\n" +

                                            "You do not arrive at the world as a whole, only within one of its continuing places.\n\n" +

                                            "Where you will take your place?"
                                            );*/

                    //string locations = $"1) Goldfield" + "\n2) The Rot-Blight Steading";
                    //await soul.SendPossibleMovesAsync(locations);

                    RoomKey startingRoom = RoomKey.GoldfieldSquare;
                    soul.Character.Kindred = Kindred.Human;
                    /*
                    while (true)
                    {
                        string startingInput = await soul.AwaitInput(false);
                        if (startingInput == "1")
                        {
                            soul.Character.Kindred = Kindred.Human;
                            soul.Character.SetFaction(FactionKey.Goldfield);
                            startingRoom = RoomKey.GoldfieldSquare;
                            break;
                        }
                        else if (startingInput == "2")
                        {
                            soul.Character.Kindred = Kindred.Orc;
                            
                            soul.Character.Strength += 2;
                            soul.Character.Dexterity -= 1;
                            soul.Character.Constition += 2;
                            soul.Character.Intelligence -= 1;
                            soul.Character.Charisma -= 2;
                            soul.Character.SetFaction(FactionKey.KettleKeepers);
                            startingRoom = RoomKey.RotBlightSteading;
                            break;
                        }

                        await soul.SendAsync("Invalid input. Please choose a gender.");
                        await soul.SendPossibleMovesAsync(locations);
                    }
                    */

                    await soul.SendAsync("What gender are you?");

                    string genderChoices = $"1) Male" + "\n2) Female" + "\n3) Dual-soul";
                    await soul.SendPossibleMovesAsync(genderChoices);

                    while (true)
                    {
                        string genderInput = await soul.AwaitInput(false);
                        if (genderInput == "1" || genderInput == "m")
                        {
                            Random rand = new Random();
                            soul.Character.Intelligence += (rand.Next(2) == 0) ? 1 : -1;
                            soul.Character.Gender = Gender.Male;
                            break;
                        }
                        else if (genderInput == "2" || genderInput == "f")
                        {
                            soul.Character.Gender = Gender.Female;
                            soul.Character.Strength -= 1;
                            soul.Character.Wisdom += 1;
                            break;
                        }
                        else if (genderInput == "3" || genderInput == "d")
                        {
                            soul.Character.Gender = Gender.DualSoul;
                            soul.Character.Constition -= 3;
                            soul.Character.Wisdom += 1;
                            soul.Character.Charisma += 1;
                            soul.Character.Intelligence += 1;
                            break;
                        }
                        await soul.SendAsync("Invalid input. Please choose a gender.");
                        await soul.SendPossibleMovesAsync(genderChoices);
                    }

                    await soul.SendAsync($"Ah, a {Description(soul.Character.Gender).ToLower()}. What name lingers in your mind? (Enter name)");
                    soul.Character.Name = await soul.AwaitInput(true);

                    await soul.SendAsync("A name remembered.");

                    soul.Character.HP = 8 + soul.Character.GetModifer(Ability.Constitution);
                    string messageToSoul =
                        "This is your character\n" +
                        soul.Character.StatsToString();
                    await soul.SendAsync(messageToSoul + "\n\n");

                    await soul.MoveCharToRoomAndSendDescriptionAsync(startingRoom);
                }

                
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.AbandonedArcade);                
                //if (Program.WorldSoul.World == null)
                //    throw new Exception("World is not initiatited and is null");
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.GoldfieldSquare);
                //await soul.MoveCharToRoomAndSendDescriptionAsync(Program.WorldSoul.World.startingRoom.CreateIncubator());
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.ThresholdOfTheNameless);

                GoldfieldCreator.SetFactions();

                soul.InitToolTipCounters();

                while (true)//(!soul.Character.Dead)
                {
                    if (soul.Socket == null) //soul has been unsockedet
                        return;
                    if (!soul.Character.InCombat)
                    {
                        soul.ClearMoves();
                        soul.GeneratePossibleMoves();
                        
                        if (soul.AllPossibleMoves.Count > 0)
                        {
                            
                            await soul.SendAndReceiveMoveOutOfCombatAsync();
                            soul.Character.TryEnableCombat();
                        }
                        else
                            await Task.Delay(100);
                    }
                    else
                        await Task.Delay(400);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client disconnected with error: {ex.Message}");
            }
            finally
            {
                await soul.BanishAsync();
            }
        }

        private void LoadPersistedWorld()
        {
            // Deserialize
            var options = new JsonSerializerOptions
            {
                IncludeFields = false,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };


            string fileNameItemLimits = Path.Combine(Program.SaveFolder, "ItemLimits.json");
            if (File.Exists(fileNameItemLimits))
            {
                string loadedExitsSatesJson = File.ReadAllText(fileNameItemLimits);
                Dictionary<string, (int Current, int Maximum)>? itemLimits = JsonSerializer.Deserialize<Dictionary<string, (int Current, int Maximum)>>(loadedExitsSatesJson, options);
                if (itemLimits != null)
                    ItemPopulation.Limits = itemLimits;
            }

            foreach (Room room in Program.WorldSoul.Rooms.Values)
            {
                //Load Exits
                string fileNameExits = Path.Combine(Program.SaveFolder, $"{room.Name}_exitstates.json");

                if (File.Exists(fileNameExits))
                {
                    string loadedExitsSatesJson = File.ReadAllText(fileNameExits);

                    List<ExitState>? savedExitStatesInRoom = JsonSerializer.Deserialize<List<ExitState>>(loadedExitsSatesJson, options);
                    if (savedExitStatesInRoom != null)
                        foreach (ExitState exitState in savedExitStatesInRoom)
                            exitState.ConnectToExitInRoom(room);
                }

                //Load Items
                string fileName = Path.Combine(Program.SaveFolder, $"{room.Name}_items.json");
                if (File.Exists(fileName))
                {
                    string loadedJson = File.ReadAllText(fileName);

                    List<Item>? savedItemsInRoom = JsonSerializer.Deserialize<List<Item>>(loadedJson, options);

                    if (savedItemsInRoom != null)
                    {
                        foreach (Item savedItem in savedItemsInRoom)
                        {
                            if (!savedItem.WorldProp)
                            {
                                room.AddItem(savedItem);                               
                            }
                            else
                            {
                                Item? unpickupableItemInRoom = room.GetItemById(savedItem.Id);
                                if (unpickupableItemInRoom != null)
                                    savedItem.AddPersistetItemsRecursive(unpickupableItemInRoom);
                            }


                        }
                    }
                }

                //Load Characters
                string fileNameChars = Path.Combine(Program.SaveFolder, $"{room.Name}_characters.json");

                if (File.Exists(fileNameChars))
                {

                    string loadedCharsJson = File.ReadAllText(fileNameChars);

                    List<Character>? savedCharsInRoom = JsonSerializer.Deserialize<List<Character>>(loadedCharsJson, options);
                    if (savedCharsInRoom != null)
                        foreach (Character character in savedCharsInRoom)
                        {
                            character.Soul = new Soul(character);
                            character.GoToRoom(room);
                            character.SetDialogueManager();
                            if (!character.NPC)
                            {
                                character.Dies("");
                            }

                        }
                }
                //Load Groupings
                //string fileNameGroup = $"{room.Name}_groups.json";
                string fileNameGroup = Path.Combine(Program.SaveFolder, $"{room.Name}_groups.json");

                if (!File.Exists(fileNameGroup))
                    continue;

                string loadedGroupingsJson = File.ReadAllText(fileNameGroup);

                List<List<string>>? savedGroupingsInRoom = JsonSerializer.Deserialize<List<List<string>>>(loadedGroupingsJson, options);
                if (savedGroupingsInRoom != null)
                {
                    foreach (List<string> group in savedGroupingsInRoom)
                    {
                        Grouping grouping = new Grouping();
                        foreach (string propId in group)
                        {
                            Prop? prop = room.GetBaseLevelPropById(propId);
                            if (prop != null)
                                grouping.Props.Add(prop);
                        }

                        if (grouping.Props.Count >= 2)
                            room.Groupings.Add(grouping);
                    }
                }
                /*
                //cleanUp loaded items
                List<Item> allItems = Program.WorldSoul.GetAllItems();
                foreach(Item item in allItems)
                {
                    if (item.Subtype == PropSubtype.Soulstone)
                    {
                        item.ReplaceItem(ConsumableList.SoulstoneDust());
                    }
                }*/
            }

            //load soulstoned characters
            string fileNameSoulstoneChars = Path.Combine(Program.SaveFolder, $"soulstoned_NPCs.json");

            if (File.Exists(fileNameSoulstoneChars))
            {
                string loadedCharsJson = File.ReadAllText(fileNameSoulstoneChars);

                List<Character>? savedSoulstonedChars = JsonSerializer.Deserialize<List<Character>>(loadedCharsJson, options);
                if (savedSoulstonedChars != null)
                    foreach (Character character in savedSoulstonedChars)
                    {
                        character.Soul = new Soul(character);
                        character.SetDialogueManager();
                        Program.WorldSoul.SoulstonedCharacters.TryAdd(character.Id, character);
                    }
            }

            List<Consumable> Soulstones = Program.WorldSoul.GetAllItems().OfType<Consumable>().Where(c => c.Subtype == PropSubtype.Soulstone).ToList();
            foreach (Consumable soulStone in Soulstones)
            {
                Program.WorldSoul.SoulstonedCharacters.TryGetValue(soulStone.CharacterId, out Character? character);
                if (character != null)
                    character.Soul.PlacedInSoulstone = soulStone;
                else
                    soulStone.ReplaceItem(ConsumableList.SoulstoneDust());
            }
        }

        private void PrintLogo()
        {
            Console.WriteLine("\n\n\n\n");
            Console.WriteLine("                   ┏┓┳┳┓┏┓  ┏┓  ┏┓┏┓┓┏┏┓┏┓     ");
            Console.WriteLine("                   ┣ ┃┣┫┣   ┣╋  ┣┫┗┓┣┫┣ ┗┓     ");
            Console.WriteLine("                   ┻ ┻┛┗┗┛  ┗┻  ┛┗┗┛┛┗┗┛┗┛     ");
            Console.WriteLine("               ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("                 ┏┳┓┓┏┏┓  ┳┓┏┓┓ ┏  ┳┓┏┓┓ ┏┳┓   ");
            Console.WriteLine("                  ┃ ┣┫┣   ┃┃┣ ┃┃┃  ┃┃┣┫┃┃┃┃┃   ");
            Console.WriteLine("                  ┻ ┛┗┗┛  ┛┗┗┛┗┻┛  ┻┛┛┗┗┻┛┛┗   ");
            Console.WriteLine("\n\n\n\n");
        }
    }
}
