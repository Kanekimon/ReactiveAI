using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InventorySystem))]
public class InventoryCustomEditor : Editor
{
    Item item = null;
    List<ItemType> item_type = new List<ItemType>();
    int item_amount = 0;

    int flags = 0;



    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InventorySystem inventorySystem = (InventorySystem)target;

        EditorGUILayout.BeginHorizontal();
        item = EditorGUILayout.ObjectField("", item, typeof(Item), true) as Item;
        item_amount = int.Parse(EditorGUILayout.TextField(item_amount.ToString()));
        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Add Item"))
        {
            inventorySystem.AddItem(item, item_amount);
            Debug.Log(flags);
            item = null;
        }
        //foreach (KeyValuePair<Item, InventoryItem> item in inventorySystem._inventory)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    EditorGUILayout.TextField("Item-Name", item.Key.Name);
        //    EditorGUILayout.TextField("Item-Amount", item.Value.Amount.ToString());
        //    EditorGUILayout.EndHorizontal();
        //}
        //EditorGUI.BeginChangeCheck();

        //if (EditorGUI.EndChangeCheck())
        //{
        //    foreach (KeyValuePair<Item, InventoryItem> item in inventorySystem._inventory)
        //    {
        //        EditorGUILayout.BeginHorizontal();
        //        EditorGUILayout.TextField("Item-Name", item.Key.Name);
        //        EditorGUILayout.TextField("Item-Amount", item.Value.Amount.ToString());
        //        EditorGUILayout.EndHorizontal();
        //    }
        //}

    }


}

