using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SatisfyConditionGoal : BaseGoal
{
    ConditionSystem CSystem;


    [SerializeField] public List<string> preconKeys = new List<string>();

    protected override void Start()
    {
        CSystem = Agent.ConditionSystem;
        base.Start();
    }

    public override void OnTickGoal()
    {
        Condition con = CSystem.GetActiveCondition();

        if (con != null)
        {

            foreach (KeyValuePair<string, object> precon in Preconditions)
            {
                Agent.WorldState.RemoveWorldState($"is{precon.Key}");
            }
            Preconditions.Clear();
            Preconditions.Add(new KeyValuePair<string, object>($"is{con.Name}", false));
            Agent.WorldState.AddWorldState($"is{con.Name}", true);
            Prio = MaxPrio;
            preconKeys = Preconditions.Select(a => a.Key).ToList();

        }
        else
            Prio = MinPrio;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        if (CSystem.GetActiveCondition().Name.ToLower().Contains("hungry"))
        {
            WorldState.AddWorldState("possibleResources", new List<ResourceType>() { ResourceType.bush, ResourceType.mushroom });
            WorldState.AddWorldState("gatherAmount", 1);
        }
    }

}

