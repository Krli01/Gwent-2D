using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Player Player1;
    public static Player Player2;
    public GameObject WeatherZone;
    
    void Start()
    {
        //Debug.Log("started");
        WeatherZone = GameObject.Find("WeatherZone");
        Player1 = new Player("Player 1", "Seaborn", "P1");
        //Player2 = new Player("Player 2", "Seaborn", "P2");
        StartCoroutine(Player1.Deck.DrawCards(Player1.thisHand, 1));
    }

}
