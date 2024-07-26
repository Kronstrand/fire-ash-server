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
        public LookAt(Soul soul, Prop prop) : base("l", CreateDescription(prop), prop)
        {
            InitValues();

            Action = async () =>
                {
                    soul.Character.SetLookAt(prop);
                    await LookAtAction(soul, prop);
                };
        }

        public LookAt(Soul soul) : base("l", $"Look at current prop.")
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

        private static string CreateDescription(Prop prop)
        {
            string description = $"Look at {prop.Name}.";

            if (prop is Character)
            {
                Character character = (Character)prop;
                if (character.Dead)
                    description += " (Dead)";
            }
            return description;
        }

        private void InitValues()
        {
            Type = MoveType.MinorAction;
            Range = RangeType.None;
        }

        public static async Task LookAtAction(Soul soul, Prop? prop)
        {
            if (prop == null)
            {
                await soul.SendAsync($"{soul.Character.Name} are staring absentmindedly into the air...");
                return;
            }

            if (prop is Room)
            {
                Room room = (Room)prop;
                await soul.SendAsync(room.GetFullRoomDescription(soul.Character));
                return;
            }
            if (prop is Character)
            {
                Character character = (Character)prop;
                await soul.SendAsync(prop.GetDescription() + "\n\n" + $"The relationship to {character.Name} is {Description(soul.Character.GetRelationshipStatus(character))}.");
                return;
            }
            await soul.SendAsync(prop.GetDescription());
        }
    }
}
