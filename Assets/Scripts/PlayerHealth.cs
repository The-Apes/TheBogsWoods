using System;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3; // Maximum health of the player
    public int currentHealth; // Current health of the player
    
    [SerializeField]private float invincibilityDuration = 1f; // Duration of invincibility after taking damage
    private float _invincibilityTimer; // Timer for invincibility
    private bool _invincible;

    public HealthUI healthUI; // Reference to the HealthUI script
    [Header("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    private Rigidbody2D _rb;

    public static event Action OnPlayerDied;

   

    private void Awake()
    {
        GameController.OnReset += ResetHealth; // Subscribe to the reset event
        healthUI = FindFirstObjectByType<HealthUI>(); // Find the HealthUI in the scene
        currentHealth = maxHealth;
        _invincibilityTimer = invincibilityDuration;
        
        // Initialize the health UI
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
        _rb = GetComponent<Rigidbody2D>();
        
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
            AudioManager.instance.PlaySound(deathSound); // Play the hurt sound
            // Player dead! -- call game over, animation, etc.
            OnPlayerDied?.Invoke();
        }
        else
        {
            ApplyKnockback(source.transform.position,10f);
            GameManager.Instance.HitStop(0.1f);
            AudioManager.instance.PlaySound(hurtSound); // Play the hurt sound
        }
    }

    private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        // Unsubscribe from the reset event to avoid memory leaks
        GameController.OnReset -= ResetHealth;
    }
}