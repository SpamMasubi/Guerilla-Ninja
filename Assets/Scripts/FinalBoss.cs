using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    //For Idle Stage
    [Header("Idle")]
    [SerializeField] float idleMovement;
    [SerializeField] Vector2 idleMoveDirection;
    [SerializeField] Transform player;
    //Other
    [Header("Other")]
    [SerializeField] Transform groundCheckUp;
    [SerializeField] Transform groundCheckDown;
    [SerializeField] Transform groundCheckWallFront;
    [SerializeField] Transform groundCheckWallBack;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float wallCheckRadius;
    [SerializeField] LayerMask wallLayer;
    private bool isTouchingUp;
    private bool isTouchingDown;
    private bool isTouchingWallF;
    private bool isTouchingWallB;
    private bool goingUp = true;
    private bool goingLeft = true;
    private bool facingLeft = true;
    private Rigidbody2D enemyRB;
    private Animator enemyAnim;
    //Other
    [Header("Final Boss Attributes")]
    public static bool isDead = false; //check if player is Dead
    public static bool stageClear = false;
    public GameObject bullet;
    public Transform bulletLauncher;

    public GameObject Explosion;

    [Header("Invincibility Flash")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public static bool isInvincible;
    public SpriteRenderer bossSprite;

    public string bossName;
    public int bossHealth;
    public static bool gameComplete;

    // Start is called before the first frame update
    void Start()
    {
        idleMoveDirection.Normalize();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        bossSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BossStart.startBoss)
        {
            isTouchingUp = Physics2D.OverlapCircle(groundCheckUp.position, groundCheckRadius, groundLayer);
            isTouchingDown = Physics2D.OverlapCircle(groundCheckDown.position, groundCheckRadius, groundLayer);
            isTouchingWallF = Physics2D.OverlapCircle(groundCheckWallFront.position, wallCheckRadius, wallLayer);
            isTouchingWallB = Physics2D.OverlapCircle(groundCheckWallBack.position, wallCheckRadius, wallLayer);
            IdleState();
            FlipTowardsPlayer();
        }
    }

    public void IdleState()
    {
        if(isTouchingUp && goingUp)
        {
            ChangeDirectionY();
        }
        else if(isTouchingDown && !goingUp)
        {
            ChangeDirectionY();
        }

        if (isTouchingWallF || isTouchingWallB)
        {
            if (goingLeft)
            {
                ChangeDirectionX();
            }
            else if (!goingLeft)
            {
                ChangeDirectionX();
            }

        }
        enemyRB.velocity = idleMovement * idleMoveDirection;
    }

    void FlipTowardsPlayer()
    {
        float playerDirection = player.position.x - transform.position.x;

        if(playerDirection > 0 && facingLeft)
        {
            Flip();
        }
        else if(playerDirection < 0 && !facingLeft)
        {
            Flip();
        }
    }

    void ChangeDirectionY()
    {
        goingUp = !goingUp;
        idleMoveDirection.y *= -1;
    }

    void ChangeDirectionX()
    {
        goingLeft = !goingLeft;
        idleMoveDirection.x *= -1;
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        transform.Rotate(0, 180, 0);
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "playerProjectiles" || collision.tag == "playerAttack")
            {
                if (!isInvincible && collision.tag == "playerProjectiles")
                {
                    if (SwitchWeapons.shuriken)
                    {
                        StartCoroutine(InvincibilityFlash());
                        FindObjectOfType<BossHealthBar>().LoseHealth(2);
                        AudioManager.instance.PlaySFX("GeneralHurt");
                    }
                    else if (SwitchWeapons.handgun)
                    {
                        StartCoroutine(InvincibilityFlash());
                        FindObjectOfType<BossHealthBar>().LoseHealth(2);
                        AudioManager.instance.PlaySFX("GeneralHurt");
                    }
                    else if (SwitchWeapons.AR)
                    {
                        StartCoroutine(InvincibilityFlash());
                        FindObjectOfType<BossHealthBar>().LoseHealth(3);
                        AudioManager.instance.PlaySFX("GeneralHurt");
                    }
                    enemyAnim.SetTrigger("Hurt");
                }
                else if (!isInvincible && collision.tag == "playerAttack")
                {
                    StartCoroutine(InvincibilityFlash());
                    FindObjectOfType<BossHealthBar>().LoseHealth(1);
                    AudioManager.instance.PlaySFX("GeneralHurt");
                    enemyAnim.SetTrigger("Hurt");
                }
                else
                {
                    AudioManager.instance.PlaySFX("bossHit");
                }
            }
        }
    }

    public void BossDead()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.03f;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        AudioManager.instance.PlaySFX("GeneralDeath");
        isDead = true;
        ExplosionEffect();
        Destroy(FindObjectOfType<PlayMusic>().gameObject);
        GetComponent<FinalBoss>().enabled = false;
        FindObjectOfType<FinalBoss>().GetComponentInChildren<BossSpecial>().enabled = false;
        enemyAnim.SetTrigger("Defeated");
    }

    //Function to instantiate explosion
    void ExplosionEffect()
    {
        //instiantiate explosion effect
        GameObject explode = (GameObject)Instantiate(Explosion);

        //set explosion position
        explode.transform.position = transform.position;
    }

    void firePistol()
    {
        AudioManager.instance.PlaySFX("shootHandgun");
        GameObject projectile = Instantiate(bullet, bulletLauncher.position, bulletLauncher.rotation);
        if (!facingLeft)
        {
            projectile.GetComponent<EnemyBullet>().direction = 1;
        }
        else
        {
            projectile.GetComponent<EnemyBullet>().direction = -1;
        }
    }

    public void StageClear()
    {
        ChapterIntro.chapters += 1;
        stageClear = true;
        AudioManager.instance.PlaySFX("Win");
        gameComplete = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheckUp.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckDown.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWallFront.position, wallCheckRadius);
        Gizmos.DrawWireSphere(groundCheckWallBack.position, wallCheckRadius);
    }
}
