using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Dialogue
{
    [Serializable]
    internal class DialogueChoice
    {
        public string Text { get; set; }
        private Func<DialogueManager,bool> PreReq { get; set; }
        private Func<DialogueManager,DialogueNode> Result { get; set; }
        public DialogueChoice(string text, Func<DialogueManager,DialogueNode> result)
        {
            Text = text;
            Result = result;
            PreReq = (DialogueManager dm) => { return true; };
        }

        public DialogueChoice(string text, DialogueNode node)
        {
            Text = text;
            Result = (DialogueManager dm) => { return node; };
            PreReq = (DialogueManager dm) => { return true; };
        }

        public DialogueNode GetNextDialogueNode(DialogueManager dialogueManager)
        {            
            return Result.Invoke(dialogueManager);
        }

        public bool IsValid(DialogueManager dialogueManager)
        {
            return PreReq.Invoke(dialogueManager);
        }
    }
}
