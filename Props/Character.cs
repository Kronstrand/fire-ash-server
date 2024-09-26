using System;
using System.Collections.Generic;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.Props.Items.Armor;
using fire_ash_server.Abstract_Entities;
using static fire_ash_server.Helpers;
using fire_ash_server.World;
using System.Linq;
using System.Collections.Concurrent;

namespace fire_ash_server.Props
{
    internal class Character : Prop
    {
        public Room CurrentRoom;
        public Room? LastRoom;
        private Prop? lookAt;
        public ThreadSafeList<Prop> LookedAt = new ThreadSafeList<Prop>();
        private List<CreatureType> types;
        public Kindred Kindred { get; set; }
        public Gender Gender { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constition { get; set; }
        public int Wisdom { get; set; }
        public int Intelligence { get; set; }
        public int Charisma { get; set; }
        public int HP { get; set; }

        public Weapon DefaultHand = new Fist();

        public bool UniqueName;

        public List<string> Feats = new List<string>();
        public List<Action<Character>> Conditions = new List<Action<Character>>();
        public Faction Faction;
        public bool IsInfluencer = true; //consider change to enum: 1) None (can't infuence), 2) Normal, 3) High (2x)

        public Dictionary<Skill, int> Skills = new Dictionary<Skill, int>();
        public ConcurrentDictionary<InventorySlot, Item> EquippedItems = new ConcurrentDictionary<InventorySlot, Item>();
        public Inventory Inventory = new Inventory();
        public int GP;
        public Journal Journal;
        public DialogueManager? DialogueManager;
        public Character? TradingWith;
        public bool IsTrader = false;
        public Character? SpeakingTo;
        private Dictionary<Prop, ThreadSafeList<string>> UsedMovesOnProp = new Dictionary<Prop, ThreadSafeList<string>>();

        public ToxicRelationship? EnableCombatWith = null;
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
            Journal = new Journal(this);

            types = new List<CreatureType> { CreatureType.Humanoid };
            Kindred = Kindred.Human;
            Gender = RollGender();
            Strength = Roll(3, 6).Sum();
            Dexterity = Roll(3, 6).Sum();
            Constition = Roll(3, 6).Sum();
            Wisdom = Roll(3, 6).Sum();
            Intelligence = Roll(3, 6).Sum();
            Charisma = Roll(3, 6).Sum();

            HP = 8 + GetModifer(Ability.Constitution);
            UniqueName = true;

            Faction = NewPlayerFaction(Name);

            DeathDescription = "todo"; //Todo
        }
        public Character(string name, string description, Kindred kindred, CreatureType creatureType, int strength, int dexterity, int constition, int intelligence, int wisdom, int charisma, string deathDescription) : base(name, description)
        {

            CurrentRoom = Program.WorldSoul.Rooms[Description(RoomKey.Void)];
            init();
            Journal = new Journal(this);

            types = new List<CreatureType> {creatureType};
            Kindred = kindred;
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

        public static Faction NewPlayerFaction(string name)
        {
            return Program.WorldSoul.GetFaction(FactionKey.Players);

            //individual factions for players?
            Faction newPlayerFaction = new Faction(name);
            Faction playerFactionTemplate = Program.WorldSoul.GetFaction(FactionKey.Players);

            var relationships1 = Program.WorldSoul.Relationships.Where(r => r.Faction1 == playerFactionTemplate).ToList();
            foreach (Relationship rel in relationships1)
            {
                Relationship.CreateNew(newPlayerFaction, rel.Faction2, rel.Value);
            }

            var relationships2 = Program.WorldSoul.Relationships.Where(r => r.Faction2 == playerFactionTemplate).ToList();
            foreach (Relationship rel in relationships2)
            {
                Relationship.CreateNew(rel.Faction1, newPlayerFaction, rel.Value);
            }

            return newPlayerFaction;
        }

        public void SetEnableCombatWith(Character enemy)
        {
            EnableCombatWith = new ToxicRelationship(enemy, true);
        }

        public void SetFaction(FactionKey faction)
        {
            Faction = Program.WorldSoul.GetFaction(faction);
        }

        public void AddCreatureType(CreatureType creatureType)
        {
            types.Add(creatureType);
        }

        public bool IsOfCreatureType(CreatureType creatureType)
        {
            return types.Contains(creatureType);
        }

        public void ModifyRelationshipTo(Character? character, int modifier)
        {
            if (character == null)
                return;

            if (!(IsInfluencer && character.IsInfluencer))
                return;

            Relationship rel = GetRelationShipTo(character);
            rel.Value += modifier;

            string message = "";

            if (modifier > 0)
            {
                if (modifier == 1)
                {
                    message = $"{character.Faction.Name} seems pleased with {Faction.Name}.";
                }
                else if (modifier <= 5)
                {
                    message = $"{character.Faction.Name} seems delighted with {Faction.Name}.";
                }
                else
                {
                    message = $"{character.Faction.Name} seems ecstatic with {Faction.Name}.";
                }
            }
            else if (modifier < 0)
            {
                if (modifier == -1)
                {
                    message = $"{character.Faction.Name} seems annoyed with {Faction.Name}.";
                }
                else if (modifier >= -5)
                {
                    message = $"{character.Faction.Name} seems furious with {Faction.Name}.";
                }
                else
                {
                    message = $"{character.Faction.Name} seems enraged with {Faction.Name}.";
                }
            }

            if (message != "")
                CurrentRoom.BroadcastToSoulsInRoom(message);

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
        public void TakeDamage(Damage damage, string sourceName)
        {
            HP -= damage.DmgRoll.GetSum();

            string message = "";
            if (IsHidden())
            {
                message = $"In the shadows {Name} takes {damage} from {sourceName} and is revealed.";
                Unhide();
            }
            else
            {
                message = $"{Name} takes {damage} from {sourceName}.";
            }

            BroadcastToSoulsInRoom(message);
            TestDeath();
        }

        /*public void TakeDamage(Damage damage)
        {
            HP -= damage.DmgRoll.GetSum();
            TestDeath();
        }*/

        public void TestDeath()
        {
            if (HP > 0)
                return;

            if (!Dead)
            {
                Dead = true;
                BroadcastToSoulsInRoom($"{Name} falls to the ground - dead.\n\n" + DeathDescription);
            }           

            CurrentRoom.FlagCombatMightBeResolved();
        }

        public async Task Interrupt()
        {
            if (Soul.Socket == null)
                return;
            
            await Soul.SendAsync("$[cancel]");
            if (TradingWith != null)
            {
                SetLookAt(TradingWith);
                TradingWith = null;
            }
            
            Soul.CancelAndResetTokenSource();          
        }

        public int GetAC()
        {
            int ac = 10;

            ac += GetModifer(Ability.Dexterity);

            EquippedItems.TryGetValue(InventorySlot.OffHand, out Item? item);
            if (item is Shield)
                ac += 2;

            return ac;
        }

        public string? GetRangedAttackDescription(Prop prop)
        {
            Weapon? weapon = GetRangedWeapon();
            if (weapon == null)
                return null;

            return weapon.GetAttackDescription(Name, prop);
        }

        public string? GetMainHandAttackDescription(Prop prop)
        {
            return GetMainHand().GetAttackDescription(
                Name,
                prop);
        }

        public string? GetOffHandAttackDescription(Prop prop)
        {
            return GetOffHand().GetOffHandAttackDescription(Name, prop.Name);
        }

        public string? GetTeethAttackDescription(Prop prop)
        {
            return GetTeethWeapon().GetAttackDescription(
                Name,
                prop);
        }

        public Roll GetMeleeAttackRoll(Weapon meleeWeapon)
        {
           return new Roll(GetModifer(Skill.CloseCombat) + meleeWeapon.Modifier, RollType.AttackRoll, this);
        }

        public Roll GetRangedAttackRoll(Weapon rangedWeapon)
        {
            return new Roll(GetModifer(Skill.RangedCombat) + rangedWeapon.Modifier, RollType.AttackRoll, this);
        }

        public Damage GetRangedDamageRoll(Weapon rangedWeapon)
        {
            return new Damage(
                new Roll(rangedWeapon.DamageDie, GetModifer(Ability.Dexterity) + rangedWeapon.Modifier, RollType.DamageRoll, this),
                rangedWeapon.DamageType);
        }

        public Damage GetMainMeleeDamageRoll(Weapon mainHandWeapon)
        {
            return new Damage(
                new Roll(mainHandWeapon.DamageDie, GetModifer(Ability.Strength) + mainHandWeapon.Modifier, RollType.DamageRoll, this),
                mainHandWeapon.DamageType);
        }

        public Damage GetOffHandDamageRoll(Weapon offHandWeapon)
        {
            return new Damage(
                new Roll(offHandWeapon.DamageDie, offHandWeapon.Modifier, RollType.DamageRoll, this),
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

            return DefaultHand;
        }

        public Weapon GetOffHand()
        {
            if (EquippedItems.TryGetValue(InventorySlot.OffHand, out Item? offHand))
                if (offHand is Weapon)
                    return (Weapon)offHand;

            return DefaultHand;
        }

        public Weapon GetTeethWeapon()
        {
            if (EquippedItems.TryGetValue(InventorySlot.Teeth, out Item? teeth))
                if (teeth is Weapon)
                    return (Weapon)teeth;

            return DefaultHand;
        }

        public bool TryUnequipFromSlot(InventorySlot inventorySlot)
        {
            EquippedItems.TryRemove(inventorySlot, out Item? removedItem);
            if (removedItem != null)
            {
                Inventory.AddItem(removedItem);

                foreach (Effect effect in removedItem.EquipEffects)
                    Effects.Remove(effect);

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
            Roll roll = GetRangedAttackRoll(weapon);
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
            Weapon weapon = GetMainHand();
            Roll roll = GetMeleeAttackRoll(weapon);
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetMainMeleeDamageRoll(weapon);
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
            Weapon wapon = GetOffHand();
            Roll roll = GetMeleeAttackRoll(wapon);
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetMainMeleeDamageRoll(wapon);
                characterToAttack.TakeDamage(
                    damage,
                    this,
                    wapon.Name,
                    attack + SingleHitMessage(roll),
                    true);
            }
            else
            {
                BroadcastToSoulsInRoom(attack + SingleMissMessage(roll));
            }
        }

        public void AttackWithTeeth(Character characterToAttack)
        {
            string? attack = GetTeethAttackDescription(characterToAttack);
            Weapon weapon = GetTeethWeapon();
            Roll roll = GetMeleeAttackRoll(weapon);
            if (roll.GetSum() >= characterToAttack.GetAC())
            {
                Damage damage = GetMainMeleeDamageRoll(weapon);
                characterToAttack.TakeDamage(
                    damage,
                    this,
                    weapon.Name,
                    attack + SingleHitMessage(roll),
                    true);
                    
                //poison damage    
                Roll savingThrow = new Roll(characterToAttack.GetModifer(Ability.Constitution), RollType.SavingThrow, characterToAttack);
                if (!savingThrow.BeatsDC(13))
                {
                    Roll dmgRoll = new Roll(new Die(1,4), 0, RollType.DamageRoll, this);
                    Damage poisonDmg = new Damage(dmgRoll, DamageType.Poison);
                    string preDmgMessage = $"{characterToAttack.Name} fails constitution saving throw against {FormatPossessive(this.Name)} poisonous bite with a roll of {savingThrow}.";
                    characterToAttack.TakeDamage(poisonDmg, this, "venom", preDmgMessage, true);
                }
                else
                {
                    BroadcastToSoulsInRoom($"{characterToAttack.Name} succeeds constitution saving throw against {FormatPossessive(this.Name)} poisonous bite with a roll of {savingThrow}.");
                }   
            }
            else
            {
                BroadcastToSoulsInRoom(attack + SingleMissMessage(roll));
            }
        }

        public string AddToInventory(Item itemToAdd)
        {
            itemToAdd.RemoveFromCurrentGrouping();
            Inventory.AddItem(itemToAdd);
            LookBackFromItem(itemToAdd);
            return $"{Name} added {itemToAdd.Name} to their inventory.";
        }

        public void AddEquippedItem(InventorySlot inventorySlot, Item item)
        {
            item.ClearHeldBy();
            if (!EquippedItems.TryAdd(inventorySlot, item))
                throw new Exception($"{Description(inventorySlot)} has already an eqipped item.");
            item.HeldBy = this;

            foreach(Effect effect in item.EquipEffects)
                Effects.Add(effect);

            LookBackFromItem(item);
        }
        public void AddCarriedItem(Item item, Item MoveTo)
        {
            MoveTo.AddItem(item);
            LookBackFromItem(item);
        }

        public void GoToRoom(Room room)
        {
            LastRoom = CurrentRoom;
            RemoveFromCurrentRoom();
            CurrentRoom = room;
            CurrentRoom.Characters.Add(this);

            Exit? exitInNewRoom = CurrentRoom.Exits.Where(exit => exit.GoToRoom == LastRoom).FirstOrDefault();
            if (exitInNewRoom != null)
                MoveToGroup(exitInNewRoom);

            ResetLookAt();
            LastRoom.BroadcastToSoulsInRoom($"{Name} left, heading in the direction of {room.Name}.", this);
            if (LastRoom.RoomKey == Description(RoomKey.Void))
                room.BroadcastToSoulsInRoom($"{Name} enters The {room.Name}.", this);
            else
                room.BroadcastToSoulsInRoom($"{Name} enters The {room.Name} from The {LastRoom.Name}.", this);
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

        public void RemoveFromCurrentRoom()
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
                "Kindred: " + Kindred + "\n" +
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

            Skills.TryGetValue(skill, out int modifier);
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

        public void EnableCombat(ToxicRelationship toxicRel)
        {
            if (InCombat && toxicRel.ToxicCharacter.InCombat)
                return;

            Character aggressor = this;
            Character victim = toxicRel.ToxicCharacter;
            if (toxicRel.ToxicCharacterIsInitiator)
            {
                aggressor = toxicRel.ToxicCharacter;
                victim = this;
            }

            RelationshipStatus relStatus = aggressor.GetRelationshipStatus(victim);

            if (relStatus == RelationshipStatus.good)
                aggressor.ModifyRelationshipTo(victim, -10);
            else if (relStatus == RelationshipStatus.neutral)
                aggressor.ModifyRelationshipTo(victim, -5);

            CurrentRoom.EnableOrUpdateCombat(this, toxicRel.ToxicCharacter);
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
            else if (move.Range == RangeType.CloseSingleTarget && IsInGroupWith(target) == false)
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

        public bool HasPointLight()
        {
            foreach(Effect effect in GetAllEffectsIncludingFeats())
            {
                if (effect.LightPointerModifer >= Light.Dim)
                    return true;
            }
            return false;
        }

        public List<Effect> GetAllEffectsIncludingFeats()
        {
            List<Effect> allEffects = Effects.ToList();
            foreach(string featName in Feats)
            {
                Feat? feat = World.Feats.GetWithoutMoves(featName, Soul);
                if (feat != null)
                {
                    allEffects.AddRange(feat.Effects);
                }
            }
            return allEffects;
        }

        public void AddFeat(FeatKey key)
        {
            Feats.Add(Description(key));
        }

        public bool HasFeat(EffectKey key)
        {
            return Feats.Contains(Description(key));
        }
    }
}
