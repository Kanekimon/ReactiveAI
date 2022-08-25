using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonolithPlanner : MonoBehaviour
{

    BaseGoal ActiveGoal;
    ActionBase ActiveAction;

    GameObject ActionContainer;
    GameObject GoalContainer;

    List<ActionBase> AllActions;
    List<BaseGoal> AllGoals;


    private void Awake()
    {
        ActionContainer = transform.Find("ActionContainer").gameObject;
        GoalContainer = transform.Find("GoalContainer").gameObject;

        AllActions = GetAllActions();
        AllGoals = GetAllGoals();
    }


    List<ActionBase> GetAllActions()
    {
        return ActionContainer.GetComponents<ActionBase>().ToList();
    }
    List<BaseGoal> GetAllGoals()
    {
        return GoalContainer.GetComponents<BaseGoal>().ToList();
    }


    // Update is called once per frame
    void Update()
    {

        BaseGoal bestGoal = null;
        ActionBase bestAction = null;

        foreach (var goal in AllGoals)
        {
            //Tick goals
            goal.OnTickGoal();


            //can it run
            if (!goal.CanRun())
                continue;

            if (!(bestGoal == null || goal.CalculatePriority() > bestGoal.CalculatePriority()))
                continue;

            ActionBase candidateAction = null;
            foreach (var action in AllActions)
            {
                if (!action.GetSupportedGoals().Contains(goal.GetType()))
                    continue;

                if (candidateAction == null || action.Cost() < candidateAction.Cost())
                    candidateAction = action;
            }

            if (candidateAction != null)
            {
                bestGoal = goal;
                bestAction = candidateAction;
            }
        }

        if (ActiveGoal == null)
        {

            ActiveGoal = bestGoal;
            ActiveAction = bestAction;

            if (ActiveGoal != null)
                ActiveGoal.OnGoalActivated();
            if (ActiveAction != null)
                ActiveAction.OnActivated(ActiveGoal);
        }
        else if (ActiveGoal == bestGoal)
        {
            if (bestAction != ActiveAction)
            {
                ActiveAction.OnDeactived();
                ActiveAction = bestAction;
                ActiveAction.OnActivated(ActiveGoal);
            }
        }
        else if (ActiveGoal != bestGoal)
        {
            ActiveGoal.OnGoalDeactivated();
            ActiveAction.OnDeactived();

            ActiveGoal = bestGoal;
            ActiveAction = bestAction;

            if (ActiveGoal != null)
                ActiveGoal.OnGoalActivated();
            if (ActiveAction != null)
                ActiveAction.OnActivated(ActiveGoal);
        }

        if (ActiveAction != null)
            ActiveAction.OnTick();
    }


    void MonolithicPlanner()
    {

    }
}
