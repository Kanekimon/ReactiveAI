using System.Linq;
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

        WorldState.AddWorldState("inventoryFull", (InventorySystem.GetNumberOfFreeSlots() == 0 || InventorySystem.CurrentWeight >= InventorySystem.MaximumWeight - 3));
        //if (InventoryHasSpace && InventorySystem.GetNumberOfFreeSlots() == 0 || InventorySystem.CurrentWeight == InventorySystem.MaximumWeight-3)
        //{
        //    InventoryHasSpace = false;
        //    WorldState.AddWorldState("inventoryFull", true);
        //}
        //else if (!InventoryHasSpace && (InventorySystem.GetNumberOfFreeSlots() > 0 && InventorySystem.GetWeightLeftUntilFull() > 3))
        //{
        //    InventoryHasSpace = true;
        //    WorldState.AddWorldState("inventoryFull", false);
        //}

        WorldState.ChangeValue("hasFood", InventorySystem.HasItemWithType(ItemType.Food));
        if (WorldState.GetValue<Request>("activeRequest") != null)
        {
            Request r = WorldState.GetValue<Request>("activeRequest");
            int amount = r.RequestedAmount;
            Item requested = r.RequestedItem;
            bool hasItem = InventorySystem.HasItemWithAmount(requested, amount);

            if (!hasItem && Agent.Job.JobType == JobType.Crafter && requested.HasRecipe)
            {
                WorldState.AddWorldState("hasMaterials", Agent.CraftingSystem.HasEnoughToCraft(requested));
            }

            WorldState.AddWorldState("hasItem", hasItem);
        }
        else
            WorldState.AddWorldState("hasItem", false);


        if (Agent.Job != null)
        {
            WorldState.AddWorldState("hasTool", Agent.Job.Tools.Any(a => InventorySystem.HasItem(a)));
        }
    }

}
