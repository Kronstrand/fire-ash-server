using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Dialogue
{
    internal class DialogueNode
    {
        private Func<DialogueManager, string> Text { get; set; }
        public List<DialogueChoice> Choices { get; set; } = new List<DialogueChoice>();

        public DialogueNode(string text)
        {
            Text = (DialogueManager dm) => { return text; };
        }
        public DialogueNode(Func<DialogueManager, string> text)
        {
            Text = text;
        }

        public void AddChoice(string choiceText, DialogueNode nextNode)
        {
            Choices.Add(
                new DialogueChoice(
                    choiceText, 
                    (DialogueManager dm) => { return nextNode; })
                );
        }
        public void AddChoice(string choiceText, Func<DialogueManager,DialogueNode> result)
        {
            Choices.Add(new DialogueChoice(choiceText, result));
        }

        public string GetText(DialogueManager dialogueManager)

        {
            return Text.Invoke(dialogueManager);
        }
    }
}
