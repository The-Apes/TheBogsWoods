using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float chaseSpeed = 2f; // Speed at which the enemy chases the player
    public float detectionRange = 5f; // Range within which the enemy detects the player
    private GameObject _chaseTarget;

    private Rigidbody2D rb;
    [SerializeField] private Collider2D detectionZone;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (detectionZone == null)
        {
            Debug.LogError("DetectionZone is not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_chaseTarget)
        {
            Vector2 direction = (_chaseTarget.transform.position - transform.position).normalized; // Direction towards the player
            rb.linearVelocity = direction * chaseSpeed; // Move the enemy towards the player
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Stop moving if no target
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (detectionZone == null || other == null) return;

        if (detectionZone.IsTouching(other))
        {
            if (other.isTrigger) return; // Ignore triggers
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return; // Check if it's on the Player layer
            if (other.gameObject.name == "Ruri" || other.gameObject.name.Contains("Otto"))
            {
                _chaseTarget = other.gameObject; // Set the target
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (detectionZone == null || other == null) return;

        if (!detectionZone.IsTouching(other))
        {
            if (other.isTrigger) return; // Ignore triggers
            if (other.gameObject == _chaseTarget) // If the exiting object is the current target
            {
                _chaseTarget = null; // Clear the target
                rb.linearVelocity = Vector2.zero; // Stop movement
            }
        }
    }
}