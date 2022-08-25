using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoapPlanner : MonoBehaviour
{
    Queue<ActionBase> CurrentActionSequence { get; set; } = new Queue<ActionBase>();
    ActionBase CurrentAction;
    BaseGoal CurrentGoal;


    GameObject ActionContainer;
    GameObject GoalContainer;

    List<ActionBase> AllActions;
    List<BaseGoal> AllGoals;
    Agent Agent;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        ActionContainer = transform.Find("ActionContainer").gameObject;
        GoalContainer = transform.Find("GoalContainer").gameObject;

        AllActions = GetAllActions();
        AllGoals = GetAllGoals();
    }

    public List<ActionBase> GetAllActions()
    {
        return ActionContainer.GetComponents<ActionBase>().ToList();
    }
    public List<BaseGoal> GetAllGoals()
    {
        return GoalContainer.GetComponents<BaseGoal>().ToList();
    }


    private void Start()
    {

    }


    private void Update()
    {
        BaseGoal HighestPrioGoal = TickGoals();

        if (HighestPrioGoal != null && HighestPrioGoal != CurrentGoal)
        {
            if (CurrentGoal != null)
                DeactiveOldGoal();

            CurrentGoal = HighestPrioGoal;
            Plan();
            CurrentGoal.OnGoalActivated();
        }

        if (CurrentActionSequence.Count > 0 && (CurrentAction == null || CurrentAction != CurrentActionSequence.Peek()))
        {
            CurrentAction = CurrentActionSequence.Peek();
            CurrentAction.OnActivated(CurrentGoal);
        }

        if (CurrentAction != null)
        {
            if (CurrentAction.HasFinished())
            {
                CurrentActionSequence.Dequeue();
                if (CurrentActionSequence.Count > 0)
                    CurrentActionSequence.Peek().OnActivated(CurrentGoal);
                else
                    CurrentAction = null;
            }
            else
            {
                CurrentAction.OnTick();
            }
        }

        if (CurrentAction == null)
            Plan();


    }

    BaseGoal TickGoals()
    {
        BaseGoal bestGoal = null;
        foreach(BaseGoal goal in AllGoals)
        {
            goal.OnTickGoal();

            if ((CurrentGoal == null || goal.CalculatePriority() > CurrentGoal.CalculatePriority()) && (bestGoal != null && bestGoal.CalculatePriority() < goal.CalculatePriority() || bestGoal == null))
                bestGoal = goal;
        }

        return bestGoal;
    }

    void DeactiveOldGoal()
    {
        CurrentGoal.OnGoalDeactivated();
        if (CurrentActionSequence.Count > 0)
        {
            CurrentActionSequence.Peek().OnDeactived();
        }
        CurrentActionSequence.Clear();
    }

    void Plan()
    {

        CurrentActionSequence = AStar.PlanActionSequence(CurrentGoal, AllActions);
        if(CurrentActionSequence != null && CurrentActionSequence.Count > 0)
            GoapHelper.DebugPlan(CurrentGoal, CurrentActionSequence.ToList());
    }





}

