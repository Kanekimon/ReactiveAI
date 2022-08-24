using System.Collections.Generic;
using UnityEngine;

public class ActionEat : ActionBase
{
    [SerializeField] float SearchRange = 10f;
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(ReduceHungerGoal) };

    protected override void Awake()
    {
        base.Awake();
        preconditions.Add(new KeyValuePair<string, object>("hasFood", true));
        effects.Add(new KeyValuePair<string, object>("isHungry", false));
    }

    protected override void Start()
    {
        base.Start();
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
        Agent.InventorySystem.RemoveItem(Agent.InventorySystem.GetFirstItemWithType(ItemType.Food), 1);
        Agent.ConditionSystem.ChangeValue("Hungry", 15);
    }

}
