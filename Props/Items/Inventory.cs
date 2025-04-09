using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items
{
    [Serializable]
    internal class Inventory : Item
    {
        public Inventory() : base("Inventory", "Inventory", 0)
        {
            IsContainer = true;
            MakeUnpickupable();
            Sellable = false;
        }

        public bool ContainsItemWithName(string name)
        {
            return Items.Where(i => i.Name == name).Any();
        }
    }
}
