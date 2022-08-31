using System;
using System.Collections.Generic;
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
        if (Agent.WorldState.GetValue("requestedItem") != null && !string.IsNullOrEmpty(Agent.WorldState.GetValue("requestedItem").ToString()))
        {
            int amount = int.Parse(Agent.WorldState.GetValue("gatherAmount").ToString());
            Item requested = Agent.WorldState.GetValue("requestedItem") as Item;
            bool hasItem =  InventorySystem.HasEnough(requested, amount);

            if(!hasItem && Agent.JobType == JobType.Crafter && requested.HasRecipe)
            {
                Agent.WorldState.AddWorldState("hasMaterials", Agent.CraftingSystem.HasEnoughToCraft(requested));
            }

            Agent.WorldState.AddWorldState("hasItem", hasItem);
        }
        else
            Agent.WorldState.AddWorldState("hasItem", false);
    }

}
