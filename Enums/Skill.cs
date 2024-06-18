using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum Skill
    {
        Strength,
        Dexterity,
        Constitution,
        Wisdom,
        Intelligence,
        Charisma,
        Acrobatics,
        [Description("Animal Handling")]
        AnimalHandling,
        Arcana,
        Athletics,
        Deception,
        History,
        Insight,
        Intimidation,
        Investigation,
        Medicine,
        Nature,
        Perception,
        Performance,
        Persuasion,
        Religion,
        [Description("Sleight of Hand")]
        SleightOfHand,
        Stealth,
        Survival,
        [Description("Close Combat")]
        CloseCombat,
        [Description("Ranged Combat")]
        RangedCombat,
        Hacking
    }
}
