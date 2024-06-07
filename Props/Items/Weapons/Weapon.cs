using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    internal class Weapon : Item
    {
        public Die DamageDie;
        public bool TwoHander;
        public DamageType DamageType;
        public List<Func<string, string, Weapon, string>> AttackDescriptions = new List<Func<string, string, Weapon, string>>();
        public List<Func<string, string, Weapon, string>> OffHandAttackDescriptions = new List<Func<string, string, Weapon, string>>();
        public static Dictionary<Type, List<Func<string, string, Weapon, string>>> AttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();
        public static Dictionary<Type, List<Func<string, string, Weapon, string>>> OffHandAttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();

        public Weapon(string name, string description, Die damageDie, DamageType damageType) : base(name, description)
        {
            DamageDie = damageDie;
            DamageType = damageType;
        }

        public string? GetAttackDescription(string attacker, string target)
        {
            List<Func<string, string, Weapon, string>> attackDescriptions;
            if (AttackDescriptions.Count > 0)
                attackDescriptions = AttackDescriptions;
            else if (AttackDescriptionsForType.ContainsKey(GetType()) && AttackDescriptionsForType[GetType()].Count > 0)
                attackDescriptions = AttackDescriptionsForType[GetType()];
            else
                return null;

            Random rand = new Random();
            int rndIndex = rand.Next(attackDescriptions.Count);
            return attackDescriptions[rndIndex].Invoke(attacker, target, this) + " and... ";
        }

        public string? GetOffHandAttackDescription(string attacker, string target)
        {
            List<Func<string, string, Weapon, string>> attackDescriptions;
            if (OffHandAttackDescriptions.Count > 0)
                attackDescriptions = OffHandAttackDescriptions;
            else if (OffHandAttackDescriptionsForType.ContainsKey(GetType()) && OffHandAttackDescriptionsForType[GetType()].Count > 0)
                attackDescriptions = OffHandAttackDescriptionsForType[GetType()];
            else
                return null;

            Random rand = new Random();
            int rndIndex = rand.Next(attackDescriptions.Count);
            return attackDescriptions[rndIndex].Invoke(attacker, target, this) + " and... ";
        }
    }
}
