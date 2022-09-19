using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public Image Icon;
    public Label Amount;
    public Item Item;
    public int SlotIndex;

    public int ItemAmount = -1;
    bool isDragging;
    private Vector2 targetStartPosition;
    private Vector3 pointerStartPosition;


    public InventorySlot()
    {
        Init();
    }
    public InventorySlot(int slotIndex)
    {
        SlotIndex = slotIndex;
        Init();
    }

    void Init()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);
        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
    }


    public InventorySlot(Item item, int slotIndex)
    {
        slotIndex = SlotIndex;
        Item = item;
        //Create a new Image element and add it to the root
        Icon = new Image();
        this.Icon.sprite = item.Icon;
        Add(Icon);
        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");

    }

    public void AddAmount(int amount)
    {
        if (Amount == null)
        {
            Amount = new Label();
            Amount.AddToClassList("item_amount");
            Add(Amount);
        }

        Amount.text = amount.ToString();
        ItemAmount = amount;
    }

    public void SetAsActive(bool isActive)
    {
        if (isActive)
        {
            RemoveFromClassList("slotContainer");
            AddToClassList("slotContainer_active");
        }
        else
        {
            RemoveFromClassList("slotContainer_active");
            AddToClassList("slotContainer");
        }
    }


    public void HoldItem(Item item, int amount)
    {
        Icon.image = item.Icon.texture;
        Item = item;
        AddAmount(amount);
        //RegisterCallback<MouseDownEvent>(ClickEvent);
        //RegisterCallback<PointerDownEvent>(StartDrag);
        //RegisterCallback<PointerMoveEvent>(DragEvent);
        //RegisterCallback<PointerUpEvent>(PointerUpHandler);
    }
    public void DropItem()
    {
        Icon.image = null;
        if (Amount != null)
            Remove(Amount);
        Amount = null;
        ItemAmount = -1;
        //UnregisterCallback<MouseDownEvent>(ClickEvent);

    }


    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
