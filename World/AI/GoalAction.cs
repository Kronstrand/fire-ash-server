using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Moves;
using fire_ash_server.Props;

namespace fire_ash_server.World.AI
{
    internal class GoalAction
    {
        public Func<Character, BehaviorResult> Action;
        public Goal? OnCompleted;
        public Goal? OnCantComplete;
        public bool PopOnCantComplete;

        public GoalAction(Func<Character, BehaviorResult> action, Goal? onCompleted, Goal? onCantComplete, bool popOnCantComplete)
        {
            Action = action;
            OnCompleted = onCompleted;
            OnCantComplete = onCantComplete;
            PopOnCantComplete = popOnCantComplete;
        }
    }
}