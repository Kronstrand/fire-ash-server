using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items
{
    [Serializable]
    internal class Coins : Item
    {
        public int Gold {  get; private set ; }
        public int Silver {  get; private set; }
        public Coins(int gold, int silver) : base(CreateName(gold, silver), CreateDescription(gold, silver), gold + SilverToGold(silver))
        {
            Gold = gold;
            Silver = silver;
            Sellable = false;
        }

        public void SetValues(int gold, int silver)
        {
            Gold = gold;
            Silver = silver;                                                            
            Name = CreateName(gold, silver);
            Description = CreateDescription(gold, silver);
        }

        private static string CreateName(int gold, int silver)
        {
            string goldText = "";
            if (gold == 1)
                goldText = $"{NumberToWord(gold)} Gold Coin";
            else if (gold > 1)
                goldText = $"{NumberToWord(gold)} Gold Coins";

            string silverText = "";
            if (silver == 1)
                silverText = $"{NumberToWord(silver)} Silver Coin";
            else if (silver > 1)
                silverText = $"{NumberToWord(silver)} Silver Coins";

            if (goldText != "" && silverText != "")
                return $"{goldText} and {silverText}";
            else if (goldText != "")
                return goldText ;
            else if (silverText != "")
                return silverText ;
            return "An empty space, where coins should have been."; //should not happen
        }

        private static string CreateDescription(int gold, int silver)
        {
            return CreateName(gold, silver) + ", so shiny.";
        }
        public static Coins GenerateCoins(int maxGold)
        {
            int gold = GenerateGold(maxGold);
            int silver = 0; // Default to 0 silver

            if (new Random().Next(0, 10) == 0) // 10% chance to generate silver
            {
                silver = GenerateSilver(100);
            }

            return new Coins(gold, silver);
        }

        private static int GenerateGold(int maxGold)
        {
            Random random = new Random();
            maxGold += 1; // +1 to make maxGold inclusive
            int gold = random.Next(maxGold / 2, maxGold); 
            return gold;
        }

        private static int GenerateSilver(int maxSilver)
        {
            Random random = new Random();
            maxSilver += 1; // Similar logic to gold generation
            int silver = random.Next(maxSilver / 2, maxSilver); 
            return silver;
        }
    }
}
