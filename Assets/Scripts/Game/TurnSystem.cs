using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static bool Player1Turn;
    public Player Active 
    {
        get 
        {
            return Player1Turn ? 
            PlayerManager.Instance.Player1 : PlayerManager.Instance.Player2;
        }
    } 
    public bool ActionTaken {get; private set;}
    public int Turn {get; private set;}

    Button EndTurnButton;
    Button PassButton;

    public static TurnSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = FindObjectOfType<TurnSystem>();
    }

    void Start()
    {
        ActionTaken = false;
        Player1Turn = true;
        Turn = 1;
        Button[] buttons = FindObjectsOfType<Button>();
        EndTurnButton = System.Array.Find(buttons, c => c.name == $"End Turn");
        PassButton = System.Array.Find(buttons, c => c.name == $"Pass Button");
    }

    void Update()
    {
        foreach(GameCard c in Active.thisHand.GetComponentsInChildren<GameCard>()) c.showBack = false;
        
        if (ActionTaken)
        {
            PassButton.interactable = false;
            EndTurnButton.interactable = true;
        }
    }

    public void TakeAction()
    {
        ActionTaken = true;
        //Debug.Log(ActionTaken);
    }

    public void Pass()
    {
        Active.Passed = true;
        EndTurn();
    }

    public void EndTurn()
    {
        Active.HideLeaderButton();
        
        if(Game.Selected.Count > 0)
        {
            foreach (GameCard c in Game.Selected) c.DeSelect();
            DisplayCard.Instance.Reset();
        }

        foreach(GameCard c in Active.thisHand.GetComponentsInChildren<GameCard>()) c.showBack = true;

        skipTurn();
        if (Active.Passed) skipTurn();

        ActionTaken = false;
        PassButton.interactable = true;
        Turn++;
        if(PlayerManager.Instance.Player1.Passed && PlayerManager.Instance.Player2.Passed) RoundSystem.Instance.EndRound();
    }

    public static void skipTurn()
    {
        Player1Turn = !Player1Turn; 
    }
}
