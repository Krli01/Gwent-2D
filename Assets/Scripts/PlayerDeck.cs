using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    //Functional
    public Stack<Card> Cards;

    //Visual
    public GameObject CardInDeck1;
    public GameObject CardInDeck2;
    public GameObject CardInDeck3;
    public GameObject CardInDeck4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Create(string faction)
    {
        foreach (Card c in CardDatabase.AvailableDecks[faction])
        {
            Cards.Push(c);
            System.Console.WriteLine("added 1 card");
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
