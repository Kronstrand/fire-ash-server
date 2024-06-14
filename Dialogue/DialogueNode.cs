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
        public  Action<DialogueManager>? OnAfterEvent { private get; set; }
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
            AddChoice(choiceText, nextNode, false);
        }
        public void AddChoice(string choiceText, DialogueNode nextNode, bool asLastChoice)
        {
            if (Choices.Where(choice => choice.Text == choiceText).Any())
                return;

            DialogueChoice choice = new DialogueChoice(
                    choiceText,
                    (DialogueManager dm) => { return nextNode; });

            if (asLastChoice)
                Choices.Insert(0, choice); //the choices are loop through backwards
            else
                Choices.Add(choice);
            
        }
        public void AddChoice(string choiceText, Func<DialogueManager,DialogueNode> result)
        {
            Choices.Add(new DialogueChoice(choiceText, result));
        }

        public string GetText(DialogueManager dialogueManager)

        {
            return Text.Invoke(dialogueManager);
        }

        public void RunOnAfterEvent(DialogueManager dialogueManager)
        { 
            if (OnAfterEvent != null)
                OnAfterEvent.Invoke(dialogueManager);
        }
    }
}
