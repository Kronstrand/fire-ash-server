using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server
{
    internal class Roll
    {
        public int[] DieRolls;
        public int Modifier;
        public Character RollingCharacter;
        public RollType Type;
        public Roll(int modifier, RollType type, Character rollingCharacter)
        {
            RollingCharacter = rollingCharacter;
            DieRolls = Roll(1, 20);
            Modifier = modifier;
            Type = type;
        }

        public Roll(Die die, int modifier, RollType type, Character rollingCharacter)
        {
            RollingCharacter = rollingCharacter;
            DieRolls = Roll(die.NumberOfDies, die.Sides);
            Modifier = modifier;
            Type = type;
        }

        public bool BeatsDC(int dc)
        {
            return (GetSum() >= dc && !DieRolls.Contains(1));
        }

        public override string ToString()
        {
            int modifier = Modifier + GetCharacterEffectsModifier();

            if (modifier == 0)
            {
                if (DieRolls.Length == 1)
                    return $"{GetSum()}";
                return $"{GetSum()} ({string.Join(", ", DieRolls)})";
            }
            if (modifier < 0)
            {
                return $"{GetSum()} ({string.Join(", ", DieRolls)} - {modifier * -1})";
            }
            return $"{GetSum()} ({string.Join(", ", DieRolls)} + {modifier})";
        }
        public int GetSum()
        {
            int result = DieRolls.Sum() + Modifier + GetCharacterEffectsModifier();

            if (result < 1)
                return 1;

            return result;
        }

        public int GetCharacterEffectsModifier() 
        {
            int modifer = 0;
            foreach(Effect effect in RollingCharacter.GetAllEffectsIncludingFeats()) 
            {
                foreach(RollModifier rollModifier in effect.rollModifiers)
                {
                    if (Type == rollModifier.Type)
                    {
                        modifer += rollModifier.Modifer;
                    }
                }
            }
            return modifer;
        }
    }
}
