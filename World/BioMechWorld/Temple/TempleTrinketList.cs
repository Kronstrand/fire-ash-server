using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal static class TempleTrinketList
    {
        public static Item GetRandom()
        {
            // Create a list of trinkets with their values
            List<Item> trinkets = new List<Item>{
                new Item("Ceremonial Feather", "A single, ornate feather that appears to have been used in some kind of ritual. It's vibrant and colorful, with shades of deep blue and gold. The feather is surprisingly well-preserved, as if it's been protected from the passage of time. It gives off a faint, pleasant aroma, reminiscent of incense.", 0.5),
                new Item("Ancient Coin", "A coin from a long-forgotten civilization. Its surface is worn, but faint engravings of an unknown language can still be seen.", 1.0),
                new Item("Glimmering Pebble", "A small, smooth pebble that shines with a light of its own. Its surface feels warm to the touch.", 0.2),
                new Item("Faded Map Fragment", "A piece of parchment with faded ink lines that might have once been part of a map. The edges are torn and brittle.", 0.3),
                new Item("Mechanical Insect", "A tiny clockwork insect, motionless. Its gears and tiny legs are intricately designed, hinting at a lost art of miniaturization.", 2.0),
                new Item("Mystical Amulet", "A small, round amulet with strange runes etched into its surface. A faint hum can be felt when holding it close.", 2.5),
                new Item("Old Compass", "A brass compass with a cracked glass face. Despite its age, the needle still points true.", 0.8),
                new Item("Tiny Music Box", "A diminutive music box that plays a haunting melody when opened. Its mechanism seems impossibly small.", 1.0)};

            // Initialize random number generator
            Random random = new Random();
            int randomIndex = random.Next(trinkets.Count);

            // Return a random trinket from the list
            return trinkets[randomIndex];
        }
    }
}
