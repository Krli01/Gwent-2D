using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PreGame : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button[] factionButtons;
    public Button doneButton;
    public TextMeshProUGUI doneButtonText;

    private int currentPlayer = 1;
    private string selectedFaction;

    void Start()
    {
        nameInput.text = "Jugador 1";
        doneButton.interactable = false;
        doneButtonText.text = "Listo";

        for (int i = 0; i < factionButtons.Length; i++)
        {
            int index = i;
            factionButtons[i].onClick.AddListener(() => SelectFaction(index));
        }

        doneButton.onClick.AddListener(OnDoneButtonClick);
    }

    void SelectFaction(int factionIndex)
    {
        selectedFaction = factionIndex switch
        {
            0 => "Pirate",
            1 => "Whaler",
            2 => "Seaborn",
            _ => "Unknown"
        };

        doneButton.interactable = !string.IsNullOrEmpty(nameInput.text);
    }

    void OnDoneButtonClick()
    {
        GameManager.Instance.SavePlayerInfo(currentPlayer, nameInput.text, selectedFaction);

        if (currentPlayer == 1)
        {
            // Switch to Player 2
            currentPlayer = 2;
            nameInput.text = "Jugador 2";
            selectedFaction = null;
            doneButton.interactable = false;
            doneButtonText.text = "Jugar";
        }
        else
        {
            ResetSelectMenu();
            GameManager.Instance.StartGame();
        }
    }

    void ResetSelectMenu()
    {
        nameInput.text = "Jugador 1";
        doneButton.interactable = false;
        doneButtonText.text = "Listo";
    }
}
