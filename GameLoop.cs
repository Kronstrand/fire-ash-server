using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.World.BioMechWorld;
using static fire_ash_server.Helpers;
using System.Diagnostics.Tracing;
using fire_ash_server.Props.Items;

namespace fire_ash_server
{
    internal class GameLoop
    {
        public async Task Open(int port)
        {
            PrintLogo();

       
            //new WorldCreator(this);
            //new CyberworldCreater(this);
            Program.WorldSoul.World = new BioMechCreator();
            //World = new AncientTemple();


            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(100);
            Console.WriteLine("World soul created on port " + port + ".");

            while (true)
            {
                NewSoul(await listener.AcceptAsync());
                CleanUpThreadBufferText(); //better safe than sorry
            }
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

        public void NewSoul(Socket soulSocket)
        {
            Soul soul = new Soul(soulSocket);
            _ = EnterGame(soul);
        }

        private async Task EnterGame(Soul soul)
        {
            try
            {
                Console.WriteLine("A soul entered the world.");

                soul.Character = new Character(soul, "Player" + Program.WorldSoul.Souls.Count);
                soul.Character.Strength = 14;
                soul.Character.Dexterity = 13;
                soul.Character.Constition = 11;
                soul.Character.Intelligence = 12;
                soul.Character.Wisdom = 10;
                soul.Character.Charisma = 10;

                soul.Character.HP += 8;
                //soul.Character.AddFeat(FeatKey.Stealth);
                soul.Character.AddFeat(FeatKey.MeleeAttack);
                //soul.Character.AddFeat(FeatKey.DualWield);
                soul.Character.AddFeat(FeatKey.RangedAttack);

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

                await soul.SendAsync(
                    "Welcome to Fire & Ashes: The Mecharions.");

                await soul.SendAsync("You awaken in the confines of a small, enclosed space. The interior is soft and warm, lined with a flesh-like material that pulsates gently, mimicking the rhythm of a heartbeat.");

                string genderChoices = $"1) Male" + "\n2) Female" + "\n3) Dual-soul";

                await soul.SendAsync("What gender are you?");
                await soul.SendPossibleMovesAsync(genderChoices);
                while(true)
                {
                    string genderInput = await soul.AwaitInput(false);
                    if (genderInput == "1" || genderInput == "m")
                    {
                        soul.Character.Gender = Gender.Male;
                        break;
                    }
                    else if (genderInput == "2" || genderInput == "f")
                    {
                        soul.Character.Gender = Gender.Female;
                        break;
                    }
                    else if (genderInput == "3" || genderInput == "d")
                    {
                        soul.Character.Gender = Gender.DualSoul;
                        break;
                    }
                    await soul.SendAsync("Invalid input. Please choose a gender.");
                    await soul.SendPossibleMovesAsync(genderChoices);
                }

                await soul.SendAsync($"Ah, a {Description(soul.Character.Gender).ToLower()}. What name lingers in your mind? (Enter name)");
                soul.Character.Name = await soul.AwaitInput(true);
                await soul.SendAsync("A name remembered.");

                string messageToSoul =
                    "This is your character\n" +
                    soul.Character.StatsToString();
                await soul.SendAsync(messageToSoul + "\n\n");

                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.WolfCave);
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.AbandonedArcade);                
                if (Program.WorldSoul.World == null)
                    throw new Exception("World is not initiatited and is null");
                await soul.MoveCharToRoomAndSendDescriptionAsync(Program.WorldSoul.World.startingRoom.CreateIncubator());
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.TempleCourtyard);

                BioMechCreator.SetFactions();

                soul.InitToolTipCounters();

                while (true)//(!soul.Character.Dead)
                {
                    if (soul.Socket == null) //soul has been unsockedet
                        return;
                    if (!soul.Character.InCombat)
                    {
                        soul.Character.TickConditionsDown(true, false); //this should be implemented better if ever used. Now conditions are just removed.
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
                soul.Banish();
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
