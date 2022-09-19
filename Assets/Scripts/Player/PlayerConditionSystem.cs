using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerConditionSystem : MonoBehaviour
{

    [SerializeField]
    List<Condition> Conditions = new List<Condition>();
    Condition _activeCondition;


    private void Start()
    {
        Condition health = new Condition()
        {
            Name = "Health",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = false,
            TriggerValue = 30f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(health);

        Condition hunger = new Condition()
        {
            Name = "Hunger",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = true,
            TriggerValue = 30f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(hunger);

        Condition stamina = new Condition()
        {
            Name = "Stamina",
            Value = 100,
            DecreaseRate = -10f,
            Ticked = true,
            TriggerValue = 20f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(stamina);

        Condition thirst = new Condition()
        {
            Name = "Thirst",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = true,
            TriggerValue = 10f,
            MinimumValue = 0f,
            MaximumValue = 100f
        };
        Conditions.Add(thirst);
    }

    private void Update()
    {
        foreach (Condition c in Conditions)
        {
            if (c.Ticked)
                c.Tick();
        }
    }

    public void DecreaseValue(string key, float value)
    {
        if (Conditions.Any(a => a.Name.ToLower().Equals(key.ToLower())))
        {
            Conditions.Where(a => a.Name.ToLower().Equals(key.ToLower())).FirstOrDefault().Value -= value;
        }
    }

    public float GetValueFromCondition(string conName)
    {
        if (Conditions.Any(a => a.Name.ToLower().Equals(conName.ToLower())))
        {
            return Conditions.Where(a => a.Name.ToLower().Equals(conName.ToLower())).FirstOrDefault().Value;
        }
        return 0f;
    }

    public Condition GetCondition(string conName)
    {
        if (Conditions.Any(a => a.Name.ToLower().Equals(conName.ToLower())))
        {
            return Conditions.Where(a => a.Name.ToLower().Equals(conName.ToLower())).FirstOrDefault();
        }
        return null;
    }

}

