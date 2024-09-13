using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    void Start()
    {
        InitializePersistentObjects();
        LoadMainMenu();
    }

    void InitializePersistentObjects()
    {
        if (CardDatabase.Instance == null)
        {
            GameObject cardDatabaseObject = new GameObject("CardDatabase");
            cardDatabaseObject.AddComponent<CardDatabase>();
        }
        if (DisplayCardDatabase.Instance == null)
        {
            GameObject cardDatabaseObject = new GameObject("DisplayCardDatabase");
            cardDatabaseObject.AddComponent<DisplayCardDatabase>();
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
