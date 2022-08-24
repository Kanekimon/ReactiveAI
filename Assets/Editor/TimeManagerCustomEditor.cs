using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TimeManager))]
public class TimeManagerCustomEditor : Editor
{

    string hoursfield = "";
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        TimeManager time = TimeManager.Instance;

        EditorGUILayout.BeginHorizontal();

        hoursfield = EditorGUILayout.TextField("Hours", hoursfield);

        if(GUILayout.Button("Add hours"))
        {
            int hours = int.Parse(hoursfield);
            time.AddHours(hours);
        }
        EditorGUILayout.EndHorizontal();
    }

}

