using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseSpeed = 2f; // Speed at which the enemy chases the player
    public float detectionRange = 5f; // Range within which the enemy detects the player

    private Rigidbody2D rb;

    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
    
}