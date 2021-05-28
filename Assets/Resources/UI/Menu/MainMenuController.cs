using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public void StartButton()
    {
        GameState.currentLevel = 1;
        SceneManager.LoadScene("GameLoop");
    }
    public void LoadButton()
    {
        GameState.currentLevel = 3;
        //GameState.loadGame();
        SceneManager.LoadScene("GameLoop");
    }

    public void SettingsButton()
    {

    }
    public void AboutButton()
    {

    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
