using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuriAttack : MonoBehaviour
{
    public bool isAttacking;
    private float _attackTimer;
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private float _attackDuration = 0.3f;
    private RuriMovement _ruriMovement;
    [SerializeField] private Collider2D hitBox;
    private OttoShoot _ottoShoot;

    private void Awake()
    {
        if (hitBox == null) { Debug.LogWarning("HitBox isn't Defined, check the serialized field"); return; }
        hitBox.gameObject.SetActive(false);
        _ruriMovement = GetComponent<RuriMovement>();
    }

    public void Attack(InputAction.CallbackContext context) //Called by input system
    {
        if (!context.started) return;
        if (isAttacking) return;
        if (!_ruriMovement.controlling) return;
        
        StartCoroutine(PerformAttack());
    }
    private IEnumerator PerformAttack()
    {
        
        hitBox.gameObject.SetActive(true);
        isAttacking = true;

        yield return new WaitForSeconds(_attackDuration);

        hitBox.gameObject.SetActive(false);
        isAttacking = false;
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            if (!_ruriMovement.ottoMounted) return;
            _ruriMovement.RidingOtto.GetComponent<OttoShoot>().ShootInput = true;
        }

        if (context.canceled)
        {
            if (!_ruriMovement.ottoMounted) return;
            _ruriMovement.RidingOtto.GetComponent<OttoShoot>().ShootInput = false;
        }
    }

   
    /*private void UpdateAttackTimer()
    {
        if (_isAttacking)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackDuration)
            {
                hitBox.gameObject.SetActive(false);
                _isAttacking = false;
                _attackTimer = 0f;
                print("Attack Ended");
            }
        }
    }*/

}
