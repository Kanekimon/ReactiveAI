using System.Collections.Generic;

public class ActionIdle : ActionBase
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        effects.Add(new KeyValuePair<string, object>("isIdle", true));
    }

    public override float Cost()
    {
        return 0f;
    }
}
