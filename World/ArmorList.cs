using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Armoring;

namespace fire_ash_server.World
{
    internal static class ArmorList
    {
        public static Head NocturnalOptics()
        {
            Head nocturnalOptics = new Head(
            "Nocturnal Optics",
            "These biomechanical goggles merge organic tissue with synthetic glass, " +
            "providing the wearer with unparalleled vision in complete darkness. " +
            "Their sleek design ensures that no light escapes, allowing users to see through the deepest shadows " +
            "while remaining hidden themselves.",
            4523);
            nocturnalOptics.AddEquipEffect(EffectKey.Darkvision);

            return nocturnalOptics;
        }

        public static Shield WornWoodenShield()
        {
            Shield wornWoodenShield = new Shield(
                "Worn Wooden Shield",
                "This wooden shield is battered and weathered, its surface marred by deep gouges. " +
                "Cracks run through the grain, and the edges are splintered from countless blows. " +
                "The straps are worn thin, but it still offers some protection, though far from what it once was.",
                0.6);

            return wornWoodenShield;
        }

        public static Shield SteelShield()
        {
            Shield steelShield = new Shield(
                "Steel Shield",
                "A sturdy steel shield, its surface dulled and scarred, the metal bearing old hammer marks and signs of repair. " +
                "Rivets line the rim, some mismatched, holding the piece together through years of hard use.",
                16);

            return steelShield;
        }

        public static Armor DriftersVest()
        {
            return new Armor(
            "Drifter's Vest",
            "A reinforced vest woven with interlocking metal fibers, offering protection without sacrificing mobility. " +
            "The dark, matte plating is layered beneath a flexible, synthetic fabric, " +
            "designed to absorb impact while allowing free movement. " +
            "Sturdy yet unrestrictive, it’s favored by scouts and wanderers alike.",
            ac: 13,
            value: 30);
        
        }

        public static Armor GuardHauberk()
        {
            return new Armor(
                "Guard's Hauberk",
                "A mail hauberk of interlinked rings, worn thin and patched in places. " +
                "The metal is dulled and darkened, yet still holds firm, offering protection without hindering movement.",
                ac: 14,
                value: 30);
        }

        public static Armor GuardLeather()
        {
            return new Armor(
                "Guard's Studded Leather",
                "A worn leather jerkin reinforced with metal studs. The leather is cracked and darkened, patched in places, yet still sturdy enough to protect the wearer.",
                ac: 13,
                value: 25);
        }

        public static Armor WardensScales()
        {
            return new Armor(
                "Warden's Scales",
                "A set of scale mail crafted from dark, " +
                "gleaming metal plates, each shaped like the scales of a serpent. " +
                "Brass accents form intricate snake motifs along the shoulders and chest, " +
                "and the faint glint of ruby inlays gives it a watchful, protective aura. ",
                ac: 14,
                value: 110
            );
        }

    }
}
