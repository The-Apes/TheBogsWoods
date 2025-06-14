using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class RuriAttack : MonoBehaviour
    {
        public bool isAttacking;
        private float _attackTimer;
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private const float AttackDuration = 0.3f;
        private RuriMovement _ruriMovement;
        [SerializeField] private Collider2D hitBox;
        private OttoShoot _ottoShoot;
        private Animator _animator;

        private void Awake()
        {
            if (hitBox == null) { Debug.LogWarning("HitBox isn't Defined, check the serialized field"); return; }
            hitBox.gameObject.SetActive(false);
            _ruriMovement = GetComponent<RuriMovement>();
            _animator = GetComponent<Animator>();
        }

        public void Attack(InputAction.CallbackContext context) //Called by input system
        {
            if (!context.started) return;
            if(!_ruriMovement.hasWeapon) return;
            if (isAttacking) return;
            if (!_ruriMovement.controlling) return;
            isAttacking = true;
            _animator.SetTrigger("Attack");
            RuriMovement.instance.AttackMove();
            //_ruriMovement.isAttacking = true

        }
        public void AttackFinish()
        {
            isAttacking = false;
        }
        private IEnumerator PerformAttack()
        {
        
            hitBox.gameObject.SetActive(true);
            isAttacking = true;

            yield return new WaitForSeconds(AttackDuration);

            hitBox.gameObject.SetActive(false);
            isAttacking = false;
        }
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
