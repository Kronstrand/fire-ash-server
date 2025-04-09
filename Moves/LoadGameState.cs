using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.World;
using fire_ash_server.World.BioMechWorld;

namespace fire_ash_server.Moves
{
    internal class LoadGameState : Move
    {
        public LoadGameState(Soul soul) : base(MoveKey.lg.ToString(), $"Load Game")
        {
            Type = MoveType.MinorAction;
            AllowedInCombat = false;
            Hidden = true;
            Action = () =>
            {
                if (soul.Socket == null)
                    return Task.CompletedTask;
                
                //Program.WorldSoul = PersistenceManager.LoadData<WorldSoul>("WorldSoul.dat");
                //Program.GlobalVariables = PersistenceManager.LoadData<GlobalVariables>("GlobalVariables.dat");
                //Program.gameHasBeenLoaded = true;

                Soul? newIncarnation = Program.WorldSoul.GetSoul(soul.Id);
                if (newIncarnation != null)
                {
                    _ = newIncarnation.SendAsync("Game has been loaded.");
                }

                return Task.CompletedTask;
            };
        }
    }
}
