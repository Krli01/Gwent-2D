using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public static bool Player1Turn;
    public TextMeshProUGUI TurnText;
    public static Player Active; 
    public static bool ActionTaken;

    // Start is called before the first frame update
    void Start()
    {
        ActionTaken = false;
        Active = Game.Player1;
    }

    // Update is called once per frame
    void Update()
    {
        TurnText.text = Player1Turn ? "Player 1" : "Player 2";
        foreach(GameCard c in Active.thisHand.GetComponentsInChildren<GameCard>())
        {
            c.showBack = false;
        }
    }

    public void Pass()
    {
        Active.Passed = true;
        ActionTaken = true;
        EndTurn();
    }

    public void EndTurn()
    {
        if (ActionTaken)
        {
            if(Game.Selected.Count > 0)
            {
                foreach (GameCard c in Game.Selected)
                {
                    c.isSelected = false;
                    c.transform.position -= c.popUpOnHover;
                } 
                Game.Selected.Clear();
                Game.displayCard.Reset();
            }

            foreach(GameCard c in Active.thisHand.GetComponentsInChildren<GameCard>())
            {
                c.showBack = true;
            }

            Player1Turn = !Player1Turn;
            Active = Player1Turn ? Game.Player1 : Game.Player2;  

            if (Active.Passed) 
            {
                Player1Turn = !Player1Turn;
                Active = Player1Turn ? Game.Player1 : Game.Player2;
            }
        }

        ActionTaken = false;
        if(Game.Player1.Passed && Game.Player2.Passed) RoundSystem.EndRound();
    }
}
