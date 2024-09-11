using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class RoundSystem : MonoBehaviour
{
    static Player Winner;
    static float WinScore = 0;

    public static void StartRound()
    {
        Debug.Log("Starting new round");
        Game.Player1.battlefield.score = 0;
        Game.Player2.battlefield.score = 0;
        Game.Player1.Passed = false;
        Game.Player2.Passed = false;
        //if (player == null || player == Game.Player1) TurnSystem.Player1Turn = true;
        //else TurnSystem.Player1Turn = false;
        if (Winner != null)
        {
            if (Winner != TurnSystem.Active) TurnSystem.skipTurn();
            Winner = null;
        }
        WinScore = 0;
        Debug.Log(Game.Player1.RoundsWon + Game.Player2.RoundsWon);
    }

    public static void EndRound()
    {
        Player P1 = Game.Player1;
        Player P2 = Game.Player2;
        float pts_1 = P1.battlefield.score;
        float pts_2 = P2.battlefield.score;
        if (pts_1 > pts_2)
        {
            Winner = P1;
            WinScore += pts_1;
            P1.RoundsWon++;
        }
        else if (pts_2 > pts_1)
        {
            Winner = P2;
            WinScore += pts_2;
            P2.RoundsWon++;
        }
        else 
        {
            P1.RoundsWon++;
            P2.RoundsWon++;
            Tie(pts_1);
        }

        //Activar la monedita
        TryFindWinner(P1, P2);
        Debug.Log("Round ended");
        if (Winner == null) Debug.Log("    ...it's a tie");
        else Debug.Log($"    ...{Winner.Name} wins");
        StartRound();
    }

    static void Tie(float points)
    {
        Winner = null;
        WinScore = points;
    }

    static void TryFindWinner(Player P1, Player P2)
    {
        if (P1.RoundsWon == 2)
        {
            if (P2.RoundsWon == 2) Game.SetWinner(null);
            else Game.SetWinner(P1);
        }
        else if (P2.RoundsWon == 2)
        {
            Game.SetWinner(P2);
        }
    }
}
