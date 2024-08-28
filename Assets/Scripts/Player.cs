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
    //public GameObject BoardSection;
    //public GameObject Cemetery;
    public float TotalPoints;
    public int RoundsWon;

    public GameCard cardPrefab;
    // Start is called before the first frame update
    public Player(string name, string f, string player)
    {
        Name = name;
        faction = f;
        
        PlayerDeck[] decks = GameObject.FindObjectsOfType<PlayerDeck>();
        Deck = System.Array.Find(decks, c => c.name == $"{player} Deck");
        //Debug.Log("trying to assign deck");
        //if (Deck != null) Debug.Log("success");
        Deck.Create(f);
        
        Hand[] hands = GameObject.FindObjectsOfType<Hand>();
        thisHand = System.Array.Find(hands, c => c.name == $"{player} Hand");
        //Debug.Log("trying to assign hand");
        //if (Deck != null) Debug.Log("success");

        TotalPoints = 0;
        RoundsWon = 0;
    }

    public IEnumerator DrawCards(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if(Deck.Cards.Count > 0)
            {
                yield return new WaitForSeconds(0.2f);
                Card cardData = Deck.Cards.Pop();
                
                GameCard newCard = Object.Instantiate(cardPrefab, thisHand.transform.position, thisHand.transform.rotation);
                newCard.Assign(cardData);
                newCard.transform.SetParent(thisHand.transform, false);
                thisHand.ArrangeCards();
            }
        }
    }

}
