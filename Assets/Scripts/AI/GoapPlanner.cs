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

        foreach (Transform child in GoalContainer.transform)
        {
            AllGoals.AddRange(child.gameObject.GetComponents<BaseGoal>().ToList());
        }

    }

    internal void FollowPlayer(bool state)
    {
        WorldState.AddWorldState("followPlayer", state);
    }


    private void Update()
    {
        Think();
    }

    void Think()
    {
        BaseGoal HighestGoal = TickGoalNew();

        if ((CurrentGoal == null && HighestGoal != null) || (CurrentGoal != null && CurrentGoal != HighestGoal))
        {
            ActivateNewGoal(HighestGoal);
        }

        TickActionSequence();
    }

    void TickActionSequence()
    {
        if (CurrentActionSequence != null)
        {
            if (CurrentActionSequence.Count == 0 && CurrentAction == null)
            {
                Debug.Log("No more actions and CurrentAction is null");
                ResetCurrentPlan();
                return;
            }
            HandleAction();
        }
    }

    void HandleAction()
    {
        if (CurrentAction == null && CurrentActionSequence.Count > 0)
        {
            CurrentAction = CurrentActionSequence.Peek();
            CurrentAction.OnActivated(CurrentGoal);
        }
        else if (CurrentAction != null)
        {
            if (CurrentAction.HasFinished())
            {
                CurrentActionSequence.Dequeue();
                CurrentAction = null;
            }
            else if (CurrentAction.NeedsReplanning())
            {
                ResetCurrentPlan();
            }
            else
            {
                CurrentAction.OnTick();
            }
        }
    }


    void ResetCurrentPlan()
    {
        CurrentAction = null;
        CurrentActionSequence = null;
        CurrentGoal = null;
    }

    void ActivateNewGoal(BaseGoal newGoal)
    {
        if (CurrentGoal != null)
            CurrentGoal.OnGoalDeactivated();
        if (CurrentAction != null)
        {
            CurrentAction.OnDeactived();
            CurrentAction = null;
        }

        CurrentGoal = newGoal;
        CurrentGoal.OnGoalActivated();
        Plan();
    }


    BaseGoal TickGoalNew()
    {
        BaseGoal bestGoal = null;
        int highest = -1;
        foreach (BaseGoal goal in AllGoals)
        {
            goal.OnTickGoal();

            if (!goal.CanRun() || goal.IsPaused)
                continue;

            if(bestGoal == null)
            {
                bestGoal = goal;
                highest = bestGoal.CalculatePriority();
            }

            int tmpPrio = goal.CalculatePriority();
            if(bestGoal != null && highest < tmpPrio)
            {
                bestGoal = goal;
                highest = tmpPrio;
            }
        }

        return bestGoal;
    }

    void Plan()
    {
        CurrentActionSequence = AStar.PlanActionSequence(CurrentGoal, AllActions, WorldState);
        if (CurrentActionSequence != null && CurrentActionSequence.Count > 0)
            GoapHelper.DebugPlan(this.Agent, CurrentGoal, CurrentActionSequence.ToList());
        if(CurrentActionSequence == null)
        {
            CurrentGoal.PauseGoal();
            ResetCurrentPlan();
        }
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

