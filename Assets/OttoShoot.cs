using System;
using UnityEngine;

//Possibly the most complicated code here
public class OttoShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; //this is a variable that stores the projectile prefab
    [SerializeField] private Transform shootPoint; //this is a variable that stores the shoot point of the player
    [SerializeField] private LayerMask targetLayer; //this is a variable that stores the enemy layer
    [SerializeField] private Vector2 capsuleSize = new Vector2(5f, 2f); //this is a variable that stores the size of the capsule
    [SerializeField] private float projectileSpeed = 5f;
    
    public float shootCooldown = 1f; // Cooldown time in 
    private float _shootTimer; // Timer to track cooldown
    [NonSerialized] public bool ShootInput;
    private bool _canShoot = true;
    
    private GameObject _nearestEnemy;

    private void Update()
    {
        FindNearestEnemy();
        UpdateShootTimer();
        TryShoot();
    }

    private void Awake()
    {
        if (GameObject.Find("OttoSocket")){
         //shootPoint = GameObject.Find("OttoSocket").transform;
        }
    }

    private void FindNearestEnemy()
    {
        // ReSharper disable once Unity.PreferNonAllocApi
        Collider2D[] enemyColliders = Physics2D.OverlapCapsuleAll(shootPoint.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, targetLayer);
        float nearestDistance = Mathf.Infinity;
        _nearestEnemy = null;
        
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            float distance = Vector2.Distance(shootPoint.position, enemyCollider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                _nearestEnemy = enemyCollider.gameObject;
            }
        }
    }
    private void UpdateShootTimer()
    {
        if (_canShoot) {_shootTimer= shootCooldown; return;}
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
        {
            _canShoot = true;
            _shootTimer = shootCooldown; // Reset the timer
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void TryShoot()
    {
        if (ShootInput && _canShoot)
        {
            Shoot();
        }
    }

    private void Shoot(){
        if (_nearestEnemy != null) {
           Vector2 direction = (_nearestEnemy.transform.position - shootPoint.position).normalized;
          GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
         projectile.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * projectileSpeed; // Set the projectile's velocity
         _canShoot = false; // Set the cooldown
        }
        else
        {
            Transform lookDir;
            if (transform.root.gameObject.name == "Ruri")
            {
                lookDir = transform.root.GetComponent<RuriMovement>().lookDir;
                print("smart man");
            }
            else
            {
                lookDir = transform.root.GetComponent<OttoScript>().lookDir;
            }
            
            //Credits to CoPilot, no way I was doing this on my own.
            float angleInRadians = (lookDir.eulerAngles.z - 90f) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
            
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * projectileSpeed; // Set the projectile's velocity
            _canShoot = false; // Set the cooldown
            //ruriMovement.lookDir;
        }
    }
}
