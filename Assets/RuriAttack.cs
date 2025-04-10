using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RuriAttack : MonoBehaviour
{
    private bool _isAttacking;
    private float _attackTimer;
    private float _attackDuration = 0.3f;
    [SerializeField] private Collider2D hitBox;

    private void Awake()
    {
        if (hitBox == null) { Debug.LogWarning("HitBox isn't Defined, check the serialized field"); return; }
        hitBox.gameObject.SetActive(false);
    }

    public void Attack(InputAction.CallbackContext context) //Called by input system
    {
        if (!context.started) return;
        if (_isAttacking) return;
        if (!GetComponent<RuriMovement>().controlling) return;
        
        StartCoroutine(PerformAttack());
    }
    private IEnumerator PerformAttack()
    {
        
        hitBox.gameObject.SetActive(true);
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDuration);

        hitBox.gameObject.SetActive(false);
        _isAttacking = false;
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
