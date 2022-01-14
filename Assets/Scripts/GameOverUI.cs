using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverUIMain;
    public GameObject gameOverOptionPanel;
    public static bool retry;
    private int delayEnable = 5;

    // Update is called once per frame
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
    }

    public void Retry()
    {
        FindObjectOfType<PlayMusic>().ResumeMusic();
        retry = true;
        FindObjectOfType<Player>().Retry();
    }
}
