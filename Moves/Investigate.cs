using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;

namespace fire_ash_server.Moves
{
    internal class Investigate : Move
    {
        public Investigate(Soul soul, Prop prop) : base("i", CreateDescription(prop), prop)
        {
            Repeatable = false;
            Action = CreateAction(soul, prop);
        }

        private static string CreateDescription(Prop prop)
        {
            return $"Examine {prop.Name}. (Investigation)";
        }

        private Action CreateAction(Soul soul, Prop prop)
        {
            return async () =>
            {
                soul.Character.SetLookAt(prop);
                Roll investigationRoll = new Roll(soul.Character.GetModifer(Skill.Investigation), RollType.SkillCheck, soul.Character);
                await InvestigateProp(soul, prop, investigationRoll);
            };
        }

        private async Task InvestigateProp(Soul soul, Prop prop, Roll investigationRoll)
        {
            List<Item> foundItems = prop.FoundItems(investigationRoll.GetSum());
            if (foundItems.Count > 0)
            {
                foreach (Item item in foundItems)
                {
                    item.Unhide();
                }

                if (prop.GetType() == typeof(Room))
                    await soul.SendAsync($"{soul.Character.Name} rolls {investigationRoll} and finds {ListToString(foundItems)}.");
                else
                    await soul.SendAsync(
                        $"{prop.GetDescription()}\n\n" +
                        $"{soul.Character.Name} rolls {investigationRoll} and finds {ListToString(foundItems)}.");
            }
            else
            {
                if (prop.GetType() == typeof(Room))
                    await soul.SendAsync($"{soul.Character.Name} rolls {investigationRoll} and didn't seem to find anything...");
                else
                    await soul.SendAsync(
                        $"{prop.GetDescription()}\n\n" +
                        $"{soul.Character.Name} rolls {investigationRoll} and didn't seem to find anything...");
            }
        }
    }
}
