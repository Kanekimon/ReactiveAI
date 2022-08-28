using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GoapHelper
{
    public static void DebugPlan(Agent agent, BaseGoal goal, List<ActionBase> actions)
    {
        string plan_sequence = $"Goal {goal.GetType().ToString()} -> ";

        for (int i = actions.Count - 1; i >= 0; i--)
        {
            string name = actions[i].GetType().ToString();
            if (i == 0)
                plan_sequence += ($"{name}");
            else
                plan_sequence += ($"{name} -> ");
        }


        Debug.Log($"{agent.name}: {plan_sequence}");
    }

}
