using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool Visible;
    public UnityEvent OnInteract;

    private void Update()
    {
        if (Visible)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract.Invoke();
            }
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Over");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");

    }
}

