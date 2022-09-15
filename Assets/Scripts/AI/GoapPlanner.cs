using System;
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
        ActionContainer = transform.Find("Actions").gameObject;
        AllActions = new List<ActionBase>();

        foreach (Transform child in ActionContainer.transform)
        {
            AllActions.AddRange(child.gameObject.GetComponents<ActionBase>().ToList());
        }
    }
    public void InitGoals()
    {
        GoalContainer = null;
        AllGoals = new List<BaseGoal>();

        GoalContainer = transform.Find("Goals").gameObject;
        AllGoals = new List<BaseGoal>();

        foreach(Transform child in GoalContainer.transform)
        {
            AllGoals.AddRange(child.gameObject.GetComponents<BaseGoal>().ToList());
        }

    }

    internal void FollowPlayer(bool state)
    {
        WorldState.AddWorldState("followPlayer", state);
    }

    private void Start()
    {
        WriteAllWorldStatesToFile();
    }


    private void Update()
    {
        Think();
    }

    public void Think()
    {
        if (CurrentGoal != null && CurrentGoal.IsPaused)
            CurrentGoal = null;

        BaseGoal HighestPrioGoal = TickGoals();

        if (HighestPrioGoal != null && HighestPrioGoal != CurrentGoal)
        {
            if (CurrentGoal != null)
                DeactiveOldGoal();

            if (HighestPrioGoal is CollectJobResourceGoal)
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

            if (!goal.CanRun() || goal.IsPaused)
                continue;

            if (bestGoal is RequestResourceGoal && Agent.Job != null && Agent.Job.JobType == JobType.Crafter)
                Debug.Log("Test");


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

    public void AddRequestToWorkOn(Request r)
    {
        WorldState.AddWorldState("activeRequest", r);
    }

    public void AddCraftingRequestToWorkOn(Request r)
    {
        WorldState.AddWorldState("activeRequest", r);


        if (Agent.InventorySystem.HasItemWithAmount(r.RequestedItem, r.RequestedAmount))
        {
            WorldState.AddWorldState("hasItem", true);
        }
        else if (Agent.CraftingSystem.HasEnoughToCraft(r.RequestedItem))
        {
            WorldState.AddWorldState("hasMaterial", true);
        }
        else
        {
            List<Request> reqs = new List<Request>();
            foreach (KeyValuePair<Item, int> n in Agent.CraftingSystem.GetMissingResources(r.RequestedItem))
            {
                reqs.Add(new Request(Agent.gameObject, n.Key, n.Value));
            }
            WorldState.AddWorldState("requests", reqs);
        }

    }

    public void InteractWithPlayer()
    {
        WorldState.AddWorldState("playerWantsInteraction", true);
        Agent.GetComponent<NavigationAgent>().StopMoving();
        Think();
    }

    public void StopInteracting()
    {
        WorldState.AddWorldState("playerWantsInteraction", false);
        Think();
    }
}

