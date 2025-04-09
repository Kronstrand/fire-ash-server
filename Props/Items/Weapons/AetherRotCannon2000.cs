using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    [Serializable]
    internal class AetherRotCannon2000 : Weapon
    {
        public AetherRotCannon2000() : base("Aether Rot Cannon 2000", "Developed for a war that never happened. Or maybe it did, and the ARC-2000 just erased all evidence.", new Die(1, 6), DamageType.Necrotic, 70)
        {
            
            TwoHander = true;
            Modifier = 10;
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{InventorySlot.Ranged};
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptionsForType();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(AetherRotCannon2000)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} levels {w.Name}, its pulsating core exhaling aetheric rot toward {r}.");
                descriptions.Add((a, r, w) => $"{a} fires {w.Name}, and a writhing tendril of decaying energy lashes out at {r}.");
                descriptions.Add((a, r, w) => $"{w.Name} trembles in {a}'s grip before spewing a rotting aetheric blast at {r}.");
                descriptions.Add((a, r, w) => $"{a} squeezes the trigger on {w.Name}, and a whispering bolt of entropy streaks toward {r}.");
                descriptions.Add((a, r, w) => $"{a} watches as {w.Name} emits a low, hungry growl before discharging its unraveling payload at {r}.");
                descriptions.Add((a, r, w) => $"{a} grins as {w.Name} releases a spiraling stream of spectral corrosion, dissolving reality around {r}.");
                descriptions.Add((a, r, w) => $"{a} unleashes {w.Name}, and a luminous, decaying spiral of energy rushes hungrily toward {r}.");

                GeneralAttackDescriptionsForType.Add(typeof(AetherRotCannon2000), descriptions);
            }
        }


        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(AetherRotCannon2000)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} aims {w.Name} at {r}, the weapon shuddering as it vomits forth aetheric rot.");
                descriptions.Add((a, r, w) => $"{a} grins as {w.Name} hums with anticipation, then fires a shot that *unravels* {r} from existence.");
                descriptions.Add((a, r, w) => $"{a} pulls the trigger on {w.Name}, sending a searing, spectral tendril of decay slithering toward {r}.");
                descriptions.Add((a, r, w) => $"{a} watches as {w.Name} pulses hungrily before expelling a wave of necrotic dissolution at {r}.");
                descriptions.Add((a, r, w) => $"{a} releases {w.Name}, and an eerie, spiraling surge of decaying light erupts toward {r}.");
                descriptions.Add((a, r, w) => $"{a} chuckles darkly as {w.Name} releases its cursed payload, ensuring {r} won’t be leaving a corpse behind.");
                descriptions.Add((a, r, w) => $"{a} steadies their aim, {w.Name} whispering to them as aetheric rot coils around {r} like grasping fingers.");
                descriptions.Add((a, r, w) => $"{w.Name} wheezes as {a} fires, its spectral payload writhing toward {r} like a sentient plague.");

                HumanoidAttackDescriptionsForType.Add(typeof(AetherRotCannon2000), descriptions);
            }
        }

    }
}
