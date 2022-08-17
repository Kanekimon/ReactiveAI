using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBase : MonoBehaviour
{

    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected BaseGoal LinkedGoal;

    private void Awake()
    {
        NavAgent = GetComponent<NavigationAgent>();
        AwarenessSystem = GetComponent<AwarenessSystem>();
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
}
