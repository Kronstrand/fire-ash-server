using System;
using System.Collections.Generic;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.World.BioMechWorld.Complex;
using static fire_ash_server.Helpers;

namespace fire_ash_server.World.BioMechWorld
{
    internal class CyberTempleCreator
    {
        public Room startingRoom;
        public CyberTempleCreator()
        {
            startingRoom = ThreshholdOfTheNameless.Create();
        }

        public void SetFactions()
        {
            //Relationship.Set(FactionKey.Players, FactionKey.Technomancers, 13);
            //Relationship.Set(FactionKey.Technomancers, FactionKey.TechnomancersDefenceSystem, 18);
            //Relationship.Set(FactionKey.Players, FactionKey.TechnomancersDefenceSystem, 6);
            //Relationship.Set(FactionKey.Players, FactionKey.Wilders, -1);
        }
    }
}