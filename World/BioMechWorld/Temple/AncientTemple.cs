using System;
using System.Collections.Generic;
using fire_ash_server.World.BioMechWorld;
using fire_ash_server.Props;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using System.Net.Sockets;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld
{
    internal class AncientTemple
    {
        public AncientTemple()
        {
            Create();
        }

        public static Room Create()
        {
            // Entrance Hall
            Room entranceHall = new Room(
                Description(RoomKey.TempleEntranceHall),
                "Ancient Temple Entrance",
                "You enter through a large, crumbling archway. The hall is grand but decayed, with stone pillars covered in moss and vines. Dust motes float in the air, illuminated by faint light filtering through cracks in the ceiling. The silence is broken only by the distant sound of dripping water and the rustle of unseen creatures."
            );

            // Exit to connect parent room with the entrance hall
            Exit toEntranceHall = new Exit(
                "A large, crumbling archway.",
                "Beyond the archway lies the entrance to an ancient temple.",
                entranceHall
            );

            // Main Hall
            Room mainHall = new Room(
                "Main Hall",
                "Temple Main Hall",
                "A vast, open space with a large statue of Zathar in the center. The floor is made of cracked stone tiles, and the air is thick with the scent of mildew and decay. Dim light filters through cracks in the ceiling, casting eerie shadows."
            );

            // Exit from entrance hall to main hall
            entranceHall.AddExit(new Exit(
                "A grand archway leading deeper into the temple.",
                "The path to the heart of the temple.",
                mainHall
            ));
            mainHall.AddExit(new Exit(
                "The way back to the entrance hall.",
                "A passage leading back to the entrance.",
                entranceHall
            ));

            // Side Chambers
            Room libraryChamber = new Room(
                "Library Chamber",
                "Ancient Library",
                "Shelves filled with ancient scrolls and books. The air is thick with dust, and the faint smell of old parchment fills the room. Dim light filters through cracks in the walls, illuminating the faded texts."
            );

            Room armoryChamber = new Room(
                "Armory Chamber",
                "Ancient Armory",
                "Old weapons and armor, some still functional and enchanted, are displayed on stands. The room is protected by ancient traps, with pressure plates and dart shooters hidden in the shadows."
            );

            Room ritualChamber = new Room(
                "Ritual Chamber",
                "Ancient Ritual Chamber",
                "An altar with symbols and offerings to Zathar. The walls are covered with carvings depicting ritual sacrifices and worship. The air is thick with the scent of incense and burnt offerings."
            );

            // Exits from main hall to side chambers
            mainHall.AddExit(new Exit(
                "A doorway leading to the library chamber.",
                "The ancient library lies beyond this door.",
                libraryChamber
            ));
            mainHall.AddExit(new Exit(
                "A doorway leading to the armory chamber.",
                "The ancient armory lies beyond this door.",
                armoryChamber
            ));
            mainHall.AddExit(new Exit(
                "A doorway leading to the ritual chamber.",
                "The ancient ritual chamber lies beyond this door.",
                ritualChamber
            ));

            // Exits from side chambers to main hall
            libraryChamber.AddExit(new Exit(
                "The way back to the main hall.",
                "A passage leading back to the main hall.",
                mainHall
            ));
            armoryChamber.AddExit(new Exit(
                "The way back to the main hall.",
                "A passage leading back to the main hall.",
                mainHall
            ));
            ritualChamber.AddExit(new Exit(
                "The way back to the main hall.",
                "A passage leading back to the main hall.",
                mainHall
            ));

            // Hidden Passage
            Room hiddenPassage = new Room(
                "Hidden Passage",
                "Secret Passageway",
                "A narrow, dark passage with roots and vines creeping through the walls. The air grows colder with each step, and faint whispers seem to echo from the walls."
            );

            // Exit from main hall to hidden passage
            mainHall.AddExit(new Exit(
                "A concealed doorway behind the statue of Zathar.",
                "The hidden passage behind the statue.",
                hiddenPassage
            ));
            hiddenPassage.AddExit(new Exit(
                "The way back to the main hall.",
                "A path leading back to the main hall.",
                mainHall
            ));

            // Ancient Tunnels
            Room ancientTunnels = new Room(
                "Ancient Tunnels",
                "Tunnel Entrance",
                "Rough, uneven walls and damp, musty air. The weight of the earth above is palpable, and the tunnels seem to stretch on forever in darkness."
            );

            // Exit from hidden passage to ancient tunnels
            hiddenPassage.AddExit(new Exit(
                "A narrow tunnel, its entrance barely visible.",
                "The ancient tunnels, forgotten by time.",
                ancientTunnels
            ));
            ancientTunnels.AddExit(new Exit(
                "The way back to the hidden passage.",
                "A path leading back to the hidden passage.",
                hiddenPassage
            ));

            // Central Chamber
            Room centralChamber = new Room(
                "Central Chamber",
                "Temple Central Chamber",
                "A vast chamber dominated by a towering statue of Zathar. The walls are adorned with faded murals depicting scenes of worship and ritual sacrifice. The floor is covered in dust, disturbed only by the faint impressions of long-forgotten footsteps."
            );

            // Exit from ancient tunnels to central chamber
            ancientTunnels.AddExit(new Exit(
                "A grand archway leading to the central chamber.",
                "The path to the heart of the temple.",
                centralChamber
            ));
            centralChamber.AddExit(new Exit(
                "The way back to the ancient tunnels.",
                "A passage leading back to the ancient tunnels.",
                ancientTunnels
            ));

            // Exit to the Purists' Stronghold
            Room puristStrongholdEntrance = new Room(
                "Purist Stronghold Entrance",
                "Entrance to the Purists' Stronghold",
                "A hidden door or tunnel at the back of the central chamber, leading out of the temple and towards the Purists' territory. The atmosphere shifts from ancient and mystical to mechanical and hostile."
            );

            // Exit from central chamber to Purists' stronghold
            centralChamber.AddExit(new Exit(
                "A hidden door at the back of the chamber.",
                "The path to the Purists' stronghold.",
                puristStrongholdEntrance
            ));
            puristStrongholdEntrance.AddExit(new Exit(
                "The way back to the central chamber.",
                "A passage leading back to the central chamber.",
                centralChamber
            ));

            // Populate the rooms with items and characters
            //PopulateTemple(entranceHall, mainHall, libraryChamber, armoryChamber, ritualChamber, hiddenPassage, ancientTunnels, centralChamber, puristStrongholdEntrance);

            // Items and descriptions
            Item ancientRelic = new Item(
                "Ancient Relic",
                "A mysterious artifact, its surface covered in intricate carvings.",
                "The relic exudes an aura of ancient power, its purpose long forgotten but still palpable."
            );
            mainHall.AddItem(ancientRelic);

            Item sacredScroll = new Item(
                "Sacred Scroll",
                "An old scroll with faded writing.",
                "The scroll contains ancient knowledge and rituals dedicated to Zathar."
            );
            libraryChamber.AddItem(sacredScroll);

            Item enchantedSword = new Item(
                "Enchanted Sword",
                "A sword with a faint, magical glow.",
                "The sword is imbued with protective magic, a relic from the temple's guardians."
            );
            armoryChamber.AddItem(enchantedSword);

            Item ritualDagger = new Item(
                "Ritual Dagger",
                "A ceremonial dagger with intricate designs.",
                "The dagger was used in ancient rituals to honor Zathar."
            );
            ritualChamber.AddItem(ritualDagger);

            // NPCs and dialogue
            Character templeGuardian = new Character(
                "Temple Guardian",
                "A spectral figure clad in ancient armor, its eyes glowing with an otherworldly light.",
                Race.Undead,
                CreatureType.Humanoid,
                16, // strength
                12, // dexterity
                14, // constitution
                10, // intelligence
                12, // wisdom
                8,   // charisma
                "The once imposing figure of the Temple Guardian lies in a crumpled heap.The glow in its eyes has faded, leaving dark, empty sockets.The ancient armor is now dull and cracked, revealing the spectral form that inhabited it.The air around the fallen guardian is still, the echo of its haunting presence now silenced."
            );
            templeGuardian.UniqueName = true;
            templeGuardian.HP = 45;
            templeGuardian.AddFeat(FeatKey.MeleeAttack);
            templeGuardian.Faction = Program.WorldSoul.GetFaction(FactionKey.Zathar);
            templeGuardian.GoToRoom(centralChamber);

            // Dialogue nodes
            DialogueNode guardianGreeting = new DialogueNode("Who dares to disturb the sanctity of Zathar's domain?");
            DialogueNode guardianLore = new DialogueNode(
                "Zathar, the old snake god, was revered for his wisdom, stealth, and transformative powers. " +
                "This temple was his sacred ground, where the faithful gathered to seek his favor and guidance."
            );
            DialogueNode guardianWarning = new DialogueNode(
                "Beware, for the ancient tunnels are filled with dangers unseen for centuries. " +
                "Only the worthy may pass and uncover the secrets that lie within."
            );

            guardianGreeting.AddChoice("Who are you?", guardianLore);
            guardianGreeting.AddChoice("What is this place?", guardianWarning);

            guardianLore.AddChoice("What should I know about Zathar?", guardianWarning);

            guardianWarning.AddChoice("Thank you for the warning.", guardianGreeting, true);

            // Assign dialogue to the guardian
            templeGuardian.CreateDialogueManager(guardianGreeting);

            return entranceHall;
        }
    }
}