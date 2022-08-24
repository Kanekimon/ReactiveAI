using System;
using UnityEngine;

[RequireComponent(typeof(InventorySystem))]
public class BeloningsSensor : MonoBehaviour
{
    Agent Agent;
    InventorySystem InventorySystem;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
        InventorySystem = GetComponent<InventorySystem>();
    }


    private void Start()
    {
        Agent.WorldState.AddWorldState("hasFood", false);
    }

    private void Update()
    {
        Agent.WorldState.ChangeValue("hasFood", InventorySystem.HasItemWithType(ItemType.Food));

    }

}
