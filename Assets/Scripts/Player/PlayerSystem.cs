using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    PlayerConditionSystem _conditionSystem;
    InventorySystem _inventorySystem;
    public PlayerConditionSystem ConditionSystem => _conditionSystem;
    public InventorySystem InventorySystem => _inventorySystem;

    private void Awake()
    {
        _conditionSystem = GetComponent<PlayerConditionSystem>();
        _inventorySystem = GetComponent<InventorySystem>(); 
    }

}

