using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource playGame;
    private bool hasStarted;

    void Start()
    {
        playGame = GetComponent<AudioSource>();
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
   
    public void StoryPlot()
    {

    }
}
