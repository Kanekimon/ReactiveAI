using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Condition
{
    public string Name;
    public float Value;
    public bool Ticked;
    public float DecreaseRate;

    public void Tick()
    {
        Value -= DecreaseRate * Time.deltaTime;
    }
}

public class ConditionSystem : MonoBehaviour
{
    [SerializeField]
    List<Condition> Conditions = new List<Condition>();
    Agent Agent;

    private void Start()
    {
        Agent = GetComponent<Agent>();
        Condition hunger = new Condition()
        {
            Name = "hunger",
            Value = 100,
            DecreaseRate = 0.1f,
            Ticked = true
        };
        Conditions.Add(hunger);
    }


    private void Update()
    {
        for (int i = 0; i < Conditions.Count; i++)
        {
            var cond = Conditions[i];
            if (cond.Ticked)
            {
                bool currentlyActive = (bool)Agent.GetValueFromMemory("isHungry");
                cond.Tick();
                Debug.Log($"Condition {cond.Name} Value: {cond.Value}");
                if(!currentlyActive && cond.Value <= 30)
                    Agent.SaveValueInMemory<bool>("isHungry", true);
                else if(currentlyActive && cond.Value > 30)
                    Agent.SaveValueInMemory<bool>("isHungry", false);
            }
        }
    }

    public void ChangeValue(string condKey, float value)
    {
        Conditions.Where(a => a.Name.Equals(condKey)).FirstOrDefault().Value += value;
    }

}

