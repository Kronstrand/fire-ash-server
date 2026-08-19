using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.World.AI;

namespace fire_ash_server.World
{
    internal static class Behavior
    {
        public static Dictionary<BehaviorKey, Action<Character>> behaviors = new();
        public static Dictionary<BehaviorKey, Dictionary<Goal, GoalAction>> goalActionsByBehaviorKey = new();


        static public void InitBehavior()
        {
            behaviors.TryAdd(BehaviorKey.CaveWolf, UpdateCaveWolf);
            InitWolfCafeGoalAction();
        }

        static private void InitWolfCafeGoalAction()
        {
            if (!goalActionsByBehaviorKey.ContainsKey(BehaviorKey.CaveWolf))
            {
                GoalAction caveWolfRest = new GoalAction((c) => c.AI_MoveToDarkSpot(), null, Goal.LeaveRoom, false);
                GoalAction caveWolfhuntPray = new GoalAction((c) => c.AI_Prey(), Goal.Rest, Goal.LeaveRoom, false);
                GoalAction caveWolfleaveRoom = new GoalAction((c) => c.AI_ExitRoom(), null, null, true);

                Dictionary<Goal, GoalAction> goalActions = new();
                goalActions.TryAdd(Goal.Rest, caveWolfRest);
                goalActions.TryAdd(Goal.HuntPrey, caveWolfhuntPray);
                goalActions.TryAdd(Goal.LeaveRoom, caveWolfleaveRoom);

                goalActionsByBehaviorKey.Add(BehaviorKey.CaveWolf, goalActions);
            }
        }
        static private void UpdateCaveWolf(Character character)
        {
            if (character.LastAte.AddMinutes(1) <= DateTime.UtcNow)
            {
                if (!character.Goals.Contains(Goal.HuntPrey))
                    character.Goals.Push(Goal.HuntPrey);

            }

            character.ExecuteAIGoalAndTryEnambleCombat();
        }

    }
}