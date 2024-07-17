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
    public GameObject uICamvas;

    public override void CustomStart()
    {
        base.CustomStart();
        Time.timeScale = 1;
        uICamvas.SetActive(true);
        winCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(Input.GetKey(KeyCode.P))
        {
            goMenu();
        }
    }

    public void gameLose()
    {
        loseCanvas.SetActive(true);
        uICamvas.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void gameWon()
    {
        winCanvas.SetActive(true);
        uICamvas.SetActive(false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void goMenu()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

}
