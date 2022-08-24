using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Agent))]
public class AgentCustomEditor : Editor
{
    bool showWorldStates = true;
    string status = "World State";


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Agent agent = (Agent)target;

        showWorldStates = EditorGUILayout.Foldout(showWorldStates, status);


        if (showWorldStates)
        {
            if (agent.WorldState != null && agent.WorldState.GetWorldState != null)
            {
                foreach (KeyValuePair<string, object> state in agent.WorldState.GetWorldState)
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

