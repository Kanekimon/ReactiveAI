using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseGoal), false)]
public class GoalCustomEditor : Editor
{
    string preconKey, preconValue;
    List<string> Types = new List<string>();
    int index = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Types.Add("string");
        Types.Add("int");
        Types.Add("float");
        Types.Add("bool");
        Types.Add("Vector3");



        BaseGoal goal = (BaseGoal)target;

        EditorGUILayout.BeginHorizontal();
        preconKey = EditorGUILayout.TextField(preconKey);
        preconValue = EditorGUILayout.TextField(preconValue);
        index = EditorGUILayout.Popup(index, Types.ToArray());
        EditorGUILayout.EndHorizontal();
        if(GUILayout.Button("Add Precon"))
        {
            var valueToType = ValueToType(preconValue, Types[index]);
            goal.Preconditions.Add(new KeyValuePair<string, object>(preconKey, valueToType));
        }


        foreach(KeyValuePair<string, object> state in goal.Preconditions)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(state.Key);

            if (state.Value.GetType() == (typeof(string)) || state.Value.GetType() == (typeof(int)) || state.Value.GetType() == (typeof(float)))
                EditorGUILayout.TextField(state.Value.ToString());
            else if (state.Value.GetType() == (typeof(bool)))
                EditorGUILayout.Toggle(bool.Parse(state.Value.ToString()));
            else if (state.Value.GetType() == (typeof(Vector3)))
                EditorGUILayout.Vector3Field("Position",Vector3.zero);


                EditorGUILayout.EndHorizontal();
        }


    }

    object ValueToType(string value, string type)
    {
        switch (type)
        {
            case "string":
                return value.ToString();
            case "int":
                return int.Parse(value);
            case "float":
                return float.Parse(value);
            case "bool":
                return bool.Parse(value);
            case "Vector3":
                return StringToVector3(value);
            default:
                break;
        }

        return null;
    }

    Vector3 StringToVector3(string value)
    {
        Vector3 coord = new Vector3();

        string[] coords = value.Split(',');
        coord.x = coords.Length >= 1 ? float.Parse(coords[0]) : 0f;
        coord.y = coords.Length >= 2 ? float.Parse(coords[1]) : 0f;
        coord.z = coords.Length >= 3 ? float.Parse(coords[2]) : 0f;
        return coord;
    }
}
