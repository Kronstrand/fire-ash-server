using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldInn
    {
        public static Room Create(bool initProps, Room goldfieldSquare)
        {
            Room goldfieldInn = new Room(
                RoomKey.GoldfieldInn,
                "The Golden Hearth Inn",
                "The hearth flickers weakly, smoke curling into a blackened ceiling. " +
                "The furniture is scarred and worn, the rug threadbare and damp in places. " +
                "Mugs line the shelves, some chipped, some cracked, and the lingering scent of old ale and burnt wood fills the room."
            );

            Item innCounter = new Item(
                "Scorched Counter",
                "At the back of the common room",
                "A long, scarred wooden counter stretches across the room. Plates and mugs sit unevenly, some chipped or broken, " +
                "while sagging shelves behind hold dusty cups and folded linens, darkened with stains.",
                "goldfieldInn.counter"
            );
            goldfieldInn.AddItem(innCounter);

            Exit toSquare = new Exit(
                "The inn's front door creaks as it opens onto Goldfield Square",
                "Beyond the doorway lies the cobblestone square and the town statue.",
                goldfieldSquare,
                "GoldfieldInnToSquare"
            );
            goldfieldInn.AddExit(toSquare);

            if (!initProps)
                return goldfieldInn;

            Character innkeeperMira = new Character(
                "Mira",
                "Mira, the innkeeper, moves with careful, wary motions. Her hands are calloused and scarred, and her eyes dart constantly, " +
                "as if expecting trouble at any moment. She wipes the counter with methodical precision, never fully at ease.",
                Kindred.Human,
                CreatureType.Humanoid,
                8, 10, 10, 10, 12, 13,
                "Mira lies sprawled across the floor, her eyes wide in a grim mockery of rest."
            );
            innkeeperMira.IsTrader = true;
            innkeeperMira.TradeModifier = 0.2;
            innkeeperMira.UniqueName = true;
            innkeeperMira.Title = "Innkeeper";
            innkeeperMira.HP = 10;
            innkeeperMira.AddFeat(FeatKey.MeleeAttack);
            innkeeperMira.Faction = Program.WorldSoul.GetFaction(FactionKey.Goldfield);
            innkeeperMira.GoToRoom(goldfieldInn);
            innkeeperMira.MoveToGroup(innCounter);
            innkeeperMira.ItemRespawns.Add(new ItemRespawn(1, 2, 2, ItemFactoryKey.GoldBerryPie));

            Character bardEldrin = new Character(
                "Felendrik",
                "Felendrik, the bard, carries himself with a quiet, measured presence. His voice is low and steady when he speaks or sings, " +
                "and his gaze lingers just a moment too long, as if weighing more than what is said. His clothes are worn from travel, " +
                "and there is a sense that he is always observing, always listening.",
                Kindred.Human,
                CreatureType.Humanoid,
                9, 12, 10, 11, 11, 16,
                "Felendrik lies still, his expression distant and unreadable, as though his thoughts had already drifted elsewhere."
            );
            innkeeperMira.IsTrader = true;
            bardEldrin.TradeModifier = 0.2;
            bardEldrin.UniqueName = true;
            bardEldrin.Title = "Bard";
            bardEldrin.HP = 10;
            bardEldrin.AddFeat(FeatKey.MeleeAttack); // basic self-defense
            bardEldrin.Faction = Program.WorldSoul.GetFaction(FactionKey.Goldfield);
            bardEldrin.GoToRoom(goldfieldInn);
            bardEldrin.MoveToGroup(innCounter);

            return goldfieldInn;
        }
    }
}
