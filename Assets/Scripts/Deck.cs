using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    //Funcional
    public Stack<Card> Deck;
    public int deckSize = 30;
    public string faction;

    //Visual
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject Card4;

    // Start is called before the first frame update
    void Start()
    {
        //shufflear la lista correcta segun faccion
        int x;
        int available = CardDatabase.AvailableDecks[faction].Count;
        if (available-1 < deckSize) throw new System.Exception($"Starting deck must contain {deckSize} cards");
        
        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, available);
            // verificar q el random no haga repeticiones
            Deck.Push(CardDatabase.AvailableDecks[faction][x]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deckSize < deckSize - 5)
            Card4.SetActive(false);
        if (deckSize < deckSize - 15)
            Card3.SetActive(false);
        if (deckSize < 7)
            Card2.SetActive(false);
        if (deckSize < 1)
            Card1.SetActive(false);
    }
}
