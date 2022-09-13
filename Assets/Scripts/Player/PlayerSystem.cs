using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    float _timer;
    Animator _anim;

    PlayerConditionSystem _conditionSystem;
    InventorySystem _inventorySystem;
    CraftingSystem _craftingSystem;
    PlayerInteractionSystem _interactionSystem;
    public PlayerConditionSystem ConditionSystem => _conditionSystem;
    public InventorySystem InventorySystem => _inventorySystem;
    public CraftingSystem CraftingSystem => _craftingSystem;
    public PlayerInteractionSystem InteractionSystem => _interactionSystem;

    GameObject _target;

    private void Awake()
    {
        _conditionSystem = GetComponent<PlayerConditionSystem>();
        _inventorySystem = GetComponent<InventorySystem>();
        _craftingSystem = GetComponent<CraftingSystem>();
        _anim = GetComponent<Animator>();
        _interactionSystem = GetComponent<PlayerInteractionSystem>();   
    }

}

