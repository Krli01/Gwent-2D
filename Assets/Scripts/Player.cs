using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player 
{
    public string Name;
    public string faction;
    //public GameObject DeckSlot;
    public PlayerDeck Deck;
    public GameObject Hand;
    //public GameObject BoardSection;
    //public GameObject Cemetery;
    public float TotalPoints;
    public int RoundsWon;

    // Start is called before the first frame update
    public Player(string name, string f, string player)
    {
        Name = name;
        faction = f;
        
        PlayerDeck[] decks = GameObject.FindObjectsOfType<PlayerDeck>();
        Deck = System.Array.Find(decks, c => c.name == $"{player} Deck");
        Debug.Log("trying to assign deck");
        if (Deck != null) Debug.Log("success");
        
        Hand = GameObject.Find($"{player} Hand");
        TotalPoints = 0;
        RoundsWon = 0;
    }

    public void Draw()
    {
            GameCard current = new GameCard(); 
            current.Assign(Deck.Cards.Pop());
            current.transform.SetParent(Hand.transform);
            //Instantiate(current, transform.Role, transform.rotation);
    }

    public IEnumerator DrawCards(int n)
    {
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Draw();            
        }
    }
}
