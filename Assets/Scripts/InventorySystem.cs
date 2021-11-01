using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("General Field")]
    public List<GameObject> items; //List of pickup items
    public bool isOpen;//Flag indicate if the inventory is open or not
    [Header("UI Item Section")]
    public GameObject uiWindow;//Inventory system Window
    public Image[] itemsImages;
    [Header("UI Item Description")]
    public GameObject uiDescriptionWindow;
    public Image descriptionImage;
    public Text descriptionTitle;
    public Text descriptionText;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        uiWindow.SetActive(isOpen);
        UpdateUI();
    }

    //Add the item to the items list
    public void Pickup(GameObject item)
    {
        items.Add(item);
        UpdateUI();
    }

    //Refresh the UI elements in the inventory window
    void UpdateUI()
    {
        HideAll();
        //for each item in the items list. Show it in the respective slot in the itemsImage
        for(int i=0; i < items.Count; i++)
        {
            itemsImages[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            itemsImages[i].gameObject.SetActive(true);
        }
    }

    //Hide all the items ui images
    void HideAll()
    {
        foreach(var i in itemsImages)
        {
            i.gameObject.SetActive(false);
        }
        HideDescription();
    }

    public void ShowDescription(int id)
    {
        //Set Image
        descriptionImage.sprite = itemsImages[id].sprite;
        //Set Title
        descriptionTitle.text = items[id].name;
        //Set Description
        descriptionText.text = items[id].GetComponent<Item>().descriptionText;
        //Show the elements
        descriptionImage.gameObject.SetActive(true);
        descriptionTitle.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

    public void Consume(int id)
    {
        if(items[id].GetComponent<Item>().item == Item.ItemType.CONSUMABLE)
        {
            //Invoke the consume custom event
            items[id].GetComponent<Item>().consumeEvent.Invoke();
            //Destroy item in very tiny time
            Destroy(items[id], 0.1f);
            //Clear item from list
            items.RemoveAt(id);
            //Update UI
            UpdateUI();
        }
    }
}
