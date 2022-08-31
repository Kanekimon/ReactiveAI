using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoapPlanner : MonoBehaviour
{
    Queue<ActionBase> CurrentActionSequence { get; set; } = new Queue<ActionBase>();
    [SerializeField] ActionBase CurrentAction;
    [SerializeField] BaseGoal CurrentGoal;


    GameObject ActionContainer;
    GameObject GoalContainer;

    List<ActionBase> AllActions;
    List<BaseGoal> AllGoals;
    Agent Agent;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        InitActions();
        InitGoals();
    }


    public void InitActions()
    {
        ActionContainer = transform.Find("ActionContainer").gameObject;
        AllActions = ActionContainer.GetComponents<ActionBase>().ToList();
    }
    public void InitGoals()
    {
        GoalContainer = null;
        AllGoals = new List<BaseGoal>();

        GoalContainer = transform.Find("GoalContainer").gameObject;
        AllGoals = GoalContainer.GetComponents<BaseGoal>().ToList();
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
            CurrentGoal.OnGoalActivated();
            Plan();
        }
        try
        {
            if (CurrentActionSequence.Count > 0 && (CurrentAction == null || CurrentAction != CurrentActionSequence.Peek()))
            {
                CurrentAction = CurrentActionSequence.Peek();
                CurrentAction.OnActivated(CurrentGoal);
            }
        } catch(System.Exception ex)
        {
            Debug.Log("Test");
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
                if (CurrentAction.NeedsReplanning())
                    Plan();
            }
        }

        if (CurrentAction == null)
        {
            DeactiveOldGoal();
            Plan();
        }



    }

    BaseGoal TickGoals()
    {
        BaseGoal bestGoal = null;
        foreach (BaseGoal goal in AllGoals)
        {
            goal.OnTickGoal();

            if (!goal.CanRun())
                continue;

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
        if (CurrentActionSequence != null && CurrentActionSequence.Count > 0)
            GoapHelper.DebugPlan(this.Agent, CurrentGoal, CurrentActionSequence.ToList());
    }

    public void RequestResource(Item item, int amount)
    {
        Agent.WorldState.AddWorldState("requestedItem", item);
        Agent.WorldState.AddWorldState("gatherAmount", amount);
    }

    public void RequestCraftedItem(Item item, int amount)
    {
        Agent.WorldState.AddWorldState("requestedItem", item);
        Agent.WorldState.AddWorldState("gatherAmount", amount);

        if (Agent.InventorySystem.HasEnough(item, amount))
        {
            Agent.WorldState.AddWorldState("hasItem", true);
        }
        else if (Agent.CraftingSystem.HasEnoughToCraft(item))
        {
            Agent.WorldState.AddWorldState("hasMaterial", true);
        }
        else
        {
            List<Request> reqs = new List<Request>();
            foreach(KeyValuePair<Item, int> n in Agent.CraftingSystem.GetMissingResources(item))
            {
                reqs.Add(new Request(Agent.gameObject, n.Key, n.Value));
            }
            Agent.WorldState.AddWorldState("requests", reqs);
        }

    }
}

