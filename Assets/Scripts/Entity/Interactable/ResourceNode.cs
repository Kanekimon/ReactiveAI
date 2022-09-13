using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




public class ResourceNode : Interactable
{
    PlayerSystem player;
    public InteractionType InteractionType;


    private void Start()
    {
        player = GameManager.Instance.Player;
        OnInteract = new UnityEngine.Events.UnityEvent();
        OnInteract.AddListener(()=> player.InteractionSystem.Gather(InteractionType, this.gameObject));


    }
}

