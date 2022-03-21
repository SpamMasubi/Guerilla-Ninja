using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class itemSpawn
{
    public GameObject item;
    public float spawnRate;
    [HideInInspector]public float minSpawnProb, maxSpawnProb;
}

public class Loot : MonoBehaviour
{
    public itemSpawn[] itemsDrop;
    public Transform dropPoint;

    private Animator anim;
    public static bool playerInZone;
    private AudioSource sfx;
    private bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();
        for (int i = 0; i < itemsDrop.Length; i++)
        {

            if (i == 0)
            {
                itemsDrop[i].minSpawnProb = 0;
                itemsDrop[i].maxSpawnProb = itemsDrop[i].spawnRate - 1;
            }
            else
            {
                itemsDrop[i].minSpawnProb = itemsDrop[i - 1].maxSpawnProb + 1;
                itemsDrop[i].maxSpawnProb = itemsDrop[i].minSpawnProb + itemsDrop[i].spawnRate - 1;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOpen)
        {
            if (Input.GetButton("Fire1") && playerInZone)
            {
                anim.SetTrigger("Open");
                sfx.Play();
                lootItems();
                isOpen = true;
            }
        }
    }

    void lootItems()
    {
        float randomNum = Random.Range(0, 100);

        for (int i = 0; i < itemsDrop.Length; i++)
        {
            if(randomNum>=itemsDrop[i].minSpawnProb && randomNum<= itemsDrop[i].maxSpawnProb)
            {
                Instantiate(itemsDrop[i].item, dropPoint.position, dropPoint.rotation);
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInZone = false;
        }
    }

    public IEnumerator destroyCrate()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        isOpen = false;
    }

}
