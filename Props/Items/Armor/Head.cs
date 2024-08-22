using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Armor
{
    internal class Head : Item
    {
        public Head(string name, string description) : base(name, description)
        {
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                                                            InventorySlot.Head
                                                            };
        }
    }
}
