using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(AwarenessSystem))]
public class Agent : MonoBehaviour
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

    AwarenessSystem _awareness;

    private void Awake()
    {
        CoVisionAngle = Mathf.Cos(VisionAngle * Mathf.Deg2Rad);
        _awareness = GetComponent<AwarenessSystem>();
    }

    public void CanSee(DetectableTarget seen)
    {
        _awareness.ReportCanSee(seen);
    }

    internal void ReportProximity(DetectableTarget inProximity)
    {
        _awareness.ReportCanSense(inProximity);

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Agent))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as Agent;

        Handles.color = new Color(90,90,90,0.25f);
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = new Color(0,120, 0, 0.25f);
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionAngle * 2f, ai.VisionRange);
    }
}
#endif // UNITY_EDITOR