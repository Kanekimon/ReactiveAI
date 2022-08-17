using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner : MonoBehaviour
{
    BaseGoal[] Goals;
    ActionBase[] Actions;

    BaseGoal ActiveGoal;
    ActionBase ActiveAction;

    private void Awake()
    {
        Goals = GetComponents<BaseGoal>();
        Actions = GetComponents<ActionBase>();
    }

    // Update is called once per frame
    void Update()
    {

        BaseGoal bestGoal = null;
        ActionBase bestAction = null;

        foreach(var goal in Goals)
        {
            //Tick goals
            goal.OnTickGoal();


            //can it run
            if (!goal.CanRun())
                continue;

            if (!(bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority()))
               continue;

            ActionBase candidateAction = null;
            foreach(var action in Actions)
            {
                if (!action.GetSupportedGoals().Contains(goal.GetType()))
                    continue;

                if(candidateAction == null || action.Cost() < candidateAction.Cost())
                    candidateAction = action;
            }

            if(candidateAction != null)
            {
                bestGoal = goal;
                bestAction = candidateAction;
            }  
        }

        if(ActiveGoal == null)
        {
          
            ActiveGoal = bestGoal;
            ActiveAction = bestAction;

            if (ActiveGoal != null)
                ActiveGoal.OnGoalActivated(ActiveAction);
            if (ActiveAction != null)
                ActiveAction.OnActivated(ActiveGoal);
        } 
        else if(ActiveGoal == bestGoal)
        {
            if(bestAction != ActiveAction)
            {
                ActiveAction.OnDeactived();
                ActiveAction = bestAction;
                ActiveAction.OnActivated(ActiveGoal);
            }
        }
        else if(ActiveGoal != bestGoal)
        {
            ActiveGoal.OnGoalDeactivated();
            ActiveAction.OnDeactived();

            ActiveGoal = bestGoal;
            ActiveAction = bestAction;

            if (ActiveGoal != null)
                ActiveGoal.OnGoalActivated(ActiveAction);
            if (ActiveAction != null)
                ActiveAction.OnActivated(ActiveGoal);
        }

        if(ActiveAction != null)
            ActiveAction.OnTick();
    }
}
