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
    protected NavigationAgent NavAgent;
    protected AwarenessSystem AwarenessSystem;
    protected GOAPUI DebugUI;
    protected ActionBase LinkedAction;

  
    void Awake()
    {
        NavAgent = GetComponent<NavigationAgent>();
        AwarenessSystem = GetComponent<AwarenessSystem>();
    }

    void Start()
    {
        DebugUI = FindObjectOfType<GOAPUI>();
    }

    void Update()
    {
        OnTickGoal();
        DebugUI.UpdateGoal(this, GetType().Name, LinkedAction ? "Running" : "Paused", CalculatePriority());
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

