using System.Collections.Concurrent;
using System.Diagnostics.Eventing.Reader;
using fire_ash_server.Props;
using fire_ash_server.World;

namespace fire_ash_server
{
    internal class WorldTick()
    {
        public ConcurrentQueue<Action> WorldQueue = new();

        private bool runningRooms = true;
        private bool runningWorldLoop = true;
        public DateTime LastTurn = DateTime.UtcNow;

        /*
        public void StartWorldEventQueue()
        {
            Thread loopThread = new Thread(() =>
            {
                while (runningWorldLoop)
                {
                    if(WorldQueue.TryDequeue(out var action))
                    {
                        action(); // executes in order
                    }
                    else
                        Thread.Sleep(1);
                }
            });
            loopThread.IsBackground = true;
            loopThread.Start();
        }
        */

        public void StartRoomsLoop(int tickMilliseconds = 1000)
        {
            Thread loopThread = new Thread(() =>
            {
                while (runningRooms)
                {
                    LastTurn = CheckIfNewTurn(LastTurn);

                    foreach (Room room in Program.WorldSoul.Rooms.Values)
                    {
                        room.Update?.Invoke();
                        foreach (Character character in room.Characters.Where(c => !c.Dead).ToList())
                        {
                            character.TickConditionsDown(true);
                        }

                        foreach (Character character in room.Characters.Where(c => c.Soul.IsDaemon).ToList())
                        {
                            if (character.Dead)
                            {
                                DateTime expiryTime = character.TimeOfDeath.AddMinutes(character.UniqueName ? 10000 : 20);

                                if (DateTime.UtcNow > expiryTime)
                                    character.ConsumeCorpse();
                            }

                            if (!character.InCombat)
                            {
                                character.Update?.Invoke();

                                Action<Character>? behavior;
                                Behavior.behaviors.TryGetValue(character.BehaviorKey, out behavior);
                                behavior?.Invoke(character);

                                character.RespawnItems();
                            }
                        }
                    }

                    Thread.Sleep(tickMilliseconds);
                    Program.NewGlobalTurn = false;
                }
            });
            loopThread.IsBackground = true;
            loopThread.Start();
        }

        private DateTime CheckIfNewTurn(DateTime lastTurn)
        {
            DateTime lastTurnPlusTurnTime = lastTurn.AddSeconds(Program.SecondsPerTurn);
            if (DateTime.UtcNow > lastTurnPlusTurnTime)
            {
                Program.NewGlobalTurn = true;
                return lastTurnPlusTurnTime;
            }

            return lastTurn;
        }

        public void StopWorldLoop()
        {
            runningWorldLoop = false;
        }

        public void StopRoomsLoop()
        {
            runningRooms = false;
            Program.NewGlobalTurn = false;
        }
    }
}