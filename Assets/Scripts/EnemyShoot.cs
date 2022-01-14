using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyShoot : MonoBehaviour
{
    public Transform player;

    [SerializeField]
    Transform castPoint;

    [SerializeField]
    float agroRange;

    Animator anim;

    public static bool isAgros = false;
    public static bool isShooting = false;
    float nextTimeToSearch = 0;

    public GameObject enemyBullet;

    public Transform launchPoint;

    float fireRate = 2f;
    float nextShot;
    private float distToPlayer;

    private bool faceDirection;

    //Reference to waypoints
    public List<Transform> points;
    //The int value for the next point index
    int nextID;
    //The value of that applies to the ID for changing
    int idChangeValue = 1;
    //Speed of movement or flying
    public float maxSpeed = 2;
    public static float currentSpeed;

    public static bool isDead = false; //check if player is Dead

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        nextShot = Time.time;
        anim = GetComponent<Animator>();
        currentSpeed = maxSpeed;

    }

    private void Reset()
    {
        Init();
    }

    void Init()
    {
        //Make boxCollider trigger
        GetComponent<BoxCollider2D>().isTrigger = true;

        GameObject root = new GameObject(name + "_Root");
        //Reset Position of Root to this enemy
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

    void Update()
    {
        if (player != null)
        {
            distToPlayer = Vector2.Distance(transform.position, player.position);
        }
        if (player == null)
        {
            StopShootingPlayer();
            FindPlayer();
            return;
        }

        if (CanSeePlayer(agroRange) && !Player.isDead)
        {
            //agro player
            isAgros = true;

            if (distToPlayer <= 10)
            {
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }

        }
        else
        {
            StopShootingPlayer();
            MoveToNextPoint();
        }

        if (isAgros && isShooting)
        {
            anim.SetBool("Shoot", true);
            shootPlayer();
        }

    }

    public void MoveToNextPoint()
    {
        //Get the next Point transform
        Transform goalPoint = points[nextID];
        //Flip the enemy transform to look into the point's direction

        if (goalPoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            faceDirection = false;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            faceDirection = true;
        }

        //Move the enemy towards the goal point
        transform.position = Vector2.MoveTowards(transform.position, goalPoint.position, currentSpeed * Time.deltaTime);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "playerProjectiles" || collision.tag == "playerAttack")
            {
                AudioManager.instance.PlaySFX("enemyHit");
                FindObjectOfType<GameManager>().scores += Random.Range(1, 100);
                isDead = true;
                GetComponent<EnemyShoot>().enabled = false;
                anim.SetTrigger("Killed");
            }
        }
    }

    public void EnemyDead()
    {
        Destroy(transform.parent.gameObject);
        isDead = false;
        isAgros = false;
        isShooting = false;
        currentSpeed = maxSpeed;
    }

    //Enemy's viewpoint if player is in Enemy's range of sight
    bool CanSeePlayer(float distance)
    {
        bool value = false;
        var castDist = distance;

        //change directions of the casting point when Enemy's direction change.
        if (!faceDirection)
        {
            castDist = distance;
        }
        else if (faceDirection)
        {
            castDist = -distance;
        }

        Vector2 endPoint = castPoint.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPoint, 1 << LayerMask.NameToLayer("Player"));
         //***FOR EDITOR ONLY** if the player is in Enemy's range of sight, change the raycast color
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

    void StopShootingPlayer()
    {
        isAgros = false;
        isShooting = false;
        currentSpeed = maxSpeed;
        anim.SetBool("Shoot", false);
    }

    void shootPlayer()
    {
        if (Time.time > nextShot)
        {
            AudioManager.instance.PlaySFX("shootAssaultRifle");
            GameObject projectile = Instantiate(enemyBullet, launchPoint.position, launchPoint.rotation);
            nextShot = Time.time + fireRate;
            if (!faceDirection)
            {
                projectile.GetComponent<EnemyBullet>().direction = 1;
            }
            else
            {
                projectile.GetComponent<EnemyBullet>().direction = -1;
            }
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
