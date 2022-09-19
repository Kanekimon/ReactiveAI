using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TrackedTarget
{
    public DetectableTarget Detectable;
    public Vector3 RawPosition;

    public float LastSensedTime = -1f;
    public float Awareness; // 0    = not aware (will be culled)
                            // 0-1  = rought idea (no set location)
                            // 1-2  = likely target (location)
                            // 2    = fully detected

    public bool UpdateAwareness(DetectableTarget target, Vector3 position, float awareness, float visualMinimumAwareness)
    {
        var oldAwareness = Awareness;
        Detectable = target;
        RawPosition = position;
        LastSensedTime = Time.time;
        Awareness = Mathf.Clamp(Mathf.Max(Awareness, visualMinimumAwareness) + awareness, 0f, 2f);

        if (oldAwareness < 2f && awareness >= 2f)
            return true;

        if (oldAwareness < 1f && awareness >= 1f)
            return true;

        return false;
    }

    public bool DecayAwareness(float decayTime, float amount)
    {
        if ((Time.time - LastSensedTime) < decayTime)
            return false;

        var oldAwareness = Awareness;

        Awareness -= amount;

        if (oldAwareness >= 2f && Awareness < 2f)
            return true;

        if (oldAwareness >= 1f && Awareness < 1f)
            return true;

        return Awareness <= 0f;
    }


}

[RequireComponent(typeof(Agent))]
public class AwarenessSystem : MonoBehaviour
{

    [SerializeField]
    private float _visionAngle = 60f;
    [SerializeField]
    private float _visionRange = 10f;
    [SerializeField]
    private float _proximityDetectionRange = 3f;

    public float VisionAngle => _visionAngle;
    public float VisionRange => _visionRange;
    public float ProximityDetectionRange => _proximityDetectionRange;
    public float CoVisionAngle { get; private set; } = 0f;

    [SerializeField] AnimationCurve VisionSensitivity;
    [SerializeField] float VisualMinimumAwareness = 1f;
    [SerializeField] float VisionAwarenessBuildRate = 10f;

    [SerializeField] float ProximityMinimumAwareness = 0f;
    [SerializeField] float ProximityBuildRate = 1f;

    [SerializeField] float AwarenessDecayDelay = 0.1f;
    [SerializeField] float AwarenessDecayRate = 0.1f;

    Dictionary<GameObject, TrackedTarget> Targets = new Dictionary<GameObject, TrackedTarget>();
    Dictionary<GameObject, TrackedTarget> ResourceNodes = new Dictionary<GameObject, TrackedTarget>();
    Agent Agent;
    StateMemory WorldState;

    public List<TrackedTarget> ActiveTargets => Targets.Values.ToList();
    public List<TrackedTarget> ResourceNodesInRange => ResourceNodes.Values.ToList();

    GameObject currentTarget;

    [SerializeField] public int ResourceCount = 0;
    [SerializeField] public int TargetsCount = 0;

    private void Start()
    {
        CoVisionAngle = Mathf.Cos(VisionAngle * Mathf.Deg2Rad);
        Agent = GetComponent<Agent>();
        WorldState = Agent.WorldState;
        WorldState.AddWorldState("hasTarget", false);
    }

    private void Update()
    {
        CleanUpAwareness(Targets);
        CleanUpAwareness(ResourceNodes);

        ResourceCount = ResourceNodesInRange.Count;
        TargetsCount = ActiveTargets.Count;

        if (WorldState.GetValue<List<ResourceType>>("possibleResources") != null)
        {
            SeesResourceTarget(WorldState.GetValue<List<ResourceType>>("possibleResources"));
        }

        if (WorldState.GetValue<GameObject>("target") != null)
        {
            GameObject currentTarget = WorldState.GetValue<GameObject>("target");
            WorldState.AddWorldState("hasTarget", true);
            WorldState.AddWorldState("isAtTarget", (Vector3.Distance(currentTarget.transform.position, this.transform.position) <= Agent.InteractionRange));
        }
        else
        {
            WorldState.AddWorldState("isAtTarget", false);
            WorldState.AddWorldState("hasTarget", false);
             
        }

        if (Agent.Home != null)
        {
            if (IsCloseToObject(Agent.Home))
                WorldState.AddWorldState("isHome", true);
            else
                WorldState.AddWorldState("isHome", false);
        }

        if (Agent.Job != null)
        {
            if (IsCloseToObject(Agent.Job.Workplace))
                WorldState.AddWorldState("isAtWork", true);
            else
                WorldState.AddWorldState("isAtWork", false);
        }


        SaveToWorldState();
    }

    public bool IsCloseToObject(GameObject toCheck)
    {
        Vector3 vectorToTarget = Agent.transform.position - toCheck.transform.position;
        vectorToTarget.y = 0;
        return vectorToTarget.magnitude < Agent.InteractionRange;
    }



    private void SaveToWorldState()
    {
        WorldState.AddWorldState("hasTargets", Targets.Count > 0);
        WorldState.AddWorldState("hasResourceTargets", ResourceCount > 0);

        GameObject currentTarget = WorldState.GetValue<GameObject>("target");
        if (currentTarget != null)
        {
            //if (WorldState.GetValue<GameObject>("target") != currentResourceTarget)
            //    WorldState.AddWorldState("target", currentResourceTarget);

            if (Vector3.Distance(currentTarget.transform.position, this.transform.position) < Agent.InteractionRange)
            {
                WorldState.ChangeValue("isAtResource", true);
                WorldState.ChangeValue("isAtTarget", true);
            }
            else
            {
                WorldState.ChangeValue("isAtTarget", false);
                WorldState.ChangeValue("isAtResource", false);
            }
        }
        else
        {
            WorldState.ChangeValue("isAtResource", false);
        }

    }

    private void CleanUpAwareness(Dictionary<GameObject, TrackedTarget> targetsToclean)
    {
        List<GameObject> cleanUp = new List<GameObject>();
        foreach (var target in targetsToclean.Keys)
        {
            if (target == null)
            {
                cleanUp.Add(target);
                continue;
            }
            if (targetsToclean[target].DecayAwareness(AwarenessDecayRate, AwarenessDecayRate * Time.deltaTime))
            {
                if (targetsToclean[target].Awareness <= 0f)
                    cleanUp.Add(target);
            }
        }

        foreach (var target in cleanUp)
        {
            targetsToclean.Remove(target);
        }
    }

    private void UpdateAwareness(GameObject targetGo, DetectableTarget seen, Vector3 position, float awareness, float minAwareness)
    {
        if (seen.GetType() == typeof(ResourceTarget))
            HandleAwareness(ResourceNodes, targetGo, seen, position, awareness, minAwareness);
        else
            HandleAwareness(Targets, targetGo, seen, position, awareness, minAwareness);



    }

    private void HandleAwareness(Dictionary<GameObject, TrackedTarget> typeOfTarget, GameObject targetGo, DetectableTarget seen, Vector3 position, float awareness, float minAwareness)
    {
        if (!typeOfTarget.ContainsKey(targetGo))
            typeOfTarget[targetGo] = new TrackedTarget();

        if (typeOfTarget[targetGo].UpdateAwareness(seen, position, awareness, minAwareness))
        {
            //Debug.Log("Threshold change for " + targetGo.name + " " + typeOfTarget[targetGo].Awareness);
        }

        WorldState.AddWorldState("hasTargets", typeOfTarget.Count > 0);

    }



    internal void ReportCanSee(DetectableTarget seen)
    {
        var vectorToTarget = (seen.transform.position - Agent.transform.position).normalized;
        var dotProduct = Vector3.Dot(vectorToTarget, Agent.transform.forward);

        var awareness = VisionSensitivity.Evaluate(dotProduct) * VisionAwarenessBuildRate * Time.deltaTime;

        UpdateAwareness(seen.gameObject, seen, seen.transform.position, awareness, VisualMinimumAwareness);

    }

    internal void ReportCanSense(DetectableTarget inProximity)
    {
        var awareness = ProximityBuildRate * Time.deltaTime;
        UpdateAwareness(inProximity.gameObject, inProximity, inProximity.transform.position, awareness, ProximityMinimumAwareness);
    }

    public bool SeesResourceTarget(List<ResourceType> resourceTargets)
    {
        if (resourceTargets == null || resourceTargets.Count == 0)
            return false;

        TrackedTarget candidate = null;
        float closest = float.MaxValue;
        foreach (TrackedTarget res in ResourceNodes.Values)
        {
            if (resourceTargets.Contains((((ResourceTarget)res.Detectable).ResourceType)))
            {
                float distance = Vector3.Distance(res.RawPosition, this.transform.position);
                if (candidate == null)
                {
                    candidate = res;
                    closest = distance;
                }
                else
                {
                    if (distance < closest)
                    {
                        candidate = res;
                        closest = distance;
                    }
                }
            }

        }
        if (candidate != null)
        {
            currentTarget = candidate.Detectable.gameObject;
            WorldState.AddWorldState("target", currentTarget);
        }
        else
            currentTarget = null;



        return candidate != null;
    }

}

