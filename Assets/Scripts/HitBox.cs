using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    public GameObject owner; // The object that owns this hit box
    private int _layer;
    private string _tag;

    public void Hit()
    {
        // Implement hit logic here
    }

    private void Awake()
    {
        if (owner == null) //if the owner is not set, set it to the objects that owns this script
        {
            owner = transform.root.gameObject;
        }
        _layer = gameObject.layer; //get the layer of this object, layers are stored in a bitmask, so it's weird...
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("HurtBox")) return; //if the object that entered the trigger is not a HitBox, return
        HurtBox hurtBox = other.GetComponent<HurtBox>(); //Otherwise, get the Hurt box component
        if (hurtBox.Owner == null) return; //if my owner is null, return (I have no one to deal damage to)
        if (other.gameObject.layer == _layer) return;

        IDamageable damageable = hurtBox.Owner.GetComponent<IDamageable>(); //get the IDamageable component from my owner
        if (damageable == null) return;
        damageable.ReceiveDamage(damage, owner);
        hurtBox.Owner.GetComponent<DamageFlash>().CallDamageFlash();
        Projectile projectile = transform.root.GetComponentInChildren<Projectile>();
        if (projectile == null) return;
        Destroy(projectile.gameObject);
    }
}
