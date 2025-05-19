using System;
using System.Collections;
using Managers;
using UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 3; 
    public int currentHealth; 
    
    [SerializeField]private float invincibilityDuration = 1f; 
    private float _invincibilityTimer; 
    private bool _invincible;
    private bool _dead;

    public HealthUI healthUI; 
    
    [Header("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
    private Rigidbody2D _rb;

    public static event Action OnPlayerDied;

   

    private void Awake()
    {
        GameController.OnReset += ResetHealth; // Subscribe to the reset event
        healthUI = FindFirstObjectByType<HealthUI>(); 
        currentHealth = maxHealth;
        _invincibilityTimer = invincibilityDuration;
        
        // Initialize the health UI
        healthUI.SetMaxHearts(maxHealth);
        healthUI.UpdateHearts(currentHealth);
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        
    }

    IEnumerator InvincibilityTimer(float duration)
    {
        _invincible = true;
        yield return new WaitForSeconds(duration);
        _invincible = false;
    }

    public void BecomeInvulnerable(float duration)
    {
        StartCoroutine(InvincibilityTimer(duration));
    }


    private void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth); // Reset the hearts in the UI
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the reset health
    }

    public void ReceiveDamage(int damageTaken, GameObject source){
        if (_invincible) return; 
        currentHealth -= damageTaken;
        GetComponent<DamageFlash>().CallDamageFlash();
        healthUI.UpdateHearts(currentHealth); // Update the hearts to reflect the new health
        BecomeInvulnerable(_invincibilityTimer);
        if (currentHealth <= 0 && !_dead)
        {
            _dead = true;
            StopAllCoroutines();
            _invincible = true;
            AudioManager.instance.PlaySFXAt(deathSound, transform);
            OnPlayerDied?.Invoke();
        }
        else
        {
            ApplyKnockback(source.transform.position,10f);
            GameManager.instance.HitStop(0.1f);
            AudioManager.instance.PlaySFXAt(hurtSound, transform);
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