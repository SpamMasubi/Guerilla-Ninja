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
        if (FindObjectOfType<BossVehicle>() != null)
        {
            bossNameText.text = FindObjectOfType<BossVehicle>().bossName;
            health = FindObjectOfType<BossVehicle>().bossHealth;
        }
        if (FindObjectOfType<FinalBoss>() != null)
        {
            bossNameText.text = FindObjectOfType<FinalBoss>().bossName;
            health = FindObjectOfType<FinalBoss>().bossHealth;
        }
    }

    public void LoseHealth(int value)
    {
        if (FindObjectOfType<BossVehicle>() != null)
        {
            //Do nothing if you are out of health
            if (FindObjectOfType<BossVehicle>().bossHealth <= 0)
            {
                return;
            }
            //Reduce the health
            FindObjectOfType<BossVehicle>().bossHealth -= value;
            //Refresh the UI fillbar
            fillbar.fillAmount = FindObjectOfType<BossVehicle>().bossHealth / health;
            //Check if your health is zero or less => Dead
            if (FindObjectOfType<BossVehicle>().bossHealth <= 0)
            {
                FindObjectOfType<BossVehicle>().BossDead();
            }
        }
        else if(FindObjectOfType<FinalBoss>() != null)
        {
            //Do nothing if you are out of health
            if (FindObjectOfType<FinalBoss>().bossHealth <= 0)
            {
                return;
            }
            //Reduce the health
            FindObjectOfType<FinalBoss>().bossHealth -= value;
            //Refresh the UI fillbar
            fillbar.fillAmount = FindObjectOfType<FinalBoss>().bossHealth / health;
            //Check if your health is zero or less => Dead
            if (FindObjectOfType<FinalBoss>().bossHealth <= 0)
            {
                FindObjectOfType<FinalBoss>().BossDead();
            }
        }
    }
}
