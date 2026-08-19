using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World.BioMechWorld.Temple;
using fire_ash_server.World.Goldfield;
using static fire_ash_server.World.ConsumableList;

namespace fire_ash_server.World
{
    static class ItemFactory
    {
        public static Dictionary<ItemFactoryKey, Func<Item>> Registry = new Dictionary<ItemFactoryKey, Func<Item>>();

        public static void InitDicts()
        {
            Registry.TryAdd(ItemFactoryKey.GoldBerryPie, GoldBerryPie);
            Registry.TryAdd(ItemFactoryKey.HealthPotion, HealthPotion);
        }
    }
}