using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum FactionKey
    {
        Players,
        Wilders,
        [Description("Light Shades")]
        LightShades,
        Corporates,
        Resistance,
        Technomancers
    }
}
