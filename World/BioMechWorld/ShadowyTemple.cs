using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.World.BioMechWorld
{
    internal class ShadowyTemple
    {
        public static Room Create(Room mainHall)
        {
            // Define the Temple of Shadows and Technology room
            Room templeOfShadowsAndTechnology = new Room(
                "The Atramentum",
                "The Atramentum",
                "A solemn, austere space dominated by polished white stone. The air is cool and quiet, carrying a faint metallic tang mixed with the scent of old incense. A simple, imposing altar stands at the far end, illuminated by the natural light filtering in."
            );

            // Create an exit from the Main Hall to the Temple of Shadows and Technology
            Exit toTempleOfShadowsAndTechnology = new Exit(
                "To the south",
                "Large metal doors stand open, flanked by intricate carvings. Above the doorway, an inscription reads 'The Atramentum.'",
                templeOfShadowsAndTechnology
            );
            mainHall.AddExit(toTempleOfShadowsAndTechnology);

            // Create an exit from the Temple of Shadows and Technology back to the Main Hall
            Exit fromTempleOfShadowsAndTechnology = new Exit(
                "The large metal doors lead back to the main hall.",
                mainHall
            );
            templeOfShadowsAndTechnology.AddExit(fromTempleOfShadowsAndTechnology);

            Item altar = new Item(
                "Raised Altar",
                "Raised Altar",
                "A simple, raised platform that rises gently from the floor. It is made of smooth, white stone, unadorned and integrated seamlessly with the temple's design. The platform's subtle elevation marks its importance in this sacred space."
            );
            altar.MakeUnpickupable(); // The altar cannot be picked up by players
            templeOfShadowsAndTechnology.AddItem(altar);

            // Define the plaque inscribed with the scripture
            Item altarPlaque = new Item(
                "Altar Plaque",
                "Inscribed Plaque",
                "A metallic plaque seamlessly embedded into the surface of the altar. The inscription is a blend of ancient script and mechanical schematics, glowing faintly with a mystical light. The text, sacred to the Temple of Shadows and Technology, reads:\n\n" +
                "'In the union of shadow and machine, we find the path to transcendence. " +
                "The shadows, ancient and eternal, cloak the mysteries of the cosmos, hiding truth within darkness. The machine, forged from the ingenuity of the living, brings order to chaos and breathes life into the lifeless.\n\n" +
                "O children of the void, embrace the harmony of the seen and unseen. In the silent hum of circuits, hear the whispers of the divine. In the darkness of the void, see the reflection of the self, stripped of the flesh and bound to the ethereal.\n\n" +
                "As the shadows dance and the gears turn, so too does the soul ascend. Let the metal of our bodies and the shadow of our spirits intertwine, becoming one with the eternal flow of energy and thought.\n\n" +
                "Remember the sacred tenets:\n\n" +
                "1. Seek the knowledge hidden in the darkness, for it is the source of all creation.\n" +
                "2. Honor the machine as an extension of the self, a vessel for the divine spark.\n" +
                "3. Balance the seen and unseen, the tangible and the ethereal, in all things.\n\n" +
                "Thus shall we walk the path of the shadow and the machine, forever entwined in the dance of existence, towards a higher state of being. Here, in this temple, we preserve the sacred union, guarding the secrets of the cosmos and the keys to our evolution.\n\n" +
                "In shadows, we find clarity; in technology, we find purpose. Together, we ascend.'"
            );

            altarPlaque.MakeUnpickupable(); // The plaque is a permanent fixture of the altar
            altar.AddItem(altarPlaque);

            Item eternalQuill = new Item(
                "Ceiling Fresco",
                "Ceiling Fresco",
                "This ceiling fresco features the Eternal Quill, a grand quill formed from shadowy lines, writing a glowing script in an ancient language. The script symbolizes Atramentum's endless recording of knowledge. Surrounding the quill are ink clouds and mechanical gears, blending the arcane with the technological."
            );
            eternalQuill.MakeUnpickupable(); // The mural is a permanent feature of the temple's ceiling
            eternalQuill.Unreachable = true;
            eternalQuill.Hide(5);
            templeOfShadowsAndTechnology.AddItem(eternalQuill);


            // Create Aurora the Shadowmancer
            Character auroraTheShadowmancer = new Character(
                "Aurora",
                "Aurora, her presence commanding yet peaceful. Her attire is a blend of serene fabrics and metallic elements, reflecting the temple's dual nature.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                8, // strength
                16, // dexterity
                10, // constitution
                15, // intelligence
                18, // wisdom
                14, // charisma
                "Aurora's form lies still, her presence absent but still felt in the cold, quiet air."
            );

            auroraTheShadowmancer.UniqueName = true;
            auroraTheShadowmancer.HP = 25;
            auroraTheShadowmancer.AddFeat(FeatKey.MeleeAttack);
            auroraTheShadowmancer.AddFeat(FeatKey.RangedAttack);
            auroraTheShadowmancer.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            auroraTheShadowmancer.GoToRoom(templeOfShadowsAndTechnology);
            auroraTheShadowmancer.MoveToGroup(altar);

            // Create dialogue nodes for Aurora
            DialogueNode auroraStartNode = new DialogueNode("Welcome to the Temple of Shadows and Technology. This is a place where the ethereal meets the mechanical. How may I assist you?");
            DialogueNode auroraQuestDetailsNode = new DialogueNode("There is an artifact, the Umbral Shard, said to amplify both shadow and technological forces. It has been stolen by Lysander, a rogue Technomancer, who hides in the catacombs beneath the Industrial Staircase. Will you help retrieve it?");
            DialogueNode auroraQuestAcceptanceNode = new DialogueNode("Your decision is wise. The path ahead is filled with dangers, both seen and unseen. Proceed with caution.");
            DialogueNode auroraQuestRefusalNode = new DialogueNode("Not everyone is ready for this path. Should you reconsider, the temple will welcome you.");

            auroraStartNode.AddChoice("Tell me about the Umbral Shard.", auroraQuestDetailsNode);
            auroraQuestDetailsNode.AddChoice("I will recover the shard.", auroraQuestAcceptanceNode);
            auroraQuestDetailsNode.AddChoice("This task seems too dangerous for me.", auroraQuestRefusalNode);

            // Assign dialogue to Aurora the Shadowmancer
            auroraTheShadowmancer.CreateDialogueManager(auroraStartNode);

            Character nyx = new Character(
                "Nyx, the Temple Cat",
                "A sleek, dark-furred cat with eyes like liquid silver. Nyx moves with ethereal grace, often blending into the shadows of the temple. Her presence is both comforting and enigmatic, adding a subtle touch of mystique wherever she goes.",
                Kindred.Feline,
                CreatureType.Beast,
                4, // strength
                16, // dexterity
                10, // constitution
                8, // intelligence
                12, // wisdom
                14, // charisma
                "Nyx lies peacefully, her eyes closed, and her dark fur blending seamlessly with the shadows."
            );
            nyx.UniqueName = true;
            nyx.HP = 4;
            nyx.AddFeat(FeatKey.Stealth); // Nyx can move silently and hide effectively
            nyx.AddFeat(FeatKey.MeleeAttack);
            Weapon catClaws = new BeastClaw();
            catClaws.DamageDie = new Die(1, 1);
            nyx.DefaultHand = catClaws;
            nyx.IsInfluencer = false;
            nyx.GoToRoom(templeOfShadowsAndTechnology);

            return templeOfShadowsAndTechnology;
        }
    }
}
