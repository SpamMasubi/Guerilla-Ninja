using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsPlayer;

    public float Range;
    public Transform Target;
    bool Detected = false;
    Vector2 Direction;
    public GameObject gun;
    public GameObject bullet;
    public float FireRate;
    float nextTimeToFire = 0;
    public Transform shootPoint;

    public GameObject Explosion;
    public int untilDisabled = 5;

    [Header("Invincibility Flash")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public static bool isInvincible;
    public SpriteRenderer bossSprite;

    private void Start()
    {
        Target = FindObjectOfType<Player>().transform;
        bossSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (BossStart.startBoss && (untilDisabled != 0 || !BossVehicle.isDead))
        {
            Vector2 targetPos = Target.position;
            Direction = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range, whatIsPlayer); //rayInfo will be true only if hit the player.
            if (rayInfo)
            {
                if (rayInfo.collider.gameObject.tag == "Player")
                {
                    if (Detected == false)
                    {
                        Detected = true;
                        Debug.Log("YOUVE BEEN SPOTTED " + Detected);
                    }
                }
            }
            else
            {
                if (Detected == true)
                {
                    Detected = false;
                    Debug.Log("Oh He gone " + Detected);
                }
            }

            if (Detected == true)
            {
                gun.transform.up = -Direction;
                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1 / FireRate;
                    Shoot();
                }
            }
        }
    }

    void Shoot()
    {
        GameObject bullets = Instantiate(bullet, shootPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (untilDisabled != 0 && (collision.tag == "playerProjectiles" || collision.tag == "playerAttack"))
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

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }

}
