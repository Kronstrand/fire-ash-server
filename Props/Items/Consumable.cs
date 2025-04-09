using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Props.Items
{
    [Serializable]
    internal class Consumable : Item
    {
        public Func<Soul,Task> Consume;
        public Func<Soul, RangeType, Weapon?, Prop?, bool>? Requirement;
        public Action<Soul>? NotAvailable;
        public Weapon? Weapon;
        public RangeType Range = RangeType.None;
        public bool HasTarget = false;
        public Consumable(string name, string description, Func<Soul, Task> consume, double value) : base(name, description, value)
        {
            Consume = consume;
        }
    }
}
