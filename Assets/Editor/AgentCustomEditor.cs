
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

    Color proximityColor = new Color(0, 1, 0, 0.2f);
    Color visionColor = new Color(1, 0, 0, 0.2f);

    public override void OnInspectorGUI()
    {
        Agent agent = (Agent)target;
        StateMemory WorldState = agent.WorldState;
        showWorldStates = EditorGUILayout.Foldout(showWorldStates, status);

        EditorGUILayout.BeginHorizontal();
        key = EditorGUILayout.TextField(key);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add Worldstate"))
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

        EditorGUILayout.BeginHorizontal();
        proximityColor = EditorGUILayout.ColorField(proximityColor);
        visionColor = EditorGUILayout.ColorField(visionColor);
        EditorGUILayout.EndHorizontal();

    }

    public void OnSceneGUI()
    {
        if (Application.isPlaying)
        {
            var ai = target as Agent;

            // draw the detectopm range
            Handles.color = proximityColor;
            Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.AwarenessSystem.ProximityDetectionRange);


            // work out the start point of the vision cone
            Vector3 startPoint = Mathf.Cos(-ai.AwarenessSystem.VisionAngle * Mathf.Deg2Rad) * ai.transform.forward +
                                 Mathf.Sin(-ai.AwarenessSystem.VisionAngle * Mathf.Deg2Rad) * ai.transform.right;

            // draw the vision cone
            Handles.color = visionColor;
            Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.AwarenessSystem.VisionAngle * 2f, ai.AwarenessSystem.VisionRange);
        }
    }

}

