using System;
using System.Collections;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Armoring;

namespace fire_ash_server.Props.Items
{
    internal static class ItemPopulation
    {
        public static Dictionary<string, (int Current, int Maximum)> Limits = new()
        {
            [Names.WolfSkull] = (0, 5),
        };

        public static bool TrySpawn(string itemName)
        {
            if (!Limits.TryGetValue(itemName, out var limit))
                return true;

            double populationRatio = (double)limit.Current / limit.Maximum;
            double spawnChance = 1.0 - populationRatio;

            if (Random.Shared.NextDouble() >= spawnChance)
                return false;

            Limits[itemName] = (limit.Current + 1, limit.Maximum);
            return true;
        }

        public static void Destroy(Item item)
        {
            if (!Limits.TryGetValue(item.Name, out var limit))
                return;

            Limits[item.Name] = (Math.Max(0, limit.Current - 1), limit.Maximum);
        }

        public static void TryAddLimitedItem(Item item, Character addToCharacter)
        {
            if (TrySpawn(item.Name))
            {
                    addToCharacter.AddToInventory(item);
            }
        }
    }
}