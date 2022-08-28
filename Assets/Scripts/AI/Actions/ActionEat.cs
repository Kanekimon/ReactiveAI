using System.Collections.Generic;
using UnityEngine;

public class ActionEat : ActionBase
{
    [SerializeField] float SearchRange = 10f;


    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasFood", true));
        effects.Add(new KeyValuePair<string, object>("isHungry", false));
        base.Start();
    }

    public override void OnTick()
    {
        if(Agent)
        Agent.InventorySystem.RemoveItem(Agent.InventorySystem.GetFirstItemWithType(ItemType.Food), 1);
        Agent.ConditionSystem.ChangeValue("Hungry", 15);
        OnDeactived();
    }

}
