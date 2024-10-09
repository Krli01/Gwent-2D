using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static bool drawPhase;
    public static List<GameCard> Selected;

    public static IVisitor visitor;
    
    void Start()
    {
        StartCoroutine(StartGame());
        if (Selected == null) Selected = new List<GameCard>();
    }

    IEnumerator StartGame()
    {
        visitor = new ExecutionVisitor();
        
        PlayerManager.Instance.Player1.Assign(GameManager.Instance.Player1Name, GameManager.Instance.Player1Faction);
        PlayerManager.Instance.Player2.Assign(GameManager.Instance.Player2Name, GameManager.Instance.Player2Faction);
        RoundSystem.Instance.Enable(PlayerManager.Instance.Player1, PlayerManager.Instance.Player2);

        Context.Instance.Players = new List<Player>
        {
            PlayerManager.Instance.Player1,
            PlayerManager.Instance.Player2,
        };
        
        RoundSystem.Instance.StartDrawPhase();
        yield return new WaitForSeconds(3);
        StartCoroutine(PlayerManager.Instance.Player1.Deck.DrawCards(PlayerManager.Instance.Player1, 10));
        StartCoroutine(PlayerManager.Instance.Player2.Deck.DrawCards(PlayerManager.Instance.Player2, 10));
    }

    public static void SetWinner(Player player)
    {
        if (player == null) EndGame("Tie");
        else EndGame(player.Name); 
    }

    static void EndGame(string winner)
    {
        PlayerManager.Instance.ResetPlayers();
        InfoPanel.Instance.End(winner);
        Debug.Log("End of game");
    }

    public void RestartCurrent()
    {
        
    }

    public static GameCard GetNewCard()
    {
        GameCard gameCard = GameObject.Instantiate(TurnSystem.Instance.Active.Deck.cardPrefab, TurnSystem.Instance.Active.overflow.transform.position, TurnSystem.Instance.Active.overflow.transform.rotation);
        return gameCard;
    }
}
