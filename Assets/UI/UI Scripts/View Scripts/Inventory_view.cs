using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory_view : UiView
{
    GameManager gM;
    PlayerSystem player;
    InventorySystem inventorySystem;
    VisualElement inventory;
    private static bool m_IsDragging;
    private static InventorySlot m_OriginalSlot;

    Dictionary<int, InventorySlot> _slots = new Dictionary<int, InventorySlot>();
    static VisualElement ghostIcon;
    InventorySlot originalSlot;
    bool isDragging;

    protected override void Awake()
    {
        base.Awake();

    }
    private void Start()
    {
        gM = GameManager.Instance;
        player = gM.Player;
        inventorySystem = player.InventorySystem;
        inventory = Root.Q<VisualElement>("Inventory");

        ghostIcon = Root.Q<VisualElement>("GhostIcon");
        ghostIcon.RegisterCallback<PointerUpEvent>(OnMouseUp);
        ghostIcon.RegisterCallback<PointerMoveEvent>(OnMouseMove);

        InitSlots();
    }

    void InitSlots()
    {
        VisualElement slotContainer = inventory.Q<VisualElement>("SlotContainer");
        for (int i = 0; i < inventorySystem.MaximumSlots; i++)
        {
            InventorySlot slot = new InventorySlot(i);
            _slots[i] = slot;
            slotContainer.Add(slot);

        }
    }


    public override void Open()
    {
        VisualElement slotContainer = inventory.Q<VisualElement>("SlotContainer");
        //for(int i = 0; i < inventorySystem.MaximumSlots; i++)
        //{
        //    slotContainer.Add(new InventorySlot());
        //}

        for(int i = 0; i < inventorySystem.MaximumSlots; i++)
        {
            InventoryItem inv_item = inventorySystem.GetItemAtSlot(i);
            if (inv_item != null)
            {
                _slots[i].HoldItem(inv_item.Item, inv_item.Amount);
                _slots[i].RegisterCallback<MouseDownEvent, System.Tuple<Vector2, InventorySlot>>(StartDrag, new System.Tuple<Vector2, InventorySlot>(_slots[i].layout.position, _slots[i]));
            }
            else
            {
                _slots[i].DropItem();
            }
        }
    }

    public void StartDrag(MouseDownEvent evt, System.Tuple<Vector2, InventorySlot> originalData)
    {
        //Set tracking variables
        isDragging = true;
        originalSlot = originalData.Item2;

        //Set the new position
        ghostIcon.style.top = evt.mousePosition.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = evt.mousePosition.x - ghostIcon.layout.width / 2;
        //Set the image
        ghostIcon.style.backgroundImage = new StyleBackground(originalSlot.Item.Icon);
        //Flip the visibility on
        ghostIcon.style.visibility = Visibility.Visible;
    }

    private void OnMouseMove(PointerMoveEvent evt)
    {
        //Only take action if the player is dragging an item around the screen
        if (!isDragging)
        {
            return;
        }

        //Set the new position
        ghostIcon.style.top = evt.position.y - ghostIcon.layout.height / 2;
        ghostIcon.style.left = evt.position.x - ghostIcon.layout.width / 2;
    }
    private void OnMouseUp(PointerUpEvent evt)
    {
        if (!isDragging)
        {
            return;
        }
        //Check to see if they are dropping the ghost icon over any inventory slots.
        Dictionary<int, InventorySlot> slots = _slots.Where(x =>
               x.Value.worldBound.Overlaps(ghostIcon.worldBound)).ToDictionary(x => x.Key, x => x.Value);
        //Found at least one
        if (slots.Count() != 0)
        {
            KeyValuePair<int, InventorySlot> closestSlot = slots.OrderBy(x => Vector2.Distance
               (x.Value.worldBound.position, ghostIcon.worldBound.position)).First();

            if(closestSlot.Value.Item != null)
            {
                inventorySystem.SwapSlots(originalSlot.SlotIndex, closestSlot.Value.SlotIndex);

                InventorySlot tmp = originalSlot;
                originalSlot.DropItem();
                originalSlot.HoldItem(closestSlot.Value.Item, closestSlot.Value.ItemAmount);

                closestSlot.Value.DropItem();
                closestSlot.Value.HoldItem(tmp.Item, tmp.ItemAmount);
            }
            else
            {
                inventorySystem.SwapSlots(originalSlot.SlotIndex, closestSlot.Value.SlotIndex);
                closestSlot.Value.HoldItem(originalSlot.Item, originalSlot.ItemAmount);
                originalSlot.DropItem();
            }
        }
        else
        {
            if (inventory.worldBound.Contains(ghostIcon.layout.position))
            {
                Debug.Log("Inside Inventory");
            }
            else
            {
                player.DropItem(originalSlot.Item, originalSlot.ItemAmount);
            }
        }
        //Didn't find any (dragged off the window)
        //else
        //{
        //    m_OriginalSlot.Icon.image =
        //          GameController.GetItemByGuid(m_OriginalSlot.ItemGuid).Icon.texture;
        //}
        //Clear dragging related visuals and data
        m_IsDragging = false;
        m_OriginalSlot = null;
        ghostIcon.style.visibility = Visibility.Hidden;
        Refresh();
    }



    public override void Close()
    {
        
    }

    public override void Refresh()
    {
        Open();
    }

}
