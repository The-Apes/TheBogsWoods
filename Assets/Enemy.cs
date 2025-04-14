using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseSpeed = 2f; // Speed at which the enemy chases the player
    public float detectionRange = 5f; // Range within which the enemy detects the player
    public int damage = 1; // Damage dealt to the player on collision

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player reference is valid
        if (player == null)
        {
            rb.velocity = Vector2.zero; // Stop moving if the player is null
            return;
        }

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within detection range, chase the player
        if (distanceToPlayer <= detectionRange)
        {
            Vector2 direction = (player.position - transform.position).normalized; // Direction towards the player
            rb.velocity = direction * chaseSpeed; // Move the enemy towards the player
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if the player is out of range
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the detection range in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}