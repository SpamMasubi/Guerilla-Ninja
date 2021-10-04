using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1; //player speed
    public float jumpPower = 150;
    const float groundCheckRadius = 0.2f;
    Rigidbody2D rb2d; //Rigidbody2D
    Animator animator; //Player Animator
    float horizontalValue; //horizontal values
    float runSpeedModifier = 2f; //speed modifier for running
    bool facingRight = true; //facing right
    bool isRunning; //player isn't running
    int availableJumps; //how many jumps left
    bool multipleJumps; //jump more than once
    bool coyoteJump; //coyote jump

    [SerializeField] Transform groundChecker;
    [SerializeField]LayerMask groundLayer;
    [SerializeField]bool isGrounded; //ground checker
    [SerializeField] int totalJumps; //total amount of jumps
    

    void Awake()
    {
        availableJumps = totalJumps;

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
        PlayerMove(horizontalValue); //calls function
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
            }
        }
        else
        {
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
        //Set float xVelocity according to the x value of the Rigidbody2D velcoity
        animator.SetFloat("xVelocity", Mathf.Abs(rb2d.velocity.x));

        //currentScale = transform.localScale;
        #endregion

    }
}
