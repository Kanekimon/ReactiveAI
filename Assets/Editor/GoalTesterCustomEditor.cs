using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoalTester))]
public class GoalTesterCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GoalTester goaltest = (GoalTester)target;
        foreach(var goal in goaltest.Goals)
        {
            if (GUILayout.Button(goal.GetType().ToString()))
            {
                goaltest.CreatePlan(goal);
            }
        }

    }

}

