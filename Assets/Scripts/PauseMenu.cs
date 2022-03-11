using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, confirmQuit;
    public GameObject resumeButton, quitButton, yesButton;

    public static bool isPause;

    // Update is called once per frame
    void Update()
    {
        if (!BossVehicle.isDead)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                PauseUnPause();
            }
        }
    }

    void PauseUnPause()
    {
        if (!pauseMenu.activeInHierarchy && !confirmQuit.activeInHierarchy)
        {
            isPause = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            //Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new selected object
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else
        {
            isPause = false;
            pauseMenu.SetActive(false);
            confirmQuit.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        isPause = false;
        pauseMenu.SetActive(false);
        confirmQuit.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        confirmQuit.SetActive(true);
        pauseMenu.SetActive(false);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(yesButton);
    }

    public void BackToPauseMenu()
    {
        confirmQuit.SetActive(false);
        pauseMenu.SetActive(true);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(quitButton);
    }

    public void ConfirmationQuit()
    {
        Destroy(FindObjectOfType<Canvas>().gameObject);
        Destroy(FindObjectOfType<PlayMusic>().gameObject);
        LoadMainMenu();
        Time.timeScale = 1f;
    }

    void LoadMainMenu()
    {
        isPause = false;
        BossVehicle.isDead = false;
        BossVehicle.stageClear = false;
        BossStart.startBoss = false;
        SceneManager.LoadScene("Title");
    }
}
