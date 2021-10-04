using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public Vector3 minValue, maxValue;


    private void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        //Define minimum x,y,z values and maximum x,y,z values

        Vector3 targetPos = target.position + offset;
        //Verify  if the targetPosition is out of bound or not. Limit to min and max value
        Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPos.x, minValue.x, maxValue.x), 
            Mathf.Clamp(targetPos.y, minValue.y, maxValue.y), 
            Mathf.Clamp(targetPos.z, minValue.z, maxValue.z));

        Vector3 smoothPos = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPos;
    }
}
