using System;
using System.Net.Sockets;
using System.Net;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

namespace fire_ash_server.World
{
    internal class WorldSoul
    {
        public ThreadSafeList<Soul> Souls = new ThreadSafeList<Soul>();
        public Dictionary<RoomKey, Room> Rooms = new Dictionary<RoomKey, Room>();
        public List<Faction> Factions = new List<Faction>();
        public List<Relationship> Relationships = new List<Relationship>();
        public List<Feat> Features = new List<Feat>();

        public async Task Open(int port)
        {
            PrintLogo();

            new WorldCreator(this);
            //new CyberworldCreater(this);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, port));
            listener.Listen(100);
            Console.WriteLine("World soul created on port " + port + ".");

            while (true)
            {
                NewSoul(await listener.AcceptAsync());
            }
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
            Rooms.Add(room.RoomKey, room);
        }

        public Room GetRoom(RoomKey key)
        {
            return Rooms[key];
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

                soul.Character = new Character(soul, "Player" + Souls.Count);
                soul.Character.AddFeat(FeatKey.Stealth);
                soul.Character.AddFeat(FeatKey.MeleeAttack);
                soul.Character.AddFeat(FeatKey.DualWield);
                soul.Character.AddFeat(FeatKey.RangedAttack);
                soul.Character.AddFeat(FeatKey.PickPocket);

                string messageToSoul =
                "Welcome to Fire & Ashes.\n\n" +
                "This is your character\n" +
                soul.Character.StatsToString();
                await soul.SendAsync(messageToSoul);

                await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.WolfCave);
                //await soul.MoveCharToRoomAndSendDescriptionAsync(RoomKey.AbandonedArcade);

                while (!soul.Character.Dead)
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
