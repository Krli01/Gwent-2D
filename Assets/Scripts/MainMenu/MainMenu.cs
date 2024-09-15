using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void OpenEditor()
    {
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
