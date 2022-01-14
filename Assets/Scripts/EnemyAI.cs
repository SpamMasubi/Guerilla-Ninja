using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyAI : MonoBehaviour
{
    //Reference to waypoints
    public List<Transform> points;
    //The int value for the next point index
    int nextID;
    //The value of that applies to the ID for changing
    int idChangeValue = 1;
    //Speed of movement or flying
    public float speed = 2;

    public static bool isDead = false; //check if player is Dead

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
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

    private void Update()
    {
        MoveToNextPoint();
    }
    

    public void MoveToNextPoint()
    {
        //Get the next Point transform
        Transform goalPoint = points[nextID];
        //Flip the enemy transform to look into the point's direction

        if (goalPoint.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);

        }

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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead)
        {
            if (collision.tag == "playerProjectiles" || collision.tag == "playerAttack")
            {
                AudioManager.instance.PlaySFX("enemyHit");
                isDead = true;
                FindObjectOfType<GameManager>().scores += Random.Range(1, 100);
                GetComponent<EnemyAI>().enabled = false;
                anim.SetTrigger("Killed");
            }
        }
    }

    public void EnemyDead()
    {
        Destroy(transform.parent.gameObject);
        isDead = false;
    }

}
