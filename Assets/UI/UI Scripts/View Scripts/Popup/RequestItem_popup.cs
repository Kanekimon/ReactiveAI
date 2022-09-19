using UnityEngine.UIElements;

public class RequestItem_popup : UiView
{
    public VisualElement popupRoot;
    public VisualElement itemContainer;
    public InventorySlot selected;
    public Button requestButton;
    public TextField requestAmount;
    public Requestboard_view parent;

    private void Start()
    {
        popupRoot = Root.Q<VisualElement>("RequestItem_popup");

        itemContainer = Root.Q<VisualElement>("RequestItem_popup").Q<VisualElement>("Container").Q<VisualElement>("requestItem").Q<VisualElement>("SlotContainer");
        requestButton = Root.Q<VisualElement>("RequestItem_popup").Q<VisualElement>("Container").Q<VisualElement>("requestItem").Q<VisualElement>("footer").Q<Button>("request-button");
        requestAmount = Root.Q<VisualElement>("RequestItem_popup").Q<VisualElement>("Container").Q<VisualElement>("requestItem").Q<VisualElement>("footer").Q<TextField>("request-amount");
        requestButton.clicked += RequestItemHandler;
    }

    private void RequestItemHandler()
    {
        if (selected == null)
        {
            UIManager.Instance.CreateNotification("Select an item to request", 1f);
            return;
        }
        if (string.IsNullOrEmpty(requestAmount.text))
        {
            UIManager.Instance.CreateNotification("Select an amount to request", 1f);
            return;
        }


        GameManager.Instance.Player.CurrentTown.RequestBoard.RequestItem(GameManager.Instance.Player.gameObject, selected.Item, int.Parse(requestAmount.text));
        popupRoot.style.display = DisplayStyle.None;
        parent.Refresh();
    }



    public override void Open()
    {
        itemContainer.Clear();
        selected = null;
        requestAmount.value = "0";
        foreach (Item i in ItemManager.Instance.AllItems)
        {
            InventorySlot iS = new InventorySlot(i, -1);

            iS.RegisterCallback<MouseDownEvent, InventorySlot>(SelectItemHandler, iS);
            itemContainer.Add(iS);
        }

    }

    public void Open(Requestboard_view pparent)
    {
        parent = pparent;
        Open();
    }


    public void SelectItemHandler(MouseDownEvent evt, InventorySlot iS)
    {

        if (selected == iS)
        {
            iS.SetAsActive(false);
            selected = null;
        }
        else
        {
            if (selected != null)
                selected.SetAsActive(false);
            selected = iS;
            selected.SetAsActive(true);
        }
    }

}

