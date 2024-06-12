using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Abstract_Entities;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props
{
    internal class Character : Prop
    {
        public Room CurrentRoom;
        private Prop? lookAt;
        public ThreadSafeList<Prop> LookedAt = new ThreadSafeList<Prop>();
        public Race Race { get; set; }
        public Gender Gender { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constition { get; set; }
        public int Wisdom { get; set; }
        public int Intelligence { get; set; }
        public int Charisma { get; set; }
        public int Proficiency { get; set; }
        public int HP { get; set; }

        public bool UniqueName;

        public List<string> Feats = new List<string>();
        public Faction Faction;

        public Dictionary<Skill, int> Skills = new Dictionary<Skill, int>();
        public Dictionary<InventorySlot, Item> EquippedItems = new Dictionary<InventorySlot, Item>();
        public Inventory Inventory = new Inventory();
        public DialogueManager? DialogueManager;
        public Character? SpeakingTo;
        private Dictionary<Prop, ThreadSafeList<string>> UsedMovesOnProp = new Dictionary<Prop, ThreadSafeList<string>>();

        public Character? EnableCombatWith = null;
        public bool InCombat;
        public bool Dead;
        public string DeathDescription;
        public Soul Soul;

        public static Dictionary<string, List<Func<string, string, string>>> hitReactions = new Dictionary<string, List<Func<string, string, string>>>();

        public Character(Soul soul, string name) : base(name, "")
        {
            
            CurrentRoom = Program.WorldSoul.GetRoom(RoomKey.Void);
            Soul = soul;

            init();

            Race = Race.Human;
            Gender = RollGender();
            Strength = Roll(3, 6).Sum();
            Dexterity = Roll(3, 6).Sum();
            Constition = Roll(3, 6).Sum();
            Wisdom = Roll(3, 6).Sum();
            Intelligence = Roll(3, 6).Sum();
            Charisma = Roll(3, 6).Sum();

            HP = 8 + GetModifer(Ability.Constitution);

            Proficiency = 2;
            UniqueName = true;

            Faction = Program.WorldSoul.GetFaction(FactionKey.Players);         

            DeathDescription = "todo"; //Todo
        }
        public Character(string name, string description, Race race , int strength, int dexterity, int constition, int intelligence, int wisdom, int charisma, string deathDescription) : base(name, description)
        {

            CurrentRoom = Program.WorldSoul.Rooms[RoomKey.Void];
            init();

            Race = race;
            Gender = RollGender();
            Strength = strength;
            Dexterity = dexterity;
            Constition = constition;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;

            Faction = Program.WorldSoul.GetFaction(FactionKey.Wilders);

            DeathDescription = deathDescription;

            Soul = new Soul(this);
        }

        public void init()
        {
            Inventory.HeldBy = this;
        }

        public void ModifyRelationshipTo(Character? character, int modifyer)
        {
            if (character == null)
                return;

            Relationship rel = GetRelationShipTo(character);
            rel.Value += modifyer;
        }

        public void Speak(string messaage)
        {
            CurrentRoom.BroadcastToSoulsInRoom($"{Name} says:\"{messaage}\"");
        }

        public void CreateDialogueManager(DialogueNode startingNode)
        {
            DialogueManager = new DialogueManager(this, startingNode);
        }

        public string? GetHitReaction(string attacker, string target)
        {
            if (!hitReactions.ContainsKey(Name))
            {
                return null;
            }

            Random rand = new Random();

            int rndIndex = rand.Next(hitReactions[Name].Count);
            return hitReactions[Name][rndIndex].Invoke(attacker, target);
        }

        public int RollInitiative()
        {
            return Roll(1, 20).Sum() + GetModifer(Ability.Dexterity);
        }

        public void TakeDamage(Damage damage, Character sourceChar, string sourceName, string preDmgMessage, bool fromAttack)
        {
            HP -= damage.DmgRoll.GetSum();

            if (preDmgMessage != null || preDmgMessage != "")
                preDmgMessage += "\n";

            string message = "";
            if (IsHidden())
            {             
                message = preDmgMessage + $"In the shadows {Name} takes {damage} from {sourceChar.Name}'s {sourceName} and is revealed.";
                Unhide();
            }
            else
            {
                message = preDmgMessage + $"{Name} takes {damage} from {sourceChar.Name}'s {sourceName}.";
            }

            if (fromAttack)
            {
                string? hitReaction = GetHitReaction(sourceChar.Name, this.Name);
                if (hitReaction != null)
                    message += $"\n{hitReaction}.";
            }

            BroadcastToSoulsInRoom(message);

            TestDeath();
        }

        public void TestDeath()
        {
            if (HP > 0)
                return;

            Dead = true;
            BroadcastToSoulsInRoom($"{Name} falls to the ground - dead.\n\n" +
                DeathDescription);


            CurrentRoom.FlagCombatMightBeResolved();
        }

        public int GetAC()
        {
            return 10 + GetModifer(Ability.Dexterity);
        }

        public string? GetRangedAttackDescription(Prop prop)
        {
            Weapon? weapon = GetRangedWeapon();
            if (weapon == null)
                return null;

            return weapon.GetAttackDescription(Name, prop.Name);
        }

        public string? GetMainHandAttackDescription(Prop prop)
        {
            return GetMainHand().GetAttackDescription(Name, prop.Name);
        }

        public string? GetOffHandAttackDescription(Prop prop)
        {
            return GetOffHand().GetOffHandAttackDescription(Name, prop.Name);
        }

        public Roll GetAttackRoll()
        {
           return new Roll(GetModifer(Skill.CloseCombat), RollType.AttackRoll, this);
        }

        public Damage GetRangedDamageRoll(Weapon rangedWeapon)
        {
            return new Damage(
                new Roll(rangedWeapon.DamageDie, GetModifer(Ability.Dexterity), RollType.DamageRoll, this),
                rangedWeapon.DamageType);
        }

        public Damage GetMainHandDamageRoll()
        {
            Weapon mainHandWeapon = GetMainHand();

            return new Damage(
                new Roll(mainHandWeapon.DamageDie, GetModifer(Ability.Strength), RollType.DamageRoll, this),
                mainHandWeapon.DamageType);
        }

        public Damage GetOffHandDamageRoll()
        {
            Weapon offHandWeapon = GetOffHand();

            return new Damage(
                new Roll(offHandWeapon.DamageDie, 0, RollType.DamageRoll, this),
                offHandWeapon.DamageType);
        }

        public Weapon? GetRangedWeapon()
        {
            if (EquippedItems.TryGetValue(InventorySlot.Ranged, out Item? rangedWeapon))
                if (rangedWeapon is Weapon)
                    return (Weapon)rangedWeapon;

            return null;
        }

        public Weapon GetMainHand()
        {
            if (EquippedItems.TryGetValue(InventorySlot.MainHand, out Item? mainHand))
                if (mainHand is Weapon)
                    return (Weapon)mainHand;

            return new Fist();
        }

        public Weapon GetOffHand()
        {
            if (EquippedItems.TryGetValue(InventorySlot.OffHand, out Item? offHand))
                if (offHand is Weapon)
                    return (Weapon)offHand;

            return new Fist();
        }

        public bool TryUnequipFromSlot(InventorySlot inventorySlot)
        {
            if (EquippedItems.ContainsKey(inventorySlot))
            {
                Item equippedItem = EquippedItems[inventorySlot];
                EquippedItems.Remove(inventorySlot);
                Inventory.AddItem(equippedItem);
                return true;
            }
            return false;
        }

        public void AttackWithRanged(Character characterToAttack)
        {
            Weapon? weapon = GetRangedWeapon();
            if (weapon == null) 
                return;
            string? attack = GetRangedAttackDescription(characterToAttack);
            Roll roll = GetAttackRoll();
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetRangedDamageRoll(weapon);

                characterToAttack.TakeDamage(
                    damage,
                    this,
                    weapon.Name,
                    attack + SingleHitMessage(roll),
                    true);
            }
            else
            {
                BroadcastToSoulsInRoom(attack + SingleMissMessage(roll));
            }
        }

        public void AttackWithMainHand(Character characterToAttack)
        {
            string? attack = GetMainHandAttackDescription(characterToAttack);
            Roll roll = GetAttackRoll();
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetMainHandDamageRoll();
                characterToAttack.TakeDamage(
                    damage,
                    this,
                    GetMainHand().Name,
                    attack + SingleHitMessage(roll),
                    true);
            }
            else
            {
                BroadcastToSoulsInRoom(attack + SingleMissMessage(roll));
            }
        }

        public int GetPassiveDC(Skill skill)
        {
            return 10 + GetModifer(skill);
        }
        public int GetPassiveDC(Ability ability)
        {
            return 10 + GetModifer(ability);
        }


        public void AttackWithOffhand(Character characterToAttack)
        {
            string? attack = GetOffHandAttackDescription(characterToAttack);
            Roll roll = GetAttackRoll();
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetMainHandDamageRoll();
                characterToAttack.TakeDamage(
                    damage,
                    this,
                    GetMainHand().Name,
                    attack + SingleHitMessage(roll),
                    true);
            }
            else
            {
                BroadcastToSoulsInRoom(attack + SingleMissMessage(roll));
            }
        }

        public string AddToInventory(Item itemToAdd)
        {
            Inventory.AddItem(itemToAdd);
            LookBackFromItem(itemToAdd);
            return $"{Name} added {itemToAdd.Name} to their inventory.";
        }

        public string AddToInventory2(Item itemToAdd) //obsolete
        {
            bool bothHandsFull = EquippedItems.ContainsKey(InventorySlot.MainHand) && EquippedItems.ContainsKey(InventorySlot.OffHand);
            if (bothHandsFull)
            {
                return $"{Name} tries to grab {itemToAdd.Name}, but has no free hands.";
            }

            //add to "random" container
            foreach (KeyValuePair<InventorySlot, Item> kvp in EquippedItems.Where(kvp => kvp.Value.IsContainer))
            {
                Item container = kvp.Value;
                AddCarriedItem(itemToAdd, container);
                return $"{Name} grabs {itemToAdd.Name} and put it in {container.Name}.";
            }
                 
            foreach (InventorySlot inventorySlot in itemToAdd.CarriableByInventorySlots)
            {
                if (!EquippedItems.ContainsKey(inventorySlot))
                {
                    AddEquippedItem(inventorySlot, itemToAdd);
                    switch (inventorySlot)
                    {
                        case InventorySlot.MainHand or 
                             InventorySlot.OffHand:
                            return 
                                $"{Name} grabs {itemToAdd.Name} and holds it with their {Description(inventorySlot)}.";
                        case InventorySlot.Waist:
                            return 
                                $"{Name} grabs {itemToAdd.Name} and places it at their {Description(inventorySlot)}.";
                    }
                }
            }

            if (!EquippedItems.ContainsKey(InventorySlot.MainHand))
            {
                AddEquippedItem(InventorySlot.MainHand, itemToAdd);
                return 
                    $"{Name} grabs {itemToAdd.Name} with their main hand and keeps it there since they have no place to put it.";
            }
            if (!EquippedItems.ContainsKey(InventorySlot.OffHand))
            {
                AddEquippedItem(InventorySlot.OffHand, itemToAdd);
                return 
                    $"{Name} grabs {itemToAdd.Name} with their off-hand and keeps it there since they have no place to put it.";
            }
            return
                $"By some devine intervention, {Name} can't seem to grab {itemToAdd.Name}. Is it fate, or a glitch in the matrix?";
        }

        public void AddEquippedItem(InventorySlot inventorySlot, Item item)
        {
            item.ClearHeldBy();
            EquippedItems.Add(inventorySlot, item);
            item.HeldBy = this;
            LookBackFromItem(item);
        }
        public void AddCarriedItem(Item item, Item MoveTo)
        {
            MoveTo.AddItem(item);
            LookBackFromItem(item);
        }

        public void GoToRoom(Room room)
        {
            Room xRoom = CurrentRoom;
            LeaveCurrentRoom();
            CurrentRoom = room;
            CurrentRoom.Characters.Add(this);

            Exit? exitInNewRoom = CurrentRoom.Exits.Where(exit => exit.GoToRoom == xRoom).FirstOrDefault();
            if (exitInNewRoom != null)
                MoveToGroup(exitInNewRoom);

            ResetLookAt();
            xRoom.BroadcastToSoulsInRoom($"{Name} left, heading in the direction of {room.Name}.", this);
            room.BroadcastToSoulsInRoom($"{Name} enters The {room.Name} from The {xRoom.Name}.", this);
        }

        public void BroadcastToSoulsInRoom(string message)
        {
            CurrentRoom.BroadcastToSoulsInRoom(this, message, null);
        }

        public void LookBackFromItem(Item fromItem)
        {
            if (fromItem == lookAt)
                LookBack();     
        }

        public void LookBack()
        {
            lookAt = LookedAt.RemoveLastItemAndGetNextItem();

            if (lookAt == null || lookAt.IsInRoomOrIsRoom(CurrentRoom) == false)
                ResetLookAt();
        }

        public void SetLookAt(Prop prop)
        {
            lookAt = prop;
            if (LookedAt.Count == 0 || !ReferenceEquals(prop, LookedAt.GetAt(0)))
                LookedAt.Add(prop);
        }
        public Prop? LookAt
        {
            get { return lookAt; }
        }
        public void ResetLookAt()
        {
            LookedAt = new ThreadSafeList<Prop>();
            SetLookAt(CurrentRoom);         
        }

        public void LeaveCurrentRoom()
        {
            if (CurrentRoom != null)
            {
                RemoveFromCurrentGrouping();
                CurrentRoom.Characters.Remove(this);
            }
        }

        public string StatsToString()
        {
            return
                "Name: " + Name + "\n" +
                "Race: " + Race + "\n" +
                "Gender: " + Description(Gender) + "\n" +
                "Strength: " + Strength + "\n" +
                "Dexterity: " + Dexterity + "\n" +
                "Constitution: " + Constition + "\n" +
                "Intelligence: " + Intelligence + "\n" +
                "Wisdom: " + Wisdom + "\n" +
                "Charisma: " + Charisma;
        }

        public int GetModifer(Ability ability)
        {
            switch (ability)
            {
                case Ability.Strength: return CalculateModifer(Strength);
                case Ability.Dexterity: return CalculateModifer(Dexterity);
                case Ability.Constitution: return CalculateModifer(Constition);
                case Ability.Intelligence: return CalculateModifer(Intelligence);
                case Ability.Wisdom: return CalculateModifer(Wisdom);
                case Ability.Charisma: return CalculateModifer(Charisma);
            }

            throw new Exception($"Ability not found: {Description(ability)}.");
        }

        public int GetModifer(Skill skill)
        {
            Ability ability = SkillNumber.GetRelatedAbility(skill);

            Skills.TryGetValue(skill, out  int modifier);
            modifier += GetModifer(ability);

            return modifier;
        }

        public bool HasUsedMoveOnProp(Move move)
        {
            if (move.Prop == null)
                throw new ArgumentNullException(nameof(move.Prop), $"Move {move.Description} has no Prop.");

            return HasUsedMoveOnProp(move.GetObjectName(), move.Prop);
        }

        public bool HasUsedMoveOnProp(string moveClassName, Prop prop)
        {
            if (UsedMovesOnProp.ContainsKey(prop))
            {
                return UsedMovesOnProp[prop].Contains(moveClassName);
            }
            return false;
        }

        public void RegisterUsedMoveOnProp(Move move)
        {
            if (move.Prop == null || move.Repeatable)
                return;

            if (!UsedMovesOnProp.ContainsKey(move.Prop))
            {
                UsedMovesOnProp.Add(move.Prop, new ThreadSafeList<string>());
            }
            
            UsedMovesOnProp[move.Prop].Add(move.GetObjectName());
        }

        public void TryEnableCombat()
        {
            if (EnableCombatWith == null)
                return;
            EnableCombat(EnableCombatWith);
            EnableCombatWith = null;
        }

        public void EnableCombat(Character enemy)
        {
            if (this.InCombat && enemy.InCombat)
                return;

            CurrentRoom.EnableOrUpdateCombat(this, enemy);
        }

        public Relationship GetRelationShipTo(Character character)
        {
            Relationship? rel = Program.WorldSoul.Relationships.FirstOrDefault(rel =>
                                                                (rel.Faction1 == this.Faction && rel.Faction2 == character.Faction) ||
                                                                (rel.Faction1 == character.Faction && rel.Faction2 == this.Faction));
            if (rel == null)
                rel = Relationship.CreateNew(Faction, character.Faction);

            return rel;
        }

        public void AddRelatedRelationshipToCombat(Character enemy)
        {
            Relationship relationship = GetRelationShipTo(enemy);

            if (!CurrentRoom.RelationshipsInHostileCombat.Contains(relationship))
                CurrentRoom.RelationshipsInHostileCombat.Add(relationship);
        }

        public bool IsInHostileCombatWith(Character enemy)
        {
            Relationship? relationship = CurrentRoom.RelationshipsInHostileCombat.FirstOrDefault(rel =>
                                                                ((rel.Faction1 == this.Faction && rel.Faction2 == enemy.Faction) ||
                                                                (rel.Faction1 == enemy.Faction && rel.Faction2 == this.Faction)));

            if (relationship == null)
                return false;

            return true;
        }

        public RelationshipStatus GetRelationshipStatus(Character relChar)
        {
            Relationship relationship = GetRelationShipTo(relChar);
            return relationship.GetStatus();
        }

        private Gender RollGender()
        {
            int dieRollSum = Roll(2, 20).Sum();

            if (dieRollSum <= 20)
            {
                return Gender.Male;
            }
            else if (dieRollSum <= 39)
            {
                return Gender.Female;
            }
            else
            {
                return Gender.DualSoul;
            }
        }
        /*public bool PropIsWithinReach(Prop prop)
        {

            if (prop.IsInRoomOrIsRoom(CurrentRoom) || prop.IsHidden())
            {
                _ = Soul.SendAsync($"{prop.Name} has become out of reach...");
                return false;
            }
            return true;
        }*/

        public bool PropTargetIsValid(Move move)
        {
            if (move.Prop == null)
                return true;

            return PropTargetIsValid(move, move.Prop);

        }
        public bool PropTargetIsValid(Move move, Prop? target)
        {
            if (target == null)
                return true;

            if (ValidPropTargetException(move, target))
                return true;

            if (target is Item && ((Item)target).HeldByCharacter() == this)
                return true;

            bool outOfReach = false;

            if (target.IsHidden())
                outOfReach = true;
            else if (move.PropPosition != null && move.PropPosition != target.GetPropPosition())
                outOfReach = true;
            else if (!target.IsInRoomOrIsRoom(CurrentRoom))
                outOfReach = true;
            else if (move.Range == RangeType.CloseSingleTarget && IsInGroupWith(target) != true)
                outOfReach = true;

            if (outOfReach)
            {
                _ = Soul.SendAsync($"{target.Name} is not there anymore.");
                move.EnablesCombat = false;
                LookBack();
                return false;
            }
            return true;
        }

        private bool ValidPropTargetException(Move move, Prop target)
        {
            if (move is RoomChange)
                return true;

            return false;
        }

        public bool AttackTargetIsWithinReach(Character characterToAttack, RangeType rangeType)
        {
            bool withinReach = true;
            if (characterToAttack.IsHidden())
            {
                _ = Soul.SendAsync($"{characterToAttack.Name} is nowhere to be seen.");
                withinReach = false;
            }
            else if (rangeType == RangeType.RangeSingleTarget && characterToAttack.CurrentRoom != CurrentRoom)
            {
                _ = Soul.SendAsync($"{characterToAttack.Name} is nowhere to be seen.");
                withinReach = false;
            }
            else if (rangeType == RangeType.CloseSingleTarget && IsInGroupWith(characterToAttack) != true)
            {
                _ = Soul.SendAsync($"{characterToAttack.Name} has slipped out of reach...");
                withinReach = false;
            }

            return withinReach;
        }

        public void AddFeat(FeatKey featKey)
        {
            Feats.Add(Description(featKey));
        }
    }
}
