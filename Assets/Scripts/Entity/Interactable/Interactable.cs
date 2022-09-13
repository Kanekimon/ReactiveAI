using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour
{

    public bool Visible;
    [SerializeField] protected UnityEvent OnInteract;

    private void Awake()
    {
    }
    protected virtual void Update()
    {
        if (Visible)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract.Invoke();
            }
        }
    }
}

