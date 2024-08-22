using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items.Armor;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal class BrokenBridge
    {
        public static Room Create(Room serpentsSpine)
        {
            Room brokenBridge = new Room(
                "Broken Bridge",
                "A broken bridge spans a dark, " +
                "bottomless chasm, its stone slabs cracked and uneven. " +
                "Wide gaps expose the void below, and a cold draft rises from the abyss.");

            Exit toBrokenBridge = new Exit(
                $"At the bottom of the {serpentsSpine.Name}",
                "A narrow set of worn stone stairs descends steeply, carved into the rock itself. The steps are slick with moisture, " +
                "leading down into the shadowy depths where the fractured remnants of a bridge await.",
                brokenBridge);
            serpentsSpine.AddExit(toBrokenBridge);

            UndergroundCavePath.Create(brokenBridge);

            Exit toSerpentsSpine = new Exit(
                "A plateau, connected to the fractured remnants of the two bridges",
                "A steep ascent begins here, where slick, worn stone stairs wind upwards, " +
                "leading out of the shadows toward the imposing heights of the Serpent's Spine.",
                serpentsSpine);
            brokenBridge.AddExit(toSerpentsSpine);

            return brokenBridge;
        }
    }
}
