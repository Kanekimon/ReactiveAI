using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    protected GameObject Parent;
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected BaseGoal LinkedGoal;
    protected Agent Agent;

    protected HashSet<KeyValuePair<string, object>> preconditions = new HashSet<KeyValuePair<string, object>>();
    protected HashSet<KeyValuePair<string, object>> effects = new HashSet<KeyValuePair<string, object>>();

    private void Awake()
    {
        Parent = transform.parent.gameObject;
        NavAgent = Parent.GetComponent<NavigationAgent>();
        AwarenessSystem = Parent.GetComponent<AwarenessSystem>();
        Agent = Parent.GetComponent<Agent>();
    }

    public virtual List<System.Type> GetSupportedGoals()
    {
        return null;
    }

    public virtual float Cost()
    {
        return 0f;
    }

    public virtual void OnActivated(BaseGoal linked)
    {
        LinkedGoal = linked;
    }

    public virtual void OnDeactived()
    {
        LinkedGoal = null;
    }

    public virtual void OnTick()
    {

    }

    public HashSet<KeyValuePair<string, object>> GetEffects()
    {
        return effects;
    }

    public HashSet<KeyValuePair<string, object>> GetPreconditions()
    {
        return preconditions;
    }
}
