using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CraftGoal : BaseGoal
{
    public Recipe RequestedRecipe;

    [SerializeField] int Prio;
    [SerializeField] int MinPrio;
    [SerializeField] int MaxPrio;


    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("deliveredResource", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        
    }


    public override int CalculatePriority()
    {
        return Prio;
    }


    public override void OnTickGoal()
    {
        if(RequestedRecipe != null)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }
}

