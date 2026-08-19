using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.World;

namespace fire_ash_server.Moves
{
    internal class SpeakTo : Move
    {
        public SpeakTo(Soul soul, Character targetCharacter) : base(MoveKey.sp.ToString(), $"Speak to {targetCharacter.Name}", targetCharacter)
        {
            AllowedInCombat = false;

            Action = async () =>
            {
                if (Dialogues.Registry.TryGetValue((DialogueKey)targetCharacter.GetDialogueKey(),out Func<DialogueNode>? factory))
                {
                    targetCharacter.DialogueManager.StartingNode = factory();
                }

                if (targetCharacter.OnBeforeSpeakTo != null)
                    targetCharacter.OnBeforeSpeakTo(soul, targetCharacter);

                if (targetCharacter.DialogueManager == null)
                    return;

                targetCharacter.DialogueManager.InitSpeakWith(soul.Character);
            };
        }
    }
}
