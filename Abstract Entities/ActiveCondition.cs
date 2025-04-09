using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;

namespace fire_ash_server.Abstract_Entities
{
    [Serializable]
    internal class ActiveCondition
    {
        public Condition Condition;
        public int Turns;
        public bool CreatedThisTurn;
        
        public ActiveCondition(Condition condition, int turns)
        {
            Condition = condition;
            Turns = turns;
            CreatedThisTurn = true;
        }
    }
}
