using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Moves.Attacks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.World.Goldfield;
using static fire_ash_server.Helpers;

namespace fire_ash_server
{
    internal static class Events
    {

        public static Dictionary<string, Func<Soul, Prop, Task<bool>>> events = new Dictionary<string, Func<Soul, Prop, Task<bool>>>();

        public static void InitEvents()
        {
            events.Add(Description(EventKey.TriggerBearTrap), TriggerBearTrap());
            events.Add(Description(EventKey.PickUpBookOfLorath), TriggerPickedUpFromAlterOfLorath());
            events.Add(Description(EventKey.TriggerStealFlag), TriggerStealFlag());
        }

        public static async Task RunEvents(Soul soul, Prop prop, ThreadSafeList<EventKey> events, ThreadSafeList<EventKey> eventsToBeRemoved)
        {
            
            List<EventKey> successfulEvents = new();

            foreach (EventKey eventKey in events)
            {
                string key = Description(eventKey);

                if (!Events.events.TryGetValue(key, out var evt) || evt == null)
                    continue;

                if (await evt(soul, prop))
                    successfulEvents.Add(eventKey);
            }

            foreach (EventKey evnt in successfulEvents)
            {
                if (eventsToBeRemoved.Contains(evnt))
                {
                    events.Remove(evnt);
                    eventsToBeRemoved.Remove(evnt);
                }
            }
        }

        public static Func<Soul, Prop, Task<bool>> TriggerStealFlag()
        {
            return (soul, prop) =>
            {
                if (prop.BelongsToFaction == null)
                    return Task.FromResult(true);

                FactionKey propFactionKey = (FactionKey)prop.BelongsToFaction;

                if (!prop.Flags.Any(f => f.Type == FlagKey.Stolen))
                {
                    prop.AddFlag(new Flag(FlagKey.Stolen, propFactionKey, soul.Character.CurrentRoom.RoomKey));

                    bool wasSeenByFaction = !soul.Character.IsHidden() && soul.Character.CurrentRoom.Characters.Any(c => !c.Dead && c.Faction == Faction.Get(propFactionKey));
                    if (wasSeenByFaction)
                        soul.Character.AddFlag(new Flag(FlagKey.Stole, propFactionKey, soul.Character.CurrentRoom.RoomKey));
                }

                return Task.FromResult(true);
            };
        }

        public static Func<Soul, Prop, Task<bool>> TriggerNegativeRep()
        {
            return (soul, prop) =>
            {
                foreach (Character character in soul.Character.CurrentRoom.Characters)
                {

                    if (prop.BelongsToFaction != null)
                    {
                        FactionKey factionKey = (FactionKey)prop.BelongsToFaction;
                        if (character.Faction == Faction.Get(factionKey))
                        {
                            Relationship.Set(FactionKey.Players, factionKey, -1);
                        }
                    }
                }

                return Task.FromResult(true);
            };
        }

        public static Func<Soul, Prop, Task<bool>> TriggerPickedUpFromAlterOfLorath()
        {
            return (soul, prop) =>
            {
                if (soul.Character.IsHidden())
                    return Task.FromResult(true);

                Item item = (Item)prop;
                if (item.LastHeldBy == null || item.LastHeldBy.Id != "templeOfLorath.altar")
                    return Task.FromResult(true);

                Character? servantOfLorath = soul.Character.CurrentRoom.Characters.FirstOrDefault(c => c.Title == "Servant of Lorath");
                if (servantOfLorath != null)
                {
                    servantOfLorath.Yell("Thief! Thief! You will be punished in Lorath's name!");
                }

                return Task.FromResult(true);
            };
        }

        public static Func<Soul, Prop, Task<bool>> TriggerBearTrap ()
        {
            return (soul, prop) =>
            {
                Item item = (Item)prop;
                if (item.HeldBy == null)
                    return Task.FromResult(false);
                if (soul.Character.IsHidden())
                    return Task.FromResult(false);
                if (item.SetByCharacterId == soul.Character.Id)
                    return Task.FromResult(false);

                Roll savingThrow = new Roll(soul.Character.GetModifer(Ability.Dexterity), RollType.SavingThrow, soul.Character);
                if (savingThrow.BeatsDC(14))
                {
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} steps into {item.Name}, succeeds on their Dexterity saving throw by rolling {savingThrow}, and quickly sidesteps out of its way before it snaps shut.");
                }
                else
                {
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} steps into {item.Name} fails on their Dexterity saving throw by rolling {savingThrow}.");
                    soul.Character.TakeDamage(new Damage(
                    new Roll(
                        new Die(1, 4),
                        0,
                        RollType.Damage,
                        null),
                    DamageType.Piercing), item.Name);
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} is stunned 1 turn and rooted 2 turns.");
                    soul.Character.AddBuffDebuff(new BuffDebuff(Description(Condition.Stunned), Condition.Stunned, 1));
                    soul.Character.AddBuffDebuff(new BuffDebuff(Description(Condition.Rooted), Condition.Rooted, 2));

                    if (item.SetByCharacterId != null)
                    {
                        Room? currentRoom = prop.GetRoomLocation();
                        if (currentRoom != null)
                        {
                            Character? characterThatSetTrap = currentRoom.Characters.Where(c => c.Id == item.SetByCharacterId).FirstOrDefault();
                            if (characterThatSetTrap != null)
                                soul.Character.SetEnableCombatWith(characterThatSetTrap);
                        }
                        
                    }
                }

                item.HeldBy.Items.Remove(item);

                return Task.FromResult(true);
            };
        }

        /*
        public static void AddCharacterMoveFromCharacterAndIsAttacked(Character attackingCharacter)
        {
            attackingCharacter.AddOnBeforeMoveFromEvent(async (soul) => {
                if (attackingCharacter.InCombat)
                    return false;
                if (attackingCharacter.Dead)
                    return false;
                if (soul.Character.IsHidden())
                    return false; //idealy this would would not trigger the event so it could be re-run..


                Move move = new MeleeAttack(attackingCharacter.Soul, soul.Character);
                await move.Execute(attackingCharacter.Soul, soul.Character);

                return true; 
            }, 
            true);
        }

        public static void AddCharacterMoveToAndIsrangedAttacked(Character attackingCharacter, Prop moveTo)
        {
            moveTo.AddOnAfterMoveToEvent(async (soul) =>
            {
                if (attackingCharacter.InCombat)
                    return false;
                if (attackingCharacter.Dead)
                    return false;
                if (soul.Character.IsHidden())
                    return false;
                if (!attackingCharacter.EquippedItems.TryGetValue(Enums.InventorySlot.Ranged, out Item? rangedWeaoon))
                    return false;

                Move move = new RangedAttack(attackingCharacter.Soul, soul.Character);
                await move.Execute(attackingCharacter.Soul, soul.Character);

                return true;
            },
            true);
        }
        */
    }
}
