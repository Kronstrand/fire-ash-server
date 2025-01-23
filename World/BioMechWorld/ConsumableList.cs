using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.BioMechWorld
{
    internal static class ConsumableList
    {
        public static Consumable HealthPotion()
        {
            return new Consumable(
                "Healing Potion",
                "A potion that restores health, healing for 1d4+2 health points when consumed.",
                (Soul soul) =>
                {
                    Roll hpRoll = new Roll(
                        new Die(1, 4), 
                        2, 
                        RollType.None, 
                        soul.Character);

                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} drinks a healing potion, rolls {hpRoll} and gains {hpRoll.GetSum()} health points.");
                    soul.Character.GainLife(hpRoll.GetSum());
                },
                30);
        }
    }
}
