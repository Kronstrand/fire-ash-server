using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.World;

namespace fire_ash_server.Dialogue
{
    internal class DialogueManager
    {
        public DialogueNode CurrentNode;
        public DialogueNode StartingNode { get; set; }
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
            CurrentNode.RunOnBeforeEvent(this);
            if (CurrentNode.Dialogue)
            {
                if (CurrentNode.Say)
                    SpeakingCharacter.Speak(CurrentNode.GetText(this));
                else
                    SpeakingCharacter.BroadcastToSoulsInRoom(CurrentNode.GetText(this));
            }
            CurrentNode.RunOnAfterEvent(this);

            if (!CurrentNodeHasChoices())
                EndSpeakWith();
        }

        public void EndSpeakWith()
        {
            Character? xSpeakingTo = null;

            Initiater = null;
            if (SpeakingCharacter.SpeakingTo != null)
            {
                xSpeakingTo = SpeakingCharacter.SpeakingTo;
                SpeakingCharacter.SpeakingTo.SpeakingTo = null;
            }            
            SpeakingCharacter.SpeakingTo = null;

            if (SpeakingCharacter.OnAfterSpeakTo != null && xSpeakingTo != null)
                SpeakingCharacter.OnAfterSpeakTo(xSpeakingTo.Soul, SpeakingCharacter);
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
