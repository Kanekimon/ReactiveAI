using System.Collections.Generic;

public class IdleAction : ActionBase
{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {

        effects.Add(new KeyValuePair<string, object>("isIdle", true));
        base.Start();
    }

    public override float Cost()
    {
        return 0f;
    }
}
