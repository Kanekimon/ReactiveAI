using System.Collections.Generic;
using UnityEngine;

public class PlantTreeGoal : BaseGoal
{
    public int AmountToPlace;
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("placedObject", true));
        WorldState.AddWorldState("placeAmount", 0);
        Debug.Log($"Start {Time.time}");
        base.Start();
    }

    public override void OnGoalActivated()
    {
        WorldState.AddWorldState("itemToPlace", ItemManager.Instance.GetItemByName("plot"));
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
        AmountToPlace = WorldState.GetValue<int>("placeAmount");
        if (WorldState.GetValue<int>("placeAmount") > 0)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }


}

