using Assets.Scripts.AI;
using System.Collections.Generic;
using UnityEngine;


public interface IGoal
{
    int CalculatePriority();

    bool CanRun();

    void OnTickGoal();

    void OnGoalActivated();

    void OnGoalDeactivated();
}

public class BaseGoal : MonoBehaviour, IGoal
{

    protected int _id;
    protected GameObject Parent;
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected GOAPUI DebugUI;
    protected ActionBase LinkedAction;
    protected Agent Agent;

    protected StateMemory NeededWorldState;

    public int Id { get { return _id; } }

    [SerializeField] public List<KeyValuePair<string, object>> Preconditions = new List<KeyValuePair<string, object>>();

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
        _id = this.gameObject.GetHashCode();
        DebugUI = FindObjectOfType<GOAPUI>();

        foreach (KeyValuePair<string, object> state in Preconditions)
        {
            Agent.WorldState.AddWorldState(state.Key, ObjectHelper.GetDefault(state.Value.GetType()));
        }
    }

    void Update()
    {
        OnTickGoal();
        //DebugUI.UpdateGoal(this, GetType().Name, LinkedAction ? "Running" : "Paused", CalculatePriority());
    }

    protected StateMemory CreateRequiredWorldState()
    {
        StateMemory currentWorldState = Agent.WorldState;

        return currentWorldState;

    }

    public virtual bool CanRun()
    {
        return true;
    }

    public virtual int CalculatePriority()
    {
        return -1;
    }

    public virtual void OnGoalActivated()
    {

    }

    public virtual void OnGoalDeactivated()
    {
        LinkedAction = null;
    }

    public virtual void OnTickGoal()
    {

    }
}

