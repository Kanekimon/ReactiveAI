using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEat : ActionBase
{
    [SerializeField] float SearchRange = 10f;
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(ReduceHungerGoal) };

    private void Awake()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasFood", true));
        effects.Add(new KeyValuePair<string, object>("isHungry", false));
    }

    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override float Cost()
    {
        return 0f;
    }

    public override void OnTick()
    {
        Agent.ConditionSystem.ChangeValue("hunger", 15);
    }

    public override void OnActivated(BaseGoal linked)
    {

    }

    public override void OnDeactived()
    {
   
    }
}
