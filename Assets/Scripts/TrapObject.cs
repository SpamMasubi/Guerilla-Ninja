using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapObject : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<Animator>().SetBool("TrapTrigger", true);
            AudioManager.instance.PlaySFX("explosion");
            if (!Player.isInvincible)
            {
                FindObjectOfType<Healthbar>().LoseHealth(5);
            }
        }
    }

    public void resetTrap()
    {
        GetComponent<Animator>().SetBool("TrapTrigger", false);
        Destroy(gameObject, 0.1f);
    }
}
