using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Moves
{
    internal class MoveTo : Move
    {
        public MoveTo(Soul soul, Prop targetProp) : base(MoveKey.m.ToString(), CreateMoveName(soul.Character, targetProp), targetProp)
        {
            Range = RangeType.RangeSingleTarget;

            Action = async () =>
            {
                bool interruptMove = false;
                Grouping? grouping = soul.Character.GetGrouping();
                if (grouping != null)
                    interruptMove = grouping.RunAllOnBeforeMoveFromEventsInGroup(soul);

                if (interruptMove)
                    return;

                if (targetProp.GetLightState(null) != Light.Darkness)
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} moves to {CreatePropName(targetProp)}.");
                else
                    soul.Character.BroadcastToSoulsInRoom($"{soul.Character.Name} moves into the darkness, {targetProp.ContextDescription}");

                soul.Character.MoveToGroup(targetProp);
                if (soul.Character.LookAt != targetProp)
                {
                    soul.Character.SetLookAt(targetProp);
                    await LookAt.LookAtAction(soul, targetProp);
                }

                TriggerHostileCombat(soul, targetProp);

                targetProp.RunOnAfterMoveToEvents(soul);
            };
        }

        private void TriggerHostileCombat(Soul soul, Prop targetProp)
        {
            Grouping? grouping = targetProp.GetGrouping();
            if (grouping != null)
            {
                foreach (Prop prop in grouping.Props)
                {
                    if (!(prop is Character))
                        continue;

                    Character character = (Character)prop;
                    if (character == soul.Character)
                        continue;

                    if (character.GetRelationShipTo(soul.Character).IsHostile())
                    {
                        EnablesCombat = true;
                        soul.Character.EnableCombatWith = new ToxicRelationship(character, false);
                    }
                }
            }
            else if (targetProp is Character)
            {
                Character character = (Character)targetProp;
                if (character.GetRelationShipTo(soul.Character).IsHostile())
                {
                    EnablesCombat = true;
                    soul.Character.EnableCombatWith = new ToxicRelationship(character, false);
                }
            }
        }

        public static string CreateMoveName(Character character, Prop prop)
        {
            if (prop.DynamicDescription && prop.GetLightState(character) == Light.Darkness)
            {
                if (prop.ContextDescription == null) throw new Exception($"{prop.Name} has no context description");

                return $"Move into the darkness, {ToLowerFirstChar(prop.ContextDescription)}.";
            }
            else
                return $"Move to {CreatePropName(prop)}.";
        }

        public static string CreatePropName(Prop prop)
        {
            if (prop is Exit)
                return ((Exit)prop).GoToRoom.Name + " Entrance";

            return prop.Name;
        }


    }
}
