using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Enums;

namespace fire_ash_server.Moves
{
    internal class LookAt : Move
    {
        public LookAt(Soul soul, Prop prop) : base(MoveKey.l.ToString(), CreateDescription(soul.Character, prop), prop)
        {
            InitValues();

            Action = async () =>
                {
                    soul.Character.SetLookAt(prop);
                    await LookAtAction(soul, prop);
                };
        }

        public LookAt(Soul soul) : base(MoveKey.l.ToString(), $"Look at current prop.")
        {
            InitValues();

            Hidden = true;
            Action = async () =>
                {
                    if (!soul.Character.PropTargetIsValid(this, soul.Character.LookAt))
                        return;

                    await LookAtAction(soul, soul.Character.LookAt);
                };
        }

        private static string CreateDescription(Character character, Prop targetProp)
        {
            string description = $"{targetProp.GetLightEffectedName("Look at ", "Look into the ", character)}.";

            if (targetProp is Character)
            {
                Character targetCharacter = (Character)targetProp;
                if (targetCharacter.Dead)
                    description += " (Dead)";
            }
            return description;
        }

        private void InitValues()
        {
            Type = MoveType.MinorAction;
            Range = RangeType.None;
            AllowedInTrade = true;
        }

        public static async Task LookAtAction(Soul soul, Prop? prop)
        {
            if (prop == null)
            {
                await soul.SendAsync($"{soul.Character.Name} are staring absentmindedly into the air...");
                return;
            }
            else if (prop is Room)
            {
                Room room = (Room)prop;
                await soul.SendAsync(room.GetDescription(soul.Character, true));
                await soul.SendAsync(room.GetAdditionalRoomDescription(soul.Character));
                return;
            }
            else if (prop is Character)
            {
                Character character = (Character)prop;
                await soul.SendAsync(prop.GetDescription(soul.Character) + "\n\n" + $"The relationship to {character.Name} is {Description(soul.Character.GetRelationshipStatus(character))}.");
                return;
            }
            await soul.SendAsync(prop.GetDescription(soul.Character));
        }
    }
}
