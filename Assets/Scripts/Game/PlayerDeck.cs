using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDeck : MonoBehaviour
{
    //Functional
    public GameCard cardPrefab;
    public Stack<Card> Cards;

    //Visual
    public GameCard CardInDeck1;
    public GameCard CardInDeck2;
    public GameCard CardInDeck3;
    public GameCard CardInDeck4;

    void Start()
    {
        if (Cards == null) Cards = new Stack<Card>();
    }

    public void Create(string faction)
    {
        int x;
        int index;
        if (Cards == null) Cards = new Stack<Card>();
        while(Cards.Count < 30)
        {
            x = Random.Range(0,2);
            if (x == 0)
            {
                List<Card> factionList = CardDatabase.AvailableDecks[faction];
                index = Random.Range(0, factionList.Count);
                Card nextCard = factionList[index];
                if (nextCard.instancesLeft > 0) 
                {
                    Cards.Push(factionList[index]);
                    nextCard.instancesLeft--;
                    //Debug.Log($"{nextCard.CardName}: pushed");
                }
            }
            else
            {
                List<Card> factionList = CardDatabase.AvailableDecks["Neutral"];
                index = Random.Range(0, factionList.Count);
                Card nextCard = factionList[index];
                if (nextCard.instancesLeft > 0)
                {
                    Cards.Push(factionList[index]);
                    nextCard.instancesLeft--;
                    //Debug.Log($"{nextCard.CardName}: pushed");
                }
            }
        }
    }

    public IEnumerator DrawCards(Player owner, int n)
    {
        for (int i = 0; i < n; i++)
        {
            int cardsInHand = owner.thisHand.transform.childCount;

            if(Cards.Count > 0)
            {
                yield return new WaitForSeconds(0.35f);
                Card cardData = Cards.Pop();

                if (cardsInHand < 10)
                {
                    GameCard newCard = Instantiate(cardPrefab, owner.thisHand.transform.position, owner.thisHand.transform.rotation);
                    newCard.SetOwner(owner);
                    if (owner.thisHand.transform != TurnSystem.Instance.Active.thisHand.transform)
                    {
                        newCard.showBack = true;
                    }
                    newCard.Assign(cardData);
                    newCard.transform.SetParent(owner.thisHand.transform, false);
                    owner.thisHand.ArrangeCards();
                }
                else
                {
                    GameCard newCard = Instantiate(cardPrefab, owner.overflow.transform.position, owner.overflow.transform.rotation);
                    newCard.SetOwner(owner);
                    newCard.Assign(cardData);
                    newCard.transform.SetParent(owner.overflow.transform, false);
                    newCard.transform.localPosition = Vector3.zero;
                    yield return new WaitForSeconds(0.8f);
                    owner.graveyard.SendToGraveyard(newCard);
                }
            }
            if(Cards.Count == 0) Debug.Log("empty deck");
            if (Cards == null) Debug.Log("null deck");
            if (owner.thisHand == null) Debug.Log("hand null");
            if (owner == null) Debug.Log("owner null");
        }
    }

    public void ArrangeDeck()
    {
        int deckSize = Cards.Count;
        
        if (deckSize < 22)
            CardInDeck4.enabled = false;
        if (deckSize < 12)
            CardInDeck3.enabled = false;
        if (deckSize < 3)
            CardInDeck2.enabled = false;
        if (deckSize < 1)
            CardInDeck1.enabled = false;
    }

}
