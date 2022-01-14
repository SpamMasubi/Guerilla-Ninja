using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image fillbar;

    public void UpdateHealth(float respawnHealth)
    {
        FindObjectOfType<GameManager>().health = respawnHealth;
        fillbar.fillAmount = FindObjectOfType<GameManager>().health / 100;
    }

    public void LoseHealth(int value)
    {
        //Do nothing if you are out of health
        if(FindObjectOfType<GameManager>().health <= 0)
        {
            return;
        }
        //Reduce the health
        FindObjectOfType<GameManager>().health -= value;
        //Refresh the UI fillbar
        fillbar.fillAmount = FindObjectOfType<GameManager>().health / 100;
        //Check if your health is zero or less => Dead
        if(FindObjectOfType<GameManager>().health <= 0)
        {
            FindObjectOfType<Player>().Die();
        }
    }

    public void gainHealth(int value)
    {
        //increase the health
        FindObjectOfType<GameManager>().health += value;
        
        //Play health consume sound
        AudioManager.instance.PlaySFX("HealthRestore");

        //Do increase if you are in of full health
        if (FindObjectOfType<GameManager>().health < 100)
        {
            //Refresh the UI fillbar
            fillbar.fillAmount = FindObjectOfType<GameManager>().health / 100;
        }
        else if(FindObjectOfType<GameManager>().health >= 100)
        {
            //if gain health goes over max health, set to max health(100)
            FindObjectOfType<GameManager>().health = 100;
            //Refresh the UI fillbar
            fillbar.fillAmount = FindObjectOfType<GameManager>().health / 100;
        }
        
    }
}
