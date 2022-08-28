using UnityEngine;

public class DetectableTarget : MonoBehaviour
{

    private void Awake()
    {

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        DetectableTargetManager.Instance.RegisterTarget(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        if (DetectableTargetManager.Instance != null)
            DetectableTargetManager.Instance.DeregisterTarget(this);
    }
}
