using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    protected GameObject Parent;
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected BaseGoal LinkedGoal;
    protected Agent Agent;
    protected StateMemory WorldState;
    protected bool _hasFinished;
    protected bool _needsReplanning = false;


    [SerializeField] protected float _cost;

    protected List<KeyValuePair<string, object>> preconditions = new List<KeyValuePair<string, object>>();
    protected List<KeyValuePair<string, object>> effects = new List<KeyValuePair<string, object>>();


    protected virtual void Awake()
    {
        Parent = transform.parent.parent.gameObject;
        NavAgent = Parent.GetComponent<NavigationAgent>();
        AwarenessSystem = Parent.GetComponent<AwarenessSystem>();
        Agent = Parent.GetComponent<Agent>();
        WorldState = Agent.WorldState;
    }

    protected virtual void Start()
    {
        string name = this.name;

        foreach (KeyValuePair<string, object> kvp in preconditions)
        {
            WorldState.AddWorldState(kvp.Key, ObjectHelper.GetDefault(kvp.Value.GetType() ?? null));
        }
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            WorldState.AddWorldState(kvp.Key, ObjectHelper.GetDefault(kvp.Value.GetType() ?? null));
        }
    }

    public virtual bool CanRun()
    {
        return true;
    }

    public virtual List<System.Type> GetSupportedGoals()
    {
        return null;
    }

    public virtual float Cost()
    {
        return _cost;
    }

    public virtual void OnActivated(BaseGoal linked)
    {
        _needsReplanning = false;
        LinkedGoal = linked;
        _hasFinished = false;
    }

    public virtual void OnDeactived()
    {
        LinkedGoal = null;
        _hasFinished = true;
    }

    public virtual void OnTick()
    {

    }

    public virtual bool NeedsReplanning()
    {
        return _needsReplanning;
    }
    public List<KeyValuePair<string, object>> GetEffects()
    {
        return effects;
    }

    public List<KeyValuePair<string, object>> GetPreconditions()
    {
        return preconditions;
    }

    public List<KeyValuePair<string, object>> GetOpenPreconditions()
    {
        List<KeyValuePair<string, object>> open = new List<KeyValuePair<string, object>>();

        foreach (KeyValuePair<string, object> precon in preconditions)
        {
            object valueInMemory = WorldState.GetValue(precon.Key);
            if (valueInMemory != null)
            {
                if (!valueInMemory.Equals(precon.Value))
                    open.Add(precon);
            }
        }
        return open;
    }

    public float OpenPreconditionCount()
    {
        float count = 0f;
        foreach (KeyValuePair<string, object> precon in preconditions)
        {
            object valueInMemory = WorldState.GetValue(precon.Key);
            if (valueInMemory != null)
            {
                if (!valueInMemory.Equals(precon.Value))
                    count++;
            }
        }
        return count;
    }



    public bool HasFinished()
    {
        return _hasFinished;
    }
}
