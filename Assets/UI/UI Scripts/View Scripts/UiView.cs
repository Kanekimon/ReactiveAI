using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UiView : MonoBehaviour
{
     [SerializeField] public VisualElement Root;

    private void Awake()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }

    public virtual void Open() { }
    public virtual void Close() { }
    public virtual void Update() { }

}

