using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    private bool inDoor;
    public string nameofNextStage;

    // Update is called once per frame
    void Update()
    {
        if (inDoor)
        {
            SceneManager.LoadScene(nameofNextStage);
            inDoor = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            inDoor = true;
        }
    }
}
