
using System.Collections.Generic;
using Managers;
using Pathfinding;
using Player;
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
        [SerializeField] protected int maxHealth = 5;
        [SerializeField] protected float detectionRadius = 3.5f;
        [SerializeField] protected float chaseRadius = 5f;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip[] deathSounds;
        [SerializeField] private AudioClip[] hurtSounds;
        
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

        public GameObject player;
        private Rigidbody2D _rb;
        
        public enum StateMachine
        {
            Patrol,
            Engage,
            Search,
            Flee
        }
        public enum MoveType
        {
            Pathfind,
            StraightLine,
        }
        
        public StateMachine currentState;
        public MoveType moveType;

        protected bool inDetectionRange;
        protected bool los;
 
      
        public Vector2 searchLocation;
        public bool shouldSearch;

        public float radius;
        public float moveSpeed;

        public int lineCastLayerMask;
        private Animator _animator;
        
 


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        
        private void Start()
        {
         currentState = StateMachine.Patrol;
         currHealth = maxHealth;
         radius = detectionRadius;
         
         _animator = GetComponent<Animator>();
         
         
         if (currentNode == null) currentNode = AStarManager.instance.FindNearestNode(transform.position);
         
         lineCastLayerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Environment"));
         player = FindFirstObjectByType<RuriMovement>().gameObject;

        }
        private void FixedUpdate()
        {
            //Check if player is in detection range
            //if(!player) player = RuriMovement.instance;
            if(player)inDetectionRange = Vector2.Distance(transform.position, player.transform.position) < radius;
            if (inDetectionRange)
            {
                //Ches Line of Sight
                var hit = Physics2D.Linecast(transform.position, player.transform.position,lineCastLayerMask);
                los = hit.collider != null && (hit.collider.transform == player.transform || hit.collider.transform.root == player.transform);
            }
            
            SwitchOnState();
            ChangeState(); 
            Move();
            UpdateCurrentNode();
        }
        
        #region Logic
        // =======================
        // Logic
        // =======================
        public virtual void ReceiveDamage(int damageTaken, GameObject source){
            currHealth -= damageTaken; 
            GetComponent<DamageFlash>().CallDamageFlash();
            GameManager.instance.HitStop(0.1f);
            if (currHealth <= 0) 
            {
                if(deathSounds.Length > 0) AudioManager.instance.PlayRandomSFXAt(deathSounds, transform);
                Destroy(gameObject); 
            }
            else
            {
                ApplyKnockback(source.transform.position, 25f);
                if(hurtSounds.Length > 0) AudioManager.instance.PlayRandomSFXAt(hurtSounds, transform);
            }
        }
        private void ApplyKnockback(Vector2 sourcePosition, float knockbackForce)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)sourcePosition).normalized;
            _rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
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
                case StateMachine.Search:
                    Search();
                    break;
            }
        }

        protected virtual void ChangeState()
        {
            if (!(inDetectionRange && los) && currentState != StateMachine.Search && shouldSearch && currHealth > (maxHealth * 50) / 100)
            {
                    searchLocation = player.transform.position;
                    currentState = StateMachine.Search;
                    moveSpeed = chaseSpeed;
                    path.Clear();
            }

            if (inDetectionRange == false && currentState != StateMachine.Patrol && !shouldSearch &&currHealth > (maxHealth*50)/100)
            {
                currentState = StateMachine.Patrol;
                path.Clear();
            }else if (inDetectionRange && los && currentState != StateMachine.Engage && currHealth > (maxHealth*50)/100)
            {
                shouldSearch = true;
                //startedSearching = false;
                currentState = StateMachine.Engage;
                moveSpeed = chaseSpeed;
                path.Clear();
            }
        }


        private void Patrol()
        {
            moveSpeed = patrolSpeed;
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
            moveSpeed = chaseSpeed;
            moveType = los? MoveType.StraightLine : MoveType.Pathfind;
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(GetNearestNode(),AStarManager.instance.FindNearestNode(player.transform.position));
            }
        }

        private void Search()
        {
            radius = chaseRadius;
            moveSpeed = chaseSpeed;
            moveType = MoveType.Pathfind;
                if (path.Count == 0)
                {
                    path = AStarManager.instance.GeneratePath(GetNearestNode(), AStarManager.instance.FindNearestNode(searchLocation));    
                }  
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
        }
        void PathfindTo()
        {
            if (path.Count > 0)
            {
                int x = 0;
                var direction = (path[x].transform.position - transform.position).normalized;
                _rb.linearVelocity = direction * moveSpeed;
                
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
            var direction = (player.transform.position - transform.position).normalized;
            _rb.linearVelocity = direction * moveSpeed;
            
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
            Gizmos.DrawWireSphere(transform.position, inDetectionRange ? detectionRadius+1 : detectionRadius);
            
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
