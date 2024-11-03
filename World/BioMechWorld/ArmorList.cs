using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Armor;
using fire_ash_server.World.BioMechWorld;

namespace fire_ash_server.World.BioMechWorld
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

        public static Armor WardensScales()
        {
            return new Armor(
                "Warden's Scales",
                "A set of scale mail crafted from dark, " +
                "gleaming metal plates, each shaped like the scales of a serpent. " +
                "Brass accents form intricate snake motifs along the shoulders and chest, " +
                "and the faint glint of ruby inlays gives it a watchful, protective aura. ",
                ac: 15,
                value: 120
            );
        }

    }
}
