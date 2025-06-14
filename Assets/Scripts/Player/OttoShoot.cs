using System;
using Managers;
using UnityEngine;

//Possibly the most complicated code here
namespace Player
{
    public class OttoShoot : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab; 
        [SerializeField] private Transform shootPoint; //where the projectile is spawned from
        [SerializeField] private LayerMask targetLayer; 
        [SerializeField] private Vector2 capsuleSize = new Vector2(5f, 2f); 
        [SerializeField] private float projectileSpeed = 5f;
    
        public float shootCooldown = 1f; 
        private float _shootTimer; 
        [NonSerialized] public bool ShootInput;
        private bool _canShoot = true;
    
        private GameObject _nearestEnemy;
    
        public enum Direction { Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft }
        private static Direction _aimDirection = Direction.Up;

        private Camera cam;
        [SerializeField] private AudioClip shootSfx;


        private void Update()
        {
            //FindNearestEnemy();
            UpdateAimDirection();
            UpdateShootTimer();
            TryShoot();
        }

        private void Awake()
        {
            if (GameObject.Find("OttoSocket")){
            }
        }

        private void Start()
        {
            cam = Camera.main;
        }

        private void UpdateAimDirection()
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDirection = mouseWorldPos - transform.position;
            mouseDirection.z = 0;

            float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360f;

            if (angle >= 337.5f || angle < 22.5f) _aimDirection = Direction.Right;
            else if (angle >= 22.5f && angle < 67.5f) _aimDirection = Direction.UpRight;
            else if (angle >= 67.5f && angle < 112.5f) _aimDirection = Direction.Up;
            else if (angle >= 112.5f && angle < 157.5f) _aimDirection = Direction.UpLeft;
            else if (angle >= 157.5f && angle < 202.5f) _aimDirection = Direction.Left;
            else if (angle >= 202.5f && angle < 247.5f) _aimDirection = Direction.DownLeft;
            else if (angle >= 247.5f && angle < 292.5f) _aimDirection = Direction.Down;
            else _aimDirection = Direction.DownRight;
        }

        /*private void FindNearestEnemy()
    {
        // ReSharper disable once Unity.PreferNonAllocApi
        Collider2D[] enemyColliders = Physics2D.OverlapCapsuleAll(shootPoint.position, capsuleSize, CapsuleDirection2D.Vertical, 0f, targetLayer);
        float nearestDistance = Mathf.Infinity;
        _nearestEnemy = null;
        
        foreach (Collider2D enemyCollider in enemyColliders)
        {
            float distance = Vector2.Distance(shootPoint.position, enemyCollider.transform.position);
            if (!(distance < nearestDistance)) continue;
            nearestDistance = distance;
            _nearestEnemy = enemyCollider.gameObject;
        }
    }*/
        private void UpdateShootTimer()
        {
            if (_canShoot) {_shootTimer= shootCooldown; return;}
            _shootTimer -= Time.deltaTime;
            if (!(_shootTimer <= 0)) return;
            _canShoot = true;
            _shootTimer = shootCooldown; // Reset the timer
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void TryShoot()
        {
            if (ShootInput && _canShoot)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            Vector2 direction = _aimDirection switch
            {
                Direction.Up => Vector2.up,
                Direction.UpRight => new Vector2(1, 1).normalized,
                Direction.Right => Vector2.right,
                Direction.DownRight => new Vector2(1, -1).normalized,
                Direction.Down => Vector2.down,
                Direction.DownLeft => new Vector2(-1, -1).normalized,
                Direction.Left => Vector2.left,
                Direction.UpLeft => new Vector2(-1, 1).normalized,
                _ => new Vector2(0, 0).normalized
            };
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed; 
            AudioManager.instance.PlaySFXAt(shootSfx, shootPoint, 0.7f, 0.5f);
            _canShoot = false; 
        }
    }
}
