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

    public List<TrackedTarget> ActiveTargets => Targets.Values.ToList();
    public List<TrackedTarget> ResourceNodesInRange => ResourceNodes.Values.ToList();



    [SerializeField] public int ResourceCount = 0;
    [SerializeField] public int TargetsCount = 0;

    private void Start()
    {
        Agent = GetComponent<Agent>();
        Agent.WorldState.AddWorldState("hasTarget", false);
    }

    private void Update()
    {
        CleanUpAwareness(Targets);
        CleanUpAwareness(ResourceNodes);

        ResourceCount = ResourceNodesInRange.Count;
        TargetsCount = ActiveTargets.Count;
        Agent.SaveValueInMemory("hasTargets", ResourceCount > 0 || Targets.Count > 0);
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
                else
                    Debug.Log("Threshold change for " + target.name + " " + targetsToclean[target].Awareness);
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
            Debug.Log("Threshold change for " + targetGo.name + " " + typeOfTarget[targetGo].Awareness);
        }

        Agent.SaveValueInMemory("hasTargets", typeOfTarget.Count > 0);

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
}

