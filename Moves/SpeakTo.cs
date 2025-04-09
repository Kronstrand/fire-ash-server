using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    [Serializable]
    internal class SpeakTo : Move
    {
        public SpeakTo(Soul soul, Character targetCharacter) : base(MoveKey.sp.ToString(), $"Speak to {targetCharacter.Name}", targetCharacter)
        {
            Action = async () =>
            {
                if (targetCharacter.OnBeforeSpeakTo != null)
                    targetCharacter.OnBeforeSpeakTo(soul, targetCharacter);

                if (targetCharacter.DialogueManager == null)
                    return;

                targetCharacter.DialogueManager.InitSpeakWith(soul.Character);
            };
        }
    }
}
