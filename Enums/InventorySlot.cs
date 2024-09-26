using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum InventorySlot
    {
        //each value should be handled in character.AddToInventory

        [Description("Main hand")]
        MainHand,
        [Description("Off-hand")]
        OffHand,
        Ranged,
        Waist,
        Head,
        Teeth
    }
}
