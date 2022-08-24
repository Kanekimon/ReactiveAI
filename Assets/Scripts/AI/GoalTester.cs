using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GoalTester : MonoBehaviour
{

    Agent Agent;
    GoapPlanner GoapPlanner;
    public List<BaseGoal> Goals;

    private void Awake()
    {
       Agent = GetComponent<Agent>();
        GoapPlanner = GetComponent<GoapPlanner>();
    }

    private void Start()
    {
        Goals = GoapPlanner.GetAllGoals();
    }


    public void CreatePlan(BaseGoal goal)
    {
        Debug.Log($"Goal: {goal.GetType().ToString()}");
        Queue<ActionBase> actionSequence = AStar.PlanActionSequence(goal, GoapPlanner.GetAllActions());

        List<ActionBase> actions = actionSequence.ToList();
        string plan_sequence = $"Goal {goal.GetType().ToString()} -> ";

        for(int i = actions.Count-1; i >= 0; i--)
        {
            string name = actions[i].GetType().ToString();
            if (i == 0)
                plan_sequence += ($"{name}");
            else
                plan_sequence += ($"{name} -> ");
        }


        Debug.Log(plan_sequence);

    }
}

