using UnityEngine;


public class HurtBox : MonoBehaviour
{
    [SerializeField] private GameObject owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("HitBox")) return;    //if the object that entered the trigger is not a HitBox, return
        HitBox hitBox = other.GetComponent<HitBox>();    //Otherwise, get the HitBox component
        if(owner == null) return;   //if my owner is null, return (I have no one to deal damage to)
        IDamageable damage = owner.GetComponent<IDamageable>();    //get the IDamageable component from my owner
        if (damage == null) return;     //if my owner does not have an IDamageable component, return
        damage.ReceiveDamage(hitBox.damage, other.gameObject); //deal damage to my owner
    }


}
