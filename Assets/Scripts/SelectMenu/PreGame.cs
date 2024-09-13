using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PreGame : MonoBehaviour
{
    public string player1NameInput;
    public string player1Faction;
    public string player2NameInput;
    public string player2Faction;

    public void SaveAndStartGame()
    {
        // Ensure GameManager instance exists
        if (GameManager.Instance == null)
        {
            GameObject gameManagerObject = new GameObject("GameManager");
            gameManagerObject.AddComponent<GameManager>();
        }

        // Save player information
        GameManager.Instance.SavePlayerInfo(1, player1NameInput, player1Faction);
        GameManager.Instance.SavePlayerInfo(2,  player2NameInput, player2Faction);

        // L        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
