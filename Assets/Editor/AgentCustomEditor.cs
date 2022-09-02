
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Agent))]
public class AgentCustomEditor : Editor
{
    bool showWorldStates = true;
    string status = "World State";

    string key = "placeAmount";
    string value = "2";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Agent agent = (Agent)target;
        StateMemory WorldState = agent.WorldState;
        showWorldStates = EditorGUILayout.Foldout(showWorldStates, status);

        EditorGUILayout.BeginHorizontal();
        key = EditorGUILayout.TextField(key);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();
        if(GUILayout.Button("Add Worldstate"))
        {
            WorldState.AddWorldState(key, value);
        }


        if (showWorldStates)
        {
            if (WorldState != null && WorldState.GetWorldState != null)
            {
                foreach (KeyValuePair<string, object> state in WorldState.GetWorldState)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("State: " + state.Key);
                    if (state.Value != null)
                        EditorGUILayout.LabelField("Value: " + state.Value.ToString());
                    else
                        EditorGUILayout.LabelField("Value: null");
                    EditorGUILayout.EndHorizontal();
                }

            }
        }


    }

}

