using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class BurnCorpse : Move
    {
        public BurnCorpse(Soul soul, Character targetCharacter)
            : base(MoveKey.bc.ToString(), $"Burn the corpse of {targetCharacter.Name}", targetCharacter)
        {
            Type = MoveType.Action;

            Action = () =>
            {
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom(
                    $"{soul.Character.Name} lowers their torch to the remains of {targetCharacter.Name}. The corpse catches fire, and the flames slowly consume the flesh."
                );
                targetCharacter.ConsumeCorpse();
                if (soul.Character.LookAt == targetCharacter)
                    soul.Character.LookBack();

                return Task.CompletedTask;
            };
        }

        public override bool IsValid(Soul soul)
        {
            return soul.Character.LookAt is Character character && character.Dead;
        }
    }
}