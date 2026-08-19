using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World
{
    internal static class ConsumableList
    {
        public static Dictionary<ConsumableKey, Func<Soul, Item, Task>> ConsumableEffects = new Dictionary<ConsumableKey, Func<Soul, Item, Task>>();

        public static string BearTrapName = "Bear Trap";
        public static string ScrollofEntanglementName = "Scroll of Entanglement";
        public static string HealingPotionName = "Healing Potion";

        public static void InitConsumableDicts()
        {
            ConsumableEffects.TryAdd(ConsumableKey.HealthPotion, UseHealthPotion());
            ConsumableEffects.TryAdd(ConsumableKey.SetBearTrap, SetBearTrap());
            ConsumableEffects.TryAdd(ConsumableKey.EatFood, EatFood());
            ConsumableEffects.TryAdd(ConsumableKey.ResurrectSoul, ResurrectSoul());
            ConsumableEffects.TryAdd(ConsumableKey.RechargeResurrectionStone, RechargeResurrectionStone());
        }

        public static string SetResurrectionMark(Character character)
        {
            while (true)
            {
                int rndInt = GetRandomInt(100);

                if (rndInt < 55)
                {
                    return $"{character.Name} draws breath once more.";
                }

                BuffDebuff buffDebuff = new BuffDebuff();
                buffDebuff.UnlimitedTurn = true;

                if (rndInt < 75)
                {
                    if (character.Constition < 5)
                        continue;

                    buffDebuff.Name = Names.MarkOfDeath;
                    character.Wisdom += 2;
                    character.ChangeConstitutionAndUpdateHp(-2);

                    character.AddBuffDebuff(buffDebuff);
                    return $"{character.Name} draws breath once more, clearly marked by death.";
                }
                else if (rndInt < 95)
                {
                    if (character.Wisdom < 5)
                        continue;

                    buffDebuff.Name = Names.CameBackWrong;
                    character.Strength += 2;
                    character.Wisdom -= 2;

                    character.AddBuffDebuff(buffDebuff);
                    return $"{character.Name} is alive once more, but the person who returned is not quite the same.";
                }
                else
                {
                    if (character.Intelligence < 5 || character.Constition < 4)
                        continue;

                    buffDebuff.Name = Names.HollowedOut;
                    character.Intelligence += 2;
                    character.ChangeConstitutionAndUpdateHp(+1);
                    character.Wisdom -= 1;
                    character.Charisma -= 2;

                    character.AddBuffDebuff(buffDebuff);
                    return $"{character.Name} returns to life, but their eyes seem distant, as if a part of them was left behind.";
                }
            }
        }

        public static Func<Soul, Item, Task> ResurrectSoul()
        {
            return async (soul, consumedItem) =>
            {
                if (consumedItem is not Consumable)
                    return;
                Consumable consumable = (Consumable)consumedItem;
                if (consumable.CharacterId == "")
                    return;

                Character? resurrectCharacter;
                Program.WorldSoul.SoulstonedCharacters.TryGetValue(consumable.CharacterId, out resurrectCharacter);

                if (resurrectCharacter == null)
                    return;
                Item? ressurectionStone = soul.Character.GetItem(Names.ResurrectionStone);
                Item? wolfSkull = soul.Character.GetItem(Names.WolfSkull);

                if (ressurectionStone != null && wolfSkull != null)
                {
                    if (ressurectionStone.State != PropState.Depleted)
                    {
                        string txt = $"{soul.Character.Name} sinks into the energy of {FormatPossessive(resurrectCharacter.Name)} soulstone, " +
                            $"the ambient air turns heavy, summoning the faint, unseen presence of Azel. Guided by the stone's pull, " +
                            $"{soul.Character.Name} uses their resurrection stone to release {resurrectCharacter.Name} from the stone's silent prison.";

                        await resurrectCharacter.Soul.SendAsync(txt);
                        soul.Character.BroadcastToSoulsInRoom(txt);

                        resurrectCharacter.CurrentHP = resurrectCharacter.HP;
                        string returnedTxt = SetResurrectionMark(resurrectCharacter);

                        resurrectCharacter.LivesAgain();
                        resurrectCharacter.GoToRoom(soul.Character.CurrentRoom, false);
                        resurrectCharacter.MoveToGroup(soul.Character);
                        soul.Character.BroadcastToSoulsInRoom(returnedTxt);

                        if (GetRandomInt(4) == 0)
                        {
                            _ = soul.SendAsync($"{wolfSkull.Name} cracks from the immense energy and disintegrates.");
                            soul.Character.DestroyHeldItem(wolfSkull);
                        }

                        ressurectionStone.State = PropState.Depleted;
                        _ = soul.SendAsync($"{ressurectionStone.Name} has been depleted.");
                    }
                    else
                    {
                        _ = soul.SendAsync($"{ressurectionStone.Name} is depleted. Azel whispers in your mind: \"The stone is empty. Pay with your own blood, and it shall awaken again.\"");
                        consumable.WasNotConsumed = true;
                    }

                }
                else
                {
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} sinks into the energy of {FormatPossessive(resurrectCharacter.Name)} soulstone, but nothing happens...");
                    consumable.WasNotConsumed = true;
                }

                return;
            };
        }

        public static Func<Soul, Item, Task> RechargeResurrectionStone()
        {
            return (soul, consumedItem) =>
            {
                if (consumedItem is not Consumable)
                    return Task.CompletedTask;
                Consumable consumable = (Consumable)consumedItem;
                
                consumable.WasNotConsumed = true;

                if (consumable.State == PropState.Depleted)
                {
                    string txt = $"{soul.Character.Name} sacrifices a part of their vitality, and {consumable.Name} begins to glow with renewed power.";
                    soul.Character.BroadcastToSoulsInRoom(txt);
                    soul.Character.SetHealth(soul.Character.CurrentHP /= 2);
                    consumable.State = PropState.Default;
                }
                else
                {
                    _ = soul.SendAsync($"{consumable.Name} vibrates softly, a faint warmth flowing through it. Azel's power already fills the stone.");
                }
                return Task.CompletedTask;
            };
        }

        public static Func<Soul, Item, Task> EatFood()
        {
            return (soul, consumedItem) =>
            {
                Roll hpRoll = new Roll(
                    new Die(1, 12),
                    2,
                    RollType.None,
                    soul.Character);

                soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} eats {consumedItem.Name}, rolls {hpRoll} and gains {hpRoll.GetSum()} health points.");
                soul.Character.GainLife(hpRoll.GetSum());

                return Task.CompletedTask;
            };
        }

        public static Func<Soul, Item, Task> UseHealthPotion()
        {
            return (soul, consumedItem) =>
            {
                Roll hpRoll = new Roll(
                    new Die(1, 6),
                    2,
                    RollType.None,
                    soul.Character);

                soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} drinks a healing potion, rolls {hpRoll} and gains {hpRoll.GetSum()} health points.");
                soul.Character.GainLife(hpRoll.GetSum());

                return Task.CompletedTask;
            };
        }

        public static Func<Soul, Item, Task> SetBearTrap()
        {
            Item trap = new Item("Loaded Beartrap", "This bear trap deals 1d4 piercing damage, stuns its target for 1 round, and roots them for 2 rounds.", 25);
            trap.AddOnAfterMoveToEvent(
                EventKey.TriggerBearTrap,
                true);
            trap.MakeUnpickupable();

            return (soul, consumedItem) =>
            {
                soul.Character.CurrentRoom.AddItem(trap);
                trap.MoveToGroup(soul.Character);
                //trap.Hide(10 + soul.Character.GetModifer(Skill.Stealth));
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} sets {trap.Name}.");
                trap.SetByCharacterId = soul.Character.Id;

                return Task.CompletedTask;
            };
        }

        public static Item BearTrap()
        {
            //should be hidden

            Item trap = new Item("Loaded Beartrap", "This bear trap deals 1d4 piercing damage, stuns its target for 1 round, and roots them for 2 rounds.", 25);
            trap.AddOnAfterMoveToEvent(
                EventKey.TriggerBearTrap, 
                true);

            Consumable unloadedTrap = new Consumable(
                BearTrapName,
                "When set, this bear trap deals 1d4 piercing damage, stuns its target for 1 round, and roots them for 2 rounds.",
                ConsumableKey.SetBearTrap,
                trap.VendorValue);

            return unloadedTrap;
        }
        public static Func<Soul, Item, Task> ReadBookOfTwoWeapons()
        {
            return (soul, consumedItem) =>
            {
                ((Consumable)consumedItem).WasNotConsumed = true;

                if (soul.Character.HasFeat(FeatKey.DualWield))
                {
                    _ = soul.SendAsync(
                    "This book has nothing to teach you.");
                }
                else
                {
                    _ = soul.SendAsync(
                        "* As you read through the book, you acquire the feat of dual-wielding. *");

                    soul.Character.AddFeat(FeatKey.DualWield);
                }

                return Task.CompletedTask;
            };
        }


        /*
        public static Consumable ScrollOfEntanglement()
        {
            int turns = 4;
            Consumable scrollOfEntanglement = new Consumable(
                ScrollofEntanglementName,
                $"Casting Entanglement successfully will root the target for {turns} turns. A rooted target can't move.",
                (soul) =>
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
                async (soul) =>
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
                (soul) =>
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
                async (soul) =>
                {
                    await soul.SendAsync("* As you read through the book, you acquire the feat of dual-wielding. *");
                    soul.Character.AddFeat(FeatKey.DualWield);
                },
                130);
        }
        */

        public static Item HealthPotion()
        {
            return new Consumable(
                HealingPotionName,
                "A potion that restores health, healing for 1d6+2 health points when consumed.",
                ConsumableKey.HealthPotion,
                30);
        }

        public static Item GoldBerryPie()
        {
            return Food("Gold Berry Pie");
        }

        private static Item Food(string name)
        {
            Item food = new Consumable(
                name,
                "A potion that restores health, healing for 1d6+2 health points when consumed.",
                ConsumableKey.SetBearTrap,
                0.8);
            food.UsableInCombat = false;
            
            return food;
        }

        public static Item Soulstone(Character character)
        {
            Consumable soulstone = new Consumable(
                $"Soulstone of {character.NameWithTitle()}",
                $"A flickering soul is trapped within the stone, bearing the likeness of {character.NameWithTitle()}. {character.Description}", 
                ConsumableKey.ResurrectSoul, 
                10000);
            soulstone.Subtype = PropSubtype.Soulstone;
            soulstone.CharacterId = character.Id;
            Program.WorldSoul.SoulstonedCharacters.TryAdd(character.Id, character);
            character.Soul.PlacedInSoulstone = soulstone;

            return soulstone;
        }

        public static Item SoulstoneDust()
        {
            return new Item(
                "Soulstone Ashes",
                "The pale remnants of a shattered soulstone, carrying the last traces of a forgotten soul's essence.",
                0.2
);
        }

        public static Item ResurrectionStone()
        {
            Consumable resurrectionStone = new Consumable(
                Names.ResurrectionStone,
                "A stone orb carrying the faint presence of Azel. " +
                "The sigil of Lorath has been etched into its surface, " +
                "marking it as a vessel entrusted to their care.",
                ConsumableKey.RechargeResurrectionStone,
                500);
            resurrectionStone.State = PropState.Depleted;

            return resurrectionStone;
        }

        public static Item BookOfTwoWeapons()
        {
            Consumable bookofTwoWeapons =  new Consumable(
                "Book of Two Weapons",
                "A book that teaches the art of wielding two weapons at once.",
                ConsumableKey.BookOfTwoWeapons,
                130);
            bookofTwoWeapons.UsableInCombat = false;
            
            return bookofTwoWeapons;
        }

    }
}
