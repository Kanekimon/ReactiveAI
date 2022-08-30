using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventorySystem))]
public class InventoryCustomEditor : Editor
{
    string item_name = "";
    List<ItemType> item_type = new List<ItemType>();
    int item_amount = 0;

    int flags = 0;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventorySystem inventorySystem = (InventorySystem)target;

        EditorGUILayout.BeginHorizontal();
        item_name = EditorGUILayout.TextField(item_name);
        item_amount = int.Parse(EditorGUILayout.TextField(item_amount.ToString()));
        EditorGUILayout.EndHorizontal();


        flags = EditorGUILayout.MaskField("Item Types", flags, Enum.GetNames(typeof(ItemType)));


        if (GUILayout.Button("Add Item"))
        {
            Item i = new Item()
            {
                Id = 0,
                Name = item_name
                //ItemTypes = new List<ItemType>() { ItemType.Food }
            };
            inventorySystem.AddItem(i, item_amount);
            Debug.Log(flags);

        }


        foreach (KeyValuePair<Item, InventoryItem> item in inventorySystem._inventory)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Item-Name", item.Key.Name);
            EditorGUILayout.TextField("Item-Amount", item.Value.Amount.ToString());
            EditorGUILayout.EndHorizontal();
        }

    }

}

