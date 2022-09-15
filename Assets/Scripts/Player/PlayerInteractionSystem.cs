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
    mine,
    communicate
}

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] GameObject _target;
    Animator _anim;
    InteractionType _currentInteractionType;
    PlayerSystem _player;
    float _timer;
    public bool IsInteractingWithAI;
    GameObject follower;

    public bool HasFollower => follower != null;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<PlayerSystem>();
    }


    public void Interact(InteractionType iType, GameObject target)
    {
        if(_target != null && _target != target && _target.GetComponent<Agent>() != null)
        {
            StopAgentInteraction();
        }

        _target = target;


        if (_currentInteractionType != InteractionType.none)
            _anim.SetBool(_currentInteractionType.ToString(), false);

        _currentInteractionType = iType;

        if (iType == InteractionType.communicate && _target.GetComponent<InventorySystem>() != null)
        {
            UIManager.Instance.ToggleWindow("Interaction", GetComponent<InventorySystem>(), _target.GetComponent<InventorySystem>());
            IsInteractingWithAI = _currentInteractionType == InteractionType.communicate && _target != null;
            _target.GetComponent<GoapPlanner>().InteractWithPlayer();
            return;
        }

        _anim.SetBool(_currentInteractionType.ToString(), true);

    }

    internal void SetFollowingAgent(bool shouldFollow)
    {
        _target.GetComponent<GoapPlanner>().FollowPlayer(shouldFollow);
        follower = shouldFollow ? _target : null;
    }

    public void StopAgentInteraction()
    {
        IsInteractingWithAI = false;
        _target.GetComponent<GoapPlanner>().StopInteracting();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _target = null;
        }


        if (_currentInteractionType != InteractionType.none)
        {
            if (_currentInteractionType != InteractionType.communicate)
            {
                if (_target == null)
                {
                    if (QuestManager.Instance.HasQuestWithInteractionType(_currentInteractionType))
                    {
                        EventManager.Instance.TriggerEvent(new TeachGameEvent(_currentInteractionType, 1));
                    }

                    _anim.SetBool(_currentInteractionType.ToString(), false);
                    _currentInteractionType = InteractionType.none;

                }
                else
                {
                    if (_timer >= _anim.GetCurrentAnimatorStateInfo(0).length)
                    {
                        List<InventoryItem> gathered = _target.GetComponent<ResourceTarget>().Interact(_player.InventorySystem);
                        foreach (InventoryItem invItem in gathered)
                        {
                            if (QuestManager.Instance.HasQuestWithItem(invItem.Item))
                            {
                                EventManager.Instance.TriggerEvent(new GatherGameEvent(invItem.Item, invItem.Amount));
                            }
                        }

                        _timer = 0;
                    }
                }
                _timer += Time.deltaTime;


            }
        }
    }
}

