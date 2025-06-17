using Pathfinding;
using UnityEngine;

namespace AI
{
    public class CowardMonkeyAI : BaseAI    {

        public enum StateMachine
        {
            Patrol,
            Engage,
            Flee
        }
        
        void Evade()
        {
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
            }
        }
    }
}
