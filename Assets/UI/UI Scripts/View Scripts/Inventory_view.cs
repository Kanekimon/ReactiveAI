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

    private void Start()
    {
        gM = GameManager.Instance;
        player = gM.Player;
        inventorySystem = player.InventorySystem;
        inventory = Root.Q<VisualElement>("Inventory");



        InitSlots();
    }

    void InitSlots()
    {
        VisualElement slotContainer = inventory.Q<VisualElement>("SlotContainer");
        for (int i = 0; i < inventorySystem.MaximumSlots; i++)
        {
            InventorySlot slot = new InventorySlot();
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
                _slots[i].HoldItem(inv_item.Item);
                _slots[i].AddAmount(inv_item.Amount);
            }
            else
            {
                _slots[i].DropItem();
            }
        }
    }

    public override void Close()
    {
        
    }

    public override void Update()
    {
        Open();
    }

}
