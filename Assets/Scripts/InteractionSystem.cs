using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("Detection Parameters")]
    public Transform detectionPoint;//DetectionPoint
    private const float detectionRadius = 0.2f;//DetectionRadius
    public LayerMask detectionLayer;//DetectionLayer
    public GameObject detectedObject;//Cached Trigger Object
    [Header("Examine Field")]
    public GameObject examineWindow; //examine window object
    public Image examineImage; //examine image
    public Text examineText; //examine text
    public bool isExamining; //examine checker

    // Update is called once per frame
    void Update()
    {
        if (detectObject())
        {
            if (interactionInput())
            {
                detectedObject.GetComponent<Item>().Interact();
            }
        }
    }

    bool interactionInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    bool detectObject()
    {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
        if(obj == null)
        {
            detectedObject = null;
            return false;
        }
        else
        {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void ExamineItem(Item item)
    {
        if (isExamining)
        {
            examineWindow.SetActive(false);//Disable examine window
            isExamining = false; //disable boolean
        }
        else
        {
            examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite;//Show the item's image in the middle
            examineText.text = item.descriptionText;//Write the description text underneath
            examineWindow.SetActive(true);//Display examine window
            isExamining = true; //enable boolean
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(detectionPoint.position, detectionRadius);
    }
}
