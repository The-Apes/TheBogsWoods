using HeightObjects;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    public GameObject owner;
    private int _layer;
    private string _tag;
    [SerializeField] private bool continuousDamage;
    
    private void Awake()
    {
        owner = gameObject; 
        _layer = gameObject.layer; //get the layer of this object, layers are stored in a bitmask, so it's weird...
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("HurtBox")) return; 
        HurtBox hurtBox = other.GetComponent<HurtBox>(); 
        if (hurtBox.Owner == null) return; 
        if (other.gameObject.layer == _layer) return;

        IDamageable damageable = hurtBox.Owner.GetComponent<IDamageable>(); 
        if (damageable == null) return;
        damageable.ReceiveDamage(damage, owner);
        Projectile projectile = transform.root.GetComponentInChildren<Projectile>();
        if (projectile == null) return;
        Destroy(projectile.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!continuousDamage) return;
        if (!other.CompareTag("HurtBox")) return; 
        HurtBox hurtBox = other.GetComponent<HurtBox>(); 
        if (hurtBox.Owner == null) return; 
        if (other.gameObject.layer == _layer) return;

        IDamageable damageable = hurtBox.Owner.GetComponent<IDamageable>(); 
        if (damageable == null) return;
        damageable.ReceiveDamage(damage, owner);
        Projectile projectile = transform.root.GetComponentInChildren<Projectile>();
        if (projectile == null) return;
        Destroy(projectile.gameObject);
    }
}
