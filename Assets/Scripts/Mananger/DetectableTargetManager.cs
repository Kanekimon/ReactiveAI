using System.Collections.Generic;
using UnityEngine;


public class DetectableTargetManager : MonoBehaviour
{
    public static DetectableTargetManager Instance;

    public List<DetectableTarget> Targets { get; private set; } = new List<DetectableTarget>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void RegisterTarget(DetectableTarget target)
    {
        Targets.Add(target);
    }

    public void DeregisterTarget(DetectableTarget target)
    {
        Targets.Remove(target);
    }

}
