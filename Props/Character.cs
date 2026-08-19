using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Armoring;
using fire_ash_server.Props.Items.Weapons;
using fire_ash_server.World;
using fire_ash_server.World.AI;
using Newtonsoft.Json.Linq;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props
{
    internal class Character : Prop
    {       
        [JsonInclude]   public string Title = "";
        [JsonInclude]   public bool NPC = true;
        [JsonIgnore]    public Room CurrentRoom;
        [JsonPropertyName("CurrentRoom")]
        [JsonInclude]   public string CurrentRoomSerialization
                        { 
                            get => Program.WorldSoul.GetRoomKey(CurrentRoom);
                            set => CurrentRoom = Program.WorldSoul.GetRoom(value);     
                        }      

        [JsonIgnore]    public Room? LastRoom;
        [JsonIgnore]    private Prop? lookAt;
        [JsonIgnore]    public Prop? lookAtBeforeInventory;
        [JsonIgnore]    public ThreadSafeList<Prop> LookedAt = new ThreadSafeList<Prop>();
        [JsonInclude]   private List<CreatureType> Types;
        [JsonInclude]   public Kindred Kindred { get; set; }
        [JsonInclude]   public Gender Gender { get; set; }
        [JsonInclude]   public int Strength { get; set; }
        [JsonInclude]   public int Dexterity { get; set; }
        [JsonInclude]   public int Constition { get; set; }
        [JsonInclude]   public int Wisdom { get; set; }
        [JsonInclude]   public int Intelligence { get; set; }
        [JsonInclude]   public int Charisma { get; set; }
        [JsonInclude]   private int hp { get; set; }
        [JsonInclude]   public int CurrentHP { get; set; }
        [JsonInclude]   public Weapon DefaultHand = new Fist();

        [JsonInclude]   public bool UniqueName;

        [JsonInclude]   public List<string> Feats = new List<string>();

        [JsonIgnore]    public ThreadSafeList<BuffDebuff> BuffDebuffs = new ThreadSafeList<BuffDebuff>();
        [JsonPropertyName("BuffDebuffs")]
        [JsonInclude]   public List<BuffDebuff> ConditionsSerializable
                        {
                            get => BuffDebuffs.ToList();
                            set => BuffDebuffs = new ThreadSafeList<BuffDebuff>(value);
                        }
        [JsonInclude]   public Faction Faction;
        [JsonInclude]   public bool IsInfluencer = true; //consider change to enum: 1) None (can't infuence), 2) Normal, 3) High (2x)
        [JsonInclude]   public Dictionary<Skill, int> Skills = new Dictionary<Skill, int>();
        [JsonInclude]   public ConcurrentDictionary<InventorySlot, Item> EquippedItems = new ConcurrentDictionary<InventorySlot, Item>();
        [JsonInclude]   public Inventory Inventory = new Inventory();
        [JsonInclude]   public Journal Journal;
        [JsonIgnore]    public DialogueManager? DialogueManager;
        [JsonInclude]   private DialogueKey? dialogueKey;
        [JsonIgnore]    public Character? TradingWith;
        [JsonInclude]   public bool IsTrader = false;
        [JsonInclude]   public double TradeModifier = 0.0;
        [JsonIgnore]    public Character? SpeakingTo;
        [JsonIgnore]    private Dictionary<Prop, ThreadSafeList<string>> UsedMovesOnProp = new Dictionary<Prop, ThreadSafeList<string>>();
        [JsonIgnore]    public bool InitAttack = true;
        [JsonIgnore]    public ToxicRelationship? EnableCombatWith = null;
        [JsonInclude]   public bool InCombat;
        [JsonInclude]   public bool Dead;
        [JsonInclude]   private string deathDescription;
        [JsonInclude]   public DateTime TimeOfDeath;
        [JsonIgnore]    public Soul Soul;
        [JsonInclude]   public static Dictionary<string, List<Func<string, string, string>>> hitReactions = new Dictionary<string, List<Func<string, string, string>>>();
        [JsonIgnore]    public Action<Soul, Character>? OnBeforeSpeakTo;
        [JsonIgnore]    public Action<Soul, Character>? OnAfterSpeakTo;

        [JsonInclude]   public BehaviorKey BehaviorKey = BehaviorKey.None;
        [JsonInclude]   public Stack<Goal> Goals = new Stack<Goal>();
        [JsonInclude]   public DateTime LastAte { get; set; } = DateTime.UtcNow;

        public Character() { }

        public Character(Soul soul, string name) : base(name, "", name + "-" + Guid.NewGuid().ToString())
        {
            CurrentRoom = Program.WorldSoul.GetRoom(RoomKey.Void);
            Soul = soul;
            NPC = false;

            init();
            Journal = new Journal(this);

            Types = new List<CreatureType> { CreatureType.Humanoid };
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

            DeathDescription = "";

        }
        public Character(string name, string description, Kindred kindred, CreatureType creatureType, int strength, int dexterity, int constition, int intelligence, int wisdom, int charisma, string deathDescription) : base(name, description, Guid.NewGuid().ToString())
        {

            CurrentRoom = Program.WorldSoul.Rooms[Description(RoomKey.Void)];
            init();
            Journal = new Journal(this);

            Types = new List<CreatureType> {creatureType};
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

        public static Character? GetCharacterFromId(string id)
        {
            //not including solestoned characters
            foreach(Room room in Program.WorldSoul.Rooms.Values)
            {
                Character? character = room.Characters.Where(c => c.Id == id).FirstOrDefault();
                if (character != null) 
                    return character;
            }
            
            return null;
        }

        [JsonInclude]
        public string DeathDescription
        {
            get 
            {
                if (deathDescription == "")
                    return $"{Name} lies motionless, their frame slumped as if the weight of existence itself had finally become too much. No grand farewell, no final words, just the quiet departure of something that was here, and now is not..";
                return deathDescription; 
            } 
            set { deathDescription = value; }
        }

        public string NameWithTitle()
        {
            if (Title == "")
                return Name;

            return $"{Name} the {Title}";
        }

        public static Faction NewPlayerFaction(string name)
        {
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

        public void ChangeConstitutionAndUpdateHp(int addCon)
        {
            int oldModifier = GetModifer(Ability.Constitution);

            Constition += addCon;

            int newModifier = GetModifer(Ability.Constitution);

            int modifierDifference = newModifier - oldModifier;

            SetMaxHpOnly(hp + modifierDifference);
        }

        public void SetMaxHpOnly(int newMaxHp)
        {
            hp = newMaxHp;
            if (CurrentHP > hp)
                CurrentHP = hp;
        }

        [JsonIgnore]
        public int HP
        {
            get => hp;
            set
            {
                if (value < 0)
                    throw new ArgumentException("HP cannot be negative.");

                int diff = value - hp; // Calculate the difference between the new HP and the current HP

                if (diff > 0) // If HP is being increased
                {
                    CurrentHP = Math.Min(CurrentHP + diff, value); // Add the difference to CurrentHP, but not exceeding new HP
                }
                else if (diff < 0) // If HP is being decreased
                {
                    CurrentHP = Math.Max(0, CurrentHP + diff); // Subtract the difference from CurrentHP, but not below zero
                }

                hp = value; // Finally, set the new HP value
            }
        }

        public bool HasItem(string itemName)
        {
            return GetItem(itemName) != null;
        }

        public Item? GetItem(string itemName)
        {
            //should this exlude bodyparts?
            Item? item = Inventory.Items.FirstOrDefault(i => i.Name == itemName);
            if (item != null)
                return item;

            return EquippedItems.Values.FirstOrDefault(i => i.Name == itemName);
        }

        public void SetDialogue(DialogueKey dialogueKey)
        {
            this.dialogueKey = dialogueKey;
            SetDialogueManager();
        }
        public DialogueKey? GetDialogueKey()
        {
            return dialogueKey;
        }

        public void SetDialogueManager()
        {
            if (dialogueKey == null)
                return;
            
            Dialogues.Registry.TryGetValue((DialogueKey)dialogueKey, out Func<DialogueNode>? getDialogeuNode);
            if (getDialogeuNode != null)
                CreateDialogueManager(getDialogeuNode());      
        }

        public Item? GetLookingAtUnpickupable()
        {
            return LookedAt.LastOrDefault(p => p.GetType() == typeof(Item) && !p.IsPickupable()) as Item;
        }

        public void AddBuffDebuff(BuffDebuff buffDebuff)
        {
            if (buffDebuff.Unique)
                BuffDebuffs.RemoveAll(b => b.Name == buffDebuff.Name);

            BuffDebuffs.Add(buffDebuff);
        }

        public bool HasCondition(Condition condition)
        {
            foreach(BuffDebuff activeCondition in BuffDebuffs)
            {
                if (activeCondition.Condition == condition)
                    return true;
            }
            return false;
        }

        public void TickConditionsDown(bool endOfCombat)
        {
            TickBuffsDown(endOfCombat, true);
        }
        public void TickBuffsDown(bool endOfCombat, bool broardcast)
        {
            if (!InCombat && !Program.NewGlobalTurn)
                return;
            
            List<string> removedConditionsAndBuffs = new List<string>();
            ThreadSafeList<BuffDebuff> buffsDebuffsToBeRemoved = new ThreadSafeList<BuffDebuff>();
            foreach (BuffDebuff buffDebuff in BuffDebuffs)
            {

                if (buffDebuff.CreatedThisTurn)
                {
                    buffDebuff.CreatedThisTurn = false;
                    if (InCombat)
                        continue;
                }
                buffDebuff.Turns--;
                

                if (buffDebuff.Turns == 0)
                {
                    if (!removedConditionsAndBuffs.Contains(buffDebuff.Name))
                        removedConditionsAndBuffs.Add(buffDebuff.Name);

                    buffsDebuffsToBeRemoved.Add(buffDebuff);
                }
            }
            BuffDebuffs.RemoveAll(buffsDebuffsToBeRemoved);

            string endOfCombatOrTurn = "their turn";
            if (endOfCombat)
                endOfCombatOrTurn = "combat";

            string conditions = "";
            for (int i = 0; i < removedConditionsAndBuffs.Count; i++)
            {
                if (i == 0)
                {
                    if (InCombat)
                    {
                        conditions = $"At end of {endOfCombatOrTurn}, {Name} is no longer " + removedConditionsAndBuffs[i];
                    }
                    else
                    {
                        conditions = $"{Name} is no longer " + removedConditionsAndBuffs[i];
                    }
                }
                //not last item
                else if (i + 1 != removedConditionsAndBuffs.Count)
                {
                    conditions += $", {removedConditionsAndBuffs[i]}";
                }
                //last item and not first
                else
                {
                    conditions += $", and {removedConditionsAndBuffs[i]}";
                }

                if (i + 1 == removedConditionsAndBuffs.Count)
                    conditions += ".";
            }
            if (conditions != "" && broardcast)
            {
                BroadcastToSoulsInRoom(conditions);
            }
        }

        public void BroadcastActiveConditions()
        {
            string conditions = "";
            List<BuffDebuff> buffDebuffs = BuffDebuffs.Where(b => b.Condition != null).ToList();
            for (int i = 0; i < buffDebuffs.Count; i++)
            {
                if (i == 0)
                    conditions += $"{Name} is " + Description(buffDebuffs[i].Condition);
                //not last item
                else if (i + 1 != buffDebuffs.Count)
                {
                    conditions += $", {Description(buffDebuffs[i].Condition)}";
                }
                //last item and not first
                else
                {
                    conditions += $", and {Description(buffDebuffs[i].Condition)}";
                }

                if (i + 1 == buffDebuffs.Count)
                    conditions += ".";
            }
            if (conditions != "")
                BroadcastToSoulsInRoom(conditions);
        }

        public double GetTotalCoinValue()
        {
            Coins? coins = GetCoins();
            if (coins == null)
                return 0.0;
            return coins.Gold + (coins.Silver * 0.1);
        }
        public Tuple<int, int> TransferCoinTo(Character toCharacter, int gp, int silver)
        {
            Coins? coins = GetCoins();
            if (coins == null)
                return Tuple.Create(0, 0);

            //antagelse: der er nok total
            if (coins.Gold >= gp && coins.Silver >= silver) //giver has enough gold and silver
            {
                TransferExactCoinTo(toCharacter, gp, silver);
                return Tuple.Create(gp, silver);
            }
            else if(coins.Gold < gp) //giver has too little gold so gives the rest in silver
            {
                int gpTransfered = coins.Gold;
                int silverTransfered = silver + ((gp - gpTransfered) * 10);

                TransferExactCoinTo(toCharacter, gpTransfered, silverTransfered);
                return Tuple.Create(gpTransfered, silverTransfered);
            }
            else if (coins.Silver < silver) //giver has too little silver so gives it in gold (round up)
            {
                int silverTransfered = coins.Silver;
                double result = gp + ((silver - silverTransfered) * 0.1);                
                int gpTransfered = (int)Math.Ceiling(result);

                TransferExactCoinTo(toCharacter, gpTransfered, silverTransfered);
                return Tuple.Create(gpTransfered, silverTransfered);
            }

            return Tuple.Create(0, 0);

        }

        private void TransferExactCoinTo(Character toCharacter, int gp, int silver)
        {
            if (gp == 0 && silver == 0)
                return;

            toCharacter.AddCoins(new Coins(gp, silver));
            RemoveCoins(new Coins(gp, silver));
        }

        private Coins? GetCoins()
        {
            foreach(Item item in Inventory.Items)
            {
                if (item is Coins)
                    return (Coins)item;
            }

            return null;
        }

        private void AddCoins(Coins coins)
        {
            Coins? charCoins = GetCoins();
            if (charCoins == null)
                AddToInventory(coins, true);
            else
            {
                charCoins.SetValues(
                    charCoins.Gold + coins.Gold,
                    charCoins.Silver + coins.Silver);
            }
        }

        private void RemoveCoins(Coins coins)
        {
            Coins? charCoins = GetCoins();
            if (charCoins == null)
                throw new Exception("no coin to remove");
            else
            {
                charCoins.SetValues(
                    charCoins.Gold - coins.Gold,
                    charCoins.Silver - coins.Silver);
                if (charCoins.Gold == 0 && charCoins.Silver == 0)
                {
                    Inventory.Items.Remove(charCoins);
                }


            }
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
            Types.Add(creatureType);
        }

        public bool IsOfCreatureType(CreatureType creatureType)
        {
            return Types.Contains(creatureType);
        }

        public void ModifyRelationshipTo(Character? character, int modifier)
        {
            if (character == null)
                return;

            if (!(IsInfluencer && character.IsInfluencer))
                return;

            Relationship rel = GetRelationShipTo(character);
            rel.Value += modifier;

            /*string message = "";

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
                CurrentRoom.BroadcastToSoulsInRoom(message);*/

        }

        public void Speak(string message)
        {
            CurrentRoom.BroadcastToSoulsInRoom($"{Name} says:\"{message}\"");
        }

        public void Yell(string message)
        {
            var rooms = CurrentRoom.Exits.Select(e => e.GoToRoom).Distinct();

            foreach (Room room in rooms)
            {
                room.BroadcastToSoulsInRoom($"From {CurrentRoom.Name}, someone yells: \"{message}\"");
            }
            CurrentRoom.BroadcastToSoulsInRoom($"{Name} yells: \"{message}\"");
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

        public void SendCurrentHpSate()
        {
            if (Soul.Socket == null)
                return;

            _ = Soul.SendAsync($"$[hp]{CurrentHP}/{HP}$[hpend]");
        }

        public void TakeDamage(Damage damage, Character sourceChar, string sourceName, string preDmgMessage, bool fromAttack)
        {
            CurrentHP -= damage.DmgRoll.GetSum();

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
            SendCurrentHpSate();
            TestDeath();
        }
        public void TakeDamage(Damage damage, string sourceName)
        {
            CurrentHP -= damage.DmgRoll.GetSum();

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
            SendCurrentHpSate();
            TestDeath();           
        }

        public void GainLife(int addHp)
        {
            if (CurrentHP + addHp > HP)
                CurrentHP = HP;
            else
                CurrentHP += addHp;
            SendCurrentHpSate();
        }

        public void SetHealth(int hp)
        {
            if (hp > HP)
                CurrentHP = HP;
            else
                CurrentHP = hp;

            SendCurrentHpSate();
            TestDeath();
        }

        public void TestDeath()
        {
            if (CurrentHP > 0)
                return;

            Dies($"{Name} falls to the ground - dead.\n\n" + DeathDescription);
        }

        public void Dies(string deathMessage)
        {
            if (!Dead)
            {
                if (CurrentHP > 0)
                    CurrentHP = 0;

                Dead = true;
                TimeOfDeath = DateTime.UtcNow;
                BuffDebuffs.Clear();
                if (deathMessage != "")
                    BroadcastToSoulsInRoom(deathMessage);           
            }

            CurrentRoom.FlagCombatMightBeResolved();
        }

        public void LivesAgain()
        {
            Dead = false;
            int tenPercent = (int)Math.Ceiling(HP * 0.10);
            CurrentHP = Math.Max(tenPercent, CurrentHP);
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

            //Soul.CancelAndResetTokenSource();
            Soul.StopReceiveFromLoop();
        }

        public int GetAC()
        {
            int ac = 10;
            EquippedItems.TryGetValue(InventorySlot.Body, out Item? armor);
            if (armor is Armor)
                ac = ((Armor)armor).AC;
            else  
                ac += GetModifer(Ability.Dexterity);

            EquippedItems.TryGetValue(InventorySlot.OffHand, out Item? shield);
            if (shield is Shield)
                ac += 2;

            return ac;
        }

        public string? GetRangedAttackDescription(Weapon weapon, Prop prop)
        {
            return weapon.GetAttackDescription(Name, prop);
        }

        public string? GetMainHandAttackDescription(Prop prop)
        {
            Weapon weapon = GetMainHand();
            string? s = weapon.GetType().FullName;
            return weapon.GetAttackDescription(Name, prop);
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
           return new Roll(GetModifer(Skill.CloseCombat) + meleeWeapon.Modifier, RollType.Attack, this);
        }

        public Roll GetRangedAttackRoll(Weapon rangedWeapon)
        {
            int modifier = GetModifer(Skill.RangedCombat) + rangedWeapon.Modifier;
            if (lookAt is Character && IsInGroupWith((Character)lookAt) == true)
                modifier -= 5;
            return new Roll(modifier, RollType.Attack, this);
        }

        public Damage GetRangedDamageRoll(Weapon rangedWeapon)
        {
            return new Damage(
                new Roll(rangedWeapon.DamageDie, GetModifer(Ability.Dexterity) + rangedWeapon.Modifier, RollType.Damage, this),
                rangedWeapon.DamageType);
        }

        public Damage GetMainMeleeDamageRoll(Weapon mainHandWeapon)
        {
            return new Damage(
                new Roll(mainHandWeapon.DamageDie, GetModifer(Ability.Strength) + mainHandWeapon.Modifier, RollType.Damage, this),
                mainHandWeapon.DamageType);
        }

        public Damage GetOffHandDamageRoll(Weapon offHandWeapon)
        {
            return new Damage(
                new Roll(offHandWeapon.DamageDie, offHandWeapon.Modifier, RollType.Damage, this),
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

        public void AttackWithRangedWeapon(Character characterToAttack, Weapon? weapon)
        {
            if (weapon == null)  
                weapon = GetRangedWeapon();
            if (weapon == null)
                return;

            string? attack = GetRangedAttackDescription(weapon, characterToAttack);
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

        public void AttackWithMainHand(Character characterToAttack, Weapon? weaponOverride)
        {
            //weapon override not implemented.
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

        public void AttackWithOffhand(Character characterToAttack, Weapon? weaponOverride)
        {
            //weapon override not implemented.
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

        public void AttackWithTeeth(Character characterToAttack, Weapon? weaponOverride)
        {
            //weapon override not implemented.
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
                
                if (characterToAttack.Dead)
                    return;

                //poison damage    
                Roll savingThrow = new Roll(characterToAttack.GetModifer(Ability.Constitution), RollType.SavingThrow, characterToAttack);
                if (!savingThrow.BeatsDC(13))
                {
                    Roll dmgRoll = new Roll(new Die(1,4), 0, RollType.Damage, this);
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
            return AddToInventory(itemToAdd, false);
        }

        private string AddToInventory(Item itemToAdd, bool newCoins)
        {
            itemToAdd.RemoveFromCurrentGrouping();
            if (itemToAdd is Coins && !newCoins)
            {
                itemToAdd.ClearHeldBy();
                AddCoins((Coins)itemToAdd);              
            }
            else
                Inventory.AddItem(itemToAdd);
            LookBackFromItem(itemToAdd);
            string toolTip = "";
            if (Soul.InventoryToolTip < 4)
            {
                toolTip = " (Type 'i' and press Enter to open inventory)";
                Soul.InventoryToolTip++;
            }
            return $"{Name} added {itemToAdd.Name} to their inventory.{toolTip}";
        }

        public void AddEquippedItem(InventorySlot inventorySlot, Item item)
        {
            item.LastHeldBy = item.HeldBy;
            item.ClearHeldBy();
            if (!EquippedItems.TryAdd(inventorySlot, item))
                throw new Exception($"{Description(inventorySlot)} has already an equipped item.");
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

        public void DestroyHeldItem(Item item)
        {
            if (item.HeldBy is Inventory)
                Inventory.Items.Remove(item);
            else
                foreach (KeyValuePair<InventorySlot, Item> kvp in EquippedItems)
                {
                    if (kvp.Value.Name == item.Name)
                        EquippedItems.Remove(kvp.Key, out Item? removedItem);
                }
            ItemPopulation.Destroy(item);
        }

        public void GoToRoom(Room room, bool broadcast)
        {
            Console.WriteLine($"{Name} changes room: {CurrentRoom.Name} -> {room.Name}");
            InCombat = room.InCombat;

            LastRoom = CurrentRoom;
            RemoveFromCurrentRoom();
            CurrentRoom = room;
            CurrentRoom.Characters.Add(this);

            Exit? exitInNewRoom = CurrentRoom.Exits.Where(exit => exit.GoToRoom == LastRoom).FirstOrDefault();
            if (exitInNewRoom != null)
                MoveToGroup(exitInNewRoom);

            ResetLookAt();

            if (broadcast)
            {
                LastRoom.BroadcastToSoulsInRoom($"{Name} left, heading in the direction of {room.Name}.", this);
                if (LastRoom.RoomKey == Description(RoomKey.Void))
                    room.BroadcastToSoulsInRoom($"{Name} enters The {room.Name}.", this);
                else
                    room.BroadcastToSoulsInRoom($"{Name} enters The {room.Name} from The {LastRoom.Name}.", this);
            }

            if (LastRoom != null && LastRoom.InCombat)
                LastRoom.FlagCombatMightBeResolved();

            if (broadcast)
                _ = Soul.SendAsync(room.GetDescription(this, true));

            if (room.OnEnterEvent != null)
                room.OnEnterEvent(Soul);

            if (broadcast)
                _ = Soul.SendAsync(room.GetAdditionalRoomDescription(this));

            if (room.InCombat)
                room.EnableOrUpdateCombat(this, null);
        }

        public void GoToRoom(Room room)
        {
            GoToRoom(room, true);
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
            if (LookedAt.Count == 0 || LookedAt.GetAt(LookedAt.Count - 1) != prop)
                LookedAt.Add(prop);
        }

        [JsonIgnore]
        public Prop? LookAt
        {
            get { return lookAt; }
        }
        public void ResetLookAt()
        {
            LookedAt = new ThreadSafeList<Prop>();
            SetLookAt(CurrentRoom);         
        }

        public void ConsumeCorpse()
        {
            EmptyInventoryOnGround();
            
            if (UniqueName)
            {
                if (NPC || Soul.Socket != null)
                {
                    Item soulstone = ConsumableList.Soulstone(this);
                    CurrentRoom.AddItem(soulstone);
                    soulstone.MoveToGroup(this);
                }
            }

            RemoveFromCurrentRoom();
        }

        public void EmptyInventoryOnGround()
        {
            foreach(Item item in Inventory.Items)
            {
                DropItem.RemoveItemFromCharacter(this, item);
                CurrentRoom.AddItem(item);
                item.MoveToGroup(this);
            }
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
                "Name: " + Name + "\n\n" +

                "Kindred: " + Kindred + "\n" +
                "Gender: " + Description(Gender) + "\n\n" +

                "Health Points: " + HP + "\n\n" +

                $"Strength: {Strength} ({GetModifer(Ability.Strength).ToString("+0;-#;+0")})" + "\n" +
                $"Dexterity: {Dexterity} ({GetModifer(Ability.Dexterity).ToString("+0;-#;+0")})" + "\n" +
                $"Constitution: {Constition} ({GetModifer(Ability.Constitution).ToString("+0;-#;+0")})" + "\n" +
                $"Intelligence: {Intelligence} ({GetModifer(Ability.Intelligence).ToString("+0;-#;+0")})" + "\n" +
                $"Wisdom: {Wisdom} ({GetModifer(Ability.Wisdom).ToString("+0;-#;+0")})" + "\n" +
                $"Charisma: {Charisma} ({GetModifer(Ability.Charisma).ToString("+0;-#;+0")})";
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

        public bool IsHostileTowards(Character character)
        {
            Relationship relationship = GetRelationShipTo(character);

            return relationship.IsHostile() || character.Flags.Any(f => f.Type == FlagKey.Stole && Faction.KeyIs(f.FactionKey));
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
            foreach(Effect effect in GetAllEffectsIncludingFeatsAndBuffs())
            {
                if (effect.LightPointerModifer >= Light.Dim)
                    return true;
            }
            return false;
        }

        public List<Effect> GetAllEffectsIncludingFeatsAndBuffs()
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
            foreach(BuffDebuff buffDebuff in BuffDebuffs)
            {
                if (buffDebuff.Effect != null)
                allEffects.Add(buffDebuff.Effect);
            }
            return allEffects;
        }

        public void AddFeat(FeatKey key)
        {
            Feats.Add(Description(key));
        }

        public bool HasFeat(FeatKey key)
        {
            return Feats.Contains(Description(key));
        }

        public void RespawnItems()
        {
            if (ItemRespawns == null)
                return;

            foreach (ItemRespawn itemSpawn in ItemRespawns)
            {
                if (DateTime.UtcNow < itemSpawn.NextRespawn)
                    continue;

                if (Inventory.Items.Count(i => i.Name.Contains(Description(itemSpawn.ItemFactoryKey))) >= itemSpawn.MaxItems)
                {
                    itemSpawn.SetNextRespawnTime();
                    continue;
                }

                Item item = ItemFactory.Registry[itemSpawn.ItemFactoryKey]();

                AddToInventory(item);

                itemSpawn.SetNextRespawnTime();
            }
        }

        #region AI

        public void ExecuteAIGoalAndTryEnambleCombat()
        {
            ExecuteAiGoal();
            TryEnableCombat();
        }

        public void ExecuteAiGoal()
        {
            if (Dead)
                return;

            if (!Goals.Any()) 
                return;

            Goal currentGoal = Goals.Peek();
            
            Dictionary<Goal, GoalAction>? goalActions;
            Behavior.goalActionsByBehaviorKey.TryGetValue(BehaviorKey, out goalActions);
            if (goalActions == null)
                return;

            GoalAction? goalAction;
            goalActions.TryGetValue(currentGoal, out goalAction);
            if (goalAction == null)
                return;

            BehaviorResult result = goalAction.Action(this);
            if (result == BehaviorResult.Completed)
            {
                Goals.Pop();
                if (goalAction.OnCompleted.HasValue)
                {
                    Goals.Push(goalAction.OnCompleted.Value);
                }
            }
            else if (result == BehaviorResult.CantComplete)
            {
                if (goalAction.PopOnCantComplete)
                    Goals.Pop();
                if (goalAction.OnCantComplete.HasValue)
                {
                    Goals.Push(goalAction.OnCantComplete.Value);
                }
            }
        }

        public BehaviorResult AI_MoveToDarkSpot()
        {
            if (GetLightState(null) == Light.Darkness)
                return BehaviorResult.Completed;

            List<Item> darkSpots = CurrentRoom.Items.Where(i => i.GetLightState(null) == Light.Darkness && i.DynamicDescription).ToList();
            if (darkSpots.Any())
            {
                _ = new MoveTo(Soul, darkSpots[GetRandomInt(darkSpots.Count)]).Execute(Soul);
                return BehaviorResult.Completed;
            }

            return BehaviorResult.CantComplete;
        }

        public BehaviorResult AI_ExitRoom()
        {
            List<Exit> relevantExits = CurrentRoom.Exits.Where(e => e.State.IsOpen).ToList();

            if (!relevantExits.Any())
                return BehaviorResult.CantComplete;

            // Prefer open exits that don't lead back to LastRoom
            List<Exit> forwardExits = relevantExits.Where(e => e.GoToRoom != LastRoom).ToList();

            // If there are forward exits, pick one; otherwise, fall back to any open exit
            List<Exit> candidateExits = forwardExits.Any() ? forwardExits : relevantExits;

            Exit ChosenExit = candidateExits[GetRandomInt(candidateExits.Count)];

            if (IsInGroupWith(ChosenExit) == false)
            {
                _ = new MoveTo(Soul, ChosenExit).Execute(Soul);
                return BehaviorResult.Inprogress;
            }

            _ = new RoomChange(Soul, ChosenExit).Execute(Soul);
            return BehaviorResult.Completed;
        }
        public BehaviorResult AI_Prey()
        {
            //add food


            List<Character> deadBodies = CurrentRoom.Characters.Where(c => c.Dead && !c.IsHidden()).ToList();
            if (deadBodies.Any())
            {
                List<Character> closeDeadBodies = deadBodies.Where(c => c.IsInGroupWith(this) == true).ToList();
                if (closeDeadBodies.Any())
                {
                    int randomIndex = new Random().Next(closeDeadBodies.Count);
                    Character closeDeadBody = closeDeadBodies[randomIndex];

                    _ = new EatCorpse(Soul, closeDeadBody).Execute(Soul);
                    return BehaviorResult.Completed;
                }
                else
                {
                    int randomIndex = new Random().Next(deadBodies.Count);
                    Character deadBody = deadBodies[randomIndex];

                    _ = new MoveTo(Soul, deadBody).Execute(Soul);
                    return BehaviorResult.Inprogress;
                }
            }

            List<Character> charactersThatDoNotShareType = CurrentRoom.Characters
                .Where(c => c != this && !c.Dead && !c.IsHidden() && !c.Types.Any(t => this.Types.Contains(t)))
                .ToList();



            if (!charactersThatDoNotShareType.Any())
                return BehaviorResult.CantComplete;

            Character? preyCloseBy = charactersThatDoNotShareType.Where(c => c.IsInGroupWith(this) == true).FirstOrDefault();
            if (preyCloseBy == null)
            {
                preyCloseBy = charactersThatDoNotShareType[GetRandomInt(charactersThatDoNotShareType.Count)];
                _ = new MoveTo(Soul, preyCloseBy).Execute(Soul);
                return BehaviorResult.Inprogress;
            }
            _ = new MeleeAttack(Soul, preyCloseBy).Execute(Soul);
            return BehaviorResult.Inprogress;

        }

        #endregion

    }
}
