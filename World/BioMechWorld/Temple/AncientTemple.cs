using System;
using System.Collections.Generic;
using fire_ash_server.World.BioMechWorld;
using fire_ash_server.Props;
using fire_ash_server.Dialogue;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items;
using System.Net.Sockets;
using static fire_ash_server.Helpers;
using fire_ash_server.Props.Items.Weapons;
using System.Diagnostics.Metrics;
using System.Threading.Channels;
using fire_ash_server.World.BioMechWorld.Temple;

namespace fire_ash_server.World.BioMechWorld
{
    internal class AncientTemple
    {
        public AncientTemple()
        {
            Room room = SerpentsSpine.Create();
        } 
    }
}