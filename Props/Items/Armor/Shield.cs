using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Armor
{
    [Serializable]
    internal class Shield: Item
    {
        public Shield(string name, string description, double value) : base(name, description, value) 
        {
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                InventorySlot.OffHand
            };
        }
    }
}
