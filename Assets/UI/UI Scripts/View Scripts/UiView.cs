using UnityEngine;
using UnityEngine.UIElements;

public abstract class UiView : MonoBehaviour
{
    [SerializeField] public VisualElement Root;

    protected virtual void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }

    public virtual void Open() { }

    public virtual void Open(InventorySystem left, InventorySystem right) { }
    public virtual void Close() { }
    public virtual void Refresh() { }

}

