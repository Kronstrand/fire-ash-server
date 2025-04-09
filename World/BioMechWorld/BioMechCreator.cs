using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using fire_ash_server.Props;
using fire_ash_server.Moves;
using fire_ash_server.Props.Items.Weapons;
using static fire_ash_server.Helpers;
using fire_ash_server.World.BioMechWorld.Complex;

namespace fire_ash_server.World.BioMechWorld
{
    internal class BioMechCreator
    {
        public CreationChamber startingRoom;
        public BioMechCreator()
        {
            startingRoom = new CreationChamber();
        }

        public static void SetFactions()
        {
            Relationship.Set(FactionKey.Players, FactionKey.Technomancers, 13);
            Relationship.Set(FactionKey.Technomancers, FactionKey.TechnomancersDefenceSystem, 18);
            Relationship.Set(FactionKey.Players, FactionKey.TechnomancersDefenceSystem, 6);
            Relationship.Set(FactionKey.Players, FactionKey.Wilders, -1);
        }
    }
}
