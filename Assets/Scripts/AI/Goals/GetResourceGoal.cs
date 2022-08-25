using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GetResourceGoal : BaseGoal
{

    [SerializeField] int Priority = 10;
    [SerializeField] int MinPrio = 1;
    [SerializeField] int MaxPrio = 100;
    [SerializeField] int GatherAmount = 100;



    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("interactWithResource", true));
        
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        Agent.WorldState.AddWorldState("gatherAmount", GatherAmount);
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {
        if (Agent.InventorySystem.HasEnough("stone", GatherAmount))
            Priority = MinPrio;
        else
            Priority = MaxPrio;

    }
}
