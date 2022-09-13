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

    private Request _workingOn;
    public Request WorkingOn { get { return _workingOn; } set { _workingOn = value; } }
    public bool IsOccupied => _workingOn != null;

    public GameObject Home;

    Animator _anim;

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
        _anim = GetComponent<Animator>();   
    }

    internal void AddWork(Request r)
    {
        _workingOn = r;
        if (r.RequestedItem.IsResource)
        {
            _goapPlanner.AddRequestToWorkOn(r);
        }
        else if (r.RequestedItem.HasRecipe)
        {
            _goapPlanner.AddCraftingRequestToWorkOn(r);
        }
    }

    private void Start()
    {
        if (HomeTown != null)
            HomeTown.RegisterAgent(this);
    }

    private void Update()
    {

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

    public void GetJob(Job job)
    {
        this.Job = job;
    }


    public void PickUpResourceFromTarget(Request r)
    {
        ReadyForPickUp.Enqueue(r);
    }


}