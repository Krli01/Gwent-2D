using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Player 
{
    public string Name;
    public string faction;
    public PlayerDeck Deck;
    public Hand thisHand;
    public Battlefield battlefield;
    public Graveyard graveyard;
    public float RoundPoints;
    public float TotalPoints;
    public int RoundsWon;
    public bool Passed;

    // Start is called before the first frame update
    public Player(string name, string f, string player)
    {
        Name = name;
        faction = f;
        
        PlayerDeck[] decks = GameObject.FindObjectsOfType<PlayerDeck>();
        Deck = System.Array.Find(decks, c => c.name == $"{player} Deck");
        ////Debug.Log($"trying to assign deck to {name}");
        //if (Deck != null) Debug.Log("success");
        Deck.Create(f);
        
        Hand[] hands = GameObject.FindObjectsOfType<Hand>();
        thisHand = System.Array.Find(hands, c => c.name == $"{player} Hand");
        //Debug.Log($"trying to assign hand to {name}");
        //if (Deck != null) Debug.Log("success");

        Graveyard[] graves = GameObject.FindObjectsOfType<Graveyard>();
        graveyard = System.Array.Find(graves, c => c.name == $"{player} Graveyard");
        //Debug.Log($"trying to assign graveyard to {name}");
        //if (Deck != null) Debug.Log("success");

        battlefield = new Battlefield(player);

        RoundPoints = 0;
        TotalPoints = 0;
        RoundsWon = 0;
        Passed = false;
    }
}

public class Battlefield
{
    public CardSlot[] MeleRow;
    public CardSlot[] RangeRow;
    public CardSlot[] SiegeRow;

    public Battlefield(string player)
    {
        GameObject Mele = GameObject.Find($"{player} Mele");
        GameObject Range = GameObject.Find($"{player} Range");
        GameObject Siege = GameObject.Find($"{player} Siege");

        MeleRow = Mele.GetComponentsInChildren<CardSlot>();
        RangeRow = Range.GetComponentsInChildren<CardSlot>();
        SiegeRow = Siege.GetComponentsInChildren<CardSlot>();
    }
}
