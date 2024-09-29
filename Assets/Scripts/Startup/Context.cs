using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour
{
    public static Context Instance { get; private set; }
    public Dictionary<string, Effect> AvailableEffects {get; set;}

    public List<Player> Players {get; set;}

    void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Players = null;
            AvailableEffects = CardDatabase.Effects;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
