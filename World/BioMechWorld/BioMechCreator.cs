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

namespace fire_ash_server.World.BioMechWorld
{
    internal class BioMechCreator
    {
        public BioMechCreator()
        {
            Relationship.Set(FactionKey.Players, FactionKey.Technomancers, 13);
            Relationship.Set(FactionKey.Technomancers, FactionKey.TechnomancersDefenceSystem, 18);
            Relationship.Set(FactionKey.Players, FactionKey.TechnomancersDefenceSystem, 6);

            new CreationChamber();
        }
    }
}
