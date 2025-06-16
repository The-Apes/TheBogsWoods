using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    // Easy Pathfinding for Unity 2D and 3D Games! [Pathfinding Tutorial]
    // Game Dev Garnet
    // 16 June 2025
    // https://youtu.be/UHnOW-OimLQ?si=sHR9m9zjHw7JTgUh
    public class NodeGenerator : MonoBehaviour
    {
        private Tilemap tilemap;
        [SerializeField] private GameObject parent;
        [SerializeField] private GameObject nodePrefab;
        
        public List<Node> nodeList;
        public TestSprite npc;
        
        private bool canDrawGizmos;
        

        void Awake()
        {
            tilemap = GetComponent<Tilemap>();
           CreateNodes();
        }

        void CreateNodes()
        {
            BoundsInt bounds = tilemap.cellBounds;

            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    Vector3 worldPos = tilemap.CellToWorld(pos);
                    
                    worldPos += tilemap.cellGap + new Vector3(0.5f,0.5f,0); // centers da tile

                    Node node = Instantiate(nodePrefab, parent.transform).GetComponent<Node>();
                    node.gameObject.transform.position = worldPos;
                    nodeList.Add(node);
                }
            }
            CreateConnections();
        }

        void CreateConnections()
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                for (int j = i + 1; j < nodeList.Count; j++)
                {
                    if (Vector2.Distance(nodeList[i].transform.position, nodeList[j].transform.position) <= 1f)
                    {
                        ConnectNodes(nodeList[i], nodeList[j]);
                        ConnectNodes(nodeList[j], nodeList[i]);
                    }
                }
            }
            canDrawGizmos = true;
        }

        void ConnectNodes(Node from, Node to)
        {
            if (from == to) return;
            
            from.connections.Add(to);
        }
        
        //left out spawnAI

        /*private void OnDrawGizmos()
        {
            if (canDrawGizmos)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    for (int j = 0; j < nodeList[i].connections.Count; j++)
                    {
                        Gizmos.DrawLine(nodeList[i].transform.position, nodeList[i].connections[j].transform.position);
                    }
                }
            }
            
        }*/
    }
}
