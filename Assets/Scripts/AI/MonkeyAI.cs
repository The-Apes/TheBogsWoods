
using System.Collections.Generic;
using System.IO;
using HeightObjects;
using Managers;
using Pathfinding;
using Player;
using Saving;
using UnityEngine;


namespace AI
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class BaseAI : MonoBehaviour, IDamageable
    { 
        [Header("Stats")] 
        [SerializeField] protected float chaseSpeed = 3; 
        [SerializeField] protected float patrolSpeed = 1;
        public int currHealth;
        [SerializeField] protected int maxHealth = 2;
        [SerializeField] protected float detectionRadius = 3.5f;
        [SerializeField] protected float chaseRadius = 5f;
        [SerializeField] protected float attackDistance = 2f;

        [Header("Potions")]
        [SerializeField] private int potionDropAmount;
        [SerializeField] private GameObject potionPrefab;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip[] deathSounds;
        [SerializeField] private AudioClip[] hurtSounds;
        [SerializeField] private AudioClip[] chaseSounds;
        
        
        
        [Header("Wander")]
        [SerializeField] protected Vector2 wanderLocation;
        [SerializeField] protected float wanderRadius = 5f;
        [SerializeField] private float searchRadius =3f;
        [SerializeField] private float searchDuration =10f;
        
        [Header("Node")]
        public Node currentNode;
        public List<Node> path;
        
       protected float patrolTimer = 0f;
       protected float patrolInterval = 5f;

       private GameObject player;
        private Rigidbody2D _rb;
        
        public enum StateMachine
        {
            Patrol,
            Engage,
            Attack,
            Search,
            Flee,
            Death
        }
        public enum MoveType
        {
            Pathfind,
            StraightLine,
        }
        
        public StateMachine currentState;
        public MoveType moveType;

        public bool inDetectionRange;
        public bool los;
 
      
        public Vector2 searchLocation;
        public bool shouldSearch;

        public float radius;
        public float moveSpeed;

        public int lineCastLayerMask;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _direction;
        private bool dead;
        private HitBox _hitBox;
        private HurtBox _hurtBox;
        [SerializeField] private AudioClip footstepSound;
        private bool attacking;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        
        private void Start()
        {
            _hurtBox = GetComponentInChildren<HurtBox>();
            _hitBox = GetComponentInChildren<HitBox>();
            currentState = StateMachine.Patrol;
         currHealth = maxHealth;
         radius = detectionRadius;
         
         _animator = GetComponent<Animator>();
         _spriteRenderer = GetComponent<SpriteRenderer>();
         
         
         if (currentNode == null) currentNode = AStarManager.instance.FindNearestNode(transform.position);
         
         lineCastLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Environment"));
         player = FindFirstObjectByType<RuriMovement>().gameObject;

        }
        private void FixedUpdate()
        {
            //Check if player is in detection range
            if (!player) player = FindFirstObjectByType<RuriMovement>().gameObject;
            if (los) radius = chaseRadius;
            inDetectionRange = Vector2.Distance(transform.position, player.gameObject.transform.position) < radius;
            if (inDetectionRange)
            {
                //Checks Line of Sight
                RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position,lineCastLayerMask);
                los = hit.collider && (hit.collider.transform == player.transform || hit.collider.transform.root == player.transform);
            }
            
            SwitchOnState();
            ChangeState(); 
            if(!attacking)Move();
            UpdateCurrentNode();
        }
        private void Update()
        {
            /*if (!_run)
            {
                if (!_chaseTarget) return;
                _direction = (_chaseTarget.transform.position - transform.position).normalized;
                _rb.linearVelocity = _direction.normalized * chaseSpeed;
            }
            else
            {
                if (!_runTarget) return;
                _direction = (transform.position - _runTarget.transform.position).normalized;
                _rb.linearVelocity = _direction.normalized * (chaseSpeed * 1.5f);
            }*/




            if (!_animator) return;

            if (_direction.Equals(Vector2.zero))
            {
                _animator.SetFloat("BodyX", 1f);
                _animator.SetFloat("BodyY", 0f);
            }

            if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
            {
                _animator.SetFloat("BodyX", _direction.x > 0 ? 1f : -1f);
                _animator.SetFloat("BodyY", 0f);
            }
            else
            {
                _animator.SetFloat("BodyX", 0f);
                _animator.SetFloat("BodyY", _direction.y > 0 ? 1f : -1f);
            }
        }
        void Step()
        {
                AudioManager.instance.PlaySFXAt(footstepSound, transform, 0.9f, 0.3f, 1.25f);
        }

        void StopAttacking()
        {
            attacking = false;
        }
        
        #region Logic
        // =======================
        // Logic
        // =======================
        public virtual void ReceiveDamage(int damageTaken, GameObject source){
            if (dead) return; // If already dead, ignore further damage
            currHealth -= damageTaken; 
            
            // Get hit point and direction
            Vector2 hitPoint = transform.position;
            Vector2 hitDir = (-1*_rb.linearVelocity).normalized;

            // Flash + Visuals
            GetComponent<DamageFlash>().CallDamageFlash();
            HitEffectManager.Instance?.PlayHitEffects(hitPoint, hitDir);

            // Hit stop + knockback
            GameManager.instance.HitStop(0.1f);
            ApplyKnockback(source.transform.position, 20f);
            if (currHealth <= 0) 
            {
                if(deathSounds.Length > 0) AudioManager.instance.PlayRandomSFXAt(deathSounds, transform);
                dead = true;
            }
            else
            {
                
                ApplyKnockback(source.transform.position, 25f);
                if(hurtSounds.Length > 0) AudioManager.instance.PlayRandomSFXAt(hurtSounds, transform);
                DropPotions();
                TryGetComponent<SaveableObject>(out SaveableObject saveable);
                if (saveable)
                {
                    SaveManager.instance.ChangeFlag(saveable.saveID, false);
                }
            }
        }
        private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        
        private void DropPotions()
        {
            if (potionDropAmount <= 0 || !potionPrefab) return;

            for (int i = 0; i < potionDropAmount; i++)
            {
                GameObject potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
                potion.GetComponent<FakeHeightObject>().Initialize(
                    new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * Random.Range(3, 6),
                    Random.Range(3, 6)
                );
            }
        }
        #endregion



        #region state machine
        // =======================
        // state machine
        // =======================
        protected virtual void SwitchOnState()
        {
            switch (currentState)
            {
                case StateMachine.Patrol:
                    Patrol();
                    break;
                case StateMachine.Engage:
                    Engage();
                    break;
                case StateMachine.Attack:
                    Attack();
                    break;
                case StateMachine.Search:
                    Search();
                    break;
                case StateMachine.Flee:
                    Flee();
                    break;
                case StateMachine.Death:
                    Death();
                    break;
            }
        }

         void ChangeState()
        {
            if ((inDetectionRange && los) && Vector2.Distance(transform.position, player.gameObject.transform.position) <= attackDistance && currentState != StateMachine.Attack && currHealth == 2)
            {
                currentState = StateMachine.Attack;
                moveSpeed = chaseSpeed;
                path.Clear();
            }
            if (!(inDetectionRange && los) && currentState != StateMachine.Search && shouldSearch && currHealth == 2)
            {
                    path.Clear();
                    moveType = MoveType.Pathfind;
                    searchLocation = player.transform.position;
                    path = AStarManager.instance.GeneratePath(GetNearestNode(),AStarManager.instance.FindNearestNode(searchLocation));
                    currentState = StateMachine.Search;
                    moveSpeed = chaseSpeed;
            }

            if (dead && currentState != StateMachine.Death)
            {
                currentState = StateMachine.Death;
                _animator.SetTrigger("Death");
                //Destroy(_hurtBox);
                path.Clear();
            }
            else if (inDetectionRange == false && currentState != StateMachine.Patrol && !shouldSearch &&currHealth == 2)
            {
                currentState = StateMachine.Patrol;
                path.Clear();
            }else if (inDetectionRange && los && Vector2.Distance(transform.position, player.gameObject.transform.position) > attackDistance && currentState != StateMachine.Engage && currHealth == 2)
            {
                if(chaseSounds.Length > 0) AudioManager.instance.PlayRandomSFXAt(chaseSounds, transform);
                shouldSearch = true;
                //startedSearching = false;
                currentState = StateMachine.Engage;
                moveSpeed = chaseSpeed;
                path.Clear();
            }   else if (!dead && inDetectionRange && los && currentState != StateMachine.Flee && currHealth <= (maxHealth*50)/100)
            {
                currentState = StateMachine.Flee;
                Destroy(_hitBox);
                path.Clear();
            }
        }


        private void Patrol()
        {
            moveSpeed = patrolSpeed;
            _animator.SetFloat("Speed", 0.5f);
            moveType = MoveType.Pathfind;
            radius = detectionRadius;
            
            if(path.Count == 0){
                patrolTimer += Time.deltaTime;
                if (patrolTimer >= patrolInterval)
                {
                    Node randomNode = AStarManager.instance.RandomNodeInRadius(wanderLocation,wanderRadius);
                    path = AStarManager.instance.GeneratePath(GetNearestNode(), randomNode);
                    
                    patrolTimer = 0f;
                }
            }
            else
            { 
                patrolTimer = 0f;
            }
        }

        private void Engage()
        {
            radius = chaseRadius;
            _animator.SetFloat("Speed", 1f);
            moveSpeed = chaseSpeed;
            moveType = los? MoveType.StraightLine : MoveType.Pathfind;
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(GetNearestNode(),AStarManager.instance.FindNearestNode(player.transform.position));
            }
        }

        private void Attack()
        {
            moveType = MoveType.StraightLine;
            if(!attacking)
            {
                _animator.SetTrigger("Attack");
                attacking = true;
            }
        }

        private void Search()
        {
            moveType = MoveType.Pathfind;
            radius = chaseRadius;
            moveSpeed = chaseSpeed;
            _animator.SetFloat("Speed", 1f);
                if (path.Count == 0)
                {
                    //path = AStarManager.instance.GeneratePath(GetNearestNode(), AStarManager.instance.FindNearestNode(searchLocation));
                    shouldSearch = false;
                }  
        }
        void Flee()
        {
            moveType = MoveType.Pathfind;
            _animator.SetFloat("Speed", 1.25f);
            moveSpeed = chaseSpeed + 1f;
            if (path.Count == 0)
            {
                  path = AStarManager.instance.GeneratePath(GetNearestNode(), AStarManager.instance.FindFurthestNode(player.transform.position));
            }
            if(!_spriteRenderer.isVisible) Destroy(gameObject);

        }
        void Death()
        {
            if(!_spriteRenderer.isVisible) Destroy(gameObject);
        }
        #endregion
        #region pathfinding
        public Node GetNearestNode()
        {
            return currentNode = AStarManager.instance.FindNearestNode(transform.position);
        }
        private void UpdateCurrentNode()
        {
            if(path.Count == 0) return;
            int x = 0;
            if (!(Vector2.Distance(_rb.position, path[x].transform.position) <= 0.2f)) return;
            currentNode = path[x];
            path.RemoveAt(x);
        }

        void Move()
        {
            switch (moveType)
            {
                case MoveType.Pathfind:
                    PathfindTo();
                    break;
                case MoveType.StraightLine:
                    StraightLineToPlayer();
                    path.Clear();
                    break;
                
            }
            _animator.SetBool("Moving", _rb.linearVelocity != Vector2.zero);
        }
        void PathfindTo()
        {
            if (path.Count > 0)
            {
                int x = 0;
                 _direction = (path[x].transform.position - transform.position).normalized;
                _rb.linearVelocity = _direction * moveSpeed;
                
                UpdateCurrentNode();
            }
            else
            {
                // Stop when there’s no path
                _rb.linearVelocity = Vector2.zero;
            }
        }
        
        void StraightLineToPlayer()
        {
             _direction = (player.transform.position - transform.position).normalized;
            _rb.linearVelocity = _direction * moveSpeed;
            
            //so when it switched back to pathfinding it isn't broke
            UpdateCurrentNode();

            // in range will be it's own thing
            // if (Vector2.Distance(rb.position, player.transform.position) <= 0.1f)
            // {
            //     // Stop when close enough to the player
            //     rb.linearVelocity = Vector2.zero;
            // }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
            
            if (path.Count > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 1; i < path.Count; i++)
                {
                    Gizmos.DrawLine(path[i].transform.position, path[i - 1].transform.position);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(wanderLocation, wanderRadius);

        }
        #endregion
    }
}
