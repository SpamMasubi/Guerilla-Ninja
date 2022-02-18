using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class BossVehicle : MonoBehaviour
{
    //Reference to waypoints
    public List<Transform> points;
    //The int value for the next point index
    int nextID;
    //The value of that applies to the ID for changing
    int idChangeValue = 1;
    //Speed of movement or flying
    public float speed = 5;
    public string bossName;
    public float bossHealth = 0f;

    public static bool isDead = false; //check if player is Dead
    public static bool stageClear = false;

    Animator anim;

    public GameObject Explosion;

    [Header("Invincibility Flash")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public static bool isInvincible;
    public SpriteRenderer bossSprite;

    void Start()
    {
        anim = GetComponent<Animator>();
        bossSprite = GetComponent<SpriteRenderer>();
    }

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        //Make boxCollider trigger
        GetComponent<PolygonCollider2D>().isTrigger = true;

        GameObject root = new GameObject(name + "_Root");
        //Reset Position of Root to this Boss
        root.transform.position = transform.position;
        //Set enemy object as child of root
        transform.SetParent(root.transform);
        //Create Waypoints object
        GameObject waypoints = new GameObject("Waypoints");
        //Reset waypoints position to root
        //Make waypoints object child of root
        waypoints.transform.SetParent(root.transform);
        waypoints.transform.position = root.transform.position;
        //Create two points and reset their position to waypoint objects
        //Make the points children of waypoint object
        GameObject p1 = new GameObject("Point1");
        p1.transform.SetParent(waypoints.transform);
        p1.transform.position = root.transform.position;
        GameObject p2 = new GameObject("Point2");
        p2.transform.SetParent(waypoints.transform);
        p2.transform.position = root.transform.position;

        //Init point lists then add the points to it
        points = new List<Transform>();
        points.Add(p1.transform);
        points.Add(p2.transform);
    }

    private void Update()
    {
        if (BossStart.startBoss)
        {
            MoveToNextPoint();
            gameObject.SetActive(true);
        }
    }


    public void MoveToNextPoint()
    {
        //Get the next Point transform
        Transform goalPoint = points[nextID];

        //Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, speed * Time.deltaTime);
        //Check the distance between the enemy and goal point to trigger next point
        if (Vector2.Distance(transform.position, goalPoint.position) < 0.2f)
        {
            //check if we are at the end of the line(make the change -1)
            if (nextID == points.Count - 1)
            {
                idChangeValue = -1;
            }
            //check if we are at the start of the line(make the change +1)
            if (nextID == 0)
            {
                idChangeValue = 1;
            }
            //Apply the change on the nextID
            nextID += idChangeValue;
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
                        AudioManager.instance.PlaySFX("bossDamage");
                    }
                    else if (SwitchWeapons.handgun)
                    {
                        StartCoroutine(InvincibilityFlash());
                        FindObjectOfType<BossHealthBar>().LoseHealth(3);
                        AudioManager.instance.PlaySFX("bossDamage");
                    }
                    else if (SwitchWeapons.AR)
                    {
                        StartCoroutine(InvincibilityFlash());
                        FindObjectOfType<BossHealthBar>().LoseHealth(5);
                        AudioManager.instance.PlaySFX("bossDamage");
                    }
                }
                else if(!isInvincible && collision.tag == "playerAttack")
                {
                    StartCoroutine(InvincibilityFlash());
                    FindObjectOfType<BossHealthBar>().LoseHealth(1);
                    AudioManager.instance.PlaySFX("bossDamage");
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
        if (gameObject.name == "North Star Army AH (Boss)")
        {
            GetComponent<AudioSource>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0.01f;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        isDead = true;
        ExplosionEffect();
        Destroy(FindObjectOfType<PlayMusic>().gameObject);
        GetComponent<BossVehicle>().enabled = false;
        FindObjectOfType<BossVehicle>().GetComponentInChildren<BossSpecial>().enabled = false;
        anim.SetTrigger("Defeated");
    }

    //Function to instantiate explosion
    void ExplosionEffect()
    {
        //instiantiate explosion effect
        GameObject explode = (GameObject)Instantiate(Explosion);

        //set explosion position
        explode.transform.position = transform.position;
    }

    public void StageClear()
    {
        ChapterIntro.chapters += 1;
        stageClear = true;
        AudioManager.instance.PlaySFX("Win");
    }
}
