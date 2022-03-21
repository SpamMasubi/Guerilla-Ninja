using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUIMain;
    public GameObject gameOverOptionPanel;
    public static bool retry;
    public GameObject retryButton;

    private int delayEnable = 5;

    private void Start()
    {
        //Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    void Update()
    {
        if (Player.gameOver)
        {
            FindObjectOfType<PlayMusic>().StopSong();
            gameOverUIMain.SetActive(true);
            StartCoroutine(enableOption());
        }
        else
        {
            gameOverUIMain.SetActive(false);
            gameOverOptionPanel.SetActive(false);
        }
    }

    public IEnumerator enableOption()
    {
        yield return new WaitForSeconds(delayEnable);
        gameOverOptionPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Player.isDead = false;
        Player.gameOver = false;
        gameOverUIMain.SetActive(false);
        gameOverOptionPanel.SetActive(false);
    }

    public void Retry()
    {
        FindObjectOfType<PlayMusic>().ResumeMusic();
        retry = true;
        FindObjectOfType<Player>().Retry();
    }
}
