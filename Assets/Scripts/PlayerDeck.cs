using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    //Functional
    public string faction;
    public Stack<Card> Cards;
    public GameObject Slot;

    //Visual
    public GameObject CardInDeck1;
    public GameObject CardInDeck2;
    public GameObject CardInDeck3;
    public GameObject CardInDeck4;

    // Start is called before the first frame update
    void Start()
    {
        faction = Game.Player1.faction;
        Slot = GameObject.Find("Deck1");
        foreach (Card c in CardDatabase.AvailableDecks[faction])
        {
            Cards.Push(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*int deckSize = Cards.Count;
        
        if (deckSize < deckSize - 5)
            CardInDeck4.SetActive(false);
        if (deckSize < deckSize - 15)
            CardInDeck3.SetActive(false);
        if (deckSize < 7)
            CardInDeck2.SetActive(false);
        if (deckSize < 3)
            CardInDeck1.SetActive(false);*/
    }

}

//shufflear la lista correcta segun faccion
        /*int x;
        int available = CardDatabase.AvailableDecks[faction].Count;
        if (available-1 < deckSize) throw new System.Exception($"Starting deck must contain {deckSize} cards");
        
        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, available);
            // verificar q el random no haga repeticiones
            Deck.Push(CardDatabase.AvailableDecks[faction][x]);
        }*/
