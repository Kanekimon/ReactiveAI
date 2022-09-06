using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerSystem Player;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }


}
