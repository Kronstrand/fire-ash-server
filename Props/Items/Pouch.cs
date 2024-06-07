using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items
{
    internal class Pouch : Item
    {
        public Pouch(string name, string description) : base(name, description)
        {
            //not implemeted
            //IsContainer = true;
            //CarriableByInventorySlots = new List<Enums.InventorySlot>() { InventorySlot.Waist };
        }
    }
}
