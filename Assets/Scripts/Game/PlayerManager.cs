using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePlayers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePlayers()
    {
        Player[] players = FindObjectsOfType<Player>();
        Player1 = System.Array.Find(players, c => c.name == $"Player 1");
        Player2 = System.Array.Find(players, c => c.name == $"Player 2");
    }

    public void ResetPlayers()
    {
        Player1.Reset();
        Player2.Reset();
    }
}

