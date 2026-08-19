using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Props.Items.Weapons
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(AetherRotCannon2000), "AetherRotCannon2000")]
    [JsonDerivedType(typeof(AssaultRifle), "AssaultRifle")]
    [JsonDerivedType(typeof(BeastClaw), "BeastClaw")]
    [JsonDerivedType(typeof(Club), "Club")]
    [JsonDerivedType(typeof(Dagger), "Dagger")]
    [JsonDerivedType(typeof(Fist), "Fist")]
    [JsonDerivedType(typeof(InsectClaw), "InsectClaw")]
    [JsonDerivedType(typeof(ShortBow), "ShortBow")]
    [JsonDerivedType(typeof(Sling), "Sling")]
    [JsonDerivedType(typeof(Sword), "Sword")]
    [JsonDerivedType(typeof(TailSnakeBite), "TailSnakeBite")]
    [JsonDerivedType(typeof(Tendril), "Tendril")]
    [JsonDerivedType(typeof(VenomousSnakeBite), "VenomousSnakeBite")]
    internal class Weapon : Item
    {
        [JsonInclude] public Die DamageDie;
        [JsonInclude] public bool TwoHander;
        [JsonInclude] public DamageType DamageType;
        [JsonInclude] public int Modifier;
        [JsonInclude] public List<Func<string, string, Weapon, string>> AttackDescriptions = new List<Func<string, string, Weapon, string>>();
        [JsonInclude] public List<Func<string, string, Weapon, string>> OffHandAttackDescriptions = new List<Func<string, string, Weapon, string>>();
        [JsonIgnore]  public static Dictionary<Type, List<Func<string, string, Weapon, string>>> GeneralAttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();
        [JsonIgnore]  public static Dictionary<Type, List<Func<string, string, Weapon, string>>> GeneralOffHandAttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();
        [JsonIgnore]  public static Dictionary<Type, List<Func<string, string, Weapon, string>>> HumanoidAttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();
        [JsonIgnore]  public static Dictionary<Type, List<Func<string, string, Weapon, string>>> HumanoidOffHandAttackDescriptionsForType = new Dictionary<Type, List<Func<string, string, Weapon, string>>>();

        public Weapon() { }

        public Weapon(string name, string description, Die damageDie, DamageType damageType, double value) : base(name, description, value)
        {
            DamageDie = damageDie;
            DamageType = damageType;
        }

        public virtual void InitAttackDescriptions() { }

        public string? GetAttackDescription(string attacker, Prop target)
        {
            InitAttackDescriptions();

            List<Func<string, string, Weapon, string>> attackDescriptions;
            if (AttackDescriptions.Count > 0)
                attackDescriptions = AttackDescriptions;
            else if (target is Character & ((Character)target).IsOfCreatureType(CreatureType.Humanoid) && HumanoidAttackDescriptionsForType.ContainsKey(GetType()) && HumanoidAttackDescriptionsForType[GetType()].Count > 0)
                attackDescriptions = HumanoidAttackDescriptionsForType[GetType()];
            else if (GeneralAttackDescriptionsForType.ContainsKey(GetType()) && GeneralAttackDescriptionsForType[GetType()].Count > 0)
                attackDescriptions = GeneralAttackDescriptionsForType[GetType()];
            else
                return null;

            Random rand = new Random();
            int rndIndex = rand.Next(attackDescriptions.Count);
            return attackDescriptions[rndIndex].Invoke(attacker, target.Name, this) + " and... ";
        }

        public string? GetOffHandAttackDescription(string attacker, string target)
        {
            InitAttackDescriptions();

            List<Func<string, string, Weapon, string>> attackDescriptions;
            if (OffHandAttackDescriptions.Count > 0)
                attackDescriptions = OffHandAttackDescriptions;
            else if (HumanoidOffHandAttackDescriptionsForType.ContainsKey(GetType()) && HumanoidOffHandAttackDescriptionsForType[GetType()].Count > 0)
                attackDescriptions = HumanoidOffHandAttackDescriptionsForType[GetType()];
            else
                return null;

            Random rand = new Random();
            int rndIndex = rand.Next(attackDescriptions.Count);
            return attackDescriptions[rndIndex].Invoke(attacker, target, this) + " and... ";
        }

        public string GetDmgAsString()
        {
            string output = $"{DamageDie}";

            if (Modifier > 0)
                output += $"+{Modifier}";
            else if (Modifier < 0) 
                output += $"{Modifier}";

            return output;

        }
    }
}
