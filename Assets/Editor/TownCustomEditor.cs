using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TownSystem))]
public class TownCustomEditor : Editor
{
    string amount;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TownSystem town = (TownSystem)target;

        EditorGUILayout.BeginHorizontal();
        amount = EditorGUILayout.TextField("Menge", amount);

        if (GUILayout.Button("Request"))
        {
            town.RequestResoruces(ResourceType.Wood, int.Parse(amount));
            amount = "";
        }
        EditorGUILayout.EndHorizontal();

    }

}

