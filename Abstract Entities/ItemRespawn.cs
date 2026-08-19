using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Abstract_Entities
{
    internal class ItemRespawn
    {
        public int MinMin;
        public int MaxMin;
        public int MaxItems;
        public ItemFactoryKey ItemFactoryKey;
        public DateTime NextRespawn = DateTime.UtcNow;

        public ItemRespawn() { }

        public ItemRespawn(int minMin, int maxMin, int maxItems, ItemFactoryKey itemFactoryKey)
        {
            MinMin = minMin;
            MaxMin = maxMin;
            MaxItems = maxItems;
            ItemFactoryKey = itemFactoryKey;
        }

        public void SetNextRespawnTime()
        {
            NextRespawn = DateTime.UtcNow.AddMinutes(GetRandomInt(MaxMin - MinMin + 1) + MinMin);
        }
    }
}
