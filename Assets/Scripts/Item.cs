using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;     
    public string Name; 
    public int value;  // Healing amount or other effect value

    public virtual void Pickup()
    {
        Sprite itemIcon = GetComponent<SpriteRenderer>().sprite;
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