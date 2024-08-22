using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Sling : Weapon
    {
        public Sling(string name, string description) : base(name, description, new Die(1, 4), DamageType.Bludgeoning)
        {
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                                                        InventorySlot.Ranged
                                                        };
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptionsForType();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(Sling)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} pulls back the {w.Name}, launching a small projectile towards {r} with a snap.");
                descriptions.Add((a, r, w) => $"{a} skillfully swings the {w.Name} and lets fly a well-aimed stone towards {r}.");
                descriptions.Add((a, r, w) => $"{a} twirls their {w.Name} in a circular motion before sending a projectile hurtling towards {r}.");
                descriptions.Add((a, r, w) => $"{a} takes careful aim and releases the {w.Name}, the projectile sailing straight towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a quick shot from their {w.Name}, the small missile whistling through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} uses their {w.Name} to hurl a stone with precision, targeting {r} from a distance.");
                descriptions.Add((a, r, w) => $"{a} draws back the {w.Name}, releasing a sharp snap as the projectile speeds towards {r}.");
                descriptions.Add((a, r, w) => $"{a} launches a smooth stone from their {w.Name}, the projectile cutting through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} deftly swings their {w.Name} and releases, the projectile arcing gracefully towards {r}.");
                descriptions.Add((a, r, w) => $"{a} spins the {w.Name} with practiced ease, the projectile flying true towards {r}.");

                GeneralAttackDescriptionsForType.Add(typeof(Sling), descriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(Sling)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => $"{a} pulls back the {w.Name}, launching a small projectile towards {r} with a snap.");
                descriptions.Add((a, r, w) => $"{a} skillfully swings the {w.Name} and lets fly a well-aimed stone towards {r}.");
                descriptions.Add((a, r, w) => $"{a} twirls their {w.Name} in a circular motion before sending a projectile hurtling towards {r}.");
                descriptions.Add((a, r, w) => $"{a} takes careful aim and releases the {w.Name}, the projectile sailing straight towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a quick shot from their {w.Name}, the small missile whistling through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} uses their {w.Name} to hurl a stone with precision, targeting {r} from a distance.");
                descriptions.Add((a, r, w) => $"{a} draws back the {w.Name}, releasing a sharp snap as the projectile speeds towards {r}.");
                descriptions.Add((a, r, w) => $"{a} launches a smooth stone from their {w.Name}, the projectile cutting through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} deftly swings their {w.Name} and releases, the projectile arcing gracefully towards {r}.");
                descriptions.Add((a, r, w) => $"{a} spins the {w.Name} with practiced ease, the projectile flying true towards {r}.");

                HumanoidAttackDescriptionsForType.Add(typeof(Sling), descriptions);
            }
        }
    }
}
