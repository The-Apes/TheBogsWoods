using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

//Possibly the most complicated code here
public class OttoShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; //this is a variable that stores the projectile prefab
    [SerializeField] private Transform shootPoint; //this is a variable that stores the shoot point of the player
    [SerializeField] private LayerMask enemyLayer; //this is a variable that stores the enemy layer
    [SerializeField] private Vector2 capsuleSize = new Vector2(5f, 2f); //this is a variable that stores the size of the capsule
    [SerializeField] private CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical;
    private GameObject _nearestEnemy;

    private void Update()
    {
        FindNearestEnemy();
    }
    private void FindNearestEnemy()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCapsuleAll(shootPoint.position, capsuleSize, capsuleDirection, 0f, enemyLayer);
        float nearestDistance = Mathf.Infinity;
        _nearestEnemy = null;
        
        foreach (Collider2D collider in enemyColliders)
        {
            float distance = Vector2.Distance(shootPoint.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                _nearestEnemy = collider.gameObject;
            }
        }
    }
    public void Shoot(){
        if (_nearestEnemy == null) return;
        Vector2 direction = (_nearestEnemy.transform.position - shootPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 5; // Set the projectile's velocity
        
    }
}
