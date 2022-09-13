using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum InteractionType
{
    none,
    gather,
    chop,
    mine
}

public class PlayerInteractionSystem : MonoBehaviour
{
    GameObject _target;
    Animator _anim;
    InteractionType _currentInteractionType;
    PlayerSystem _player;
    float _timer;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<PlayerSystem>();
    }

    public void Gather(InteractionType iType, GameObject target)
    {
        _target = target;
        if (_currentInteractionType != InteractionType.none)
            _anim.SetBool(_currentInteractionType.ToString(), false);

        _currentInteractionType = iType;
        _anim.SetBool(_currentInteractionType.ToString(), true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _target = null;
        }

        if (_currentInteractionType != InteractionType.none)
        {
            if (_target == null) {
                _anim.SetBool(_currentInteractionType.ToString(), false);
                _currentInteractionType = InteractionType.none;
            }
            else
            {
                if (_timer >= _anim.GetCurrentAnimatorStateInfo(0).length)
                {
                    List<InventoryItem> gathered = _target.GetComponent<ResourceTarget>().Interact(_player.InventorySystem);
                    foreach(InventoryItem invItem in gathered)
                    {
                        EventManager.Instance.TriggerEvent(new GatherGameEvent(invItem.Item, invItem.Amount));
                    }
                    _timer = 0;
                }
            }
            _timer += Time.deltaTime;
        }
    }
}

