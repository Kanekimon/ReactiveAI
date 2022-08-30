using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> AllItems = new List<Item>();
   

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {

    }

    public Item GetItemByName(string name)
    {
        return AllItems.Where(a => a.Name.Equals(name)).FirstOrDefault();
    }
}

