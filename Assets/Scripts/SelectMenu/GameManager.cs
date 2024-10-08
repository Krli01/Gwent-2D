using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public string Player1Name { get; private set; }
    public string Player1Faction { get; private set; }
    public string Player2Name { get; private set; }
    public string Player2Faction { get; private set; }

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
        Debug.Log($"Player {playerNumber}: Name {name}, Faction {faction}");
    }

    public void StartGame()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }
}