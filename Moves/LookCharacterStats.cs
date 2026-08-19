using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class LookCharacterStats : Move
    {
        public  LookCharacterStats(Soul soul) : base(MoveKey.c.ToString(), "Character Stats")
        {
            Type = MoveType.MinorAction;
            AllowedInTrade = true;
            Hidden = true;

            Action = async () =>
            {
                string output =
                        soul.Character.StatsToString() + "\n\n";

                if (soul.Character.BuffDebuffs.Any())
                {
                    foreach (BuffDebuff buffDebuff in soul.Character.BuffDebuffs)
                    {
                        if (soul.Character.InCombat)
                            output += $"{buffDebuff.Name} - turns left: {buffDebuff.Turns}.";
                        else
                        {
                            int elapsed = (int)(DateTime.UtcNow - Program.WorldTick.LastTurn).TotalSeconds;
                            output += $"{buffDebuff.Name} - time left: {FormatTimeLeft(buffDebuff.Turns * Program.SecondsPerTurn - elapsed)}.";
                        }


                        if (buffDebuff.Condition != null)
                        {
                            output += $"\n  {Description(buffDebuff.Condition)}";
                        }

                        if (buffDebuff.Effect != null)
                        {
                            foreach (RollModifier rollModidifer in buffDebuff.Effect.rollModifiers)
                            {
                                output += $"\n- {rollModidifer.ToString()}";
                            }
                            if (buffDebuff.Effect.LightRadiusModifer != Light.None)
                                output += $"\n- Light Radius Modifer: {buffDebuff.Effect.LightRadiusModifer}";
                            if (buffDebuff.Effect.LightPointerModifer != Light.None)
                                output += $"\n- Light Pointer Modifer: {buffDebuff.Effect.LightPointerModifer}";
                        }
                        output += "\n\n";
                    }
                }
                else
                    output += "No buffs or debuffs.";

                await soul.SendAsync(output);
                               
            };
        }
    }
}
