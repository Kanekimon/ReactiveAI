using System.Collections.Generic;
using System.IO;
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
    StateMemory WorldState;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        WorldState = Agent.WorldState;
        InitActions();
        InitGoals();

    }

    public void WriteAllWorldStatesToFile()
    {
        string states = "";
        foreach (KeyValuePair<string, object> state in WorldState.GetWorldState)
        {
            states += $"{state.Key}" + "\n";
        }

        string file = Path.Combine(@"D:\Unity\Dump", "states.txt");
        using (StreamWriter stW = new StreamWriter(file)) { stW.Write(states); }
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
        WriteAllWorldStatesToFile();
    }


    private void Update()
    {
        BaseGoal HighestPrioGoal = TickGoals();

        if (HighestPrioGoal != null && HighestPrioGoal != CurrentGoal)
        {
            if (CurrentGoal != null)
                DeactiveOldGoal();

            if (HighestPrioGoal is SatisfyConditionGoal)
                Debug.Log("Test");

            CurrentGoal = HighestPrioGoal;
            CurrentGoal.OnGoalActivated();
            Plan();
        }

        if (CurrentActionSequence == null)
        {
            if (CurrentGoal != null)
                DeactiveOldGoal();
            CurrentGoal = null;
            return;
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
        if (CurrentActionSequence != null && CurrentActionSequence.Count > 0)
        {
            CurrentActionSequence.Peek().OnDeactived();
            CurrentActionSequence.Clear();
        }

    }

    void Plan()
    {
        CurrentActionSequence = AStar.PlanActionSequence(CurrentGoal, AllActions, WorldState);
        if (CurrentActionSequence != null && CurrentActionSequence.Count > 0)
            GoapHelper.DebugPlan(this.Agent, CurrentGoal, CurrentActionSequence.ToList());
    }

    public void RequestResource(Item item, int amount)
    {
        WorldState.AddWorldState("requestedItem", item);
        WorldState.AddWorldState("gatherAmount", amount);
    }

    public void RequestCraftedItem(Item item, int amount)
    {
        WorldState.AddWorldState("requestedItem", item);
        WorldState.AddWorldState("gatherAmount", amount);

        if (Agent.InventorySystem.HasItemWithAmount(item, amount))
        {
            WorldState.AddWorldState("hasItem", true);
        }
        else if (Agent.CraftingSystem.HasEnoughToCraft(item))
        {
            WorldState.AddWorldState("hasMaterial", true);
        }
        else
        {
            List<Request> reqs = new List<Request>();
            foreach (KeyValuePair<Item, int> n in Agent.CraftingSystem.GetMissingResources(item))
            {
                reqs.Add(new Request(Agent.gameObject, n.Key, n.Value));
            }
            WorldState.AddWorldState("requests", reqs);
        }

    }
}

