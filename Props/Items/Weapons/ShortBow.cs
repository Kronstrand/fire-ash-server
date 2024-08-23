using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class ShortBow : Weapon
    {
        public ShortBow(string name, string description) : base(name, description, new Die(1, 6), DamageType.Piercing)
        {
            TwoHander = true;
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot> {
                InventorySlot.Ranged
            };
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptionsForType();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(ShortBow)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} draws back the string of their {w.Name}, releasing an arrow that whistles towards {r}.");
                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} with a steady hand, sending an arrow flying towards {r}.");
                descriptions.Add((a, r, w) => $"{a} notches an arrow in their {w.Name}, the bow creaking softly as they let it fly towards {r}.");
                descriptions.Add((a, r, w) => $"{a} swiftly pulls an arrow from their quiver, their {w.Name} launching it towards {r}.");
                descriptions.Add((a, r, w) => $"{a} releases the string of their {w.Name}, an arrow darting towards {r} with deadly intent.");
                descriptions.Add((a, r, w) => $"{a} lines up their shot, the {w.Name} sending a sharp arrow towards {r}.");
                descriptions.Add((a, r, w) => $"{a} pulls back their {w.Name}, the arrow loosing in a blur towards {r}.");
                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} at {r}, releasing a precise and silent shot.");
                descriptions.Add((a, r, w) => $"{a} carefully draws an arrow in their {w.Name}, the string singing as it releases towards {r}.");
                descriptions.Add((a, r, w) => $"{a} focuses their aim, their {w.Name} sending a swift arrow arcing towards {r}.");

                GeneralAttackDescriptionsForType.Add(typeof(ShortBow), descriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(ShortBow)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} draws back the string of their {w.Name}, releasing an arrow that whistles towards {r}.");
                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} with a steady hand, sending an arrow flying towards {r}.");
                descriptions.Add((a, r, w) => $"{a} notches an arrow in their {w.Name}, the bow creaking softly as they let it fly towards {r}.");
                descriptions.Add((a, r, w) => $"{a} swiftly pulls an arrow from their quiver, their {w.Name} launching it towards {r}.");
                descriptions.Add((a, r, w) => $"{a} releases the string of their {w.Name}, an arrow darting towards {r} with deadly intent.");
                descriptions.Add((a, r, w) => $"{a} lines up their shot, the {w.Name} sending a sharp arrow towards {r}.");
                descriptions.Add((a, r, w) => $"{a} pulls back their {w.Name}, the arrow loosing in a blur towards {r}.");
                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} at {r}, releasing a precise and silent shot.");
                descriptions.Add((a, r, w) => $"{a} carefully draws an arrow in their {w.Name}, the string singing as it releases towards {r}.");
                descriptions.Add((a, r, w) => $"{a} focuses their aim, their {w.Name} sending a swift arrow arcing towards {r}.");

                HumanoidAttackDescriptionsForType.Add(typeof(ShortBow), descriptions);
            }
        }
    }
}
