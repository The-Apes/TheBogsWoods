using Pathfinding;
using UnityEngine;


namespace AI
{
    public class CowardMonkeyAI : BaseAI    {
        private SpriteRenderer spriteRenderer;
        /*public virtual void ReceiveDamage(int damageTaken, GameObject source)
        {
            currHealth -= damageTaken;

            // Get hit point and direction
            Vector2 hitPoint = GetComponent<Collider2D>().ClosestPoint(source.transform.position);
            Vector2 hitDir = (transform.position - source.transform.position).normalized;

            // Flash + Visuals
            GetComponent<DamageFlash>().CallDamageFlash();
            HitEffectManager.Instance?.PlayHitEffects(hitPoint, hitDir);

            // Hit stop + knockback
            GameManager.instance.HitStop(0.1f);
            ApplyKnockback(source.transform.position, 20f);

            if (currHealth <= 0)
            {
                AudioManager.instance.PlaySFXAt(deathSound, transform);

                if (RuriMovement.instance.gameObject)
                {
                    _runTarget = RuriMovement.instance.controlling ? RuriMovement.instance.gameObject : RuriMovement.instance.Otto;
                }

                _run = true;
                DropPotions();

                Collider2D[] colliders = GetComponents<Collider2D>();
                foreach (Collider2D otherCollider in colliders)
                {
                    otherCollider.enabled = false;
                }
            }
            else
            {
                AudioManager.instance.PlaySFXAt(hurtSound, transform);
            }
        }*/


        #region State Machine

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

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
            //print("");
            if (inDetectionRange && los && currentState != StateMachine.Flee && currHealth <= (maxHealth*50)/100)
            {
                print("flee");
                currentState = StateMachine.Flee;
                path.Clear();
            }
          
        }
        
        void Flee()
        {
            moveType = MoveType.Pathfind;
            moveSpeed = chaseSpeed + 3f;
            if (path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(GetNearestNode(), AStarManager.instance.FindFurthestNode(player.transform.position));
            }
            //if(!spriteRenderer.isVisible) Destroy(gameObject);

        }
        #endregion
        
    }
}
