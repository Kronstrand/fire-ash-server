using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World;

namespace fire_ash_server
{
    internal static class Helpers
    {
        private static int randomIdentifer = 0;
        public static Dictionary<string, string> irregularNouns = new Dictionary<string, string>();

        static Dictionary<int, string> numberWords = new Dictionary<int, string>
    {
        {1, "One"}, {2, "Two"}, {3, "Three"}, {4, "Four"}, {5, "Five"},
        {6, "Six"}, {7, "Seven"}, {8, "Eight"}, {9, "Nine"}, {10, "Ten"},
        {11, "Eleven"}, {12, "Twelve"}, {13, "Thirteen"}, {14, "Fourteen"},
        {15, "Fifteen"}, {16, "Sixteen"}, {17, "Seventeen"}, {18, "Eighteen"},
        {19, "Nineteen"}, {20, "Twenty"}, {30, "Thirty"}, {40, "Forty"},
        {50, "Fifty"}, {60, "Sixty"}, {70, "Seventy"}, {80, "Eighty"},
        {90, "Ninety"}, {100, "One Hundred"}, {1000, "One Thousand"}
    };

        public static string NumberToWord(int number)
        {
            if (number == 0)
            {
                return "Zero";
            }
            if (numberWords.ContainsKey(number))
            {
                return numberWords[number];
            }
            if (number < 100)
            {
                int tens = number / 10 * 10;
                int units = number % 10;
                return numberWords[tens] + "-" + numberWords[units];
            }
            if (number < 1000)
            {
                int hundreds = number / 100;
                int remainder = number % 100;
                return numberWords[hundreds] + " Hundred" + (remainder > 0 ? " and " + NumberToWord(remainder) : "");
            }
            if (number < 100000)
            {
                int thousands = number / 1000;
                int remainder = number % 1000;

                // Special handling for multiples of 10,000 (e.g., 10,000, 20,000)
                if (number % 10000 == 0)
                {
                    return NumberToWord(thousands) + " Thousand";
                }

                // Handling cases like 21,000, 35,000 etc.
                if (remainder == 0)
                {
                    return NumberToWord(thousands) + " Thousand";
                }

                // General cases like 12,345 etc.
                return NumberToWord(thousands) + " Thousand" + (remainder > 0 ? " and " + NumberToWord(remainder) : "");
            }

            return "Number out of range";
        }

        public static double SilverToGold(int silver)
        {
            return silver * 0.1;
        }

        public static int GetNextId()
        {
            return randomIdentifer++;
        }

        public static string ToLowerFirstChar(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            char firstChar = char.ToLower(str[0]);
            if (str.Length > 1)
            {
                return firstChar + str.Substring(1);
            }
            else
            {
                return firstChar.ToString();
            }
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

        public static string FormatPossessive(string name)
        {
            return name.EndsWith("s") ? name + "'" : name + "'s";
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
            return $"HITS! - With a roll of {roll}.";
        }
        public static string SingleMissMessage(Roll roll)
        {
            return $"misses with a roll of {roll}.";
        }
        public static string GetCountedElement(CountedCharacter countedName, string elementName)
        {
            string result = "";
            if (countedName.Count == 1)
                if (countedName.UniqueName == true)
                    result = $"{elementName}";
                else
                    result = $"one {elementName}";
            else
                result = $"{NumberToWord(countedName.Count).ToLower()} {GetPluralizedName(elementName)}";

            return result;
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

        public static void SetThreadBasedBufferText(string bufferText)
        {
            Program.WorldSoul.ThreadBufferText.GetOrAdd(Thread.CurrentThread, bufferText);
        }

        public static void RemoveBufferTextForThread()
        {
            Program.WorldSoul.ThreadBufferText.TryRemove(Thread.CurrentThread, out _);
        }

        public static string RemoveLastDot(string input)
        {
            // Check if the input string is not null or empty
            if (string.IsNullOrEmpty(input))
                return input;

            // Check if the last character is a '.'
            if (input[input.Length - 1] == '.')
            {
                // Return the string without the last character (the '.')
                return input.Substring(0, input.Length - 1);
            }

            // Return the original string if there is no '.' at the end
            return input;
        }

        public static Tuple<int, int> ConvertToGoldAndSilver(double value)
        {
            int integerPart = (int)Math.Floor(value);
            int fractionalPart = (int)((value - integerPart) * 10);

            return Tuple.Create(integerPart, fractionalPart);
        }

        public static Coins  GenerateCoins(int maxGold)
        {
            int gold = GenerateGold(maxGold);
            int silver = 0; // Default to 0 silver

            if (new Random().Next(0, 10) == 0) // 10% chance to generate silver
            {
                silver = GenerateSilver(100);
            }

            return new Coins(gold, silver);
        }

        public static int GenerateGold(int maxGold)
        {
            Random random = new Random();
            int gold = random.Next(maxGold / 2, maxGold + 1); // +1 to make maxGold inclusive
            return gold;
        }

        public static int GenerateSilver(int maxSilver)
        {
            Random random = new Random();
            int silver = random.Next(maxSilver / 2, maxSilver + 1); // Similar logic to gold generation
            return silver;
        }

        public static string PriceToString(Tuple<int, int> price)
        {
            //item1 = gold
            //item2 = silver
            string priceString;
            if (price.Item2 == 0)
                priceString = $"{price.Item1} gp";
            else if (price.Item1 == 0)
                priceString = $"{price.Item2} sp";
            else
                priceString = $"{price.Item1} gp, {price.Item2} sp";
            
            return priceString;
        }
    }
}
