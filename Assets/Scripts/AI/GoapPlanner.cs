using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GoapPlanner : MonoBehaviour
{
    Queue<ActionBase> CurrentActionSequence { get; set; } = new Queue<ActionBase>();
    BaseGoal CurrentGoal;


    GameObject ActionContainer;
    GameObject GoalContainer;

    List<ActionBase> AllActions;
    List<BaseGoal> AllGoals;
    Agent Agent;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        ActionContainer = transform.Find("ActionContainer").gameObject;
        GoalContainer = transform.Find("GoalContainer").gameObject;

        AllActions = GetAllActions();
        AllGoals = GetAllGoals();
    }

    List<ActionBase> GetAllActions()
    {
        return ActionContainer.GetComponents<ActionBase>().ToList();
    }
    List<BaseGoal> GetAllGoals()
    {
        return GoalContainer.GetComponents<BaseGoal>().ToList();
    }


    private void Start()
    {
        foreach(var goal in AllGoals)
        {
            
        }
    }


    private void Update()
    {
    }


    void Plan()
    {
        BaseGoal HighestPrioGoal = GetHighestPrioGoal();

        if (HighestPrioGoal == CurrentGoal)
            return;

        foreach (var goal in AllGoals)
        {
            if (CurrentGoal == goal)
                continue;

            CurrentGoal.OnGoalDeactivated();
            CurrentActionSequence.Peek().OnDeactived();

            /***
            CurrentActionSequence = AStar(AllActions, Memory, Memory<key><currentValue>, Memory<key><desiredValue>)
            
            if currentActionSequence valid
             => Peek, FirstElementFinished ? 
                Queue.Dequeue
            Peek().OnTick

            */
        }
    }

    BaseGoal GetHighestPrioGoal()
    {
        float highestPrio = -1;
        int indexHighest = -1;

        for (int i = 0; i < AllGoals.Count; i++)
        {
            var candGoal = AllGoals[i];
            if (candGoal.CalculatePriority() > highestPrio)
                indexHighest = i;
        }
        return AllGoals[indexHighest];
    }



}

