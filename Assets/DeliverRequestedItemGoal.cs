using System.Collections.Generic;

public class DeliverRequestedItemGoal : BaseGoal
{

    RequestSystem board;
    Request currentRequest;

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("deliveredItem", true));
        base.Start();
        board = Agent.HomeTown.RequestBoard;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        currentRequest = board.Open[0];
        board.Open.RemoveAt(0);
    }



    public override void OnTickGoal()
    {
        if (board.Open.Count > 0)
            Prio = MaxPrio;
        else
            Prio = MinPrio;


        base.OnTickGoal();
    }

}
