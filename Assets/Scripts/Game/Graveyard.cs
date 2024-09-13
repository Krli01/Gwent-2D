using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    //Functional
    public GameCard cardPrefab;
    public List<Card> Cards = new List<Card>();

    //Visual
    public GameCard CardInPile1;
    public GameCard CardInPile2;
    public GameCard CardInPile3;
    public GameCard CardInPile4;
    public GameCard top;

    void Awake()
    {
        CardInPile1.gameObject.SetActive(false);
        CardInPile2.gameObject.SetActive(false);
        CardInPile3.gameObject.SetActive(false);
        CardInPile4.gameObject.SetActive(false);
    }
    void Resurrect(Transform transform, int i)
    {
        Card cardData = Cards[i];

        GameCard newCard = Instantiate(cardPrefab, transform.position, transform.rotation);
        newCard.SetOwner(TurnSystem.Instance.Active);
        newCard.Assign(cardData);
        newCard.transform.SetParent(transform, false);
    }

    public void SendToGraveyard(GameCard card)
    {
        Card c = card.BaseCard;
        Cards.Add(c);
        ArrangeGraveyard();
        //foreach (var x in Cards) Debug.Log($"{x.CardName} is in {TurnSystem.Active.Name}'s graveyard");
        top.Assign(c);
        card.transform.SetParent(null);
        Destroy(card);
        Destroy(card.prefab);
    }
    public void ArrangeGraveyard()
    {
        int bodycount = Cards.Count;
        
        if (bodycount > 0)
        {
            CardInPile1.gameObject.SetActive(true);
            top = CardInPile1;
        }
        if (bodycount > 3)
        {
            CardInPile2.gameObject.SetActive(true);
            top = CardInPile2;
        }
        if (bodycount > 12)
        {
            CardInPile3.gameObject.SetActive(true);
            top = CardInPile3;
        }
        if (bodycount > 22)
        {
            CardInPile4.gameObject.SetActive(true);
            top = CardInPile4;
        }
    }
}
