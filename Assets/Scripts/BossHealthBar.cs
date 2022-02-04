using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image fillbar;
    public float health = 30;
    public Text bossNameText;

    void Awake()
    {
        bossNameText.text = FindObjectOfType<BossVehicle>().bossName;
    }

    public void LoseHealth(int value)
    {
        //Do nothing if you are out of health
        if (health <= 0)
        {
            return;
        }
        //Reduce the health
        health -= value;
        //Refresh the UI fillbar
        fillbar.fillAmount = health / 100;
        //Check if your health is zero or less => Dead
        if (health <= 0)
        {
            FindObjectOfType<BossVehicle>().BossDead();
        }
    }
}
