using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
