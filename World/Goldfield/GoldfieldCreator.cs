using System;
using System.Collections.Generic;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.World.BioMechWorld.Complex;
using System.Text.Json;
using static fire_ash_server.Helpers;
using System.Text.Json.Serialization;

namespace fire_ash_server.World.Goldfield
{
    internal class GoldfieldCreator
    {
        public static Room Create(bool initProps)
        {
            
            return GoldfieldSquare.Create(initProps);
        }

        public static void SetFactions()
        {
            //Relationship.Set(FactionKey.Players, FactionKey.Technomancers, 13);
            //Relationship.Set(FactionKey.Technomancers, FactionKey.TechnomancersDefenceSystem, 18);
            //Relationship.Set(FactionKey.Players, FactionKey.TechnomancersDefenceSystem, 6);
            //Relationship.Set(FactionKey.Players, FactionKey.Wilders, -1);
        }
    }
}