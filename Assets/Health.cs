using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth;
    private float _health;
   
    private void Awake()
    {
         _health = maxHealth;
    }

   public void ReceiveDamage(float damageTaken, GameObject source){
        _health -= damageTaken; //subtract the damage taken from the health
        if (_health <= 0) //if the health is less than or equal to 0
        {
            Destroy(gameObject); //destroy this entity
        }
    }

}
