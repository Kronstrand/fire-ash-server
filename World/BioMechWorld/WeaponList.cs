using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
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
                "When used as a weapon, its strikes are feeble, more likely to bruise egos than break bones.", 
                15
            );
            flashLite.AddEquipEffect(EffectKey.LightPointer);
            flashLite.DamageDie = new Die(1, 2);
            return flashLite;
        }

        public static Weapon ColtARFifteen()
        {
            Weapon blackBolt = new AssaultRifle(
                "Blackbolt-15",
                "A classic semi-automatic rifle, " +
                "renowned for its reliability and precision, " +
                "equipped with a robust barrel and a sleek, ergonomic design, " +
                "exuding a sense of timeless power and modern efficiency.", 
                60);

            return blackBolt;
        }

        public static Sword HolographicBlade()
        {
            Sword holographicBlade = new Sword(
                "Holographic Blade",
                "A sleek, high-tech blade that shimmers with a holographic edge, designed for both precision and style. " +
                "Its handle is wrapped in synthetic leather, providing a comfortable grip.");
            holographicBlade.VendorValue = 110;
            holographicBlade.Modifier = +1;
            holographicBlade.AddEquipEffect(EffectKey.DimLight);

            return holographicBlade;
        }

        public static Sword CeremonialBlade()
        {
            Sword ceremonialBlade = new Sword(
                "Ceremonial Blade",
                "A finely crafted sword used in sacred rituals and rites. Its blade is adorned with intricate engravings, " +
                "and the hilt is wrapped in deep crimson cloth. Though primarily designed for ceremony, " +
                "it remains a sharp and deadly weapon in the hands of a skilled wielder.");

            ceremonialBlade.VendorValue = 250;
            ceremonialBlade.Modifier = +2;

            return ceremonialBlade;
        }

        public static Weapon LuminarBaton()
        {
            Club luminarBaton = new Club(
                "Luminar Baton",
                "The Luminar Baton is a sleek, cylindrical weapon, about two feet in length, " +
                "with a surface that alternates between smooth, " +
                "polished metal and segments of translucent crystal. " +
                "The core of the baton houses a sophisticated energy conduit that channels light through the crystalline segments, " +
                "causing them to glow with a soft, pulsating light",
                25);
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

        public static Dagger StoneKnife()
        {
            Dagger stoneknife = new Dagger(
                "Crude Stone Knife",
                "A crudely fashioned stone knife, " +
                "its jagged blade chipped from flint, " +
                "bound to a rough wooden handle with aged sinew, " +
                "exuding a primal, utilitarian essence.");

            return stoneknife;
        }

        public static Weapon WhiteCandle()
        {
            Club whiteCandle = new Club(
                "White Candle",
                "A simple white candle made of wax with a steady-burning wick. " +
                "It is unremarkable in appearance but can be used as a makeshift weapon in desperate situations.",
                0.5);
            whiteCandle.DamageDie = new Die(1, 1);
            whiteCandle.AddEquipEffect(EffectKey.DimLight);

            return whiteCandle;
        }

        public static ShortBow TribalShortBow()
        {
            ShortBow tribalShortBow = new ShortBow(
                "Tribal Short Bow",
                "A short bow crafted from weathered wood, " +
                "its limbs reinforced with sinew and decorated with tattered pieces of cloth. " +
                "The cloth strips are marked with faded tribal symbols, " +
                "each telling stories of hunts and battles long past.");

            return tribalShortBow;
        }

        public static Sword RustedSword()
        {
            Sword rustedSword = new Sword(
                "Rusted Sword",
                "A corroded blade, dulled and pitted with rust. " +
                "The hilt’s leather is worn and brittle, hinting at better days. " +
                "Despite its decay, the sword holds a lingering aura of past battles.");

            return rustedSword;
        }


        public static Sword Machete()
        {
            Sword machete = new Sword(
                "Machete",
                "A broad, single-edged blade with a brutal, unpretentious design. " +
                "Its weight is perfectly measured for hacking through dense obstacles, " +
                "while the slight curve ensures each strike bites deep.");

            return machete;
        }
    }
}
