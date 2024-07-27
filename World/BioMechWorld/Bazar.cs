using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using static fire_ash_server.Helpers;
using fire_ash_server.Props.Items.Weapons;
using System.Collections;
using fire_ash_server.World.BioMechWorld.MainHall;
using System.Diagnostics.Metrics;

namespace fire_ash_server.World.BioMechWorld
{
    internal class Bazar
    {
        public Bazar(Room nexusBridge)
        {
            // Define the bazar area as a room
            Room bazar = new Room(
                Description(RoomKey.Bazar),
                "Bazar",
                "A bustling bazar, filled with a myriad of stalls and vendors. " +
                "Dimly lit by flickering lanterns hanging from overhead pipes, the bazar exudes an eerie, mechanical charm. " +
                "Stalls are crafted from salvaged metal and adorned with various trinkets and artifacts. " +
                "The air is thick with the scent of oil and metal, mingling with the hushed whispers of bartering Mecharions. " +
                "Shadows dance across the walls as vendors showcase their wares, from weaponry and armor to mysterious artifacts and rare components."
            );
            
            //weapon stall
            Room weaponStallRoom = new Room(
                "Weapon Stall",
                "Weapon Stall",
                "Blades, blasters, and exotic weaponry of all kinds are displayed. "
                //"The vendor, a grizzled Mecharion with a patchwork of mechanical limbs, haggles with customers over prices."
                );

            bazar.AddExit(new Exit(
                "In the bazar",
                "A stall overflowing with various weapons.",
                weaponStallRoom));

            Exit weaponStallRoomExit = new Exit("The bustling bazar", bazar);
            weaponStallRoom.AddExit(weaponStallRoomExit);

            //Armor Stall
            Room armorStallRoom = new Room(
                "Armor Stall",
                "Armor Stall",
                "Suits of armor, both new and ancient, are arranged meticulously. "
            );

            bazar.AddExit(new Exit(
                $"besides the {weaponStallRoom.Name}",
                "A stall displaying a variety of armors.",
                armorStallRoom));

            Exit armorStallRoomExit = new Exit("The bustling bazar", bazar);
            armorStallRoom.AddExit(armorStallRoomExit);

            //Artifact Stall
            Room artifactStallRoom = new Room(
                "Cogs & Curios",
                "Cogs & Curios",
                "Strange, glowing artifacts and components are scattered across the table. "
            );

            Exit entranceToartifactStallRoom = new Exit(
                "In the other end",
                "An anonomous door with a small sign reading 'Cogs & Curios'.",
                artifactStallRoom);
            entranceToartifactStallRoom.Hide(1);

            bazar.AddExit(entranceToartifactStallRoom);

            Exit artifactStallRoomExit = new Exit("A door, leading to the bazar.", bazar);
            artifactStallRoom.AddExit(artifactStallRoomExit);

            // bridge -> bazar
            Exit entranceToBazar = new Exit(
                "To the west",
                "A metal door leads to what appears to pathway with lots af mecharion activity",
                bazar
            );
            nexusBridge.AddExit(entranceToBazar);

            new MainHallRoom(bazar);



        }
    }
}
