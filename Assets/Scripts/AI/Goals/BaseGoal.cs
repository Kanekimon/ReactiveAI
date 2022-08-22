using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public interface IGoal
{
    int CalculatePriority();

    bool CanRun();

    void OnTickGoal();

    void OnGoalActivated(ActionBase linkedAction);

    void OnGoalDeactivated();
}

public class BaseGoal : MonoBehaviour, IGoal
{
    protected GameObject Parent;
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected GOAPUI DebugUI;
    protected ActionBase LinkedAction;
    protected Agent Agent;

    protected StateMemory NeededWorldState;

    [SerializeField] public HashSet<KeyValuePair<string, object>> Preconditions = new HashSet<KeyValuePair<string, object>>();
  
    protected virtual void Awake()
    {
        Parent = transform.parent.gameObject;
        NavAgent = Parent.GetComponent<NavigationAgent>();
        AwarenessSystem = Parent.GetComponent<AwarenessSystem>();
        Agent = Parent.GetComponent<Agent>();
        NeededWorldState = CreateRequiredWorldState();
    }

    protected virtual void Start()
    {
        DebugUI = FindObjectOfType<GOAPUI>();

        foreach(KeyValuePair<string, object> state in Preconditions)
        {
            Agent.WorldState.AddWorldState(state.Key, ObjectHelper.GetDefault(state.Value.GetType()));
        }
    }

    void Update()
    {
        OnTickGoal();
        DebugUI.UpdateGoal(this, GetType().Name, LinkedAction ? "Running" : "Paused", CalculatePriority());
    }

    protected StateMemory CreateRequiredWorldState()
    {
        StateMemory currentWorldState = Agent.WorldState;

        return currentWorldState;

    }

    public virtual bool CanRun()
    {
        return false;
    }

    public virtual int CalculatePriority()
    {
        return -1;
    }

    public virtual void OnGoalActivated(ActionBase linkedAction)
    {
        LinkedAction = linkedAction;
    }

    public virtual void OnGoalDeactivated()
    {
        LinkedAction = null;
    }

    public virtual void OnTickGoal()
    {
       
    }
}

