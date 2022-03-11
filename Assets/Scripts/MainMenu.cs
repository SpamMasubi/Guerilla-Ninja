using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private AudioSource playGame;
    private bool hasStarted;
    public GameObject StoryMenu, mainMenu;

    public GameObject startGameButton, storyBackButton, storyCloseButton;

    void Start()
    {
        playGame = GetComponentInChildren<AudioSource>();

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    public void PlayGame()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            playGame.Play();
            Invoke("LoadScene", 3f);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadScene()
    {
        hasStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void storyOpen()
    {
        StoryMenu.SetActive(true);
        mainMenu.SetActive(false);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(storyBackButton);
    }

    public void storyClose()
    {
        StoryMenu.SetActive(false);
        mainMenu.SetActive(true);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(storyCloseButton);
    }
}
