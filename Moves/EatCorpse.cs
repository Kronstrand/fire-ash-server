using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class EatCorpse : Move
    {
        public EatCorpse(Soul soul, Character targetCharacter) : base(MoveKey.bc.ToString(), $"Eat {targetCharacter.Name}", targetCharacter)
        {
            Type = MoveType.Action;

            Action = () =>
            {
                soul.Character.CurrentRoom.BroadcastToSoulsInRoom($"{soul.Character.Name} devours the remains of {targetCharacter.Name}.");
                soul.Character.LastAte = DateTime.UtcNow;            
                targetCharacter.ConsumeCorpse();
                if (soul.Character.LookAt == targetCharacter)
                    soul.Character.LookBack();
                return Task.CompletedTask;
            };
        }

        public override bool IsValid(Soul soul)
        {
            return (soul.Character.LookAt is Character && ((Character)soul.Character.LookAt).Dead); //not tested/used
        }
    }
}