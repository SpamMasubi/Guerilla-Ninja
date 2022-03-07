using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    Transform castPoint;

    [SerializeField]
    float agroRange;

    Animator anim;

    private bool isAgros;
    private bool isAttacking;
    float nextTimeToSearch = 0;
    Vector3 lastTargetPosition;

    public GameObject enemyBullet;

    public Transform launchPoint;

    float fireRate = 3f;
    float nextShot;
    private float distToPlayer;
    public static bool isDead = false; //check if player is Dead
    public string enemyShootSFX;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        nextShot = Time.time;
        anim = GetComponent<Animator>();
        lastTargetPosition = player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);
            //print("player distance: " + distToPlayer);
        }
        if (player == null)
        {
            StopLookingForPlayer();
            FindPlayer();
            return;
        }

        if (CanSeePlayer(agroRange))
        {
            //agro player
            isAgros = true;

            if (distToPlayer <= 15)
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

        }
        else
        {
            StopLookingForPlayer();
        }


        if (isAgros && isAttacking && !isDead)
        {
            attackPlayer();
        }

    }

    bool CanSeePlayer(float distance)
    {
        bool value = false;
        var castDist = -distance;

        Vector2 endPoint = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPoint, 1 << LayerMask.NameToLayer("Player"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                value = true;
            }
            else
            {
                value = false;
            }
            Debug.DrawLine(castPoint.position, hit.point, Color.yellow);
        }
        else
        {
            Debug.DrawLine(castPoint.position, endPoint, Color.blue);
        }

        return value;
    }

    void StopLookingForPlayer()
    {
        isAgros = false;
        isAttacking = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "playerProjectiles" || collision.tag == "playerAttack")
            {
                AudioManager.instance.PlaySFX("enemyHit");
                FindObjectOfType<GameManager>().scores += Random.Range(1, 100);
                isDead = true;
                anim.SetTrigger("Killed");

            }
        }
    }

    public void EnemyDead()
    {
        Destroy(gameObject);
        isDead = false;
        isAgros = false;
        isAttacking = false;
    }

    void attackPlayer()
    {
        if (Time.time > nextShot)
        {
            AudioManager.instance.PlaySFX("shootSniper");
            GameObject projectile = Instantiate(enemyBullet, launchPoint.position, launchPoint.rotation);
            nextShot = Time.time + fireRate;
            projectile.GetComponent<EnemyBullet>().direction = -1;
        }
    }

    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchPlayer = GameObject.FindGameObjectWithTag("Player");
            if (searchPlayer != null)
                player = searchPlayer.transform;
            nextTimeToSearch = Time.time + 0.2f;
        }
    }
}
