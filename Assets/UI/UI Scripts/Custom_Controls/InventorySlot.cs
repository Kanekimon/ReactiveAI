using System.Collections;
using System.Collections.Generic;
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
        UnregisterCallback<MouseDownEvent>(ClickEvent);

    }

    private void ClickEvent(MouseDownEvent evt)
    {
        if (evt.clickCount == 2)
        {
            GameManager.Instance.Player.UseItem(Item);
        }
    }


    public void DragEvent(PointerMoveEvent evt)
    {
        if (isDragging)
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;

            this.transform.position = new Vector2(
                Mathf.Clamp(targetStartPosition.x + pointerDelta.x, 0, this.panel.visualTree.worldBound.width),
                Mathf.Clamp(targetStartPosition.y + pointerDelta.y, 0, this.panel.visualTree.worldBound.height));
        }

    }
    private void PointerUpHandler(PointerUpEvent evt)
    {
        if (isDragging && this.HasPointerCapture(evt.pointerId))
        {
            this.ReleasePointer(evt.pointerId);
            isDragging = false;
        }
    }


    public void StartDrag(PointerDownEvent evt)
    {
        Debug.Log("isNowDraggable");
        isDragging = true;

        targetStartPosition = this.transform.position;
        pointerStartPosition = evt.position;
        this.CapturePointer(evt.pointerId);
        //Debug.Log("star drag");
        //UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        //Vector2 mouse = RuntimePanelUtils.ScreenToPanel(this.panel, Input.mousePosition);
        //mouse.y = Screen.height - mouse.y;
        //this.style.top = mouse.y;
        //this.style.left = mouse.x;


    }


    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
