using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class SkillCheck : Move
    {
        public SkillNumber SkillNumber { get; set; }
        private Func<string?> successFunc;
        private Func<string?>? failFunc;

        public SkillCheck(string key, string description, SkillNumber skillNumber, Func<string?> successFunc, Func<string?>? failFunc) : base(key, description)
        {
            SkillNumber = skillNumber;
            Repeatable = false;
            this.successFunc = successFunc;
            this.failFunc = failFunc;
        }

        public SkillCheck(Soul soul, Prop prop, string key, string description, SkillNumber skillNumber, Func<string?> successFunc, Func<string?>? failFunc) : base(key, CreateDescription(description, skillNumber.Skill))
        {
            SkillNumber = skillNumber;
            Repeatable = true;
            this.successFunc = successFunc;
            this.failFunc = failFunc;
            Action = CreateAction(soul);
            Prop = prop;
            PropPosition = prop.GetPropPosition();
        }

        public SkillCheck CreatePossibleMove(Soul soul, Prop prop)
        {
            SkillCheck possibleMove = new SkillCheck(Key, CreateDescription(Description, SkillNumber.Skill), SkillNumber, successFunc, failFunc);
            possibleMove.Prop = prop;
            PropPosition = prop.GetPropPosition();
            possibleMove.Action = CreateAction(soul);
            return possibleMove;
        }

        public static string CreateDescription(string description, Skill skill)
        {
            return $"{description} ({skill})";
        }

        private Action CreateAction(Soul soul)
        {

            return async () =>
            {
                Roll roll = new Roll(soul.Character.GetModifer(SkillNumber.Skill), RollType.SkillCheck, soul.Character);

                if (roll.GetSum() >= SkillNumber.number)
                        await MessageHandler(successFunc(), soul, roll);
                else
                {
                    if (failFunc == null)
                        await soul.SendAsync($"{soul.Character.Name} rolled {roll} and failed the {SkillNumber.Skill} check...");
                    else
                            await MessageHandler(failFunc(), soul, roll);
                }          
            };
        }

        private async Task MessageHandler(string? result, Soul soul, Roll roll)
        {
            if (result == null) 
                return;

            string message = $"{soul.Character.Name} rolled {roll}.";
            if (result != null)
                message += " " + result;

            await soul.SendAsync(message);
        }
    }
}

