using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Pathfinding
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class AStarManager : MonoBehaviour
    {
        public static AStarManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public List<Node> GeneratePath(Node start, Node end)
        {
            List<Node> openSet = new List<Node>();

            foreach (Node node in FindObjectsByType<Node>(FindObjectsSortMode.None))
            {
                node.gScore = float.MaxValue;
            }

            start.gScore = 0;
            start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                int lowestF = default;

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FScore() < openSet[lowestF].FScore())
                    {
                        lowestF = i;
                    }
                }

                Node currentNode = openSet[lowestF];
                openSet.Remove(currentNode);

                if (currentNode == end) //We have found optimal path
                {
                    List<Node> path = new List<Node>();

                    path.Insert(0, end);

                    while (currentNode != start)
                    {
                        currentNode = currentNode.cameFrom;
                        path.Add(currentNode);
                    }

                    path.Reverse();
                    return path;

                }

                //All the code above was pre-Neighbour checks, incase you found an optimal path without checking neighbours.

                foreach (Node connectedNode in currentNode.connections)
                {
                    float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position,
                        connectedNode.transform.position);

                    if (heldGScore < connectedNode.gScore)
                    {
                        connectedNode.cameFrom = currentNode;
                        connectedNode.gScore = heldGScore;
                        connectedNode.hScore =
                            Vector2.Distance(connectedNode.transform.position, end.transform.position);

                        if (!openSet.Contains(connectedNode))
                        {
                            openSet.Add(connectedNode);
                        }
                    }
                }
            }



            return null;
        }

        public Node FindNearestNode(Vector2 position)
        {
            Node foundNode = null;
            float minDistance = float.MaxValue;

            foreach (Node node in NodesInScene())
            {
                float currentDistance = Vector2.Distance(node.transform.position, position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    foundNode = node;
                }
            }

            return foundNode;
        }

        public Node FindFurthestNode(Vector2 position)
        {
            Node foundNode = null;
            float maxDistance = 0;

            foreach (Node node in NodesInScene())
            {
                float currentDistance = Vector2.Distance(position, node.transform.position);
                if (currentDistance > maxDistance)
                {
                    maxDistance = currentDistance;
                    foundNode = node;
                }
            }

            return foundNode;
        }

        public Node[] NodesInScene()
        {
            return FindObjectsByType<Node>(FindObjectsSortMode.None);
        }

        public Node[] NodesInRadius(Vector2 center, float radius)
        {
            List<Node> nodesInRadius = new List<Node>();
            foreach (Node node in NodesInScene())
            {
                if (Vector2.Distance(node.transform.position, center) <= radius)
                {
                    nodesInRadius.Add(node);
                }
            }

            return nodesInRadius.ToArray();
        }

        public Node RandomNodeInRadius(Vector2 center, float radius)
        {
            Node[] nodes = NodesInRadius(center, radius);
            if (nodes.Length == 0) return null;
            return nodes[Random.Range(0, nodes.Length)];
        }
    }
}
