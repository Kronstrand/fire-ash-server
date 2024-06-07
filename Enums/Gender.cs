using System;
using System.ComponentModel;

namespace fire_ash_server.Enums
{
    internal enum Gender
    {
        [Description("Undefined")]
        Undefined,
        [Description("Male")]
        Male,
        [Description("Female")]
        Female,
        [Description("Dual-soul")]
        DualSoul
    }
}
