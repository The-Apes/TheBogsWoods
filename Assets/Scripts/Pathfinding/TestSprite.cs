using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pathfinding
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class TestSprite : MonoBehaviour
    {
        public Node currentNode;
        public List<Node> path = new List<Node>();

        private void Start()
        {
            currentNode = FindFirstObjectByType<Node>();
        }

        private void Update()
        {
            CreatePath();
        }

        public void CreatePath()
        {
            if (path.Count > 0)
            {
                int x = 0;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2), 3 * Time.deltaTime);

                if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
                {
                    currentNode = path[x];
                    path.RemoveAt(x);
                }
            }
            else
            {
                Node[] nodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
                while (path == null || path.Count == 0)
                {
                    path = AStarManager.instance.GeneratePath(currentNode, nodes[Random.Range(0, nodes.Length)]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (path.Count > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 1; i < path.Count; i++)
                {
                    Gizmos.DrawLine(path[i].transform.position, path[i - 1].transform.position);
                }
            }
        }
    }
}
