using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static bool Player1Turn;
    public static Player Active; 
    public static bool ActionTaken;

    Button EndTurnButton;
    Button PassButton;

    void Start()
    {
        ActionTaken = false;
        Active = Game.Player1;
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
            foreach (GameCard c in Game.Selected) c.DeSelect(c);
            Game.displayCard.Reset();
        }

        foreach(GameCard c in Active.thisHand.GetComponentsInChildren<GameCard>()) c.showBack = true;

        skipTurn();
        if (Active.Passed) skipTurn();

        ActionTaken = false;
        PassButton.interactable = true;
        if(Game.Player1.Passed && Game.Player2.Passed) RoundSystem.EndRound();
    }

    public static void skipTurn()
    {
        Player1Turn = !Player1Turn;
        Active = Player1Turn ? Game.Player1 : Game.Player2;  
    }
}
