using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldSquare
    {
        public static Room Create(bool initProps)
        {
            Room goldfieldSquare = new Room(
                RoomKey.GoldfieldSquare,
                "Goldfield Square",
                "The cobblestone square is cracked and littered with debris. " +
                "At its center stands a weathered statue, depicting a figure clutching a sheaf of wheat, " +
                "its face chipped and worn, bronze darkened with age. " +
                "A crooked stall leans in one corner, its wooden frame scarred and patched, displaying a jumble of goods."
            );

            goldfieldSquare.Light = Light.Bright;

            Item farmingDeityStatue = new Item(
                "Stone Statue",
                "At the center of Goldfield Square",
                "The statue is chipped and grimy, depicting a solemn figure clutching a sheaf of wheat. " +
                "An eroded plaque marks: 'Dedicated to Lorath, protector of fields and harvests, by Aldric the Steward of Goldfield, year 437 DS.' " +
                "Its solemn gaze feels more like a warning than a blessing.",
                "goldfieldSquare.farmingDeityStatue"
            );
            goldfieldSquare.AddItem(farmingDeityStatue);

            Item vendorStall = new Item(
                "Wooden Stall",
                "At the corner of Goldfield Square",
                "A small wooden stall leans at an odd angle, displaying wares covered in dust and grime.",
                "goldfieldSquare.vendorStall"
            );
            goldfieldSquare.AddItem(vendorStall);

            //Inn
            Room Inn = GoldfieldInn.Create(initProps, goldfieldSquare);

            Exit toInn = new Exit(
                "To the north, a leaning timbered inn stands at the edge of the square, smoke curling from its chimney",
                "Through the open doorway, flickering shadows and a dim hearth can be seen inside.",
                Inn,
                "GoldfieldSquareToInn"
            );
            goldfieldSquare.AddExit(toInn);

            Room goldfieldFarmland = GoldfieldFarmland.Create(initProps, goldfieldSquare);

            Exit toFarmland = new Exit(
                "A dirt path winds toward the fields, overgrown and scarred by neglect",
                "Beyond the path, the fields stretch unevenly, stalks of wheat blackened and tangled.",
                goldfieldFarmland,
                "GoldfieldSquareToFarmland"
            );
            goldfieldSquare.AddExit(toFarmland);



            //temple
            Room templeOfLorath = TempleOfLorath.Create(initProps, goldfieldSquare);

            Exit toTemple = new Exit(
                "On the western edge of the square",
                "A worn stone path leads to a small temple with carved stone walls.",
                templeOfLorath,
                "GoldfieldSquareToTemple"
            );
            goldfieldSquare.AddExit(toTemple);

            if (!initProps)
                return goldfieldSquare;

            Character vendorBorin = new Character(
                "Borin",
                "Borin is a man marked by years of toil, his face lined and weary, his posture slightly stooped.",
                Kindred.Human,
                CreatureType.Humanoid,
                12, 8, 13, 9, 7, 12,
                "Borin lies motionless, eyes closed in eternal rest."
            );
            vendorBorin.IsTrader = true;
            vendorBorin.TradeModifier = 0.1;
            vendorBorin.UniqueName = true;
            vendorBorin.Title = "Vendor";
            vendorBorin.HP = 13;
            vendorBorin.AddFeat(FeatKey.MeleeAttack);
            vendorBorin.Faction = Program.WorldSoul.GetFaction(FactionKey.Goldfield);
            vendorBorin.GoToRoom(goldfieldSquare);
            vendorBorin.MoveToGroup(vendorStall);
            vendorBorin.AddToInventory(ConsumableList.HealthPotion());
            vendorBorin.AddToInventory(ConsumableList.BearTrap());
            vendorBorin.AddToInventory(WeaponList.Torch());
            vendorBorin.AddToInventory(WeaponList.Torch());
            vendorBorin.AddToInventory(WeaponList.RustedSword());
            vendorBorin.AddToInventory(WeaponList.RustedSword());
            vendorBorin.AddToInventory(WeaponList.RustedSword());
            vendorBorin.AddToInventory(WeaponList.YShapedSlingShot());

            Character townGuard = new Character(
                "Garrick",
                "Garrick is a battle-hardened guard, armor scarred and patched. Every motion seems measured, as if shaped by years of duty in a harsh world.",
                Kindred.Human,
                CreatureType.Humanoid,
                13, 10, 14, 10, 12, 11,
                "Garrick lies still, armor dented, body at rest, a lifetime of service etched into his features."
            );
            townGuard.UniqueName = true;
            townGuard.Title = "Guard";
            townGuard.HP = 21;
            townGuard.AddFeat(FeatKey.MeleeAttack);
            townGuard.AddFeat(FeatKey.RangedAttack);
            townGuard.AddEquippedItem(InventorySlot.MainHand, WeaponList.RustedSword());
            townGuard.AddEquippedItem(InventorySlot.Body, ArmorList.GuardLeather());
            townGuard.AddEquippedItem(InventorySlot.OffHand, ArmorList.SteelShield());
            townGuard.Faction = Program.WorldSoul.GetFaction(FactionKey.Goldfield);
            townGuard.GoToRoom(goldfieldSquare);
            townGuard.MoveToGroup(toFarmland);

            return goldfieldSquare;
        }
    }
}