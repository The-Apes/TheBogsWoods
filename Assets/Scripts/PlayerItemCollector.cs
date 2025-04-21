using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    public int playerHealth = 100; // Example player health

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            // Call the Pickup method of the item
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                item.Pickup();
            }
        }
        else if (collision.CompareTag("HealthItem"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                // Heal the player and destroy the item
                HealPlayer(item.value);
                Destroy(collision.gameObject);
            }
        }
    }

    private void HealPlayer(int healAmount)
    {
        if (playerHealth < 100) // Only heal if health is not full
        {
            playerHealth = Mathf.Min(playerHealth + healAmount, 100);
            Debug.Log("Health restored! Current health: " + playerHealth);
        }
    }
}