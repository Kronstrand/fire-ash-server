using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum RollType
    {
        None,
        Attack,
        Damage,
        [Description("Ability Check")]
        AbilityCheck,
        [Description("SkillCheck")]
        SkillCheck,
        [Description("Saving Throw")]
        SavingThrow,
        Initiative,
        Contested,
        Random,
        Concentration,
        [Description("Dim Light Attack")]
        DimLightAttack
    }
}
