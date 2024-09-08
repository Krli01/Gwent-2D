using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    //Functional
    public GameCard cardPrefab;
    public List<Card> Cards = new List<Card>();

    //Visual
    public GameObject CardInPile1;
    public GameObject CardInPile2;
    public GameObject CardInPile3;
    public GameObject CardInPile4;

    void Resurrect(Transform transform, int i)
    {
        Card cardData = Cards[i];

        GameCard newCard = Object.Instantiate(cardPrefab, transform.position, transform.rotation);
        newCard.Assign(cardData);
        newCard.transform.SetParent(transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        /*int bodycount = Cards.Count;
        
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
