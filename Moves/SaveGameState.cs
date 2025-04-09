using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;

namespace fire_ash_server.Moves
{
    internal class SaveGameState : Move
    {
        public SaveGameState(Soul soul) : base(MoveKey.sg.ToString(), $"Save Game")
        {
            Type = MoveType.MinorAction;
            AllowedInCombat = false;
            Hidden = true;
            Action = async () =>
            {
                //PersistenceManager.SaveData(Program.WorldSoul, "WorldSoul.dat");
                //PersistenceManager.SaveData(Program.GlobalVariables, "GlobalVariables.dat");
                _ = soul.SendAsync("Game has been saved.");

                await Task.CompletedTask;
            };
        }
    }
}
