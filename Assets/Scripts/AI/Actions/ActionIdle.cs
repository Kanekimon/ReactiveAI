using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : ActionBase
{
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(IdleGoal)};
    public override List<System.Type> GetSupportedGoals()
    {
        StateAttribute<bool> precon = new StateAttribute<bool>();
        precon.Key = "HasTarget";
        precon.Value = false;

        Debug.Log("Precon: " + precon.Key + " Type: " + precon.ValueType + " Value: " + precon.Value);

        return SupportedGoals;
    }

    public override float Cost()
    {
        return 0f;
    }
}
