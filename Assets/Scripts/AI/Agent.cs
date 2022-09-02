using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(AwarenessSystem))]
public class Agent : MonoBehaviour
{
    public Queue<Request> ReadyForPickUp = new Queue<Request>();
    public Job Job;

    [SerializeField] private float _interactionRange = 5;
    public float InteractionRange => _interactionRange;
    private StateMemory _memory = new StateMemory();

    #region Systems
    AwarenessSystem _awareness;
    ConditionSystem _conditionSystem;
    InventorySystem _inventorySystem;
    CraftingSystem _craftingSystem;
    GoapPlanner _goapPlanner;

    public ConditionSystem ConditionSystem => _conditionSystem;
    public AwarenessSystem AwarenessSystem => _awareness;
    public InventorySystem InventorySystem => _inventorySystem;
    public CraftingSystem CraftingSystem => _craftingSystem;
    public StateMemory WorldState => _memory;
    public TownSystem HomeTown;
    #endregion

    private void Awake()
    {

        _goapPlanner = GetComponent<GoapPlanner>();
        _awareness = GetComponent<AwarenessSystem>();
        _conditionSystem = GetComponent<ConditionSystem>();
        _inventorySystem = GetComponent<InventorySystem>();
        _craftingSystem = GetComponent<CraftingSystem>();
    }

    internal void AddWork(Item requestedItem, int requestedAmount)
    {
        if (requestedItem.IsResource)
        {
            _goapPlanner.RequestResource(requestedItem, requestedAmount);
        }
        else if (requestedItem.HasRecipe)
        {
            _goapPlanner.RequestCraftedItem(requestedItem, requestedAmount);
        }
    }

    private void Start()
    {
        if (HomeTown != null)
            HomeTown.RegisterAgent(this);
    }


    public void CanSee(DetectableTarget seen)
    {
        _awareness.ReportCanSee(seen);
    }

    internal void ReportProximity(DetectableTarget inProximity)
    {
        _awareness.ReportCanSense(inProximity);

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void GetRequest(Item item, int amount)
    {
        _goapPlanner.RequestResource(item, amount);
    }

    public void GetJob(Job job)
    {
        this.Job = job;
    }


    public void PickUpResourceFromTarget(Request r)
    {
        ReadyForPickUp.Enqueue(r);
    }

}