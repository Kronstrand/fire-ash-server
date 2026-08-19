using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Abstract_Entities
{
    internal class BuffDebuff
    {
        public string Name;
        public bool Unique = true;
        public Condition? Condition;
        public int Turns;
        public bool UnlimitedTurn;
        public bool CreatedThisTurn;
        public Effect? Effect;

        public BuffDebuff() { }
        
        public BuffDebuff(string name, int turns, Effect effect)
        {
            Name = name;
            Turns = turns;
            Effect = effect;
            CreatedThisTurn = true;        
        }

        public BuffDebuff(string name, Condition condition, int turns, Effect effect)
        {
            Name = name;
            Condition = condition;
            Turns = turns;
            Effect = effect;
            CreatedThisTurn = true;

        }

        public BuffDebuff(string name, Condition condition, int turns)
        {
            Name = name;
            Condition = condition;
            Turns = turns;
            CreatedThisTurn = true;
        }
    }
}
