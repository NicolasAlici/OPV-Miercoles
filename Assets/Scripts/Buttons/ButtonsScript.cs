using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonsScript : CustomMethods
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    
}
