using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletLauncher;

    public float agroRange;
    public GameObject player;

    public GameObject Explosion;
    public int untilDisabled = 5;

    [Header("Invincibility Flash")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public static bool isInvincible;
    public SpriteRenderer bossSprite;

    public float fireRate;
    float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
        player = FindObjectOfType<Player>().gameObject;
        bossSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BossStart.startBoss && (untilDisabled != 0 || !BossVehicle.isDead))
        {
            float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distToPlayer > agroRange || distToPlayer < agroRange)
            {
                CheckIfTimeToFire();
            }
        }
    }

    void CheckIfTimeToFire()
    {
        if(Time.time > nextFire && (transform.position.x > player.transform.position.x))
        {
            Instantiate(bullet, bulletLauncher.position, bulletLauncher.rotation);
            nextFire = Time.time + fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(untilDisabled != 0 && (collision.tag == "playerProjectiles" || collision.tag == "playerAttack"))
        {
            if (!isInvincible)
            {
                StartCoroutine(InvincibilityFlash());
                untilDisabled--;
                AudioManager.instance.PlaySFX("bossDamage");
                if (untilDisabled == 0)
                {
                    ExplosionEffect();
                    isInvincible = false;
                    Destroy(gameObject);
                }
            }
            else
            {
                AudioManager.instance.PlaySFX("bossHit");
            }
        }
    }

    private IEnumerator InvincibilityFlash()
    {
        int temp = 0;
        isInvincible = true;
        while (temp < numberOfFlashes)
        {
            bossSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            bossSprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        isInvincible = false;
    }

    //Function to instantiate explosion
    void ExplosionEffect()
    {
        AudioManager.instance.PlaySFX("explosion");
        //instiantiate explosion effect
        GameObject explode = (GameObject)Instantiate(Explosion);

        //set explosion position
        explode.transform.position = transform.position;
    }
}
