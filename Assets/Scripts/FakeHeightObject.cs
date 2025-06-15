using System;
using Managers;
using UnityEngine;

using UnityEngine.Events;

// 2D Top-Down Height Illusion using Shadows
// August Dominik (GucioDevs)
// 14 June 2025
// https://www.youtube.com/watch?v=6iS0qbSbKuw

// But a lot is still me ;) - mzati

public class FakeHeightObject : MonoBehaviour
{
   public UnityEvent onGroundHitEvent;

   public Transform trnsObject;
   public Transform trnsBody;
   public Transform trnsShadow;

   private readonly float gravity = -9.81f;
   public Vector2 groundVelocity;
   public float verticalVelocity;
   
   private float lastVerticalVelocity;
   
   public Rigidbody2D rb;
   public float groundLinearDamping;

   public bool isGrounded;
   
   private int timesBounced;
   [SerializeField] private int maxBounces = 5;
   
   [SerializeField] private CollisionType collisionType = CollisionType.None;
   private enum CollisionType
   {
      None,
      Body,
      Object
   }

   private void Awake()
   {
      if (collisionType != CollisionType.None) ;
      rb = GetComponent<Rigidbody2D>();
   }

   private void FixedUpdate()
   {
      UpdatePosition2();
      CheckGroundHit();
   }

   public void Initialize(Vector2 groundVelocity, float verticalVelocity)
   {
      isGrounded = false;
      this.groundVelocity = groundVelocity;
      this.verticalVelocity = verticalVelocity;
      lastVerticalVelocity = verticalVelocity;
      
      rb.linearVelocity = new Vector2(groundVelocity.x, groundVelocity.y);
      
   }

   void UpdatePosition()
   {
      if (!isGrounded)
      {
         verticalVelocity += gravity * Time.deltaTime;
         trnsBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
      }


      // trnsObject.position += (Vector3)groundVelocity * Time.deltaTime;
      rb.MovePosition(rb.position + groundVelocity * Time.fixedDeltaTime);
      
   }

   void UpdatePosition2()
   {
      if (!isGrounded)
      {
         verticalVelocity += gravity * Time.deltaTime;
         trnsBody.position += new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
      }


      // trnsObject.position += (Vector3)groundVelocity * Time.deltaTime;
      //rb.MovePosition(rb.position + groundVelocity * Time.fixedDeltaTime);
      
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

   private void OnCollisionEnter2D(Collision2D other)
   {
      
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
   }

   public void GroundHitSound(AudioClip sound)
   {
      //float volume = bounceBased ? (1f / (timesBounced + 1)) : 1f;
      AudioManager.instance.PlaySFXAt(sound, transform, 0.85f, (1f / (timesBounced + 1)));
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
      if (timesBounced >= maxBounces){
         verticalVelocity = 0f;
         isGrounded = true;
      }
      else
      {
         Initialize(rb.linearVelocity, lastVerticalVelocity / divisionFactor);
         //rb.linearVelocity /= divisionFactor;
         timesBounced++;
      }
   }
   
   public void SlowDownGroundVelocity(float divisionFactor = 1f)
   {
      rb.linearVelocity /= divisionFactor;
      if (timesBounced >= maxBounces) rb.linearVelocity = Vector2.zero;
   }
}
