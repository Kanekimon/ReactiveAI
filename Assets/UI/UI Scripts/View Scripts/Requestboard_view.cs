using UnityEngine;
using UnityEngine.UIElements;

public class Requestboard_view : UiView
{


    //Dynamically change this in the future 
    public RequestSystem currentTownBoard;
    Button createButton;
    VisualElement requestboard;
    VisualElement requestItem;

    private void Start()
    {
        requestItem = Root.Q<VisualElement>("RequestItem_popup");
        requestboard = Root.Q<VisualElement>("Requestboard_view").Q<VisualElement>("Container").Q<VisualElement>("Requestboard").Q<VisualElement>("SlotContainer");
        createButton = Root.Q<VisualElement>("Requestboard_view").Q<VisualElement>("Container").Q<VisualElement>("Requestboard").Q<VisualElement>("footer").Q<Button>("create-request");

        createButton.clicked += CreateButton_clicked;

        if (requestboard == null)
            Debug.Log("requestboard is null");
    }

    private void CreateButton_clicked()
    {
        GetComponent<RequestItem_popup>().Open(this);
        requestItem.style.display = DisplayStyle.Flex;
    }

    public override void Open()
    {
        requestboard.Clear();
        foreach (Request r in currentTownBoard.Open)
        {
            requestboard.Add(new RequestItem(r));
        }


        base.Open();
    }

    public override void Refresh()
    {
        Open();
    }

}

