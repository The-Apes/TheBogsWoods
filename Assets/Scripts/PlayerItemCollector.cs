using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    public int playerHealth = 100; // Example player health

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;
        
        // Call the Pickup method of the item
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            item.Pickup();
        }
    }

    private void HealPlayer(int healAmount)
    {
        if (playerHealth >= 100) return; // Only heal if health is not full
        
        playerHealth = Mathf.Min(playerHealth + healAmount, 100);
        Debug.Log("Health restored! Current health: " + playerHealth);
    }
}