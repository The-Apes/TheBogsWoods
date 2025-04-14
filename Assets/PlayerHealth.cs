using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the player
    public int currentHealth; // Current health of the player
    
    public HealthUI healthUI; // Reference to the HealthUI script
    
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth); // Set the maximum hearts in the UI
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        
        //Flash Red
        StartCoroutine(FlashRed());
        
        if (currentHealth <= 0)
        {
            //player dead! -- call game over, animation, etc.
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red; // Change color to red
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white; // Change color back to white
    }
}
