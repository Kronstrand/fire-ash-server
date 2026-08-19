using System;
using System.Collections.Generic;
using fire_ash_server.World.BioMechWorld;
using fire_ash_server.Props;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using System.Net.Sockets;
using static fire_ash_server.Helpers;
using fire_ash_server.Props.Items.Weapons;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using fire_ash_server.World.BioMechWorld.Temple;
using fire_ash_server.Props.Items.Armoring;

namespace fire_ash_server.World.BioMechWorld
{
    internal class UndergroundCavePath
    {

        public static Room Create(Room brokenBridge)
        {
            Room undergroundCavePath = new Room(
                RoomKey.UndergroundCavePath,
                "Underground Cave Path",
                "The air is cool and damp, carrying a faint, musty odor. The path ahead is uneven, " +
                "illuminated by the soft, eerie glow of fluorescent pools of liquid and fireflies flitting about. " +
                "Ancient pillars, covered in moss and mysterious carvings, rise from the ground, supporting the weight of the tunnel. " +
                "The atmosphere is thick with a sense of foreboding, and the distant sound of dripping water echoes through the cavern."
            );
            undergroundCavePath.Light = Light.Dim;

            Exit exitToBorkenBridge = new Exit(
                "Through the cave wall",
                "A tunnel, with carved stairs guiding the way, ascends upwards.",
                 brokenBridge);
            undergroundCavePath.AddExit(exitToBorkenBridge);

            Item liquidPool = new Item(
                "Fluorescent Pool of Liquid",
                "In the middle",
                "A small pool of glowing liquid. The bioluminescent fluid shimmers with an eerie light, casting dancing reflections on the cave walls. It looks otherworldly and seems to pulse with a life of its own."
            );
            liquidPool.Light = Light.Bright;
            liquidPool.MakeUnpickupable();
            undergroundCavePath.AddItem(liquidPool);

            Item abandonedGuardPost = new Item(
                "Abandoned Guard Post",
                "In the cave wall, partially obscured behind an ancient pillar",
                "A wide opening in the cave wall, its edges rough but clearly shaped by deliberate hands. " +
                "Once a guard post, this hollow was carved out as a strategic lookout or last line of defense. " +
                "The interior is now dark and empty, with faint traces of old weapon racks and rusted chains embedded in the stone, " +
                "hinting at its past purpose."
            );
            abandonedGuardPost.Light = Light.Darkness;
            abandonedGuardPost.MakeUnpickupable();
            abandonedGuardPost.DynamicDescription = true; //if the description is not part the room description and need to change, typically based on the light property
            abandonedGuardPost.DarknessOverride = true;

            undergroundCavePath.AddItem(abandonedGuardPost);

            Character rustBeetle = new Character(
                "Rust Beetle",
                "The Rust Beetle inhabits the industrial wastelands of the BioMechWorld. " +
                "Its body is covered in a thick, rust-colored exoskeleton, offering it protection as it scuttles through the debris. " +
                "The beetle's mandibles, are sharp enough to cut through leather and softer metals. " +
                "A pair of beady, dark eyes peer out from its head, ever vigilant as it searches for sustenance in its harsh environment.",
                Kindred.None,
                CreatureType.Beast,
                8,  // strength
                12, // dexterity
                10, // constitution
                6,  // intelligence
                8,  // wisdom
                7,  // charisma
                "The Rust Beetle lies motionless, its legs curled inward. " +
                "The once vibrant rust-colored exoskeleton is now dull and cracked."
            );

            rustBeetle.HP = 6;
            rustBeetle.DefaultHand = new InsectClaw();
            rustBeetle.AddFeat(FeatKey.MeleeAttack);
            rustBeetle.GoToRoom(undergroundCavePath);
            rustBeetle.MoveToGroup(abandonedGuardPost);
            rustBeetle.InitAttack = false;
            //Events.AddCharacterMoveFromCharacterAndIsAttacked(rustBeetle);

            TempleCourtyard.Create(undergroundCavePath);

            return undergroundCavePath;
        }
    }
}