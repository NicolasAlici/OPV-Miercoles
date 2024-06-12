using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : CustomMethods
{
    public static GameManager Instance {get; private set;}

    public override void CustomAwake()
    {
        base.CustomAwake();
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public GameObject winCanvas;
    public GameObject loseCanvas;

    public override void CustomStart()
    {
        base.CustomStart();
        Time.timeScale = 1;
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public void gameLose()
    {
        loseCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void gameWon()
    {
        winCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void goMenu()
    {

    }

}
