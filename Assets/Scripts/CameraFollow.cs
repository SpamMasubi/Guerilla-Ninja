#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;
    public bool followFloor;
    float goalAltitude;
    [HideInInspector]
    public Vector3 minValue, maxValue;

    //Editor Fields
    [HideInInspector]
    public bool setupComplete;
    public enum SetupState { NONE, Step1, Step2}
    [HideInInspector]
    public SetupState ss = SetupState.NONE;
    float nextTimeToSearch = 0;
    Vector3 lastTargetPosition;

    void Start()
    {
        FindPlayer();
        //target = FindObjectOfType<Player>().transform;
        lastTargetPosition = target.position;
        if (followFloor)
        {
            goalAltitude = target.position.y;
        }
    }

    void OnEnable()
    {
        Player.HasLanded += UpdateCameraAltitude;
    }

    void OnDisable()
    {
        Player.HasLanded -= UpdateCameraAltitude;
    }

    void UpdateCameraAltitude()
    {
        if (!followFloor)
        {
            return;
        }
        goalAltitude = target.position.y;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }
        Follow();
    }

    void Follow()
    {
        //Define minimum x,y,z values and maximum x,y,z values

        Vector3 targetPos = target.position + offset;

        //If follow floor modify the y value accordingly
        if (followFloor)
        {
            targetPos.y = goalAltitude + offset.y;
        }
        //Verify  if the targetPosition is out of bound or not. Limit to min and max value
        Vector3 boundPosition = new Vector3(Mathf.Clamp(targetPos.x, minValue.x, maxValue.x), 
            Mathf.Clamp(targetPos.y, minValue.y, maxValue.y), 
            Mathf.Clamp(targetPos.z, minValue.z, maxValue.z));

        Vector3 smoothPos = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);
        transform.position = smoothPos;
    }

    public void ResetValues()
    {
        setupComplete = false;
        minValue = Vector3.zero;
        maxValue = Vector3.zero;
    }

    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
                target = searchResult.transform;
            nextTimeToSearch = Time.time + 0.2f;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraFollow))]
public class CameraFollowEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //Assign the MonoBehavior target script
        var script = (CameraFollow)target;
        //Check if the values are setup or not

        //Blank Sapce
        GUILayout.Space(20);

        GUIStyle defaultStyle = new GUIStyle();
        defaultStyle.fontStyle = FontStyle.Bold;
        defaultStyle.fontSize = 12;
        defaultStyle.alignment = TextAnchor.MiddleCenter;

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("-=- Camera Bound Limit Settings -=-", titleStyle);
        //If they are setup display the Min and Max values along with preview button
        //Also have a reset button for the values
        if (script.setupComplete)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Minimum Values: ", defaultStyle);
            GUILayout.Label("Maximum Values: ", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"X= {script.minValue.x}", defaultStyle);
            GUILayout.Label($"X= {script.maxValue.x}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"Y= {script.minValue.y}", defaultStyle);
            GUILayout.Label($"Y= {script.maxValue.y}", defaultStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("View Minimum"))
            {
                //snap the camera view to the minimum values
                Camera.main.transform.position = script.minValue;
            }
            else if(GUILayout.Button("View Maximum"))
            {
                //snap the camera view to the maximum values
                Camera.main.transform.position = script.maxValue;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Focus on Target"))// reset the view to the target
            {
                Vector3 targetPos = script.target.position + script.offset;
                targetPos.z = script.minValue.z;
                Camera.main.transform.position = targetPos;
            }

            if (GUILayout.Button("Reset Camera Values"))
            {
                //Reset the setupComplete boolean
                //reset the min max vec3 values
                script.ResetValues();
            }

        }
        else //If they are not setup display a start setup button
        {
            //Step 0: Show the start wizard button
            if(script.ss == CameraFollow.SetupState.NONE)
            {

                if (GUILayout.Button("Start Setting Camera Value"))
                {
                    //Change the state to step1
                    script.ss = CameraFollow.SetupState.Step1;
                }
            }
            //Step 1: Setup the bottom left bound (min values)
            else if(script.ss == CameraFollow.SetupState.Step1)
            {
                //Instruction on what to do 
                GUILayout.Label($"1-Select your main Camera", defaultStyle);
                GUILayout.Label($"2-Move it to the bottom left bound limit of your level", defaultStyle);
                GUILayout.Label($"3-Click the 'Set the Minimum Value' Button", defaultStyle);
                //Button to set the min values
                if (GUILayout.Button("Set Minimum Value"))
                {
                    script.minValue = Camera.main.transform.position;
                    //Change to step 2
                    script.ss = CameraFollow.SetupState.Step2;
                }
            }
            //Step 2: Setup the top right boundary (max values)
            else if(script.ss == CameraFollow.SetupState.Step2)
            {
                //Instruction on what to do 
                GUILayout.Label($"1-Select your main Camera", defaultStyle);
                GUILayout.Label($"2-Move it to the top right bound limit of your level", defaultStyle);
                GUILayout.Label($"3-Click the 'Set the Maximum Value' Button", defaultStyle);
                //Button to set the min values
                if (GUILayout.Button("Set Maximum Value"))
                {
                    script.maxValue = Camera.main.transform.position;
                    //Set the state to None
                    script.ss = CameraFollow.SetupState.NONE;
                    //Enable the setupComplete bool
                    script.setupComplete = true;
                    //Reset view to Player
                    Vector3 targetPos = script.target.position + script.offset;
                    targetPos.z = script.minValue.z;
                    Camera.main.transform.position = targetPos;
                }
            }
            
        }

    }
}
#endif
