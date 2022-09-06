using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory_view : UiView
{
    GameManager gM;
    PlayerSystem player;
    InventorySystem inventorySystem;
    VisualElement inventory;

    private void Start()
    {
        gM = GameManager.Instance;
        player = gM.Player;
        inventorySystem = player.InventorySystem;
        inventory = Root.Q<VisualElement>("Inventory");
    }

    public override void Open()
    {
        VisualElement slotContainer = inventory.Q<VisualElement>("SlotContainer");
        slotContainer.Add(new InventorySlot());
    }

    public override void Close()
    {
        
    }

}
