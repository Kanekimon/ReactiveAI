using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class CustomQuestEditor : Editor
{
    SerializedProperty _questInfoProperty;
    //SerializedProperty _questStatProperty;

    List<string> _questGoalType;
    SerializedProperty _questGoalListProperty;

    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest()
    {
        var newQuest = CreateInstance<Quest>();
        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }

    private void OnEnable()
    {
        _questInfoProperty = serializedObject.FindProperty(nameof(Quest.Description));

        _questGoalListProperty = serializedObject.FindProperty(nameof(Quest.Objectives));

        var lookUp = typeof(QuestObjective);
        _questGoalType = new List<string>() { "GatherObjective", "BuildObjective", "CraftObjective", "TeachObjective" };
    }

    public override void OnInspectorGUI()
    {
        var child = _questInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest info", EditorStyles.boldLabel);
        while(child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest reward", EditorStyles.boldLabel);

        while(child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, _questGoalType.ToArray());

        if(choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(_questGoalType[choice]);
            AssetDatabase.AddObjectToAsset(newInstance, target);

            _questGoalListProperty.InsertArrayElementAtIndex(_questGoalListProperty.arraySize);
            _questGoalListProperty.GetArrayElementAtIndex(_questGoalListProperty.arraySize - 1).objectReferenceValue = newInstance;
        }

        Editor ed = null;
        int toDelete = -1;

        for (int i = 0; i < _questGoalListProperty.arraySize; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = _questGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }

            EditorGUILayout.EndHorizontal();
        }
            if(toDelete != -1)
            {
                var item = _questGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
                DestroyImmediate(item, true);

                _questGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            }

            serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }

}

#endif