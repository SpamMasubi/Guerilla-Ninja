using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    public static bool startBoss;

    public GameObject exitClosed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (FindObjectOfType<BossVehicle>() != null)
            {
                if (FindObjectOfType<BossVehicle>().name == "North Star Army AH (Boss)")
                {
                    FindObjectOfType<BossVehicle>().GetComponent<AudioSource>().enabled = true;
                    FindObjectOfType<BossVehicle>().GetComponent<Animator>().enabled = true;
                }
                else if (FindObjectOfType<BossVehicle>().name == "North Star Army Tank (Boss)")
                {
                    FindObjectOfType<BossVehicle>().GetComponent<Animator>().enabled = true;
                }
            }
            else if (FindObjectOfType<FinalBoss>() != null)
            {
                if (FindObjectOfType<FinalBoss>().name == "Final Boss")
                {
                    FindObjectOfType<FinalBoss>().GetComponent<Animator>().enabled = true;
                    FindObjectOfType<FinalBoss>().GetComponent<AudioSource>().Play();
                }
            }
            startBoss = true;
            exitClosed.SetActive(true);
            FindObjectOfType<Canvas>().gameObject.transform.GetChild(4).gameObject.SetActive(true);
            FindObjectOfType<PlayMusic>().PlaySong(FindObjectOfType<PlayMusic>().bossSong);
            Destroy(gameObject,1f);
        }
    }
}
