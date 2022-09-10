using System.Collections.Generic;
using UnityEngine;

public class DeliverItemGoal : BaseGoal
{
    [SerializeField] Request workingOn;

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("deliveredItem", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        workingOn = WorldState.GetValue<Request>("activeRequest");
    }


    protected override void Update()
    {
        if (Pause)
        {
            if (WorldState.GetValue<bool>("hasItem"))
            {
                Runnable = true;
                Pause = false;
            }
        }
        else
            OnTickGoal();
    }


    public override int CalculatePriority()
    {
        return Prio;
    }

    public override void OnGoalDeactivated()
    {
        if (WorldState.GetValue<bool>("deliveredItem"))
        {
            WorldState.AddWorldState("activeRequest", null);
        }
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (WorldState.GetValue<Request>("activeRequest") == null)
            Prio = MinPrio;
        else
            Prio = MaxPrio;

    }
}

