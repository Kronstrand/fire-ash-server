using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Armoring
{
    internal class Armor: Item
    {
        public int AC;

        public Armor() { }
        public Armor(string name, string description, int ac, double value) : base(name, description, value)
        {
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                InventorySlot.Body
            };
            AC = ac;
        }
    }
}
