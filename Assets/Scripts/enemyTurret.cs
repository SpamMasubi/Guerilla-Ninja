using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class enemyTurret : MonoBehaviour
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

    private void Start()
    {
        Target = FindObjectOfType<Player>().transform;
    }

    void Update()
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
                }
            }
        }
        else
        {
            if (Detected == true)
            {
                Detected = false;
            }
        }

        if (Detected == true)
        {
            gun.transform.up = Direction;
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1 / FireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject bullets = Instantiate(bullet, shootPoint.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "playerProjectiles" || collision.tag == "playerAttack")
        {
            FindObjectOfType<GameManager>().scores += Random.Range(1, 100);
            ExplosionEffect();
            Destroy(gameObject);
        }
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
