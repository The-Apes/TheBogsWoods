using Managers;
using UnityEngine;

public class CowardEnemy : MonoBehaviour, IDamageable
{
    public float chaseSpeed = 2f; 
    public float detectionRange = 5f; 
    private GameObject _chaseTarget;
    private GameObject _runTarget;

    private Rigidbody2D _rb;
    [SerializeField] private Collider2D detectionZone;
    
    [SerializeField] private float maxHealth;
    public float health;
    private bool _run;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
        private void Update()
    {
        if (!_run)
        {
            if (!_chaseTarget) return;
            Vector2 direction = (_chaseTarget.transform.position - transform.position).normalized; // Direction towards the player
            _rb.linearVelocity = direction.normalized * chaseSpeed; // Move the enemy towards the player
        }
        else
        {
            //move in the opposite direction of the player
            if (!_runTarget) return;
            Vector2 direction = (transform.position - _runTarget.transform.position).normalized; // Direction towards the player
            _rb.linearVelocity = direction.normalized * (chaseSpeed * 1.5f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) //when entering 2D sphere
    {
        if (!detectionZone.IsTouching(other)) return;
        if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return; //if whatever entered my sphere is a part of the player layer mask
        if (other.gameObject.name == "Ruri" || other.gameObject.name.Contains("Otto"))
        {
            _chaseTarget = other.gameObject; //make the object that entered my sphere. my target
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (detectionZone.IsTouching(other)) return;
        if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
        if(other.gameObject == _chaseTarget) //if the object that exited my sphere is my target
        {
            _chaseTarget = null; 
            _rb.linearVelocity = Vector2.zero;
        }
    }
    private void Awake()
    {
        health = maxHealth;
    }

    public void ReceiveDamage(int damageTaken, GameObject source){
        health -= damageTaken; //subtract the damage taken from the health
        GetComponent<DamageFlash>().CallDamageFlash();
        GameManager.instance.HitStop(0.1f);
        ApplyKnockback(source.transform.position, 20f);
        if (health <= 0) //if the health is less than or equal to 0
        {
            AudioManager.instance.PlaySound(deathSound);
            if (RuriMovement.instance.gameObject)
            {
                _runTarget = RuriMovement.instance.controlling ? RuriMovement.instance.gameObject : RuriMovement.instance.Otto;
            }
            _run = true;

            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D otherCollider in colliders)
            {
                otherCollider.enabled = false; //disable all colliders
            }

        }
        else
        {
            AudioManager.instance.PlaySound(hurtSound);
        }
    }
    private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

}