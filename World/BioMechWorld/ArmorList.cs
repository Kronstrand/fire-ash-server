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
            "while remaining hidden themselves.");
            nocturnalOptics.AddEquipEffect(EffectKey.Darkvision);

            return nocturnalOptics;
        }
    }
}
