using System.Collections.Generic;

public class ActionIdle : ActionBase
{
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(IdleGoal) };

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    public override List<System.Type> GetSupportedGoals()
    {
        StateAttribute<bool> precon = new StateAttribute<bool>();
        precon.Key = "HasTarget";
        precon.Value = false;

        return SupportedGoals;
    }

    public override float Cost()
    {
        return 0f;
    }
}
