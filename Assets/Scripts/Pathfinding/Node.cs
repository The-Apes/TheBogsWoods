using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class Node : MonoBehaviour
    {
        public Node cameFrom;
        public List<Node> connections;

        public float gScore;
        public float hScore;

        public float FScore()
        {
            return gScore + hScore;
        }

        private void Start()
        {
            name =  transform.position.x + ", " + transform.localPosition.y;
        }
    }
}
