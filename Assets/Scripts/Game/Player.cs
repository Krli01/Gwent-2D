using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Player : MonoBehaviour
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

    public Image Coin1;
    public Image Coin2;

    public int RoundsWon;
    public bool Passed;

    void Start()
    {
        //ActivateLeader.gameObject.SetActive(false);
        ActivateLeader.enabled = false;
        ActivateLeader.image.enabled = false;
        buttontext.enabled = false;
    }
    public void Assign (string name, string f)
    {
        Name = name;
        faction = f;
        RoundsWon = 0;
        Passed = false;
        Deck.Create(f);
        Leader.Assign(CardDatabase.FactionLeaders[f]);
        Leader.SetOwner(this);
    }

    public void Reset()
    {
        Name = "";
        faction = "";
        RoundsWon = 0;
        Passed = false;
        Leader.ResetLeader();
        thisHand.Clear();
        Deck.Cards.Clear();
        graveyard.Cards.Clear();
        //battlefields are cleared by RoundSystem
    }

    public void ShowLeaderButton()
    {
        //ActivateLeader.gameObject.SetActive(true);
        ActivateLeader.enabled = true;
        ActivateLeader.image.enabled = true;
        buttontext.enabled = true;
    }

    public void HideLeaderButton()
    {
        //ActivateLeader.gameObject.SetActive(false);
        ActivateLeader.enabled = false;
        ActivateLeader.image.enabled = false;
        buttontext.enabled = false;
    }

    public IEnumerator PutCoin()
    {
        yield return new WaitForSeconds(1f);
        if (RoundsWon == 1) Coin1.enabled = true;
        if (RoundsWon == 2) Coin2.enabled = true;
    }
}
