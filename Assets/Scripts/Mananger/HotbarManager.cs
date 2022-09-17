using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class HotbarManager : MonoBehaviour
{
    VisualElement Root;
    VisualElement Hotbar;
    Dictionary<int, InventorySlot> hotbarMap = new Dictionary<int, InventorySlot>();

    public int HotbarSlots;

    private void Start()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
        Hotbar = Root.Q<VisualElement>("Hotbar");
        for(int i = 0; i < HotbarSlots; i++)
        {
            InventorySlot slot = new InventorySlot(i);
            hotbarMap.Add(i, slot);
        }
    }





}

