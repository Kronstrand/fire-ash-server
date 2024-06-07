using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server
{
    internal class Damage
    {
        public Roll DmgRoll;
        public DamageType DamageType;

        public Damage(Roll dmgRoll, DamageType damageType)
        {
            DmgRoll = dmgRoll;
            DamageType = damageType;
        }
        public override string ToString() 
        {            
            return $"{DmgRoll} {Description(DamageType).ToLower()} damage";
        }
    }
}
