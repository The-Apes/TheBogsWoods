using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float maxHealth;
    private float _health;
    [SerializeField] private float damage; //damage this entity deals
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
         float _health = maxHealth; //health of this entity
    }

    private void Damage(float damageTaken)
    {
        _health -= damageTaken;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
}
