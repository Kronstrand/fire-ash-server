using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items
{
    internal class Consumable : Item
    {
        public Action<Soul> Consume;
        public Consumable(string name, string description, Action<Soul> consume, double value) : base(name, description, value)
        {
            Consume = consume;
        }
    }
}
