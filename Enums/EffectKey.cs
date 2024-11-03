using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum EffectKey
    {
        Stealth,
        [Description("Bright Light")]
        BrightLight,
        [Description("Light Pointer")]
        LightPointer,
        [Description("Dim Light")]
        DimLight,
        [Description("Darkvision")]
        Darkvision
    }
}
