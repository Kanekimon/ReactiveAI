using UnityEngine;

[RequireComponent(typeof(Agent))]
public class ProximitySensor : MonoBehaviour
{

    Agent Agent;

    private void Start()
    {
        Agent = GetComponent<Agent>();
    }


    private void Update()
    {
        for (int i = 0; i < DetectableTargetManager.Instance.Targets.Count; ++i)
        {
            var candTarget = DetectableTargetManager.Instance.Targets[i];

            if (candTarget.gameObject == this.gameObject)
                continue;

            if (Vector3.Distance(Agent.transform.position, candTarget.transform.position) <= Agent.AwarenessSystem.ProximityDetectionRange)
            {
                Agent.ReportProximity(candTarget);
            }

        }
    }

}

