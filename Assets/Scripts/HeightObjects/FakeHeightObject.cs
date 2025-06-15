using Managers;
using UnityEngine;
using UnityEngine.Events;

// 2D Top-Down Height Illusion using Shadows
// August Dominik (GucioDevs)
// 14 June 2025
// https://www.youtube.com/watch?v=6iS0qbSbKuw

// But a lot is still me ;) - mzati

namespace HeightObjects
{
   public class FakeHeightObject : MonoBehaviour
   {
      public UnityEvent onGroundHitEvent;

      public Transform trnsObject;
      public Transform trnsBody;
      public Transform trnsShadow;

      private const float Gravity = -9.81f;
      public Vector2 groundVelocity;
      public float verticalVelocity;
   
      private float _lastVerticalVelocity;
   
      public Rigidbody2D rb;
      public float groundLinearDamping;

      public bool isGrounded;
   
      private int _timesBounced;
      [SerializeField] private int maxBounces = 5;
   


      private void Awake()
      {
         rb = GetComponent<Rigidbody2D>();
      }

      public void FixedUpdate()
      {
         UpdatePosition();
         CheckGroundHit();
      }

      public void Initialize(Vector2 groundVelocity, float verticalVelocity)
      {
         isGrounded = false;
         this.groundVelocity = groundVelocity;
         this.verticalVelocity = verticalVelocity;
         _lastVerticalVelocity = verticalVelocity;
      
         rb.linearVelocity = new Vector2(groundVelocity.x, groundVelocity.y);
      
      }

      private void UpdatePosition()
      {
         if (isGrounded) return;
         verticalVelocity += Gravity * Time.deltaTime;
         trnsBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
      }
      private void CheckGroundHit()
      {
         if (trnsBody.position.y < trnsObject.position.y && !isGrounded)
         {
            isGrounded = true;
            GroundHit();
         }
      }
      void GroundHit()
      {
         onGroundHitEvent.Invoke();
         rb.linearDamping = groundLinearDamping;
      }

      public void GroundHitSound(AudioClip sound)
      {
         //float volume = bounceBased ? (1f / (timesBounced + 1)) : 1f;
         AudioManager.instance.PlaySFXAt(sound, transform, 1f, (1f / (_timesBounced + 1)));
      }

      public void Stop()
      {
         groundVelocity = Vector2.zero;
      }

      public void Stick()
      {
         groundVelocity = Vector2.zero;
      }

      public void Bounce(float divisionFactor = 1f)
      {
         if (_timesBounced >= maxBounces){
            verticalVelocity = 0f;
            isGrounded = true;
         }
         else
         {
            Initialize(rb.linearVelocity, _lastVerticalVelocity / divisionFactor);
            //rb.linearVelocity /= divisionFactor;
            _timesBounced++;
         }
      }
   
      public void SlowDownGroundVelocity(float divisionFactor = 1f)
      {
         rb.linearVelocity /= divisionFactor;
         if (_timesBounced >= maxBounces) rb.linearVelocity = Vector2.zero;
      }
   }
}
