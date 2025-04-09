using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using static fire_ash_server.Helpers;
using fire_ash_server.Abstract_Entities;

namespace fire_ash_server.World.BioMechWorld
{
    internal static class ConsumableList
    {
        public static string BearTrapName = "Bear Trap";
        public static string ScrollofEntanglementName = "Scroll of Entanglement";
        public static string HealingPotionName = "Healing Potion";
        public static Consumable BearTrap()
        {
            //should be hidden

            Item trap = new Item("Loaded Beartrap", "This bear trap deals 1d4 piercing damage, stuns its target for 1 round, and roots them for 2 rounds.", 25);
            trap.AddOnAfterMoveToEvent(
                (Soul soul) => {
                    if (trap.HeldBy == null)
                        return Task.FromResult(false);
                    if (soul.Character.IsHidden())
                        return Task.FromResult(false);
                    if (trap.SetBy == soul.Character)
                        return Task.FromResult(false);

                    Roll savingThrow = new Roll(soul.Character.GetModifer(Ability.Dexterity), RollType.SavingThrow, soul.Character);
                    if (savingThrow.BeatsDC(14))
                    {
                        soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} steps into {trap.Name}, succeeds on their Dexterity saving throw by rolling {savingThrow}, and quickly sidesteps out of its way before it snaps shut.");
                    }
                    else
                    {
                        soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} steps into {trap.Name} fails on their Dexterity saving throw by rolling {savingThrow}.");
                        soul.Character.TakeDamage(new Damage(
                        new Roll(
                            new Die(1, 4),
                            0,
                            RollType.DamageRoll,
                            null),
                        DamageType.Piercing), trap.Name);
                        soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} is stunned 1 turn and rooted 2 turns.");
                        soul.Character.Conditions.Add(new ActiveCondition(Condition.Stunned, 1));
                        soul.Character.Conditions.Add(new ActiveCondition(Condition.Rooted, 2));
                        
                        if (trap.SetBy != null)
                            soul.Character.SetEnableCombatWith(trap.SetBy);
                    }

                    trap.HeldBy.Items.Remove(trap);
                    
                    return Task.FromResult(true);
                }, 
                true);

            Consumable unloadedTrap = new Consumable(
                BearTrapName,
                "When set, this bear trap deals 1d4 piercing damage, stuns its target for 1 round, and roots them for 2 rounds.",
                (Soul soul) => {    
                    return Task.CompletedTask; 
                },
                trap.VendorValue);

            unloadedTrap.Consume = (Soul soul) =>
            {
                DropItem.RemoveItemFromCharacter(soul.Character, unloadedTrap);
                soul.Character.CurrentRoom.AddItem(trap);
                trap.MoveToGroup(soul.Character);
                //trap.Hide(10 + soul.Character.GetModifer(Skill.Stealth));
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} sets {trap.Name}.");
                trap.SetBy = soul.Character;
                soul.Character.LookBackFromItem(unloadedTrap);

                return Task.CompletedTask;
            };

            return unloadedTrap;
        }

        public static Consumable ScrollOfEntanglement()
        {
            int turns = 4;
            Consumable scrollOfEntanglement = new Consumable(
                ScrollofEntanglementName,
                $"Casting Entanglement successfully will root the target for {turns} turns. A rooted target can't move.",
                (Soul soul) =>
                {
                    if (!(soul.Character.lookAtBeforeInventory is Character))
                        return Task.CompletedTask;

                    Character characterToAttack = (Character)soul.Character.lookAtBeforeInventory;
                    Roll savingThrow = new Roll(characterToAttack.GetModifer(Ability.Dexterity), RollType.SavingThrow, characterToAttack);
                    if (!savingThrow.BeatsDC(15 + soul.Character.GetModifer(Skill.Arcana)))
                    {
                        characterToAttack.Conditions.Add(new ActiveCondition(Condition.Rooted, turns));
                        soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} begins to read from a scroll to cast Entanglement. {characterToAttack.Name} fails their Dexterity saving throw, rolling {savingThrow}. They become entangled for {turns} turns.");
                    }
                    else
                    {
                        soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} begins to read from a scroll to cast Entanglement. {characterToAttack.Name} succeeds their Dexterity saving throw, rolling {savingThrow}.");
                    }

                    soul.Character.SetEnableCombatWith(characterToAttack);

                    return Task.CompletedTask;
                },
                39);

            scrollOfEntanglement.HasTarget = true;

            return scrollOfEntanglement;
        }

        public static Consumable ARC2000()
        {
            Weapon weapon = new AetherRotCannon2000();
            //make sure to enable combat
            Consumable cannon = new Consumable(
                weapon.Name,
                weapon.Description + $" For a single use, {weapon.Name} deals {weapon.DamageDie}+{weapon.Modifier} {weapon.DamageType} damage with a high probability of hitting its target.",
                async (Soul soul) =>
                {
                    Character lookAtCharacter;
                    if (soul.Character.lookAtBeforeInventory is Character)
                    {
                        lookAtCharacter = (Character)soul.Character.lookAtBeforeInventory;
                        RangedAttack rangedAttack = new RangedAttack(soul, lookAtCharacter);
                        rangedAttack.weapon = weapon;
                        await rangedAttack.Execute(soul);
                    }
                },
                70);
            cannon.Weapon = weapon;
            cannon.Range = RangeType.RangeSingleTarget;
            cannon.Requirement = Attack.AttackIsValid;
            cannon.NotAvailable = Attack.InvalidAttack;
            cannon.HasTarget = true;

            return cannon;
        }

        public static Consumable BookOfHealth()
        {
            return new Consumable(
                "Book of Health",
                "Grants the gift of increased health points.",
                (Soul soul) =>
                {
                    _ = soul.SendAsync("* As you read through the book, you gain 6 additional health points. *");
                    soul.Character.HP += 6;
                    soul.Character.GainLife(6);

                    return Task.CompletedTask;
                },
                45);
        }

        public static Consumable BookOfDualWield()
        {
            return new Consumable(
                "Book of Two Weapons",
                "Teaches the art of dual-wielding",
                async (Soul soul) =>
                {
                    await soul.SendAsync("* As you read through the book, you acquire the feat of dual-wielding. *");
                    soul.Character.AddFeat(FeatKey.DualWield);
                },
                130);
        }

        public static Consumable HealthPotion()
        {
            return new Consumable(
                HealingPotionName,
                "A potion that restores health, healing for 1d6+2 health points when consumed.",
                (Soul soul) =>
                {
                    Roll hpRoll = new Roll(
                        new Die(1, 6),
                        2,
                        RollType.None,
                        soul.Character);

                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} drinks a healing potion, rolls {hpRoll} and gains {hpRoll.GetSum()} health points.");
                    soul.Character.GainLife(hpRoll.GetSum());

                    return Task.CompletedTask;
                },
                30);
        }

    }        
}
