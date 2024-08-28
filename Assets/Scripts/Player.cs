using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

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

    public GameCard thisCard;

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
        yield return new WaitForSeconds(0.2f);
        
    }
    
    /*public void Draw(Hand hand)
    { 
            current.Assign(Cards.Pop());
            current.transform.SetParent(hand.transform);
            
    }

    public IEnumerator DrawCards(Hand hand, int n)
    {
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(0.2f);
            //Draw(hand);
            Instantiate(gameCard, thisHand.transform.position, thisHand.transform.rotation);            
        }
    }*/

}
