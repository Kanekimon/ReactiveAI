using UnityEngine.UIElements;

public class TwoInventory_view : UiView
{
    VisualElement left;
    VisualElement right;
    VisualElement left_item_container;
    VisualElement right_item_container;

    InventorySlot selectedLeft;
    InventorySlot selectedRight;

    InventorySystem leftInventory;
    InventorySystem rightInventory;

    private void Start()
    {
        left = Root.Q<VisualElement>("TwoInventory_view").Q<VisualElement>("Container").Q<VisualElement>("Inventory").Q<VisualElement>("Inventories").Q<VisualElement>("Left-Inventory");
        right = Root.Q<VisualElement>("TwoInventory_view").Q<VisualElement>("Container").Q<VisualElement>("Inventory").Q<VisualElement>("Inventories").Q<VisualElement>("Right-Inventory");
        left_item_container = left.Q<VisualElement>("SlotContainer");
        right_item_container = right.Q<VisualElement>("SlotContainer");
        left.Q<VisualElement>("give-area").Q<Button>().clicked += GiveItem;
        right.Q<VisualElement>("take-area").Q<Button>().clicked += TakeItem;
    }

    private void ShowMessage()
    {

    }

    private void GiveItem()
    {
        if (selectedLeft == null)
        {
            UIManager.Instance.CreateNotification("Please select an item to give!", 1f);
            return;
        }

        int amount = int.Parse(left.Q<VisualElement>("give-area").Q<TextField>("give-amount").text);
        if (amount == 0 || !leftInventory.HasItemWithAmount(selectedLeft.Item, amount))
        {
            UIManager.Instance.CreateNotification("You don't have enough of that item!", 1f);
            return;
        }

        leftInventory.TransferItemToOtherInventory(rightInventory, selectedLeft.Item, amount);
        Refresh();
    }


    private void TakeItem()
    {
        if (GameManager.Instance.Player.Reputation < 50)
        {
            UIManager.Instance.CreateNotification("You do not have enought reputation to take items!", 1f);
            return;
        }


        if (selectedRight == null)
        {
            UIManager.Instance.CreateNotification("Please select an item to take!", 1f);
            return;
        }

        int amount = int.Parse(right.Q<VisualElement>("take-area").Q<TextField>("take-amount").text);
        if (amount == 0 || !rightInventory.HasItemWithAmount(selectedRight.Item, amount))
        {
            UIManager.Instance.CreateNotification("Agent has not that many items!", 1f);
            return;
        }

        rightInventory.TransferItemToOtherInventory(leftInventory, selectedRight.Item, amount);
        Refresh();
    }

    public override void Refresh()
    {
        Open(leftInventory, rightInventory);
    }

    public override void Open(InventorySystem leftInv, InventorySystem rightInv)
    {
        leftInventory = leftInv;
        rightInventory = rightInv;

        left.Q<Label>("Left-Inventory-Owner").text = leftInv.gameObject.name;
        right.Q<Label>("Right-Inventory-Owner").text = rightInv.gameObject.name;

        left_item_container.Clear();

        foreach (InventoryItem iI in leftInv.GetAllItems())
        {
            InventorySlot inventorySlot = new InventorySlot(iI.Item, -1);
            inventorySlot.AddAmount(iI.Amount);
            inventorySlot.RegisterCallback<MouseDownEvent, InventorySlot>(SelectItemLeftHandler, inventorySlot);
            left_item_container.Add(inventorySlot);
        }

        right_item_container.Clear();

        foreach (InventoryItem iI in rightInv.GetAllItems())
        {
            InventorySlot inventorySlot = new InventorySlot(iI.Item, -1);
            inventorySlot.AddAmount(iI.Amount);
            inventorySlot.RegisterCallback<MouseDownEvent, InventorySlot>(SelectItemRightHandler, inventorySlot);
            right_item_container.Add(inventorySlot);
        }
    }


    public void SelectItemLeftHandler(MouseDownEvent evt, InventorySlot iS)
    {

        if (selectedLeft == iS)
        {
            iS.SetAsActive(false);
            selectedLeft = null;
        }
        else
        {
            if (selectedLeft != null)
                selectedLeft.SetAsActive(false);
            selectedLeft = iS;
            selectedLeft.SetAsActive(true);
        }
    }


    public void SelectItemRightHandler(MouseDownEvent evt, InventorySlot iS)
    {

        if (selectedRight == iS)
        {
            iS.SetAsActive(false);
            selectedRight = null;
        }
        else
        {
            if (selectedRight != null)
                selectedRight.SetAsActive(false);
            selectedRight = iS;
            selectedRight.SetAsActive(true);
        }
    }
}

