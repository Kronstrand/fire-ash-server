using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server
{
    internal class SkillNumber
    {
        public Skill Skill { get; set; }
        public Ability Ability { get; set; }
        public int number {  get; set; }

        public SkillNumber(Skill skill, int number) 
        {
            Skill = skill;
            Ability = GetRelatedAbility(skill);
            this.number = number;
        }

        public static Ability GetRelatedAbility(Skill skill)
        {
            switch (skill)
            {
                //ablilities
                case Skill.Strength: return Ability.Strength;
                case Skill.Dexterity: return Ability.Dexterity;
                case Skill.Constitution: return Ability.Constitution;
                case Skill.Intelligence: return Ability.Intelligence;
                case Skill.Wisdom: return Ability.Wisdom;
                case Skill.Charisma: return Ability.Charisma;
                //skills
                case Skill.Acrobatics: return Ability.Strength;
                case Skill.AnimalHandling: return Ability.Wisdom;
                case Skill.Arcana: return Ability.Intelligence;
                case Skill.Athletics: return Ability.Strength;
                case Skill.Deception: return Ability.Charisma;
                case Skill.History: return Ability.Intelligence;
                case Skill.Insight: return Ability.Wisdom;
                case Skill.Intimidation: return Ability.Charisma;
                case Skill.Investigation: return Ability.Intelligence;
                case Skill.Medicine: return Ability.Wisdom;
                case Skill.Nature: return Ability.Intelligence;
                case Skill.Perception: return Ability.Wisdom;
                case Skill.Performance: return Ability.Charisma;
                case Skill.Persuasion: return Ability.Charisma;
                case Skill.Religion: return Ability.Intelligence;
                case Skill.SleightOfHand: return Ability.Dexterity;
                case Skill.Stealth: return Ability.Dexterity;
                case Skill.Survival: return Ability.Wisdom;
                case Skill.CloseCombat: return Ability.Strength;
                case Skill.RangedCombat: return Ability.Dexterity;
                case Skill.Hacking: return Ability.Intelligence;
            }
            throw new Exception($"Ability not found for {skill}.");
        }
    }
}
