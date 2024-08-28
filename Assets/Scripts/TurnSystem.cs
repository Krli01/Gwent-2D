using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Player1Turn;
    public TextMeshProUGUI TurnText;

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
}
