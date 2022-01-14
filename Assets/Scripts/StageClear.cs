using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClear : MonoBehaviour
{
    public GameObject stageClearUI;
    // Update is called once per frame
    void Update()
    {
        if (BossVehicle.stageClear)
        {
            stageClearUI.SetActive(true);
            Invoke("LoadScene", 6f);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("TempScene");
        BossVehicle.isDead = false;
        BossVehicle.stageClear = false;
        BossStart.startBoss = false;
        Destroy(FindObjectOfType<Canvas>().gameObject);
        Destroy(FindObjectOfType<GameManager>().gameObject);
    }
}
