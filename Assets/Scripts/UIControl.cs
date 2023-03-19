using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIControl : MonoBehaviour
{
    private bool canPause = true;
    public GameObject PauseMenu;
    public GameObject WinPanel;
    public GameObject LosePanel;

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        openPauseMenu();
        Debug.Log(Time.timeScale);
    }

    void openPauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(canPause)
            {
                PauseMenu.SetActive(true);
                canPause = false;
                Time.timeScale = 0;
                AudioListener.volume = 0;
            }
            else
            {
                PauseMenu.SetActive(false);
                canPause = true;
                Time.timeScale = 1;
            }
        }
    }

    public void openLosePanel()
    {
        if(!Player.activeInHierarchy)
        {
            LosePanel.SetActive(true);
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.volume = 1;
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        canPause = true;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        PauseMenu.SetActive(false);
        canPause = true;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Exit()
    {
        Application.Quit();
    }


}           