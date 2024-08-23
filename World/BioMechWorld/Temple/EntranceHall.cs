using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class EntranceHall
    {
        public Room Create(Room courtyard)
        {

            Room entranceHall = new Room(
                "Temple Entrance Hall",
                "Temple Entrance Hall",
                "An imposing stone chamber, with towering columns lining the walls, each one etched with worn, " +
                "ancient symbols. The floor is uneven, with cracks revealing roots that have forced their way through the stone. " +
                "The air is thick with the scent of age and decay, and the faint echo of dripping water can be heard in the distance. "
            );

            Exit toCourtyard = new Exit(
                "Framed by vines and overgrown foliage, at the bottom of the stairs",
                "A weathered stone doorway, leading back to the temple courtyard.",
                courtyard
            );

            Exit toTemple = new Exit(
                "A grand staircase ascends into darkness at the far end, its steps worn from centuries of passage",
                "leading further into the unknown depths of the temple.",
                entranceHall); //fix

            Character shadecreeper = new Character(
                "Shadecreeper",
                "A small, shadowy figure lurks in the darkness, its form barely distinguishable from the surrounding gloom. " +
                "Large, dark eyes gleam faintly, reflecting the ambient light. Its matted fur blends seamlessly into the shadows, " +
                "and a primitive bow, crafted from unknown materials, is clutched in its clawed hands. The creature moves with an eerie silence, " +
                "suggesting a life spent in places where light seldom reaches.",
                Kindred.Fay,
                CreatureType.Humanoid, // Humanoid type due to their upright stance and use of tools
                8,  // strength - physically weak, relies more on agility and stealth
                14, // dexterity - high dexterity for stealth and archery
                10, // constitution - average toughness, can survive in harsh environments
                7,  // intelligence - limited intelligence, primarily instinct-driven
                12, // wisdom - has a natural cunning and awareness of its surroundings
                8,  // charisma - not charismatic, tends to induce fear or unease
                "The Shadecreeper lies still, its shadowy form now lifeless. Even in death, its presence seems to cast a darkness around it, " +
                "as if the shadows cling to its body. The primitive bow rests by its side, a reminder of the unseen dangers lurking in the darkness."
            );
            shadecreeper.AddEquippedItem(InventorySlot.MainHand, WeaponList.StoneKnife());
            shadecreeper.AddEquippedItem(InventorySlot.MainHand, WeaponList.TribalShortBow());

            // Set the creature's health
            shadecreeper.HP = 10;

            // Add special feats or abilities
            shadecreeper.AddFeat(FeatKey.Stealth); // Allows them to move undetected
            shadecreeper.AddFeat(FeatKey.DarkVision); // Can see clearly in low-light conditions
            shadecreeper.AddFeat(FeatKey.MeleeAttack);
            shadecreeper.AddFeat(FeatKey.RangedAttack);

            return entranceHall;
        }
    }
}
