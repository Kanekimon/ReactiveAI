
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Condition
{
    public string Name;
    public float Value;
    public bool Ticked;
    public float DecreaseRate;
    public float TriggerValue;
    public float MinimumValue;
    public float MaximumValue;
    public bool IsFatal;


    public void Tick()
    {
        Value = Mathf.Clamp(Value - DecreaseRate * Time.deltaTime, MinimumValue, MaximumValue);
    }
}

public class ConditionSystem : MonoBehaviour
{
    [SerializeField]
    List<Condition> Conditions = new List<Condition>();
    Agent Agent;
    StateMemory WorldState;

    private void Start()
    {
        Agent = GetComponent<Agent>();
        WorldState = Agent.WorldState;
        Condition hunger = new Condition()
        {
            Name = "Hungry",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = true,
            TriggerValue = 30f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(hunger);

        Condition healthy = new Condition()
        {
            Name = "Healthy",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = true,
            TriggerValue = 20f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(healthy);


        foreach (Condition c in Conditions)
        {
            WorldState.AddWorldState($"is{c.Name}", false);
        }

    }


    private void Update()
    {
        for (int i = 0; i < Conditions.Count; i++)
        {
            var cond = Conditions[i];
            if (cond.Ticked)
            {
                bool currentlyActive = (bool)WorldState.GetValue($"is{cond.Name}");
                cond.Tick();
                if (!currentlyActive && cond.Value <= cond.TriggerValue)
                    WorldState.AddWorldState($"is{cond.Name}", true);
                else if (currentlyActive && cond.Value > cond.TriggerValue)
                    WorldState.AddWorldState($"is{cond.Name}", false);

                if (cond.Value == cond.MinimumValue && cond.IsFatal)
                    Agent.Die();
            }
        }
    }

    public void ChangeValue(string condKey, float value)
    {
        Conditions.Where(a => a.Name.Equals(condKey)).FirstOrDefault().Value += value;
    }

}

