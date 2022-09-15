using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AgentInteractable : Interactable
{
    PlayerSystem player;
    public InteractionType InteractionType = InteractionType.communicate;


    private void Start()
    {
        player = GameManager.Instance.Player;
        OnInteract = new UnityEngine.Events.UnityEvent();
        OnInteract.AddListener(() => player.InteractionSystem.Interact(InteractionType, this.gameObject));
    }
}

