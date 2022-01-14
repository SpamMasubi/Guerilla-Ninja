using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public Text scores, shurikenCounter, HACounter, ARACounter;

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStatsUI();
    }

    public void UpdatePlayerStatsUI()
    {
        scores.text = "Score: " + FindObjectOfType<GameManager>().scores.ToString("0000000");
        shurikenCounter.text = "x" + FindObjectOfType<GameManager>().shurikenCount.ToString("00");
        HACounter.text = "x" + FindObjectOfType<GameManager>().handgunAmmo.ToString("00");
        ARACounter.text = "x" + FindObjectOfType<GameManager>().assaultRifleAmmo.ToString("00");
    }
}
