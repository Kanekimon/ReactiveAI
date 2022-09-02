using UnityEngine;

[RequireComponent(typeof(Agent))]
public class VisualSensor : MonoBehaviour
{

    [SerializeField] LayerMask DetectionMask;

    Agent Agent;


    // Start is called before the first frame update
    void Start()
    {
        Agent = GetComponent<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < DetectableTargetManager.Instance.Targets.Count; ++i)
        {
            var candTarget = DetectableTargetManager.Instance.Targets[i];

            if (candTarget.gameObject == this.gameObject)
                continue;

            var vectorToTarget = candTarget.transform.position - Agent.transform.position;

            if (vectorToTarget.sqrMagnitude > (Agent.AwarenessSystem.VisionRange * Agent.AwarenessSystem.VisionRange))
                continue;

            vectorToTarget.Normalize();

            if (Vector3.Dot(vectorToTarget, Agent.transform.forward) < Agent.AwarenessSystem.CoVisionAngle)
                continue;

            RaycastHit hit;

            if (Physics.Raycast(Agent.transform.position, vectorToTarget, out hit, Agent.AwarenessSystem.VisionRange, DetectionMask, QueryTriggerInteraction.Collide))
            {
                Agent.CanSee(candTarget);
            }

        }


    }
}
