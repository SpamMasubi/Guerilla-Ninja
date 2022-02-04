using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action HasLanded;

    public float speed = 1; //player speed
    public float jumpPower = 150;
    const float groundCheckRadius = 0.2f;
    float horizontalValue; //horizontal values
    float runSpeedModifier = 2f; //speed modifier for running

    int availableJumps; //how many jumps left

    bool facingRight = true; //facing right
    [HideInInspector]
    public bool isRunning; //player isn't running

    bool multipleJumps; //jump more than once
    bool coyoteJump; //coyote jump
    bool isGrounded; //ground checker
    [HideInInspector]
    public static bool isDead = false; //check if player is Dead
    public static bool gameOver;
    private bool isHurt;
   
    
    Rigidbody2D rb2d; //Rigidbody2D
    Animator animator; //Player Animator
    
    [SerializeField] Transform groundChecker;
    [SerializeField]LayerMask groundLayer;
    [SerializeField] int totalJumps; //total amount of jumps

    public int fallBoundary = -5;

    public float playerHealth = 100;
    int spawnDelay = 5;

    [Header("Invincibility Flash")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public static bool isInvincible;
    public SpriteRenderer playerSprite;


    void Awake()
    {
        availableJumps = totalJumps;

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= fallBoundary)
        {
            FindObjectOfType<Healthbar>().LoseHealth(999999);
        }

        if (!canMove() || BossVehicle.isDead)
        {
            return;
        }
        //Set yVelocity in animator
        animator.SetFloat("yVelocity", rb2d.velocity.y);
        //store horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //if shift keys are pressed down, running is true
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift)) //running is flase if shift keys are released
        {
            isRunning = false;
        }

        //if pressed jump, enable jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        
    }

    void FixedUpdate()
    {

        GroundCheck();
        if (!isDead && !isHurt)
        {
            PlayerMove(horizontalValue); //calls function
        }
        else
        {
            rb2d.velocity = Vector2.zero;
            horizontalValue = 0;
        }
        
    }

    bool canMove()
    {
        bool can = true;
        if (isDead || BossVehicle.isDead)
        {
            isRunning = false;
            can = false;
        }

        return can;
    }

    void GroundCheck() //Check if Groundcheck is colliding with Ground Layered object
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;

                //Trigger the hasLanded Event
                HasLanded?.Invoke();
            }

            //Check if any of the colliders is moving platform
            //Parent it to this transform
            foreach(var c in colliders)
            {
                if(c.tag == "MovingPlatform")
                {
                    transform.parent = c.transform;
                }
            }
        }
        else
        {
            //Un-parent the transform
            transform.parent = null;

            if (wasGrounded)
            {
                StartCoroutine(CoyoteJumpDelay());
            }
        }

        //As long as we are grounded, then "Jump" bool animator is disable
        animator.SetBool("Jump", !isGrounded);
    }

    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }

    void Jump()
    {
        //if pressed space and grounded, Player jump
        if (isGrounded)
        {
            multipleJumps = true;
            availableJumps--;

            //add jump force
            rb2d.velocity = Vector2.up * jumpPower;
            animator.SetBool("Jump", true);
            AudioManager.instance.PlaySFX("jump");
        }
        else
        {
            if (coyoteJump)
            {
                multipleJumps = true;
                availableJumps--;

                //add jump force
                rb2d.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
            }

            if (multipleJumps && availableJumps > 0)
            {
                availableJumps--;

                //add jump force
                rb2d.velocity = Vector2.up * jumpPower;
                animator.SetBool("Jump", true);
                AudioManager.instance.PlaySFX("jump");
            }
        }
    }

    void PlayerMove(float direction) //functions for Player movements
    {
        #region Move & Run
        //Set value of x using direction and speed
        float xVal = direction * speed * 100 * Time.fixedDeltaTime;
        if (isRunning)//if running, multiply the running speed
        {
            xVal *= runSpeedModifier;
        }
        else if (BossVehicle.isDead)
        {
            xVal = 0;
        }
        //Create Vector2 for velocity 
        Vector2 targetVelocity = new Vector2(xVal, rb2d.velocity.y);
        //Set the velocity of the player
        rb2d.velocity = targetVelocity;
        

        /**
        ////Store current scale value
        Vector3 currentScale = transform.localScale;
        **/
        ////if looking right and press left (flip left)
        if (facingRight && direction < 0)
        {
            //currentScale.x = -1;
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (!facingRight && direction > 0)
        ////if looking left and press right(flip right)
        {
            //currentScale.x = 1;
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }


        //0 idle, 6 walk, 12 idle
        //Set float xVelocity according to the x value of the Rigidbody2D velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb2d.velocity.x));

        //currentScale = transform.localScale;
        #endregion

    }

    private IEnumerator InvincibilityFlash()
    {
        int temp = 0;
        isInvincible = true;
        while(temp < numberOfFlashes)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvincible && !isDead && !ShootingOrAttack.isAttack && !EnemyAI.isDead)
        {
            //if player collides with trap or enemy
            if (collision.tag == "Trap" || collision.tag == "Enemy" || collision.tag == "enemyProjectile")
            {
                isHurt = true;
                StartCoroutine(InvincibilityFlash());
                if (collision.tag == "Enemy")
                {
                    FindObjectOfType<Healthbar>().LoseHealth(2);
                }

                //Play Sound and animation
                AudioManager.instance.PlaySFX("hurt");
                animator.SetTrigger("Hurt");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundChecker.position, groundCheckRadius);
    }

    public void PlayerRunSFX()
    {
        AudioManager.instance.PlaySFX("running");
    }

    public void Die()
    {
        isRunning = false;
        AudioManager.instance.PlaySFX("dead");
        isDead = true;
        animator.SetTrigger("isDead");
        FindObjectOfType<LifeCounter>().LoseLife();
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        if (FindObjectOfType<GameManager>().lives > 0)
        {
            yield return new WaitForSeconds(spawnDelay);
            ResetPlayer();
            isDead = false;
            FindObjectOfType<Healthbar>().UpdateHealth(playerHealth);
            StartCoroutine(InvincibilityFlash());
            animator.Rebind();
            if (!BossStart.startBoss)
            {
                float minWidth = Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).x;
                transform.position = new Vector2(minWidth, 4f);
            }
            AudioManager.instance.PlaySFX("Respawn");
        }
        else
        {
            gameOver = true;
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void Retry()
    {
        StartCoroutine(InvincibilityFlash());
        gameOver = false;
        ResetPlayer();
        isDead = false;
        FindObjectOfType<Healthbar>().UpdateHealth(playerHealth);
        FindObjectOfType<GameManager>().lives = 4;
        GameOverUI.retry = false;
        animator.Rebind();
        if (!BossStart.startBoss)
        {
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector2(10, 10)).x;
            transform.position = new Vector2(minWidth, 4f);
        }
        AudioManager.instance.PlaySFX("Respawn");
    }

    public void ResetPlayer()
    {
        horizontalValue = 0;
        isHurt = false;
        ShootingOrAttack.isAttack = false;
    }
}
