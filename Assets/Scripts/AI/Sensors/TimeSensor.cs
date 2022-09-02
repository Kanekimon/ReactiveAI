﻿
using UnityEngine;

public class TimeSensor : MonoBehaviour
{

    Agent Agent;
    StateMemory WorldState;
    float timer = 0;
    float delay = 10;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
    }

    private void Start()
    {
        WorldState.AddWorldState("isDayTime", TimeManager.Instance.IsDayTime());
    }

    private void Update()
    {
        if (timer > delay)
        {
            timer = 0;
            WorldState.ChangeValue("isDayTime", TimeManager.Instance.IsDayTime());
        }
        timer += Time.deltaTime;
    }


}

