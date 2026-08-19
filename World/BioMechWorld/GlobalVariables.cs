using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.World.BioMechWorld
{
    //Should be handled as singleton
    internal class GlobalVariables
    {
        public GlobalVariables()
        {
         
        }

        public Character vexisTheCaretaker = new Character(
                "Vexis",
                "Vexis, a friendly yet eccentric figure. " +
                "Standing at five feet, his body is a fusion of rusted metal, gears, and organic tissue. " +
                "His limbs are equipped with various tools and gadgets, constantly buzzing with energy. " +
                "Vexis' face is an odd blend of amphibian and machine, featuring large, glowing green eyes and peculiar, protruding ears. " +
                "Despite his unsettling appearance, he exudes a warm, welcoming aura. " +
                "His voice, a distorted whisper with a hint of static, often carries a tone of curiosity and friendliness.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                14, //strength
                9, //dexterity
                13, //constitution
                14, //intelligence
                13, //wisdom
                8,  //charisma
                "Vexis lies motionless, his multi-functional limbs now still. " +
                "His glowing green eyes are dim, and his peculiar, protruding ears droop lifelessly. " +
                "The once-buzzing tools and gadgets are silent, leaving behind a sense of eerie calm. " +
                "Even in death, there's a lingering warmth to his form, a testament to his friendly nature."
            );

        public DialogueNode vexisTemplePermissionNode = new DialogueNode(
                "Those eyes of yours have a purpose now, eh? " +
                "I'm guessing Ezekiel or Elara finally sent you my way. " +
                "I'll admit, I'm not keen on letting folks wander past this door, " +
                "but if they've given the all-clear, then who am I to stand in your path?"
            );
    }
}
