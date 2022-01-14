using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum ItemType {POINTS, AMMUNITIONS, HEALTH, LIFE} //type of interaction
    [Header("Attributes")]
    public ItemType item;
    public int points, healthRestore, ammo, life;
    Rigidbody2D rb;

    private void Start()
    {
        if(gameObject.tag == "Ammunitions")
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    void Reset() //reset function
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void itemTypes() //interaction of items
    {
        switch (item)
        {
            case ItemType.POINTS:
                if (gameObject.name == "Big Money")
                {
                    AudioManager.instance.PlaySFX("Points");
                    FindObjectOfType<GameManager>().scores += points;
                }
                else
                {
                    AudioManager.instance.PlaySFX("Points");
                    FindObjectOfType<GameManager>().scores += points;
                }
                break;
            case ItemType.AMMUNITIONS:
                if (gameObject.name == "AR Ammo" || gameObject.name == "AR Ammo(Clone)")
                {
                    AudioManager.instance.PlaySFX("TakeAmmo");
                    FindObjectOfType<GameManager>().assaultRifleAmmo += ammo;
                }
                else if (gameObject.name == "Shuriken Ammo" || gameObject.name == "Shuriken Ammo(Clone)")
                {
                    AudioManager.instance.PlaySFX("TakeAmmo");
                    FindObjectOfType<GameManager>().shurikenCount += ammo;

                }
                else
                {
                    AudioManager.instance.PlaySFX("TakeAmmo");
                    FindObjectOfType<GameManager>().handgunAmmo += ammo;
                }
                break;
            case ItemType.HEALTH:
                if(gameObject.name == "Bun Rieu")
                {
                    FindObjectOfType<Healthbar>().gainHealth(healthRestore);
                }
                else
                {
                    FindObjectOfType<Healthbar>().gainHealth(healthRestore);
                }
                break;
            case ItemType.LIFE:
                AudioManager.instance.PlaySFX("LifeRestore");
                FindObjectOfType<GameManager>().lives += 1;
                break;
            default:
                Debug.Log("Null");
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            itemTypes();
            Destroy(gameObject);
        }
    }
}
