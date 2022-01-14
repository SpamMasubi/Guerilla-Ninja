using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource playGame;

    void Start()
    {
        playGame = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        playGame.Play();
        Invoke("LoadScene", 3f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
   
    public void StoryPlot()
    {

    }
}
