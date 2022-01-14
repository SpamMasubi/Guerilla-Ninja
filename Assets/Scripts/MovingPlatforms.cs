using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public List<Transform> points;
    public Transform platform;
    int goalPoints = 0;
    public float moveSpeed = 2;

    private void Update()
    {
        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        //Change the position of the platform(move towards the goal point)
        platform.position = Vector2.MoveTowards(platform.position, points[goalPoints].position, Time.deltaTime * moveSpeed);
        //Check if we are in very close proximity of the next point
        if(Vector2.Distance(platform.position, points[goalPoints].position) < 0.1f)
        {
            //If so change goal point to the next one
            //Check if we reached the last point, reset to first point
            if(goalPoints == points.Count-1)
            {
                goalPoints = 0;
            }
            else
            {
                goalPoints++;
            }
        }
    }
}
