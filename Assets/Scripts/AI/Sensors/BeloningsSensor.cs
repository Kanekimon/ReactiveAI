using System;
using UnityEngine;

[RequireComponent(typeof(InventorySystem))]
public class BeloningsSensor : MonoBehaviour
{
    Agent Agent;
    InventorySystem InventorySystem;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        InventorySystem = GetComponent<InventorySystem>();
    }


    private void Start()
    {
        Agent.WorldState.AddWorldState("hasFood", false);
    }

    private void Update()
    {
        Agent.WorldState.ChangeValue("hasFood", InventorySystem.HasItemWithType(ItemType.Food));
        if (Agent.WorldState.GetValue("resourceToGather") != null && !string.IsNullOrEmpty(Agent.WorldState.GetValue("resourceToGather").ToString()))
        {
            int amount = int.Parse(Agent.WorldState.GetValue("gatherAmount").ToString());
            Item item = ItemManager.Instance.GetItemByName(Agent.WorldState.GetValue("resourceToGather").ToString().ToLower());
            if(item != null)
            {
                Agent.WorldState.AddWorldState("hasResource", (InventorySystem.HasEnough(item, amount)));
            }
        }
        else
            Agent.WorldState.AddWorldState("hasResource", false);
    }

}
