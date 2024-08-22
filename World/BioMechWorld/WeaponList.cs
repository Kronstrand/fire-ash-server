using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.World.BioMechWorld
{
    internal static class WeaponList
    {
        public static Weapon FlashLight()
        {
            Club flashLite = new Club(
                "Flash Lite",
                "The Flash Lite is a compact, dual-purpose tool. It features a cylindrical handle with a grip textured for both comfort and durability. " +
                "The head of the device doubles as a powerful flashlight, casting a bright beam of light capable of piercing the darkest environments. " +
                "When used as a weapon, its strikes are feeble, more likely to bruise egos than break bones."
            );
            flashLite.AddEquipEffect(EffectKey.LightPointer);
            flashLite.DamageDie = new Die(1, 2);
            return flashLite;
        }

        public static Weapon ColtARFifteen()
        {
            Weapon coltRifle = new AssaultRifle(
                "Colt AR-15",
                "A classic semi-automatic rifle, " +
                "renowned for its reliability and precision, " +
                "equipped with a robust barrel and a sleek, ergonomic design, " +
                "exuding a sense of timeless power and modern efficiency.");

            return coltRifle;
        }

        public static Weapon HolographicBlade()
        {
            Weapon holographicBlade = new Dagger(
                "Holographic Blade",
                "A sleek, high-tech blade that shimmers with a holographic edge, designed for both precision and style. " +
                "Its handle is wrapped in synthetic leather, providing a comfortable grip.");
            holographicBlade.Modifier = +1;
            holographicBlade.AddEquipEffect(EffectKey.DimLight);

            return holographicBlade;
        }

        public static Weapon LuminarBaton()
        {
            Club luminarBaton = new Club(
                "Luminar Baton",
                "The Luminar Baton is a sleek, cylindrical weapon, about two feet in length, " +
                "with a surface that alternates between smooth, " +
                "polished metal and segments of translucent crystal. " +
                "The core of the baton houses a sophisticated energy conduit that channels light through the crystalline segments, " +
                "causing them to glow with a soft, pulsating light");
            luminarBaton.AddEquipEffect(EffectKey.BrightLight);

            return luminarBaton;
        }

        public static Weapon YShapedSlingShot()
        {
            Sling yShapedSlingShot = new Sling(
                "Y-Shaped Slingshot",
                "A simple, Y-shaped slingshot made from a sturdy piece of scrap metal or wood. The handle is wrapped in leather strips for a comfortable grip, " +
                "and the elastic band is made from durable rubber. It can launch small stones or metal fragments with surprising force."
            );
            yShapedSlingShot.DamageDie = new Die(1, 4);
            return yShapedSlingShot;
        }
    }
}
