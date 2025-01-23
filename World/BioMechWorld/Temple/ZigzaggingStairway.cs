using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class ZigzaggingStairway
    {
        public static Room Create(Room ancientHall)
        {
            Room zigzaggingStairway = new Room(
                "Zigzagging Stairway",
                "A jagged staircase carves its way through the stone, zigzagging sharply as it climbs. " +
                "Massive stone pillars flank the staircase at irregular intervals, their surfaces etched with ancient runes. " +
                "The pillars rise like sentinels, their towering forms giving the passage an imposing feel. " +
                "The walls close in around the staircase, glowing mineral veins casting faint reflections on the worn steps. "
            );

            Exit ancientHallToZigzaggingStairway = new Exit(
                "To the northeast",
                "A gently ascending staircase bordered by arching stone pillars, " +
                "continuing upward into a cavernous tunnel.",
                zigzaggingStairway
            );
            ancientHall.AddExit(ancientHallToZigzaggingStairway);

            Exit zigzaggingStairwayToAncientHall = new Exit(
                "At the base of the jagged staircase", 
                "A passage opens into a vast hallway divided by towering stone columns.",
                ancientHall
            );
            zigzaggingStairway.AddExit(zigzaggingStairwayToAncientHall);

            Room outerComplex = new Room("Outer Complex", "x");

            Exit ironGate = new Exit(
                "At the staircase's summit",
                "A heavy, gear-driven iron door, its intricate mechanisms exposed and poised for motion.",
                outerComplex
            );
            ironGate.Light = Light.Darkness;
            ironGate.DynamicDescription = true;
            zigzaggingStairway.AddExit(ironGate);

            ironGate.OnBeforeExitEvent = async (Soul soul) =>
            {
                _ = soul.SendAsync("This passage leads to the part of the complex controlled by the Purists. Entering alone would be certain suicide. " + "\n" +
                    "Would you like to return to Ezekiel with the news that you found the way through the temple?" + "\n" + 
                    "y/n?");
                if (await soul.AwaitYesNo())
                    _ = soul.SendAsync(GetBattleSceneEnding());
                
                return true;
            };

            return zigzaggingStairway;
        }

        private static string GetBattleSceneEnding()
        {
            string sceneDescription =
                "As you return to the Main Hall, the air hums with tension. Mecharions move with purpose, " +
                "preparing for war as you step through the entrance. " +
                "Weapons are checked, armor adjusted, and final words exchanged. " +
                "At the center of it all stands Ezekiel, his towering form commanding attention. " +
                "The heavy air grows still as you approach him, the weight of your success resting on your shoulders.\n\n" +

                "Ezekiel says: \"You've returned,\" his mechanical voice low but steady. \"The path through the temple is secure?\"\n\n" +

                "You nod. \"The Purists won't see it coming. The way is clear for your forces to flank them.\"\n\n" +

                "A rare smile crosses Ezekiel's face, a mix of pride and cold satisfaction. \"Excellent. The Mother Machine will endure, thanks to you. " +
                "The Purists will fall, and with them, their delusions of purity.\"\n\n" +

                "He places a hand on your shoulder, the cold metal of his cybernetic arm contrasting with the warmth of his gaze. " +
                "\"You've done more than I could have hoped. Today, you prove yourself not just as one of us, but as a leader among the Mecharions.\"\n\n" +

                "The ambush unfolds with ruthless precision. The Purists, once so confident in their assault, are caught unprepared as Mecharion forces descend upon them from the temple's hidden passage. " +
                "The clash is brutal and efficient, a symphony of metal and fury orchestrated by Ezekiel's relentless command. " +
                "Through the chaos, you see the Purists falter, their lines breaking under the weight of Mecharion ingenuity and strength. " +
                "The final blow comes when Ezekiel himself steps onto the battlefield, his imposing form cutting through their ranks like a specter of vengeance. " +
                "When the last of the Purists flee or fall, a cheer erupts from the Mecharion forces. " +
                "Victory is theirs. The Mother Machine is safe, the sanctum secured.\n\n" +

                "In the aftermath of the battle, Ezekiel addresses the surviving Mecharions in the Main Hall, his voice echoing with a mixture of triumph and warning. " +
                "Ezekiel declares: \"This day will be remembered. The Purists sought to erase us, to deny the union of flesh and steel. " +
                "But we have proven that we are more than their fear and hatred. We are the future.\"\n\n" +

                "He turns to you, his expression somber yet proud. \"And you, our champion, have forged the path to that future. " +
                "Your name will be etched into the halls of our history.\"\n\n" +

                "The Mecharions cheer your name, their voices a chorus of mechanical fervor. " +
                "Yet, amidst the celebration, a faint unease lingers. " +
                "You can feel it, unspoken but present, a shadow cast by the temple and the memory of Lily.\n\n" +

                "As the days pass, your newfound rank within the Mecharions grants you respect and authority. " +
                "You oversee operations, make critical decisions, and shape the future of the enclave. " +
                "But the weight of your choices begins to press heavily. " +
                "The temple remains a forbidden place, its halls empty save for whispers of what transpired there. " +
                "Ezekiel himself avoids it, his once commanding presence dimmed whenever its name is spoken. " +
                "You, too, find yourself unable to return, the memory of Lily's fate too raw, too haunting. " +
                "She is gone, but her shadow lingers, an unspoken reminder of what was lost in the pursuit of victory. " +
                "Among the Mecharions, she is seldom mentioned, her story buried beneath layers of unspoken guilt and reverence. " +
                "Yet, in quiet moments, you feel her presence: in the soft hum of machinery, in the fleeting silence between your responsibilities.";

            return sceneDescription;
        }
    }
}
