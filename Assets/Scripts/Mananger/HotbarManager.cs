using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;


    VisualElement Root;
    VisualElement Hotbar;
    Dictionary<int, InventorySlot> hotbarMap = new Dictionary<int, InventorySlot>();

    public int HotbarSlots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


    private void Start()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
        Hotbar = Root.Q<VisualElement>("Hotbar");
        for (int i = 0; i < HotbarSlots; i++)
        {
            InventorySlot slot = new InventorySlot(i);
            hotbarMap.Add(i, slot);
            Hotbar.Add(slot);
        }
    }

    public void SetSlot(int index, InventorySlot invItem)
    {
        if (hotbarMap[index] == null)
            return;


        if (hotbarMap[index].Item == null)
        {
            InventorySlot hotbarSlot = hotbarMap[index];
            hotbarSlot.HoldItem(invItem.Item, invItem.ItemAmount);

        }
    }

    internal bool IsOverlapping(VisualElement ghostIcon, InventorySlot draggedItem)
    {
        foreach (KeyValuePair<int, InventorySlot> slot in hotbarMap)
        {
            Debug.Log("Distance: " + Vector2.Distance(slot.Value.worldBound.position, ghostIcon.worldBound.position));
            if (Vector2.Distance(slot.Value.worldBound.position, ghostIcon.worldBound.position) < 30f)
            {
                SetSlot(slot.Key, draggedItem);
                return true;
            }
        }
        return false;
    }
}

