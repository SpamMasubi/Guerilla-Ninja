using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCounter : MonoBehaviour
{
    public Text lives;

    private void Update()
    {
        UpdateLives(FindObjectOfType<GameManager>().lives); //update lives UI
    }

    public void LoseLife()
    {
        //if no lives remaining, return
        if(FindObjectOfType<GameManager>().lives == 0)
        {
            return;
        }
        //Decrease the value of livesRemaining
        FindObjectOfType<GameManager>().lives--;
    }

    public void UpdateLives(int remainingLives)
    {
        //Convert lives remaining number to text
        lives.text = "x" + remainingLives.ToString("00");
    }

}
