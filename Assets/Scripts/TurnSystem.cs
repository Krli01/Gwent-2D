using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool Player1Turn;
    public TextMeshProUGUI TurnText;
    static Player Active; 

    void Start()
    {
        Player1Turn = true;
        TurnText.text = "Player 1";
    }

    // Update is called once per frame
    void Update()
    {
        TurnText.text = Player1Turn ? "Player 1" : "Player 2";
    }

    public void EndTurn()
    {
        Player1Turn = !Player1Turn;
    }

    public static Player GetActive()
    {
        Active = Player1Turn ? Game.Player1 : Game.Player2;
        return Active;
    }
}
