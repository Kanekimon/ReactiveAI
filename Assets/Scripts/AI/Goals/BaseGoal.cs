
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
    [SerializeField] protected int Prio;
    [SerializeField] protected int MinPrio;
    [SerializeField] protected int MaxPrio;

    float timer;
    float delay = 10f;

    protected int _id;
    protected GameObject Parent;
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected GOAPUI DebugUI;
    protected ActionBase LinkedAction;
    protected Agent Agent;
    protected StateMemory WorldState;
    [SerializeField] protected bool Pause = false;
    protected StateMemory NeededWorldState;
    [SerializeField] protected bool Runnable = true;


    public int Id { get { return _id; } }

    [SerializeField] public List<KeyValuePair<string, object>> Preconditions = new List<KeyValuePair<string, object>>();

    protected virtual void Awake()
    {
        Parent = transform.parent.gameObject;
        NavAgent = Parent.GetComponent<NavigationAgent>();
        AwarenessSystem = Parent.GetComponent<AwarenessSystem>();
        Agent = Parent.GetComponent<Agent>();
        WorldState = Agent.WorldState;
        NeededWorldState = CreateRequiredWorldState();
    }

    protected virtual void Start()
    {
        _id = this.gameObject.GetHashCode();
        DebugUI = FindObjectOfType<GOAPUI>();

        foreach (KeyValuePair<string, object> state in Preconditions)
        {
            WorldState.AddWorldState(state.Key, ObjectHelper.GetDefault(state.Value.GetType()));
        }
    }

    void Update()
    {
        if (Pause)
        {
            if (timer > delay)
            {
                timer = 0;
                Runnable = true;
                Pause = false;
            }
            timer += Time.deltaTime;
        }
        else
            OnTickGoal();
        //DebugUI.UpdateGoal(this, GetType().Name, LinkedAction ? "Running" : "Paused", CalculatePriority());
    }

    protected StateMemory CreateRequiredWorldState()
    {
        StateMemory currentWorldState = WorldState;

        return currentWorldState;

    }

    public virtual void PauseGoal()
    {
        Pause = true;
        Runnable = false;
    }

    public virtual bool CanRun()
    {
        return Runnable;
    }

    public virtual int CalculatePriority()
    {
        return Prio;
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

