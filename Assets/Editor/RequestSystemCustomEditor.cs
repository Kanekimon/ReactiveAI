using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RequestSystem))]
public class RequestSystemCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RequestSystem r = (RequestSystem)target;


        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Open");
        foreach(Request req in r.Open)
        {
            AddReqeust(req);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


        EditorGUILayout.BeginFoldoutHeaderGroup(true, "InWork");
        foreach (Request req in r.InWork)
        {
            AddReqeust(req);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();


    }

    public void AddReqeust(Request r)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(r.Requester, typeof(GameObject));
        EditorGUILayout.ObjectField(r.Giver, typeof(GameObject));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.TextField("Item:", r.RequestedItem.Name);
        EditorGUILayout.TextField("Amount:", r.RequestedAmount.ToString());
        EditorGUILayout.EndHorizontal();
    }
}

