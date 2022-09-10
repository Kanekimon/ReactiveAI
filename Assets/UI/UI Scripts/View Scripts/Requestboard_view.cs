using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Requestboard_view : UiView
{

    //Dynamically change this in the future 
    public RequestSystem currentTownBoard;

    VisualElement requestboard;

    private void Start()
    {
        requestboard = Root.Q<VisualElement>("Requestboard_view").Q<VisualElement>("Container").Q<VisualElement>("Requestboard").Q<VisualElement>("SlotContainer");

        if (requestboard == null)
            Debug.Log("requestboard is null");
    }

    public override void Open()
    {
        foreach(Request r in currentTownBoard.Open)
        {
            requestboard.Add(new RequestItem(r));
        }


        base.Open();
    }

}

