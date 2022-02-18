using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image fillbar;
    float health;
    public Text bossNameText;

    void Awake()
    {
        bossNameText.text = FindObjectOfType<BossVehicle>().bossName;
        health = FindObjectOfType<BossVehicle>().bossHealth;
    }

    public void LoseHealth(int value)
    {
        //Do nothing if you are out of health
        if (FindObjectOfType<BossVehicle>().bossHealth <= 0)
        {
            return;
        }
        //Reduce the health
        FindObjectOfType<BossVehicle>().bossHealth -= value;
        //Refresh the UI fillbar
        fillbar.fillAmount = FindObjectOfType<BossVehicle>().bossHealth/health;
        //Check if your health is zero or less => Dead
        if (FindObjectOfType<BossVehicle>().bossHealth <= 0)
        {
            FindObjectOfType<BossVehicle>().BossDead();
        }
    }
}
