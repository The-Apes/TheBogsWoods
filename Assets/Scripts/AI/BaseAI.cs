using System.Collections.Generic;
using Pathfinding;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class BaseAI : MonoBehaviour
    {
        public Node currentNode;
        public List<Node> path;

        public RuriMovement player;
        
        private float speed = 3;
        private int currHealth = 3;
        private int maxHealth = 3;
        
        private Rigidbody2D rb;
        
        public enum StateMachine
        {
            Patrol,
            Engage,
            Evade
        }
        
        public StateMachine currentState;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            player = RuriMovement.instance;
         currentState = StateMachine.Patrol;
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
            switch (currentState)
            {
                case StateMachine.Patrol:
                    Patrol();
                    break;
                case StateMachine.Engage:
                    Engage();
                    break;
                case StateMachine.Evade:
                    Evade();
                    break;
            }
            
            bool playerSeen = Vector2.Distance(transform.position, player.transform.position) < 5f;

            if (playerSeen == false && currentState != StateMachine.Patrol && currHealth > (maxHealth*50)/100)
            {
                currentState = StateMachine.Patrol;
                path.Clear();
            }else if (playerSeen == true && currentState != StateMachine.Engage && currHealth > (maxHealth*50)/100)
            {
                currentState = StateMachine.Engage;
                path.Clear();
            }
            else if (currentState != StateMachine.Evade && currHealth <= (maxHealth*50)/100)
            {
                currentState = StateMachine.Evade;
                path.Clear();
            }
            
            CreatePath();
        }

        void Patrol()
        {
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.NodesInScene()[Random.Range(0, AStarManager.instance.NodesInScene().Length)]); //wtf
            }
        }

        void Engage()
        {
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode,AStarManager.instance.FindNearestNode(player.transform.position));
            }
        }

        void Evade()
        {
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
            }
        }

        void CreatePath()
        {
            if (path.Count > 0)
            {
                int x = 0;
                var direction = (path[x].transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;

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
    }
}
