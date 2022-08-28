using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSystem : MonoBehaviour
{
    TownSystem townSystem;

    private void Awake()
    {
        townSystem = this.transform.parent.GetComponent<TownSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RequestItem(string name, int amount)
    {
        Item requested = ItemManager.Instance.GetItemByName(name);
        if (requested.IsResource)
        {
            if (name.Equals("wood"))
                townSystem.RequestResoruces(ResourceType.Wood, amount);
        }
        //else request components which are needed to craft

    }

    public void RequestResoruce(string name, int amount)
    {

    }

}
