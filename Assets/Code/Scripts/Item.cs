using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;     // Unique identifier for the item
    public string Name; // Name of the item
    public int value;  // Healing amount or other effect value

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if(ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(Name, itemIcon);
        }
        else
        {
            Debug.LogError("ItemPickupUIController instance not found.");
        }
    }
}