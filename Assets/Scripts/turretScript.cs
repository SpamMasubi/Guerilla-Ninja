using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour
{
    public float Range;

    public Transform Target;

    bool Detected = false;

    Vector2 Direction;

    public GameObject turretBarrel;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 targetPos = Target.position;

        Direction = targetPos - (Vector2)transform.position;

        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if (rayInfo)
        {
            if(rayInfo.collider.gameObject.tag == "Player")
            {
                if (!Detected)
                {
                    Detected = true;
                    Debug.Log("You been spotted");
                }
            }
            else
            {
                if (Detected)
                {
                    Detected = false;
                    Debug.Log("Kek");
                }
            }
        }

        if (Detected)
        {
            turretBarrel.transform.up = Direction;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
