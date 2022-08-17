using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Awareness = Mathf.Clamp(Mathf.Max(Awareness, visualMinimumAwareness) +  awareness, 0f, 2f);

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
    Agent Agent;

    public List<TrackedTarget> ActiveTargets => Targets.Values.ToList();

    private void Start()
    {
        Agent = GetComponent<Agent>();
    }

    private void Update()
    {
        List<GameObject> cleanUp = new List<GameObject>();

        foreach(var target in Targets.Keys)
        {
            if (Targets[target].DecayAwareness(AwarenessDecayRate, AwarenessDecayRate * Time.deltaTime))
            {
                if (Targets[target].Awareness <= 0f)
                    cleanUp.Add(target);
                else
                    Debug.Log("Threshold change for " + target.name + " " + Targets[target].Awareness);
            }

        }

        foreach(var target in cleanUp)
        {
            Targets.Remove(target);
        }
    }

    private void UpdateAwareness(GameObject targetGo, DetectableTarget seen, Vector3 position, float awareness, float minAwareness)
    {
        if (!Targets.ContainsKey(targetGo))
            Targets[targetGo] = new TrackedTarget();

        if(Targets[targetGo].UpdateAwareness(seen, position, awareness, minAwareness))
        {
            Debug.Log("Threshold change for " + targetGo.name + " " + Targets[targetGo].Awareness);
        }
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

