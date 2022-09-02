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
        WorldState.AddWorldState("itemToPlace", ItemManager.Instance.GetItemByName("sapling"));
        WorldState.AddWorldState("placeAmount", Agent.InventorySystem.GetItemAmount(ItemManager.Instance.GetItemByName("sapling")));
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
        if (Agent.InventorySystem.HasItem("sapling"))
        {
            Prio = (int)(MaxPrio * 0.1f) * Agent.InventorySystem.GetItemAmount(ItemManager.Instance.GetItemByName("sapling"));
        }
        else
            Prio = MinPrio;
    }


}

