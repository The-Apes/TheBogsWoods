using Pathfinding;
using UnityEngine;

namespace AI
{
    public class CowardMonkeyAI : BaseAI    {
        protected override void SwitchOnState()
        {
            switch (currentState)
            {
                case StateMachine.Flee:
                    Flee();
                    break;
               default:
                   base.SwitchOnState();
                   break;
            }
        }

        protected override  void ChangeState()
        {
          base.ChangeState();
          
          if (base.playerSeen == true && currentState != StateMachine.Engage && currHealth > (maxHealth*50)/100)
          {
              currentState = StateMachine.Engage;
              path.Clear();
          }
          
        }
        
        void Flee()
        {
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
            }
        }
    }
}
