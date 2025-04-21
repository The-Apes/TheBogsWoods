using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float _health;
    
    [Header("Sounds")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;
   
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
            AudioManager.Instance.PlaySound(deathSound);
            Destroy(gameObject); //destroy this entity
        }
        else
        {
            AudioManager.Instance.PlaySound(hurtSound);
        }
    }

}
