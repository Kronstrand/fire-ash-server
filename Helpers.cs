using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.World;

namespace fire_ash_server
{
    internal static class Helpers
    {
        private static int randomIdentifer = 0;
        public static Dictionary<string, string> irregularNouns = new Dictionary<string, string>();

        static Dictionary<int, string> numberWords = new Dictionary<int, string>
        {
            {1, "one"}, {2, "two"}, {3, "three"}, {4, "four"}, {5, "five"},
            {6, "six"}, {7, "seven"}, {8, "eight"}, {9, "nine"}, {10, "ten"},
            {11, "eleven"}, {12, "twelve"}, {13, "thirteen"}, {14, "fourteen"},
            {15, "fifteen"}, {16, "sixteen"}, {17, "seventeen"}, {18, "eighteen"},
            {19, "nineteen"}, {20, "twenty"}, {21, "twenty-one"}, {22, "twenty-two"},
            {23, "twenty-three"}, {24, "twenty-four"}, {25, "twenty-five"},
            {26, "twenty-six"}, {27, "twenty-seven"}, {28, "twenty-eight"},
            {29, "twenty-nine"}, {30, "thirty"}, {31, "thirty-one"}, {32, "thirty-two"},
            {33, "thirty-three"}, {34, "thirty-four"}, {35, "thirty-five"},
            {36, "thirty-six"}, {37, "thirty-seven"}, {38, "thirty-eight"},
            {39, "thirty-nine"}, {40, "forty"}, {41, "forty-one"}, {42, "forty-two"},
            {43, "forty-three"}, {44, "forty-four"}, {45, "forty-five"},
            {46, "forty-six"}, {47, "forty-seven"}, {48, "forty-eight"},
            {49, "forty-nine"}, {50, "fifty"}, {51, "fifty-one"}, {52, "fifty-two"},
            {53, "fifty-three"}, {54, "fifty-four"}, {55, "fifty-five"},
            {56, "fifty-six"}, {57, "fifty-seven"}, {58, "fifty-eight"},
            {59, "fifty-nine"}, {60, "sixty"}, {61, "sixty-one"}, {62, "sixty-two"},
            {63, "sixty-three"}, {64, "sixty-four"}, {65, "sixty-five"},
            {66, "sixty-six"}, {67, "sixty-seven"}, {68, "sixty-eight"},
            {69, "sixty-nine"}, {70, "seventy"}, {71, "seventy-one"}, {72, "seventy-two"},
            {73, "seventy-three"}, {74, "seventy-four"}, {75, "seventy-five"},
            {76, "seventy-six"}, {77, "seventy-seven"}, {78, "seventy-eight"},
            {79, "seventy-nine"}, {80, "eighty"}, {81, "eighty-one"}, {82, "eighty-two"},
            {83, "eighty-three"}, {84, "eighty-four"}, {85, "eighty-five"},
            {86, "eighty-six"}, {87, "eighty-seven"}, {88, "eighty-eight"},
            {89, "eighty-nine"}, {90, "ninety"}, {91, "ninety-one"}, {92, "ninety-two"},
            {93, "ninety-three"}, {94, "ninety-four"}, {95, "ninety-five"},
            {96, "ninety-six"}, {97, "ninety-seven"}, {98, "ninety-eight"},
            {99, "ninety-nine"}, {100, "one hundred"}
        };

        public static int GetNextId()
        {
            return randomIdentifer++;
        }

        public static string Description(Enum value)
        {
            var descriptionAttribute = value.GetType()
                                            .GetField(value.ToString())
                                            ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                            .OfType<DescriptionAttribute>()
                                            .FirstOrDefault();

            // Return the description if found; otherwise, return the default string representation of the enum
            return descriptionAttribute?.Description ?? value.ToString();
        }

        public static int[] Roll(int numberOfDies, int die)
        { 
            Random rnd = new Random();
            int[] dieRolls = new int[numberOfDies];

            for (int i = 0; i < numberOfDies; i++)
            {
                dieRolls[i] = rnd.Next(1, die);
            }
            return dieRolls;
        }

        public static int CalculateModifer(int value)
        {
            return (value - 10) / 2;
        }

        public static string Img(string imgName)
        {
            return "$[img]" + imgName + "$[imgend]";
        }

        public static string FastText(string text)
        {
            return "$[fast]" + text + "$[fastend]";
        }

        public static string InputOk()
        {
            return "$[validinput]";
        }

        public static string ListToString<T>(List<T> list) where T : Prop
        {
            string output = "";
            int lengthOfList = list.Count;
            for (int i = 0; i < lengthOfList; i++)
            {
                //first item
                if (string.IsNullOrEmpty(output)) 
                {
                    output = list[i].Name;
                }
                //not last item
                else if (i + 1 != lengthOfList) 
                {
                    output += $", {list[i].Name}";
                }
                //last item
                else
                {
                    output += $", and {list[i].Name}";
                }
            }
            return output;
        }

        public static string SingleHitMessage(Roll roll)
        {
            return $"{FastText("HITS!")} - With a roll of {roll}.";
        }
        public static string SingleMissMessage(Roll roll)
        {
            return $"misses with a roll of {roll}.";
        }
        public static string GetCountedElement(Dictionary<string, Tuple<int, bool>> countedNamesInRoom, int i)
        {
            string elementName = countedNamesInRoom.ElementAt(i).Key;
            if (countedNamesInRoom[elementName].Item1 == 1)
                if (countedNamesInRoom[elementName].Item2 == true) //is unique name
                    return $"{elementName}";
                else
                    return $"one {elementName}";
            else
                return  $"{NumberToWord(countedNamesInRoom[elementName].Item1)} {GetPluralizedName(elementName)}";
        }

        static string GetPluralizedName(string creature)
        {
            // Check if the creature is in the irregulars dictionary
            if (irregularNouns.ContainsKey(creature))
            {
                return irregularNouns[creature];
            }
            // Regular pluralization rules
            else if (creature.EndsWith("y") && "aeiou".IndexOf(creature[creature.Length - 2]) == -1)
            {
                return creature.Substring(0, creature.Length - 1) + "ies";
            }
            else if (creature.EndsWith("s") || creature.EndsWith("x") || creature.EndsWith("z") || creature.EndsWith("sh") || creature.EndsWith("ch"))
            {
                return creature + "es";
            }
            else
            {
                return creature + "s";
            }
        }

        public static string NumberToWord(int number)
        {
            if (numberWords.ContainsKey(number))
            {
                return numberWords[number];
            }
            else
            {
                return "" + number;
            }
        }
    }
}
