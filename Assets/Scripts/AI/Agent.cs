using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(AwarenessSystem))]
public class Agent : MonoBehaviour
{
    public Queue<Request> ReadyForPickUp = new Queue<Request>();

    [SerializeField]
    private float _visionAngle = 60f;
    [SerializeField]
    private float _visionRange = 10f;
    [SerializeField]
    private float _proximityDetectionRange = 3f;

    [SerializeField] private float _interactionRange = 5;

    public float VisionAngle => _visionAngle;
    public float VisionRange => _visionRange;
    public float ProximityDetectionRange => _proximityDetectionRange;
    public float InteractionRange => _interactionRange;

    public float CoVisionAngle { get; private set; } = 0f;

    private StateMemory _memory = new StateMemory();
    [SerializeField]private JobType _jobType;

    AwarenessSystem _awareness;
    ConditionSystem _conditionSystem;
    InventorySystem _inventorySystem;
    CraftingSystem _craftingSystem;
    GoapPlanner _goapPlanner;

 
    public ConditionSystem ConditionSystem => _conditionSystem;
    public AwarenessSystem AwarenessSystem => _awareness;
    public InventorySystem InventorySystem => _inventorySystem;
    public CraftingSystem CraftingSystem => _craftingSystem;
    public JobType JobType => _jobType;

    public StateMemory WorldState => _memory;
    public TownSystem HomeTown;
    


    private void Awake()
    {
        CoVisionAngle = Mathf.Cos(VisionAngle * Mathf.Deg2Rad);
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

    public void GetJob(JobType job)
    {
        this._jobType = job;
    }


    public void PickUpResourceFromTarget(Request r)
    {
        ReadyForPickUp.Enqueue(r);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Agent))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as Agent;

        Handles.color = new Color(90, 90, 90, 0.25f);
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = new Color(0, 120, 0, 0.25f);
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionAngle * 2f, ai.VisionRange);
    }
}
#endif // UNITY_EDITOR