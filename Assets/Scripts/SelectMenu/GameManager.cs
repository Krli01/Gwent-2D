using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string Player1Name { get; set; }
    public string Player1Faction { get; set; }
    public string Player2Name { get; set; }
    public string Player2Faction { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerInfo(int playerNumber, string name, string faction)
    {
        if (playerNumber == 1)
        {
            Player1Name = name;
            Player1Faction = faction;
        }
        else if (playerNumber == 2)
        {
            Player2Name = name;
            Player2Faction = faction;
        }
    }
}