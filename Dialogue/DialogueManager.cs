using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Moves;

namespace fire_ash_server.Dialogue
{
    internal class DialogueManager
    {
        public DialogueNode CurrentNode;
        public DialogueNode StartingNode { get; private set; }
        public Character SpeakingCharacter;
        public Character? Initiater;
        public List<DialogueChoice> UsedChoices = new List<DialogueChoice>();
        
        public DialogueManager(Character speakingCharacter, DialogueNode startingNode)
        {
            SpeakingCharacter = speakingCharacter;
            CurrentNode = startingNode;
            StartingNode = startingNode;
        }

        public void InitSpeakWith(Character SpeakToCharacter)
        {
            UsedChoices.Clear();
            Initiater = SpeakToCharacter;
            SpeakingCharacter.SpeakingTo = SpeakToCharacter;        
            SpeakToCharacter.SpeakingTo = SpeakingCharacter;
            
            CurrentNode = StartingNode;
            SpeakCurrentNode();
        }

        public void SpeakCurrentNode()
        {
            SpeakingCharacter.Speak(CurrentNode.GetText(this));
            CurrentNode.RunOnAfterEvent(this);
        }

        public void EndSpeakWith()
        {
            Initiater = null;
            if (SpeakingCharacter.SpeakingTo != null)
                SpeakingCharacter.SpeakingTo.SpeakingTo = null;
            SpeakingCharacter.SpeakingTo = null;
        }

        public void SetCurrentNodeBasedOnChoice(DialogueChoice choice)
        {
            UsedChoices.Add(choice);
            CurrentNode = choice.GetNextDialogueNode(this);
        }

        public bool ChoiceIsValid(DialogueChoice choice)
        {
            if (UsedChoices.Contains(choice))
                return false;

            return choice.IsValid(this);
        }
        public DialogueNode GetNextDialogueNode(DialogueChoice choice)
        {
            return choice.GetNextDialogueNode(this);
        }

        public bool CurrentNodeHasChoices()
        {
            return CurrentNode.Choices.Where(choice => ChoiceIsValid(choice)).Count() > 0;
        }
        public void ImproveRelationship()
        {
            if (SpeakingCharacter.SpeakingTo == null)
                return;
            SpeakingCharacter.ModifyRelationshipTo(SpeakingCharacter.SpeakingTo, 1);
        }


        public void  DecreaseRelationship()
        {
            if (SpeakingCharacter.SpeakingTo == null)
                return;
            SpeakingCharacter.ModifyRelationshipTo(SpeakingCharacter.SpeakingTo, -1);
        }
    }
}
