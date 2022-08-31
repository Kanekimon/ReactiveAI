using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlantTreeGoal : BaseGoal
{
    public int Prio;
    public int MinPrio;
    public int MaxPrio;
    public int AmountToPlace;
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("placedObject", true));
        Agent.WorldState.AddWorldState("placeAmount", 0);
        Debug.Log($"Start {Time.time}");
        base.Start();
    }

    public override void OnGoalActivated()
    {
        Agent.WorldState.AddWorldState("itemToPlace", ItemManager.Instance.GetItemByName("plot"));
        base.OnGoalActivated();
    }

    public override int CalculatePriority()
    {
        if (Pause)
            return MinPrio;
        return Prio;
    }

    public override void OnTickGoal()
    {
        AmountToPlace = Agent.WorldState.GetValue<int>("placeAmount");
        if (Agent.WorldState.GetValue<int>("placeAmount") > 0)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }


}

