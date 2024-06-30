using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class AssaultRifle : Weapon
    {
        public AssaultRifle(string name, string description) : base(name, description, new Die(1, 10), DamageType.Piercing)
        {
            TwoHander = true;
            CarriableByInventorySlots = new ThreadSafeList<InventorySlot>{
                                                            InventorySlot.Ranged
                                                            };
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptionsForType();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(AssaultRifle)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} with precision, unleashing a burst of fire towards {r}.");
                descriptions.Add((a, r, w) => $"{a} squeezes the trigger of their {w.Name}, a hail of bullets speeding towards {r}.");
                descriptions.Add((a, r, w) => $"{a} steadies their {w.Name}, each shot ringing with deadly accuracy towards {r}.");
                descriptions.Add((a, r, w) => $"{a} expertly controls the recoil of their {w.Name}, bullets finding their mark on {r}.");
                descriptions.Add((a, r, w) => $"{a} unleashes a controlled burst from their {w.Name}, every round hurtling towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a deadly spray from their {w.Name}, bullets streaking towards {r}.");
                descriptions.Add((a, r, w) => $"{a} takes a deep breath, their {w.Name} spitting out a storm of lead towards {r}.");
                descriptions.Add((a, r, w) => $"{a} locks onto {r} and unloads with their {w.Name}, the air filled with the crack of gunfire.");
                descriptions.Add((a, r, w) => $"{a} adjusts their aim and fires, their {w.Name} sending a precise volley towards {r}.");
                descriptions.Add((a, r, w) => $"{a} braces their {w.Name}, each bullet fired a promise of destruction towards {r}.");
                descriptions.Add((a, r, w) => $"{a} releases the trigger in controlled bursts, their {w.Name} keeping a steady beat on {r}.");
                descriptions.Add((a, r, w) => $"{a} tracks {r} with their {w.Name}, every shot carefully aimed and fired.");
                descriptions.Add((a, r, w) => $"{a} grips their {w.Name} tightly, bullets ripping through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} sprays a burst of gunfire from their {w.Name}, the muzzle flashing as bullets fly towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a short burst, their {w.Name} roaring with controlled ferocity towards {r}.");
                descriptions.Add((a, r, w) => $"{a} zeroes in on {r}, their {w.Name} spitting a deadly stream of lead.");
                descriptions.Add((a, r, w) => $"{a} handles their {w.Name} with expertise, each shot a lethal promise towards {r}.");
                descriptions.Add((a, r, w) => $"{a} pulls the trigger of their {w.Name}, a rapid burst of bullets tearing through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} lines up their sights and fires their {w.Name}, a precise shot aimed at {r}.");
                descriptions.Add((a, r, w) => $"{a} unleashes a torrent of bullets from their {w.Name}, the muzzle flashing brightly as they target {r}.");
                descriptions.Add((a, r, w) => $"{a} shoulders their {w.Name}, firing a deadly spray of bullets towards {r}.");
                descriptions.Add((a, r, w) => $"{a} steadies their aim, their {w.Name} chattering as it spits out a stream of bullets towards {r}.");
                descriptions.Add((a, r, w) => $"{a} braces themselves and fires their {w.Name}, a burst of lead streaking towards {r}.");
                descriptions.Add((a, r, w) => $"{a} unloads a magazine from their {w.Name}, each bullet a messenger of destruction for {r}.");
                descriptions.Add((a, r, w) => $"{a} swings their {w.Name} to bear, a cascade of bullets streaming towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a rapid burst from their {w.Name}, the bullets tearing through the air towards {r}.");
                descriptions.Add((a, r, w) => $"{a} tracks {r} with their {w.Name}, every shot calculated and precise.");
                descriptions.Add((a, r, w) => $"{a} grips their {w.Name} firmly, unleashing a burst of gunfire towards {r}.");
                descriptions.Add((a, r, w) => $"{a} steadies their breathing, their {w.Name} roaring as it sends a volley of bullets towards {r}.");
                descriptions.Add((a, r, w) => $"{a} takes aim with their {w.Name}, a deadly burst of fire erupting towards {r}.");
                descriptions.Add((a, r, w) => $"{a} fires a relentless barrage from their {w.Name}, the bullets whizzing towards {r}.");
                descriptions.Add((a, r, w) => $"{a} narrows their eyes, their {w.Name} releasing a torrent of lead towards {r}.");
                descriptions.Add((a, r, w) => $"{a} takes careful aim, their {w.Name} sending a fusillade of bullets towards {r}.");
                descriptions.Add((a, r, w) => $"{a} grips their {w.Name} tightly, each shot a harbinger of destruction for {r}.");
                descriptions.Add((a, r, w) => $"{a} releases a storm of bullets from their {w.Name}, the projectiles streaking towards {r}.");
                descriptions.Add((a, r, w) => $"{a} braces their {w.Name}, firing off a rapid burst towards {r}.");
                descriptions.Add((a, r, w) => $"{a} unloads their {w.Name}, a stream of bullets racing towards {r}.");
                descriptions.Add((a, r, w) => $"{a} steadies their weapon, their {w.Name} barking as it sends a burst of fire towards {r}.");
                descriptions.Add((a, r, w) => $"{a} aims their {w.Name} with deadly intent, unleashing a barrage of bullets towards {r}.");
                descriptions.Add((a, r, w) => $"{a} pulls the trigger of their {w.Name}, a rain of lead pouring towards {r}.");

                GeneralAttackDescriptionsForType.Add(typeof(AssaultRifle), descriptions);
            }
        }

        public static void SetHumanoidAttackDescriptionsForType()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(AssaultRifle)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>> { };
                descriptions.Add((a, r, w) => { return $"{a} aims their {w.Name} with precision, unleashing a burst of fire towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} squeezes the trigger of their {w.Name}, a hail of bullets speeding towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their {w.Name}, each shot ringing with deadly accuracy towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} expertly controls the recoil of their {w.Name}, bullets finding their mark on {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unleashes a controlled burst from their {w.Name}, every round hurtling towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} fires a deadly spray from their {w.Name}, bullets streaking towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} takes a deep breath, their {w.Name} spitting out a storm of lead towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} locks onto {r} and unloads with their {w.Name}, the air filled with the crack of gunfire"; });
                descriptions.Add((a, r, w) => { return $"{a} adjusts their aim and fires, their {w.Name} sending a precise volley towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} braces their {w.Name}, each bullet fired a promise of destruction towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} releases the trigger in controlled bursts, their {w.Name} keeping a steady beat on {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} tracks {r} with their {w.Name}, every shot carefully aimed and fired"; });
                descriptions.Add((a, r, w) => { return $"{a} grips their {w.Name} tightly, bullets ripping through the air towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} sprays a burst of gunfire from their {w.Name}, the muzzle flashing as bullets fly towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} fires a short burst, their {w.Name} roaring with controlled ferocity towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} zeroes in on {r}, their {w.Name} spitting a deadly stream of lead"; });
                descriptions.Add((a, r, w) => { return $"{a} handles their {w.Name} with expertise, each shot a lethal promise towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} pulls the trigger of their {w.Name}, a rapid burst of bullets tearing through the air towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} lines up their sights and fires their {w.Name}, a precise shot aimed at {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unleashes a torrent of bullets from their {w.Name}, the muzzle flashing brightly as they target {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} shoulders their {w.Name}, firing a deadly spray of bullets towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their aim, their {w.Name} chattering as it spits out a stream of bullets towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} braces themselves and fires their {w.Name}, a burst of lead streaking towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unloads a magazine from their {w.Name}, each bullet a messenger of destruction for {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} swings their {w.Name} to bear, a cascade of bullets streaming towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} fires a rapid burst from their {w.Name}, the bullets tearing through the air towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} tracks {r} with their {w.Name}, every shot calculated and precise"; });
                descriptions.Add((a, r, w) => { return $"{a} grips their {w.Name} firmly, unleashing a burst of gunfire towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their breathing, their {w.Name} roaring as it sends a volley of bullets towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} takes aim with their {w.Name}, a deadly burst of fire erupting towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} fires a relentless barrage from their {w.Name}, the bullets whizzing towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} narrows their eyes, their {w.Name} releasing a torrent of lead towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} takes careful aim, their {w.Name} sending a fusillade of bullets towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} grips their {w.Name} tightly, each shot a harbinger of destruction for {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} releases a storm of bullets from their {w.Name}, the projectiles streaking towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} braces their {w.Name}, firing off a rapid burst towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} unloads their {w.Name}, a stream of bullets racing towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} steadies their weapon, their {w.Name} barking as it sends a burst of fire towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} aims their {w.Name} with deadly intent, unleashing a barrage of bullets towards {r}"; });
                descriptions.Add((a, r, w) => { return $"{a} pulls the trigger of their {w.Name}, a rain of lead pouring towards {r}"; });
                HumanoidAttackDescriptionsForType.Add(typeof(AssaultRifle), descriptions);
            }
        }
    }
}
