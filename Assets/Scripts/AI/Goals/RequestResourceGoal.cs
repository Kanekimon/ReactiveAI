using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RequestResourceGoal : BaseGoal
{
    [SerializeField] int Priority = 10;
    [SerializeField] int MinPrio = 1;
    [SerializeField] int MaxPrio = 100;
    [SerializeField] public int RequestAmount;
    [SerializeField] Item _requestedItem;

    public Item RequestedItem => _requestedItem;
    public bool waiting;



    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("requestResource", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        Agent.WorldState.AddWorldState("target", Agent.HomeTown.RequestBoard);
        Agent.WorldState.AddWorldState("requestAmount", RequestAmount);
        Agent.WorldState.AddWorldState("requestItem", _requestedItem);
        base.OnGoalActivated();
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override void OnGoalDeactivated()
    {
        RequestAmount = 0;
        Agent.WorldState.AddWorldState("deliveredResource", false);
        Agent.WorldState.RemoveWorldState("requestAmount");
        Agent.WorldState.RemoveWorldState("requestItem");
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if(RequestAmount > 0)
        {
            Priority = MaxPrio;//waiting ? (MinPrio + MaxPrio)/2 : MaxPrio;
        }
        else
        {
            Priority = MinPrio;
        }

    }
}

