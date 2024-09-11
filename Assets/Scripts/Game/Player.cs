using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Player 
{
    public string Name;
    public string faction;

    public GameCard Leader;
    public PlayerDeck Deck;
    public Hand thisHand;
    public Battlefield battlefield;
    public Graveyard graveyard;
    public GameObject overflow;
    
    public Image LeaderActive;
    public Button ActivateLeader;
    public TextMeshProUGUI buttontext;

    public int RoundsWon;
    public bool Passed;

    // Start is called before the first frame update
    public Player(string name, string f, string player)
    {
        Name = name;
        faction = f;
        
        GameCard[] leaders = GameObject.FindObjectsOfType<GameCard>();
        Leader = System.Array.Find(leaders, c => c.name == $"{player} Leader");
        Leader.Assign(CardDatabase.FactionLeaders[faction]);
        Leader.SetOwner(this);
        
        PlayerDeck[] decks = GameObject.FindObjectsOfType<PlayerDeck>();
        Deck = System.Array.Find(decks, c => c.name == $"{player} Deck");
        Deck.Create(f);
        
        Hand[] hands = GameObject.FindObjectsOfType<Hand>();
        thisHand = System.Array.Find(hands, c => c.name == $"{player} Hand");

        Graveyard[] graves = GameObject.FindObjectsOfType<Graveyard>();
        graveyard = System.Array.Find(graves, c => c.name == $"{player} Graveyard");

        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        ActivateLeader = System.Array.Find(buttons, c => c.name == $"{player} Activate Leader");
        buttontext = ActivateLeader.GetComponentInChildren<TextMeshProUGUI>();
        buttontext.enabled = false;
        ActivateLeader.enabled = false;
        
        Image[] images = GameObject.FindObjectsOfType<Image>();
        LeaderActive = System.Array.Find(images, c => c.name == $"{player} Activate Leader");
        LeaderActive.enabled = false;

        overflow = GameObject.Find($"{player} OverflowContainer");

        GameObject rowCount = GameObject.Find($"{player} RowCount");
        battlefield = new Battlefield(player, rowCount);

        RoundsWon = 0;
        Passed = false;
    }

    public void ShowLeaderButton()
    {
        ActivateLeader.enabled = true;
        ActivateLeader.image.enabled = true;
        buttontext.enabled = true;
    }

    public void HideLeaderButton()
    {
        ActivateLeader.enabled = false;
        ActivateLeader.image.enabled = false;
        buttontext.enabled = false;
    }
}
