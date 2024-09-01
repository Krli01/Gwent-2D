using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    //Functional
    public GameCard cardPrefab;
    public Stack<Card> Cards = new Stack<Card>();

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
        }
    }

    public IEnumerator DrawCards(Hand thisHand, int n)
    {
        for (int i = 0; i < n; i++)
        {
            if(Cards.Count > 0)
            {
                yield return new WaitForSeconds(0.4f);
                Card cardData = Cards.Pop();

                GameCard newCard = Object.Instantiate(cardPrefab, thisHand.transform.position, thisHand.transform.rotation);
                newCard.Assign(cardData);
                newCard.transform.SetParent(thisHand.transform, false);
                thisHand.ArrangeCards();
            }
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
