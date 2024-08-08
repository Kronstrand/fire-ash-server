using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.World.BioMechWorld
{
    internal class Prices
    {
        public static int GetPrice(Prop prop)
        {
            switch (prop)
            {
                case Weapon:
                    return GetWeaponPrice((Weapon)prop);
            }
            return 0;
        }

        public static int GetWeaponPrice(Weapon weapon)
        {
            switch (weapon)
            {
                case AssaultRifle:
                    return 50;
                case Dagger:
                    return 2;
            }
            return 0;
        }
    }
}
