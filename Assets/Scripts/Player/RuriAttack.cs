using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RuriAttack : MonoBehaviour
    {
        public bool isAttacking;
        private float _attackTimer;
        private const float AttackDuration = 0.3f;

        [SerializeField] private Collider2D hitBox;
        private RuriMovement _ruriMovement;
        private Animator _animator;

        private void Awake()
        {
            if (hitBox == null)
            {
                Debug.LogWarning("HitBox isn't defined, check the serialized field");
                return;
            }

            hitBox.gameObject.SetActive(false);
            _ruriMovement = GetComponent<RuriMovement>();
            _animator = GetComponent<Animator>();
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (!_ruriMovement.hasWeapon) return;
            if (isAttacking) return;
            if (!_ruriMovement.controlling) return;

            isAttacking = true;
            _animator.SetTrigger("Attack");
            RuriMovement.instance.AttackMove(); // Optional attack lunge/move
        }

        // Called at the end of the attack animation
        public void AttackFinish()
        {
            isAttacking = false;
        }

        // This is triggered by an animation event at the attack's hit frame
        public void PerformAttack()
        {
            StartCoroutine(DoHitDetection());
        }

        private IEnumerator DoHitDetection()
        {
            hitBox.gameObject.SetActive(true);
            isAttacking = true;

            Collider2D[] hits = Physics2D.OverlapBoxAll(hitBox.bounds.center, hitBox.bounds.size, 0f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Vector2 hitDirection = (hit.transform.position - transform.position).normalized;
                    Vector2 hitPoint = hit.ClosestPoint(transform.position);

                    // Play global hit effects
                    HitEffectManager.Instance.PlayHitEffects(hitPoint, hitDirection);

                    // Trigger enemy feedback
                    EnemyHitFeedback enemyHit = hit.GetComponent<EnemyHitFeedback>();
                    if (enemyHit != null)
                    {
                        enemyHit.OnHit(hitDirection);
                    }
                }
            }

            yield return new WaitForSeconds(0.05f); // short delay before hiding hitbox

            hitBox.gameObject.SetActive(false);
            isAttacking = false;
        }

        // Optional: Visualize the hitbox area in Scene view
        private void OnDrawGizmosSelected()
        {
            if (hitBox == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hitBox.bounds.center, hitBox.bounds.size);
        }

        // Called by input system
        public void Shoot(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                if (!_ruriMovement.hasOtto) return;
                if (!_ruriMovement.ottoMounted) return;
                _ruriMovement.RidingOtto.GetComponent<OttoShoot>().ShootInput = true;
            }

            if (context.canceled)
            {
                if (!_ruriMovement.ottoMounted) return;
                _ruriMovement.RidingOtto.GetComponent<OttoShoot>().ShootInput = false;
            }
        }
    }
}

