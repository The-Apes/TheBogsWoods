using HeightObjects;
using Managers;
using Player;
using UnityEngine;

namespace Enemies
{
    public class CowardEnemy : MonoBehaviour, IDamageable
    {
        public float chaseSpeed = 2f;
        public float detectionRange = 5f;
        private GameObject _chaseTarget;
        private GameObject _runTarget;
        private Vector2 direction;
        private Animator _animator;

        private Rigidbody2D _rb;
        [SerializeField] private Collider2D detectionZone;

        [SerializeField] private float maxHealth;
        public float health;
        private bool _run;

        [Header("Sounds")]
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private AudioClip deathSound;

        [Header("Potions")]
        [SerializeField] private int potionDropAmount;
        [SerializeField] private GameObject potionPrefab;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_run)
            {
                if (!_chaseTarget) return;
                direction = (_chaseTarget.transform.position - transform.position).normalized;
                _rb.linearVelocity = direction.normalized * chaseSpeed;
            }
            else
            {
                if (!_runTarget) return;
                direction = (transform.position - _runTarget.transform.position).normalized;
                _rb.linearVelocity = direction.normalized * (chaseSpeed * 1.5f);
            }

            if (!_animator) return;

            if (direction.Equals(Vector2.zero))
            {
                _animator.SetFloat("BodyX", 1f);
                _animator.SetFloat("BodyY", 0f);
            }

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                _animator.SetFloat("BodyX", direction.x > 0 ? 1f : -1f);
                _animator.SetFloat("BodyY", 0f);
            }
            else
            {
                _animator.SetFloat("BodyX", 0f);
                _animator.SetFloat("BodyY", direction.y > 0 ? 1f : -1f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!detectionZone.IsTouching(other)) return;
            if (other.isTrigger) return;
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            if (other.gameObject.name == "Ruri" || other.gameObject.name.Contains("Otto"))
            {
                _chaseTarget = other.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (detectionZone.IsTouching(other)) return;
            if (other.isTrigger) return;
            if (other.gameObject == _chaseTarget)
            {
                _chaseTarget = null;
                _rb.linearVelocity = Vector2.zero;
            }
        }

        private void Awake()
        {
            health = maxHealth;
        }

        public void ReceiveDamage(int damageTaken, GameObject source)
        {
            health -= damageTaken;

            // Get hit point and direction
            Vector2 hitPoint = GetComponent<Collider2D>().ClosestPoint(source.transform.position);
            Vector2 hitDir = (transform.position - source.transform.position).normalized;

            // Flash + Visuals
            GetComponent<DamageFlash>().CallDamageFlash();
            HitEffectManager.Instance?.PlayHitEffects(hitPoint, hitDir);

            // Hit stop + knockback
            GameManager.instance.HitStop(0.1f);
            ApplyKnockback(source.transform.position, 20f);

            if (health <= 0)
            {
                AudioManager.instance.PlaySFXAt(deathSound, transform);

                if (RuriMovement.instance.gameObject)
                {
                    _runTarget = RuriMovement.instance.controlling ? RuriMovement.instance.gameObject : RuriMovement.instance.Otto;
                }

                _run = true;
                DropPotions();

                Collider2D[] colliders = GetComponents<Collider2D>();
                foreach (Collider2D otherCollider in colliders)
                {
                    otherCollider.enabled = false;
                }
            }
            else
            {
                AudioManager.instance.PlaySFXAt(hurtSound, transform);
            }
        }

        private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        private void DropPotions()
        {
            if (potionDropAmount <= 0 || !potionPrefab) return;

            for (int i = 0; i < potionDropAmount; i++)
            {
                GameObject potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
                potion.GetComponent<FakeHeightObject>().Initialize(
                    new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * Random.Range(3, 6),
                    Random.Range(3, 6)
                );
            }
        }
    }
}
