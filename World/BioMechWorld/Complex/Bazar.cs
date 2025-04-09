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

namespace fire_ash_server.World.BioMechWorld.Complex
{
    internal class Bazar
    {
        public static Room Create(Room backAlley)
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
            bazar.Light = Light.Bright;

            // Bazar -> Back Alley
            Exit exitFromBazarToBackAlley = new Exit(
                "At the far end of the bazar, leading to the back alley southward",
                "The back alley offers a stark contrast to the bustling activity of the bazar.",
                backAlley
            );
            bazar.AddExit(exitFromBazarToBackAlley);

            //weapon stall
            Room weaponStallRoom = new Room(
                "Weapon Stall",
                "Weapon Stall",
                "Blades, blasters, and exotic weaponry of all kinds are displayed."
                );

            bazar.AddExit(new Exit(
                "In the bazar",
                "A stall overflowing with various weapons.",
                weaponStallRoom));

            Exit weaponStallRoomExit = new Exit("The bustling bazar", bazar);
            weaponStallRoom.AddExit(weaponStallRoomExit);

            Character weaponsTrader = new Character(
                "Kael",
                "Kael, a stoic figure in tactical gear with mechanical enhancements. " +
                "Kael speaks only when necessary, with a calm, authoritative voice.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                13, // strength
                11, // dexterity
                13, // constitution
                12, // intelligence
                11, // wisdom
                10, // charisma
                "Kael's form collapsed, his once imposing figure now lies still."
            );
            weaponsTrader.IsTrader = true;
            weaponsTrader.tradeModifier = 0.1;
            weaponsTrader.UniqueName = true;
            weaponsTrader.HP = 21;
            weaponsTrader.AddFeat(FeatKey.MeleeAttack);
            weaponsTrader.AddFeat(FeatKey.RangedAttack);
            weaponsTrader.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            weaponsTrader.GoToRoom(weaponStallRoom);
            weaponStallRoom.propsInImage.Add(weaponsTrader);

            weaponsTrader.AddOnAfterMoveToEvent(
                (soul) =>
                {
                    if (soul.Character.IsHidden()) return Task.FromResult(false);

                    Character movedToCharacter = weaponsTrader;
                    movedToCharacter.SetLookAt(soul.Character);
                    _ = soul.SendAsync($"{movedToCharacter.Name} gives you a measured look, a subtle nod suggesting you make a purchase or continue on your journey.");

                    return Task.FromResult(true);
                },
                false);

            weaponsTrader.AddToInventory(WeaponList.ColtARFifteen());
            weaponsTrader.AddToInventory(WeaponList.HolographicBlade());
            weaponsTrader.AddToInventory(WeaponList.LuminarBaton());
            weaponsTrader.AddToInventory(WeaponList.Machete());
            weaponsTrader.AddToInventory(ConsumableList.BookOfDualWield());
            weaponsTrader.AddToInventory(new Coins(150, 25));



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

            Character armorTrader = new Character(
                "Talon",
                "Talon, a figure clad in an array of intricately designed armor pieces. " +
                "His face is obscured by a helmet that combines old-world craftsmanship with modern enhancements, " +
                "giving him an enigmatic presence. Talon's single eye, " +
                "glowing faintly from his visor, " +
                "scan the surroundings with a keen and calculating gaze.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                16, // strength
                12, // dexterity
                14, // constitution
                10, // intelligence
                11, // wisdom
                8,  // charisma
                    // death description
                "Talon's armored form lies still, his once formidable presence now lifeless. " +
                "His helmet, once concealing his identity, now reveals an empty gaze."
            );
            armorTrader.IsTrader = true;
            armorTrader.tradeModifier = 0.1;
            armorTrader.UniqueName = true;
            armorTrader.HP = 15;
            armorTrader.AddFeat(FeatKey.MeleeAttack);
            armorTrader.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            armorTrader.GoToRoom(armorStallRoom);

            armorTrader.AddToInventory(ArmorList.DriftersVest());
            armorTrader.AddToInventory(ArmorList.MetalShield());
            armorTrader.AddToInventory(new Coins(130, 30));

            armorTrader.AddOnAfterMoveToEvent(
                (soul) =>
                {
                    if (soul.Character.IsHidden()) return Task.FromResult(false);

                    Character movedToCharacter = armorTrader;
                    string[] dialogues =
                    [
                        "Armor isn't just metal and plates; it's an art of the old world, refined with a touch of the future. What's your preference, antiquity or innovation?",
                        "In this stall, each piece of armor carries a legacy. My eye discerns the worth in every alloy. What do you seek, a shield for the soul or a mask for the mind?",
                        "Under this helmet, vision is clarity. Every curve, every seam, serves a purpose. Tell me, what do you need protection from, the world outside or the one within?"
                    ];
                    Random random = new Random();
                    int index = random.Next(dialogues.Length);
                    movedToCharacter.Speak(dialogues[index]);

                    return Task.FromResult(true);
                },
                false);

            Room artifactStallRoom = new Room(
                "Cogs & Curios",
                "Cogs & Curios",
                "You step into 'Cogs & Curios,' a dimly lit stall filled with an eclectic mix of artifacts and components. " +
                "The air is thick with the scent of old parchment and metal. " +
                "Strange, glowing artifacts and components are scattered across the tables, their origins and purposes shrouded in mystery. " +
                "Shelves crammed with ancient tomes and mechanical parts line the walls, casting long shadows in the flickering light. " +
                "Hanging from the ceiling are various curios, some slowly spinning, reflecting light in strange patterns."
            );

            Exit entranceToartifactStallRoom = new Exit(
                "In the other end",
                "An anonomous door with a small sign reading 'Cogs & Curios'.",
                artifactStallRoom);
            entranceToartifactStallRoom.Hide(9);

            bazar.AddExit(entranceToartifactStallRoom);

            Exit artifactStallRoomExit = new Exit("A door, leading to the bazar.", bazar);
            artifactStallRoom.AddExit(artifactStallRoomExit);

            Character artifactTrader = new Character(
                "Quirin",
                "Quirin, the aged trader of 'Cogs & Curios,' stands with surprising strength, supported by a mechanical suit that maintains his posture. " +
                "Despite his advanced age, his eyes are sharp and observant, though they carry a deep weariness.",
                Kindred.Mecharion,
                CreatureType.Humanoid,
                10, // strength
                10, // dexterity
                9, // constitution
                16, // intelligence
                15, // wisdom
                13, // charisma
                    // death description
                "Quirin's lifeless form and mechanical suit now motionless. " +
                "His tired eyes are closed forever, and the many secrets he guarded with such care are now silent."
            );
            artifactTrader.IsTrader = true;
            artifactTrader.tradeModifier = 0.1;
            artifactTrader.UniqueName = true;
            artifactTrader.HP = 16;
            artifactTrader.AddFeat(FeatKey.MeleeAttack);
            artifactTrader.Faction = Program.WorldSoul.GetFaction(FactionKey.Technomancers);
            artifactTrader.GoToRoom(artifactStallRoom);

            artifactTrader.AddToInventory(WeaponList.FlashLight());
            artifactTrader.AddToInventory(ConsumableList.HealthPotion());
            artifactTrader.AddToInventory(ConsumableList.ScrollOfEntanglement());
            artifactTrader.AddToInventory(new Coins(90, 40));

            new MainHallRoom(bazar);

            return bazar;
        }
    }
}
