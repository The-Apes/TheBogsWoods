using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3; // Maximum health of the player
    public int currentHealth; // Current health of the player
    
    [SerializeField]private float invincibilityDuration = 1f; // Duration of invincibility after taking damage
    private float _invincibilityTimer; // Timer for invincibility
    private bool _invincible;

    public HealthUI healthUI; // Reference to the HealthUI script
    

    public static event Action OnPlayerDied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameController.OnReset += ResetHealth; // Subscribe to the reset event
        
        currentHealth = maxHealth;
        _invincibilityTimer = invincibilityDuration;
        
        // Initialize the health UI
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
    }
    private void Update()
    {
        if (_invincible)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0)
            {
                _invincible = false;
                _invincibilityTimer = invincibilityDuration; // Reset the timer
            }
        }
    }
        

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth); // Reset the hearts in the UI
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the reset health
    }

    public void ReceiveDamage(int damageTaken, GameObject source){
        if (_invincible) return; // If the player is invisible, ignore damage
        currentHealth -= damageTaken;
        GetComponent<DamageFlash>().CallDamageFlash();
        
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the new health
        _invincible = true;
        if (currentHealth <= 0)
        {
            // Player dead! -- call game over, animation, etc.
            OnPlayerDied?.Invoke();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the reset event to avoid memory leaks
        GameController.OnReset -= ResetHealth;
    }
}