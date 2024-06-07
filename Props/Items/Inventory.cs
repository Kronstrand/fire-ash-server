using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items
{
    internal class Inventory : Item
    {
        public Inventory() : base("Inventory", "Inventory")
        {
            IsContainer = true;
            MakeUnpickupable();
        }
    }
}
