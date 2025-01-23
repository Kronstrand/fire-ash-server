using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World.BioMechWorld;

namespace fire_ash_server.Moves
{
    internal class CheckJournal : Move
    {
        public CheckJournal(Soul soul) : base(MoveKey.j.ToString(), $"Check Journal")
        {
            Type = MoveType.MinorAction;
            //AllowedInCombat = false;
            Hidden = true;
            Action = async () =>
            {
                string message = "";
                foreach(KeyValuePair<JournalKey,List<JournalEntry>> kvp in soul.Character.Journal.Entries)
                {
                    if (message != "")
                        message += "\n";
                    else
                        message += "You read the following journal entries:\n\n";

                    message += Helpers.Description(kvp.Key);

                    int count = kvp.Value.Count();
                    int start = Math.Max(0, count - 3);  // Ensure start index is at least 0
                    for (int i = start; i < count; i++)
                    {
                        message += "\n\n+ " + kvp.Value[i].Text;    
                    }
                }
                _ = soul.SendAsync(message);

                await Task.CompletedTask;
            };
        }
    }
}