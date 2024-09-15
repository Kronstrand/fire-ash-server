using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class Loot : Move
    {
        //not used.. look inventory used instead...
        public Loot(Soul soul, Character targetCharacter) : base(MoveKey.lo.ToString(), $"Loot {targetCharacter.Name}", targetCharacter)
        {
            Type = MoveType.MinorAction;
            Action = () =>
            {
                    soul.Character.SetLookAt(targetCharacter.Inventory);
            };
        }
    }
}
