using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Player Player1;
    public static Player Player2;
    public GameObject WeatherZone;
    // Start is called before the first frame update
    void Start()
    {
        WeatherZone = GameObject.Find("WeatherZone");
        Player1 = new Player("Player 1", "Seaborn");
        //Player2 = new Player();
        StartCoroutine(Player1.DrawCards(1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*void AssignPlayers()
    {
        Player1.faction = "Whaler";
        Player1.Deck = new PlayerDeck(Player1.faction, GameObject.Find("P1 Deck"));
        //Player1.Deck.faction = Player1.faction;
        Player1.Hand = GameObject.Find("P1 Hand");
    }*/
}
