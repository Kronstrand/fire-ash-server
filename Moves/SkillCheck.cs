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
    [Serializable]
    internal class SkillCheck : Move
    {
        public SkillNumber SkillNumber { get; set; }
        private Func<Soul, Task<string>?> successFunc;
        private Func<Soul, Task<string>?>? failFunc;
        public bool IsPersonal = true;

        public SkillCheck(string? key, string description, SkillNumber skillNumber, bool isPersonal, Func<Soul, Task<string>?> successFunc, Func<Soul, Task<string>?>? failFunc) : base(GetKey(key, skillNumber.Skill), description)
        {
            Range = RangeType.RangeSingleTarget;
            SkillNumber = skillNumber;
            Repeatable = false;
            this.successFunc = successFunc;
            this.failFunc = failFunc;
        }

        private static string GetKey(string? key, Skill skill)
        {
            if (key != null)
                return key;

            if (skill == Skill.Religion)
                return "r";
            else if (skill == Skill.Hacking)
                return "h";
            else return "s";
        }

        public SkillCheck(Soul soul, Prop prop, string? key, string description, SkillNumber skillNumber, bool isPersonal, Func<Soul, Task<string>?> successFunc, Func<Soul, Task<string>?>? failFunc) : base(GetKey(key, skillNumber.Skill), CreateDescription(description, skillNumber.Skill))
        {
            Range = RangeType.RangeSingleTarget;
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
            SkillCheck possibleMove = new SkillCheck(Key,CreateDescription(Description, SkillNumber.Skill), SkillNumber, IsPersonal, successFunc, failFunc);
            possibleMove.Prop = prop;
            PropPosition = prop.GetPropPosition();
            possibleMove.Action = CreateAction(soul);
            return possibleMove;
        }

        public static string CreateDescription(string description, Skill skill)
        {
            return $"{description} ({skill})";
        }

        private Func<Task> CreateAction(Soul soul)
        {

            return async () =>
            {
                Roll roll = new Roll(soul.Character.GetModifer(SkillNumber.Skill), RollType.SkillCheck, soul.Character);

                bool sucess = roll.GetSum() >= SkillNumber.number;
                await MessageHandler(sucess, soul, roll);         
            };
        }

        private async Task MessageHandler(bool success, Soul soul, Roll roll)
        {
            string rollMessage = "";
            if (success)
                rollMessage = $"{soul.Character.Name} rolls {roll} and succeeds.";
            else
                rollMessage = $"{soul.Character.Name} rolls {roll} and fails.";

            await soul.SendAsync(rollMessage, IsPersonal);


            Task<string>? result = null;
            if (success)
                result = successFunc(soul);
            else if (failFunc != null)
                result = failFunc(soul);
            else
                return;

            if (result != null) { 
                string resultString = await result;
                if (resultString != "")
                    await soul.SendAsync(resultString, IsPersonal);
            }
            
        }
    }
}

