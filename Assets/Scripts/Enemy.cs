using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float chaseSpeed = 2f; // Speed at which the enemy chases the player
    public float detectionRange = 5f; // Range within which the enemy detects the player
    private GameObject _chaseTarget;

    private Rigidbody2D _rb;
    [SerializeField] private Collider2D detectionZone;
    
    [SerializeField] private float maxHealth;
    private float _health;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_chaseTarget)
        {
            Vector2 direction = (_chaseTarget.transform.position - transform.position).normalized; // Direction towards the player
            transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * chaseSpeed); // Move the enemy towards the player
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) //when entering 2D sphere
    {
        if (detectionZone.IsTouching(other)){
            if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return; //if whatever entered my sphere is a part of the player layer mask
            if (other.gameObject.name == "Ruri" || other.gameObject.name.Contains("Otto"))
            {
                _chaseTarget = other.gameObject; //make the object that entered my sphere. my target
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!detectionZone.IsTouching(other) ) {
            if (other.isTrigger) return; //if the object that entered my sphere is a trigger, return
            if(other.gameObject == _chaseTarget) //if the object that exited my sphere is my target
            {
                _chaseTarget = null; //make my target null
                _rb.linearVelocity = Vector2.zero;
            }
        }
    }
    private void Awake()
    {
        _health = maxHealth;
    }

    public void ReceiveDamage(int damageTaken, GameObject source){
        _health -= damageTaken; //subtract the damage taken from the health
        GetComponent<DamageFlash>().CallDamageFlash();
        GameManager.Instance.HitStop(0.1f);
        if (_health <= 0) //if the health is less than or equal to 0
        {
            AudioManager.instance.PlaySound(deathSound);
            Destroy(gameObject); //destroy this entity
        }
        else
        {
            ApplyKnockback(source.transform.position, 25f);
            AudioManager.instance.PlaySound(hurtSound);
        }
    }
    private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
        _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }
}