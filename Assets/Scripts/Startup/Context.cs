using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Context : MonoBehaviour
{
    public static Context Instance { get; private set; }
    public Dictionary<string, Effect> AvailableEffects {get; set;}
    public List<Player> Players {get; set;}
    public Player TriggerPlayer {get {return TurnSystem.Instance.Active;}}
    public List<GameCard> Board 
    {
        get 
        {
            List<GameCard> board = new List<GameCard>();
            foreach (Player player in Players)
            {
                foreach (GameCard card in player.battlefield.GetBattlefield()) board.Add(card);
            }
            foreach (GameCard card in Players[0].battlefield.GetWeather()) board.Add(card);
            return board;
        }
    }
    public List<GameCard> Hand {get {return HandOfPlayer(TurnSystem.Instance.Active);}}
    public List<GameCard> otherHand 
    {
        get 
        {
            Player current = TurnSystem.Instance.Active;
            Player target = current == PlayerManager.Instance.Player1 ? PlayerManager.Instance.Player2 : PlayerManager.Instance.Player1;
            return HandOfPlayer(target);
        }
    }
    public Stack<Card> Deck {get {return TurnSystem.Instance.Active.Deck.Cards;}}
    public Stack<Card> otherDeck
    {
        get 
        {
            Player current = TurnSystem.Instance.Active;
            Player target = current == PlayerManager.Instance.Player1 ? PlayerManager.Instance.Player2 : PlayerManager.Instance.Player1;
            return DeckOfPlayer(target);
        }
    }
    public List<GameCard> Field {get {return TurnSystem.Instance.Active.battlefield.GetBattlefield();}}
    public List<GameCard> otherField 
    {
        get 
        {
            Player current = TurnSystem.Instance.Active;
            Player target = current == PlayerManager.Instance.Player1 ? PlayerManager.Instance.Player2 : PlayerManager.Instance.Player1;
            return FieldOfPlayer(target);
        }
    }
    public Stack<Card> Graveyard {get {return TurnSystem.Instance.Active.graveyard.Cards;}}
    public Stack<Card> otherGraveyard 
    {
        get 
        {
            Player current = TurnSystem.Instance.Active;
            Player target = current == PlayerManager.Instance.Player1 ? PlayerManager.Instance.Player2 : PlayerManager.Instance.Player1;
            return GraveyardOfPlayer(target);
        }
    }

    void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AvailableEffects = CardDatabase.Effects;
            Players = new List<Player>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<GameCard> HandOfPlayer(Player player)
    {
        GameCard[] cards = player.thisHand.GetComponentsInChildren<GameCard>();
        return cards.ToList();
    }

    public List<GameCard> FieldOfPlayer(Player player)
    {
        return player.battlefield.GetBattlefield();
    }

    public Stack<Card> DeckOfPlayer(Player player)
    {
        return player.Deck.Cards;      
    }

    public Stack<Card> GraveyardOfPlayer(Player player)
    {
        return player.graveyard.Cards;
    }

}
