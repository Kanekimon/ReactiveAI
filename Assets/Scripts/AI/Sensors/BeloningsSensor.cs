using UnityEngine;

[RequireComponent(typeof(InventorySystem))]
public class BeloningsSensor : MonoBehaviour
{
    Agent Agent;
    InventorySystem InventorySystem;
    StateMemory WorldState;
    bool InventoryHasSpace = true;
    private void Awake()
    {
        Agent = GetComponent<Agent>();
        WorldState = Agent.WorldState;
        InventorySystem = GetComponent<InventorySystem>();
    }


    private void Start()
    {
        WorldState.AddWorldState("hasFood", false);
    }

    private void Update()
    {
        if (InventoryHasSpace && InventorySystem.GetFreeSpaceSize() == 0)
        {
            InventoryHasSpace = false;
            WorldState.AddWorldState("inventoryFull", true);
        }
        else if (!InventoryHasSpace && InventorySystem.GetFreeSpaceSize() > 0)
        {
            InventoryHasSpace = true;
            WorldState.AddWorldState("inventoryFull", false);
        }

        WorldState.ChangeValue("hasFood", InventorySystem.HasItemWithType(ItemType.Food));
        if (WorldState.GetValue<Item>("requestedItem") != null && WorldState.GetValue<int>("gatherAmount") != null)
        {
            int amount = WorldState.GetValue<int>("gatherAmount");
            Item requested = WorldState.GetValue<Item>("requestedItem");
            bool hasItem = InventorySystem.HasEnough(requested, amount);

            if (!hasItem && Agent.JobType == JobType.Crafter && requested.HasRecipe)
            {
                WorldState.AddWorldState("hasMaterials", Agent.CraftingSystem.HasEnoughToCraft(requested));
            }

            WorldState.AddWorldState("hasItem", hasItem);
        }
        else
            WorldState.AddWorldState("hasItem", false);
    }

}
