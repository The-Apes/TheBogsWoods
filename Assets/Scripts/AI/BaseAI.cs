using System;
using System.Collections.Generic;
using Pathfinding;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace AI
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class BaseAI : MonoBehaviour
    { 
        [Header("Stats")] 
        [SerializeField] protected float moveSpeed = 3;
        [SerializeField] protected int currHealth;
        [SerializeField] protected int maxHealth = 5;
        [SerializeField] protected float detectionRadius = 3.5f;
        
        [Header("Wander")]
        [SerializeField] protected Vector2 wanderLocation;
        [SerializeField] protected float wanderRadius = 5f;
        
        [Header("Node")]
        public Node currentNode;
        public List<Node> path;
        
        protected float patrolTimer = 0f;
        protected float patrolInterval = 5f;

        public RuriMovement player;
        private Rigidbody2D rb;
        
        public enum StateMachine
        {
            Patrol,
            Engage,
            Search,
            Flee
        }
        
        public StateMachine currentState;
        protected bool playerSeen;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            player = RuriMovement.instance;
         currentState = StateMachine.Patrol;
         currHealth = maxHealth;
         if (currentNode == null)
         {
             currentNode = AStarManager.instance.FindNearestNode(transform.position);
             // var cast = Physics2D.OverlapCircleAll(transform.position, 2);
             // foreach (var thing in cast)
             // {
             //        if (thing.TryGetComponent(out Node node))
             //        {
             //            currentNode = node;
             //            break;
             //        }
             // }
         }
        }

        private void FixedUpdate()
        {
            SwitchOnState();
            ChangeState();
            CreatePath();
        }

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
            playerSeen = Vector2.Distance(transform.position, player.transform.position) < detectionRadius;

            if (playerSeen == false && currentState != StateMachine.Patrol && currHealth > (maxHealth*50)/100)
            {
                currentState = StateMachine.Patrol;
                path.Clear();
            }else if (playerSeen == true && currentState != StateMachine.Engage && currHealth > (maxHealth*50)/100)
            {
                currentState = StateMachine.Engage;
                path.Clear();
            }
        }


        private void Patrol()
        {
            if(path.Count == 0){
                patrolTimer += Time.deltaTime;
                if (patrolTimer >= patrolInterval)
                {
                    Node randomNode = AStarManager.instance.RandomNodeInRadius(wanderLocation,wanderRadius);
                    path = AStarManager.instance.GeneratePath(currentNode, randomNode);
                    
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
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode,AStarManager.instance.FindNearestNode(player.transform.position));
            }
        }

        private void Search()
        {
            rb.linearVelocity = Vector2.zero;
        }

        void CreatePath()
        {
            if (path.Count > 0)
            {
                int x = 0;
                var direction = (path[x].transform.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;

                if (Vector2.Distance(rb.position, path[x].transform.position) <= 0.1f)
                {
                    currentNode = path[x];
                    path.RemoveAt(x);
                }
            }
            else
            {
                // Stop when there’s no path
                rb.linearVelocity = Vector2.zero;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            
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
    }
}
