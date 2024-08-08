using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Moves
{
    internal class SpeakTo : Move
    {
        public SpeakTo(Soul soul, Character targetProp) : base(MoveKey.sp.ToString(), $"Speak to {targetProp.Name}", targetProp)
        {
            Action = () =>
            {
                if (targetProp.DialogueManager == null)
                    return;
                targetProp.DialogueManager.InitSpeakWith(soul.Character);
            };
        }
    }
}
