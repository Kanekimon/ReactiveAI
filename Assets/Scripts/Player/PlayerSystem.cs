using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    float _timer;
    [SerializeField] int _reputation;
    Animator _anim;

    public TownSystem CurrentTown;
    public GameObject ToolContainer;
    public int Reputation => _reputation;

    PlayerConditionSystem _conditionSystem;
    InventorySystem _inventorySystem;
    CraftingSystem _craftingSystem;
    PlayerInteractionSystem _interactionSystem;
    public PlayerConditionSystem ConditionSystem => _conditionSystem;
    public InventorySystem InventorySystem => _inventorySystem;
    public CraftingSystem CraftingSystem => _craftingSystem;
    public PlayerInteractionSystem InteractionSystem => _interactionSystem;

    GameObject _target;
    GameObject currentlyEquippedObject;
    Item currentlyEquippedItem;


    private void Awake()
    {
        _conditionSystem = GetComponent<PlayerConditionSystem>();
        _inventorySystem = GetComponent<InventorySystem>();
        _craftingSystem = GetComponent<CraftingSystem>();
        _anim = GetComponent<Animator>();
        _interactionSystem = GetComponent<PlayerInteractionSystem>();
    }


    public void AddReputation(int amount)
    {
        _reputation += amount;
    }

    public void UseItem(Item i)
    {
        if (i.ItemTypes.Contains(ItemType.Consumable))
        {
            Consume(i);
        }
        else if (i.ItemTypes.Contains(ItemType.Tool))
        {
            EquipItem(i);
        }
    }


    public void Consume(Item i)
    {
        if (i.ItemTypes.Contains(ItemType.Consumable))
        {
            foreach (ItemProperty iP in i.Properties.Where(a => a.Type == ItemPropertyType.consume))
            {
                if (_conditionSystem.GetValueFromCondition(iP.Name) + 1 >= _conditionSystem.GetCondition(iP.Name).MaximumValue)
                {
                    UIManager.Instance.CreateNotification($"{iP.Name} is already satisified",2f);
                    return;
                }
                _conditionSystem.DecreaseValue(iP.Name, -iP.Value);
            }
        }
        InventorySystem.RemoveItem(i, 1);
    }

    public void EquipItem(Item i)
    {
        if(currentlyEquippedObject == null)
        {
            currentlyEquippedObject = Instantiate(i.Prefab, ToolContainer.transform);
        }
        else
        {
            if (currentlyEquippedItem != i)
            {
                UnequipItem(currentlyEquippedItem);
                currentlyEquippedItem = i;
                currentlyEquippedObject = Instantiate(i.Prefab, ToolContainer.transform);
            }
            else
                UnequipItem(currentlyEquippedItem);

        }
    }

    public void UnequipItem(Item i)
    {
        DestroyImmediate(currentlyEquippedObject);
        currentlyEquippedItem = null;
        currentlyEquippedObject = null;
    }


}

