using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundSystem : MonoBehaviour
{
    Player Player1;
    Player Player2;
    Player Winner;
    int Round;
    bool gameEnded;

    public static RoundSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = FindObjectOfType<RoundSystem>();
    }

    void Start()
    {
        Round = 1;
    }

    public void Enable(Player p1, Player p2)
    {
        Player1 = p1;
        Player2 = p2;
    }

    public void StartDrawPhase()
    {
        Game.drawPhase = true;
        TurnSystem.Instance.TakeAction();
        InfoPanel.Instance.Show("Fase de descarte", 2f, 1f, false, $"Seleccione hasta 2 cartas para reemplazar");
        Button Discard = GameObject.Find("End Phase")?.GetComponent<Button>();
        Button Pass = GameObject.Find("Pass Button")?.GetComponent<Button>();
        TextMeshProUGUI Text = Discard.GetComponentInChildren<TextMeshProUGUI>();
        Text.text = "Continuar";
        Discard.enabled = true;
        Pass.interactable = false;
    }

    public void EndDrawPhase()
    {   
        if (TurnSystem.Instance.Active == PlayerManager.Instance.Player1)
        {
            foreach (GameCard c in PlayerManager.Instance.Player1.thisHand.GetComponentsInChildren<GameCard>())
            {
                if (Game.Selected.Contains(c))
                {
                    Game.Selected.Remove(c);
                    Player1.graveyard.SendToGraveyard(c);
                    StartCoroutine(Player1.Deck.DrawCards(Player1, 1));
                }
                else c.showBack = true;

                Button Discard = GameObject.Find("End Phase")?.GetComponent<Button>();
                TextMeshProUGUI Text = Discard.GetComponentInChildren<TextMeshProUGUI>();
                Text.text = "Continuar";
            }      
            TurnSystem.skipTurn();
        } 
        else
        {
            Button Discard = GameObject.Find("End Phase")?.GetComponent<Button>();
            Discard.gameObject.SetActive(false);
            foreach (GameCard c in PlayerManager.Instance.Player2.thisHand.GetComponentsInChildren<GameCard>())
            {
                if (Game.Selected.Contains(c))
                {
                    Game.Selected.Remove(c);
                    Player2.graveyard.SendToGraveyard(c);
                    StartCoroutine(Player1.Deck.DrawCards(Player2, 1));
                }
                else c.showBack = true;
            }      
            TurnSystem.skipTurn();
            Game.drawPhase = false;
            Button Pass = GameObject.Find("Pass Button")?.GetComponent<Button>();
            Pass.interactable = true;
            StartRound();
        }
    }
    
    public void StartRound()
    {
        //Debug.Log("Starting new round");
        Player1.battlefield.score = 0;
        Player2.battlefield.score = 0;
        Player1.Passed = false;
        Player2.Passed = false;
        if (Winner != null)
        {
            if (Winner != TurnSystem.Instance.Active) TurnSystem.skipTurn();
            Winner = null;
        }
        InfoPanel.Instance.Show($"Ronda {Round}", 1.5f, 1f, false);
        //Debug.Log(Player1.RoundsWon + ": " + Player2.RoundsWon);
    }

    public void EndRound()
    {
        float pts_1 = Player1.battlefield.score;
        float pts_2 = Player2.battlefield.score;
        if (pts_1 >= pts_2)
        {
            if(pts_1 == pts_2)
            {
                Player1.RoundsWon++;
                Player2.RoundsWon++;
                StartCoroutine(Player1.PutCoin());
                StartCoroutine(Player2.PutCoin());
                Winner = null;
            }
            else
            {
                Winner = Player1;
                Player1.RoundsWon++;
                StartCoroutine(Player1.PutCoin());
            }
        }
        else
        {
            Winner = Player2;
            Player2.RoundsWon++;
            StartCoroutine(Player2.PutCoin());
        }

        ClearBoard();
        TryFindWinner(Player1, Player2);

        if (!gameEnded)
        {
            Round++;
            StartRound();
        }
        
    }

    void TryFindWinner(Player P1, Player P2)
    {
        if (P1.RoundsWon == 2)
        {
            if (P2.RoundsWon == 2) Game.SetWinner(null);
            else Game.SetWinner(P1);
            gameEnded = true;
        }
        else if (P2.RoundsWon == 2)
        {
            Game.SetWinner(P2);
            gameEnded = true;
        }
    }

    void ClearBoard()
    {
        Player1.battlefield.Clear(Player1.graveyard);
        Player2.battlefield.Clear(Player2.graveyard);
    }
}
