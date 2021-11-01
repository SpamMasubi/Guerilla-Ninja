using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { NONE, PICKUP, EXAMINE} //type of interaction
    public enum ItemType { STATIC, CONSUMABLE} //type of items
    [Header("Attributes")]
    public InteractionType interactType;
    public ItemType item;
    [Header("Examine")]
    public string descriptionText; //description
    [Header("Custom Event")]
    public UnityEvent customEvents;
    public UnityEvent consumeEvent;

    void Reset() //reset function
    {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 7;
    }

    public void Interact() //interaction of items
    {
        switch (interactType)
        {
            case InteractionType.PICKUP:
                FindObjectOfType<InventorySystem>().Pickup(gameObject);//Add the object to the pickedup item list
                gameObject.SetActive(false);//delete object
                break;
            case InteractionType.EXAMINE:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);//Call the Examine item in the interaction system
                Debug.Log("Examine");
                break;
            default:
                Debug.Log("Null");
                break;
        }

        //Invoke (call) custom events
        customEvents.Invoke();
    }
}
