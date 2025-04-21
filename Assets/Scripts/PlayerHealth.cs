using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3; // Maximum health of the player
    public int currentHealth; // Current health of the player

    public HealthUI healthUI; // Reference to the HealthUI script

    private SpriteRenderer spriteRenderer;

    public static event Action OnPlayerDied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth; // Subscribe to the reset event

        // Initialize the health UI
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Enemy enemy = collision.GetComponent<Enemy>();
    //     if (enemy)
    //     {
    //         TakeDamage(enemy.damage);
    //     }
    // }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth); // Reset the hearts in the UI
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the reset health
    }

    public void ReceiveDamage(int damageTaken, GameObject source){
        currentHealth -= damageTaken;
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the new health

        // Flash Red
       // StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            // Player dead! -- call game over, animation, etc.
            OnPlayerDied?.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; // Change color to red
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white; // Change color back to white
    }

    private void OnDestroy()
    {
        // Unsubscribe from the reset event to avoid memory leaks
        GameController.OnReset -= ResetHealth;
    }
}