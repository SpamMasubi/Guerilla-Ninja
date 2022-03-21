using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private AudioSource playGame;
    private bool hasStarted;
    public GameObject StoryMenu, mainMenu, WebGLStoryMenu;
    public GameObject WebGLMenu, StandAloneMenu; //StandAlone and WebGLMenu

    public GameObject startGameButton, storyBackButton, storyCloseButton;
    public GameObject WebGLstartGameButton, WebGLstoryBackButton, WebGLstoryCloseButton; //WebGL

    void Start()
    {
#if UNITY_WEBGL
        WebGLMenu.SetActive(true);
        StandAloneMenu.SetActive(false);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(WebGLstartGameButton);
        playGame = GetComponentInChildren<AudioSource>();

#endif

#if UNITY_STANDALONE

        playGame = GetComponentInChildren<AudioSource>();

        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(startGameButton);

#endif

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
#if UNITY_WEBGL
        WebGLStoryMenu.SetActive(true);
        WebGLMenu.SetActive(false);
        StandAloneMenu.SetActive(false);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(WebGLstoryBackButton);
#endif

#if UNITY_STANDALONE
        StoryMenu.SetActive(true);
        mainMenu.SetActive(false);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(storyBackButton);
#endif

    }

    public void storyClose()
    {
#if UNITY_WEBGL
        WebGLStoryMenu.SetActive(false);
        WebGLMenu.SetActive(true);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(WebGLstoryCloseButton);
#endif

#if UNITY_STANDALONE
        StoryMenu.SetActive(false);
        mainMenu.SetActive(true);
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(storyCloseButton);
#endif
    }
}
