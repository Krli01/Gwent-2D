using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Player Player1;
    public static Player Player2;
    static Player Winner;
    public static CardSlot[] WeatherZone;
    public static DisplayCard displayCard;
    
    public static List<GameCard> Selected = new List<GameCard>();
    
    void Start()
    {
        //Debug.Log("started");
        GameObject weatherZone = GameObject.Find("WeatherZone");
        displayCard = FindObjectOfType<DisplayCard>();
        WeatherZone = weatherZone.GetComponentsInChildren<CardSlot>();
        Player1 = new Player("Player 1", "Seaborn", "P1");
        Player2 = new Player("Player 2", "Pirate", "P2");
        RoundSystem.StartRound();
        StartCoroutine(Player1.Deck.DrawCards(Player1, 12));
        StartCoroutine(Player2.Deck.DrawCards(Player2, 10));
    }

    public static void SetWinner(Player player)
    {
        if (player == null)
        {
            // Tie
            Debug.Log("tie");
        }
        else 
        {
            Winner = player;
            Debug.Log($"{player.Name} wins");
        }
    }

}
